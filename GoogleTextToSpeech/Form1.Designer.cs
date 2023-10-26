namespace GoogleTextToSpeech
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			btnRecordVoice = new Button();
			btnSave = new Button();
			PauseBtn = new Button();
			bindingSource1 = new BindingSource(components);
			SoundBar = new ProgressBar();
			label1 = new Label();
			InputListCb = new ComboBox();
			StopBtn = new Button();
			textBox1 = new TextBox();
			((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
			SuspendLayout();
			// 
			// btnRecordVoice
			// 
			btnRecordVoice.Location = new Point(33, 91);
			btnRecordVoice.Name = "btnRecordVoice";
			btnRecordVoice.Size = new Size(112, 34);
			btnRecordVoice.TabIndex = 0;
			btnRecordVoice.Text = "Record";
			btnRecordVoice.UseVisualStyleBackColor = true;
			btnRecordVoice.Click += btnRecordVoice_Click_1;
			// 
			// btnSave
			// 
			btnSave.Enabled = false;
			btnSave.Location = new Point(33, 162);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(112, 34);
			btnSave.TabIndex = 1;
			btnSave.Text = "Save";
			btnSave.UseVisualStyleBackColor = true;
			btnSave.Click += btnSave_Click;
			// 
			// PauseBtn
			// 
			PauseBtn.Location = new Point(221, 91);
			PauseBtn.Name = "PauseBtn";
			PauseBtn.Size = new Size(112, 34);
			PauseBtn.TabIndex = 3;
			PauseBtn.Text = "Pause";
			PauseBtn.UseVisualStyleBackColor = true;
			PauseBtn.Click += PauseBtn_Click;
			// 
			// SoundBar
			// 
			SoundBar.Location = new Point(3, 217);
			SoundBar.Margin = new Padding(4);
			SoundBar.Name = "SoundBar";
			SoundBar.Size = new Size(595, 36);
			SoundBar.TabIndex = 5;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(33, 29);
			label1.Name = "label1";
			label1.Size = new Size(123, 25);
			label1.TabIndex = 6;
			label1.Text = "SelectInputLbl";
			// 
			// InputListCb
			// 
			InputListCb.FormattingEnabled = true;
			InputListCb.Location = new Point(229, 28);
			InputListCb.Name = "InputListCb";
			InputListCb.Size = new Size(369, 33);
			InputListCb.TabIndex = 7;
			// 
			// StopBtn
			// 
			StopBtn.Location = new Point(221, 162);
			StopBtn.Name = "StopBtn";
			StopBtn.Size = new Size(112, 34);
			StopBtn.TabIndex = 8;
			StopBtn.Text = "Stop";
			StopBtn.UseVisualStyleBackColor = true;
			StopBtn.Click += StopBtn_Click;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(52, 281);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.ScrollBars = ScrollBars.Vertical;
			textBox1.Size = new Size(519, 136);
			textBox1.TabIndex = 9;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(textBox1);
			Controls.Add(StopBtn);
			Controls.Add(InputListCb);
			Controls.Add(label1);
			Controls.Add(PauseBtn);
			Controls.Add(btnSave);
			Controls.Add(btnRecordVoice);
			Controls.Add(SoundBar);
			Name = "Form1";
			Text = "Form1";
			Load += Form1_Load;
			((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btnRecordVoice;
		private Button btnSave;
		private Button PauseBtn;
		private BindingSource bindingSource1;
		private ProgressBar SoundBar;
		private Label label1;
		private ComboBox InputListCb;
		private Button StopBtn;
		private TextBox textBox1;
	}
}