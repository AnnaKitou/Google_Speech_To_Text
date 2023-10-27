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
            bindingSource1 = new BindingSource(components);
            SoundBar = new ProgressBar();
            SelectInputLbl = new Label();
            InputListCb = new ComboBox();
            StopBtn = new Button();
            richTextBox1 = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // btnRecordVoice
            // 
            btnRecordVoice.Location = new Point(420, 4);
            btnRecordVoice.Margin = new Padding(2);
            btnRecordVoice.Name = "btnRecordVoice";
            btnRecordVoice.Size = new Size(90, 30);
            btnRecordVoice.TabIndex = 0;
            btnRecordVoice.Text = "Record";
            btnRecordVoice.UseVisualStyleBackColor = true;
            btnRecordVoice.Click += btnRecordVoice_Click_1;
            // 
            // SoundBar
            // 
            SoundBar.Location = new Point(609, 4);
            SoundBar.Name = "SoundBar";
            SoundBar.Size = new Size(398, 29);
            SoundBar.TabIndex = 5;
            // 
            // SelectInputLbl
            // 
            SelectInputLbl.AutoSize = true;
            SelectInputLbl.Location = new Point(11, 9);
            SelectInputLbl.Margin = new Padding(2, 0, 2, 0);
            SelectInputLbl.Name = "SelectInputLbl";
            SelectInputLbl.Size = new Size(87, 20);
            SelectInputLbl.TabIndex = 6;
            SelectInputLbl.Text = "Select Input";
            // 
            // InputListCb
            // 
            InputListCb.FormattingEnabled = true;
            InputListCb.Location = new Point(102, 6);
            InputListCb.Margin = new Padding(2);
            InputListCb.Name = "InputListCb";
            InputListCb.Size = new Size(314, 28);
            InputListCb.TabIndex = 7;
            InputListCb.SelectedIndexChanged += InputListCb_SelectedIndexChanged_1;
            // 
            // StopBtn
            // 
            StopBtn.Location = new Point(514, 4);
            StopBtn.Margin = new Padding(2);
            StopBtn.Name = "StopBtn";
            StopBtn.Size = new Size(90, 30);
            StopBtn.TabIndex = 8;
            StopBtn.Text = "Stop";
            StopBtn.UseVisualStyleBackColor = true;
            StopBtn.Click += StopBtn_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox1.Location = new Point(12, 39);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new Size(995, 491);
            richTextBox1.TabIndex = 11;
            richTextBox1.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1019, 542);
            Controls.Add(richTextBox1);
            Controls.Add(StopBtn);
            Controls.Add(InputListCb);
            Controls.Add(SelectInputLbl);
            Controls.Add(btnRecordVoice);
            Controls.Add(SoundBar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "Google Speech To text";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRecordVoice;
        private BindingSource bindingSource1;
        private ProgressBar SoundBar;
        private Label SelectInputLbl;
        private ComboBox InputListCb;
        private Button StopBtn;
        private RichTextBox richTextBox1;
    }
}