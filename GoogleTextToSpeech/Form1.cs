using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using GoogleTextToSpeech.Entities;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using NAudio.Utils;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Net;

namespace GoogleTextToSpeech
{
	public partial class Form1 : Form
	{

		private BufferedWaveProvider bwp;
		private SpeechClient.StreamingRecognizeStream _streamingCall;

		int AudioEndPointId;

		WaveInEvent waveIn;
		WaveOut waveOut;
		WaveFileWriter writer;
		WaveFileReader reader;
		string output = "audio.raw";
		private readonly IConfiguration _configuration;

		

		public Form1(IConfiguration configuration)
		{
			_configuration = configuration;
			InitializeComponent();
			EnumerateAudioEndPoints();
		}


		private void InputListCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			var SelectedItemSerialized = JsonConvert.SerializeObject(InputListCb.SelectedItem);
			var SelectedItemDeserialized = JsonConvert.DeserializeObject<ComboboxValue>(SelectedItemSerialized);
			AudioEndPointId = SelectedItemDeserialized.Id;
		}
		async void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
		{
			Int16[] values = new Int16[e.Buffer.Length / 2];
			Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);

			// determine the highest value as a fraction of the maximum possible value
			float fraction = (float)values.Max() / 32768;

			// Show Sound Level
			SoundBar.Invoke((MethodInvoker)delegate { SoundBar.Value = (int)(fraction * 70); });

			bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

			#region Google Cloud Set Up Speech To Text Communication
			var audioRequest = new StreamingRecognizeRequest
			{
				AudioContent = Google.Protobuf.ByteString.CopyFrom(e.Buffer, 0, e.BytesRecorded)
			};
			try
			{
				await _streamingCall.WriteAsync(audioRequest);
			}
			catch (Grpc.Core.RpcException ex)
			{

			}
			#endregion


		}
		private void btnRecordVoice_Click_1(object sender, EventArgs e)
		{
			waveIn = new NAudio.Wave.WaveInEvent
			{
				DeviceNumber = AudioEndPointId, // indicates which microphone to use
				WaveFormat = new NAudio.Wave.WaveFormat(rate: 44100, bits: 16, channels: 1),
				BufferMilliseconds = 20
			};


			bwp = new BufferedWaveProvider(waveIn.WaveFormat);
			bwp.DiscardOnBufferOverflow = true;

			btnRecordVoice.Enabled = false;
			btnSave.Enabled = false;

			#region Google Cloud Set Up Speech To Text Communication
			GoogleCredential credentials;
			using (var stream = new FileStream(_configuration["JsonFilePath"], FileMode.Open, FileAccess.Read))
			{
				credentials = GoogleCredential.FromStream(stream);
			}

			var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.ToString(), credentials.ToChannelCredentials());
			var speech = CreateSpeechClient(credentials);


			_streamingCall = speech.StreamingRecognize();

			var configRequest = new StreamingRecognizeRequest
			{
				StreamingConfig = new StreamingRecognitionConfig
				{
					Config = new RecognitionConfig
					{
						Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
						SampleRateHertz = 44100,
						LanguageCode = "el-GR"
					},
					InterimResults = true
				}
			};

			_streamingCall.WriteAsync(configRequest).Wait();


			// Start a task to listen to the responses and update the textbox
			Task.Run(async () =>
			{
				var responseStream = _streamingCall.GetResponseStream();
				while (await responseStream.MoveNextAsync(CancellationToken.None))
				{
					foreach (var result in _streamingCall.GetResponseStream().Current.Results)
					{
						if (result.Alternatives != null && result.Alternatives.Count > 0)
						{
							textBox1.Invoke((MethodInvoker)delegate
							{
								textBox1.Text = result.Alternatives[0].Transcript;
							});
						}
					}
				}
			});
			#endregion

			waveIn.DataAvailable += WaveIn_DataAvailable;
			waveIn.StartRecording();
			//////waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(waveOut_PlaybackStopped);

			waveOut = new WaveOut();
			waveOut.Init(bwp);
			waveOut.Play();
		}

		private void waveOut_PlaybackStopped(object sender, StoppedEventArgs e)
		{

			waveOut.Stop();
			reader.Close();
			reader = null;
		}
		private void PauseBtn_Click(object sender, EventArgs e)
		{
			waveIn.DataAvailable -= WaveIn_DataAvailable;
			SoundBar.Invoke((MethodInvoker)delegate { SoundBar.Value = 0; });
		}

		private void EnumerateAudioEndPoints()
		{
			for (int i = -1; i < NAudio.Wave.WaveIn.DeviceCount; i++)
			{
				var caps = NAudio.Wave.WaveIn.GetCapabilities(i);
				InputListCb.Items.Add(new ComboboxValue(i, i.ToString() + " : " + caps.ProductName));
			}
		}

		private void StopBtn_Click(object sender, EventArgs e)
		{
			if (waveIn != null)
			{
				waveIn.StopRecording();
				waveIn.Dispose();
				waveIn = null;
			}

			ExitApplicationWithMessage();
		}
		private void ExitApplicationWithMessage()
		{
			SoundBar.Invoke((MethodInvoker)delegate { SoundBar.Value = 0; });
			MessageBox.Show("Goodbye");
			Application.Exit();
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			waveIn.StopRecording();

			if (File.Exists("audio.raw"))
				File.Delete("audio.raw");

			writer = new WaveFileWriter(output, waveIn.WaveFormat);

			btnRecordVoice.Enabled = false;
			btnSave.Enabled = false;

			byte[] buffer = new byte[bwp.BufferLength];
			int offset = 0;
			int count = bwp.BufferLength;

			var read = bwp.Read(buffer, offset, count);
			if (count > 0)
			{
				writer.Write(buffer, offset, read);
			}

			waveIn.Dispose();
			waveIn = null;
			writer.Close();
			writer = null;

			reader = new WaveFileReader("audio.raw"); // (new MemoryStream(bytes));
			waveOut.Init(reader);
			//waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(waveOut_PlaybackStopped);
			waveOut.Play();
			SpeechTransform();

		}
		private void SpeechTransform()
		{

			btnRecordVoice.Enabled = true;
			btnSave.Enabled = false;

			if (File.Exists("audio.raw"))
			{

				GoogleCredential credentials;
				using (var stream = new FileStream(_configuration["JsonFilePath"], FileMode.Open, FileAccess.Read))
				{
					credentials = GoogleCredential.FromStream(stream);
				}

				var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.ToString(), credentials.ToChannelCredentials());

				var speech = CreateSpeechClient(credentials);


				var response = speech.Recognize(new RecognitionConfig()
				{
					Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
					//  SampleRateHertz = 16000,
					LanguageCode = "el-GR",
				}, RecognitionAudio.FromFile("audio.raw"));


				textBox1.Text = "";

				foreach (var result in response.Results)
				{
					foreach (var alternative in result.Alternatives)
					{
						textBox1.Text = textBox1.Text + " " + alternative.Transcript;
					}
				}

				if (textBox1.Text.Length == 0)
					textBox1.Text = "No Data ";

			}
			else
			{

				textBox1.Text = "Audio File Missing ";

			}


		}
		public SpeechClient CreateSpeechClient(GoogleCredential credentials)
		{
			try
			{
				var speech = new SpeechClientBuilder
				{
					ChannelCredentials = credentials.ToChannelCredentials(),
				}.Build();
				return speech;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating SpeechClient: {ex.Message}");
				return null;
			}
		}
		private void Form1_Load(object sender, EventArgs e)
		{

		}

	}
}