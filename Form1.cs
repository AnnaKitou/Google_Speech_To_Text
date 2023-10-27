using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;
using GoogleTextToSpeech.Entities;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using NAudio.Utils;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace GoogleTextToSpeech
{
    public partial class Form1 : Form
    {

        private BufferedWaveProvider bwp;
        private SpeechClient.StreamingRecognizeStream _streamingCall;

        int AudioEndPointId;

        WaveInEvent waveIn;
        WaveFileWriter writer;
        private readonly IConfiguration _configuration;
        //int position;
        int Prevwordlength;



        public Form1(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeComponent();
            EnumerateAudioEndPoints();

            //position = 0;
            Prevwordlength = 0;
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
                while (await responseStream.MoveNextAsync())
                {
                    StreamingRecognizeResponse responseItem = responseStream.Current;
                    foreach (var result in responseStream.Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            HandleText(alternative.Transcript, alternative.Confidence);
                        }
                    }
                }
            });
            #endregion

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();
            //////waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(waveOut_PlaybackStopped);
        }

        private void EnumerateAudioEndPoints()
        {
            for (int i = -1; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                var caps = NAudio.Wave.WaveIn.GetCapabilities(i);
                if (i >= 0)
                {
                    InputListCb.Items.Add(new ComboboxValue(i, i.ToString() + " : " + caps.ProductName));
                }
            }
            if (InputListCb.Items.Count > 0)
            {
                InputListCb.SelectedIndex = 0;
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

        private void HandleText(string saidWhat, float Confidence)
        {


            //float Confidence = result.Alternatives[0].Confidence;
            Color foreColor = Color.Black;

            switch (Confidence)
            {
                case (float)0:
                    foreColor = Color.Gray;
                    break;
                case > (float)0.0:
                    foreColor = Color.Black;
                    break;
            }

            //string saidWhat = result.Alternatives[0].Transcript;

            //int wordLength = saidWhat.Length;

            if (Confidence < (float)0.1)
            {
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    if (richTextBox1.Text.Length >= Prevwordlength)
                    {
                        richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - Prevwordlength, Prevwordlength);
                    }

                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.SelectionColor = foreColor;
                    richTextBox1.AppendText(saidWhat + " ");
                    richTextBox1.SelectionColor = richTextBox1.ForeColor;
                });

                Prevwordlength = saidWhat.Length;
            }
            else
            {
                saidWhat = saidWhat.Replace("Αλλαγή γραμμής", " \r\n").Replace("κόμμα", ",");


                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    if (richTextBox1.Text.Length >= Prevwordlength)
                    {
                        richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - Prevwordlength, Prevwordlength);
                    }

                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.SelectionColor = foreColor;
                    richTextBox1.AppendText(saidWhat + " ");
                    richTextBox1.SelectionColor = richTextBox1.ForeColor;
                });

                Prevwordlength = 0;
            }

            //textBox2.Invoke((MethodInvoker)delegate { textBox2.AppendText(Confidence.ToString() + " \r\n"); });

        }

        private void InputListCb_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var SelectedItemSerialized = JsonConvert.SerializeObject(InputListCb.SelectedItem);
            var SelectedItemDeserialized = JsonConvert.DeserializeObject<ComboboxValue>(SelectedItemSerialized);
            AudioEndPointId = SelectedItemDeserialized.Id;

            btnRecordVoice.Enabled = true;
        }
    }
}