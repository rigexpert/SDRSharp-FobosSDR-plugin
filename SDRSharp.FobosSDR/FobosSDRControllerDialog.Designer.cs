namespace SDRSharp.FobosSDR
{
    partial class FobosSDRControllerDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.samplerateComboBox = new System.Windows.Forms.ComboBox();
            this.TrackBar_LNA = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TrackBar_VGA = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBox_API = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TextBox_Board = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TextBox_Serial = new System.Windows.Forms.TextBox();
            this.CheckBox_ExternalClock = new System.Windows.Forms.CheckBox();
            this.button_SetCenterFrequency = new System.Windows.Forms.Button();
            this.TextBox_CenterFrequency = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ComboBox_Input = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_GPO0 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO1 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO2 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO3 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO4 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO5 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO6 = new System.Windows.Forms.CheckBox();
            this.checkBox_GPO7 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_LNA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_VGA)).BeginInit();
            this.SuspendLayout();
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceComboBox.FormattingEnabled = true;
            this.deviceComboBox.Location = new System.Drawing.Point(12, 25);
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(276, 21);
            this.deviceComboBox.TabIndex = 0;
            this.deviceComboBox.SelectedIndexChanged += new System.EventHandler(this.deviceComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Device";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sample Rate";
            // 
            // samplerateComboBox
            // 
            this.samplerateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.samplerateComboBox.FormattingEnabled = true;
            this.samplerateComboBox.Items.AddRange(new object[] {
            "50 MSpS",
            "40 MSpS",
            "32 MSpS",
            "25 MSpS",
            "20 MSpS",
            "16 MSpS",
            "12.5 MSpS",
            "10 MSpS",
            "8 MSpS"});
            this.samplerateComboBox.Location = new System.Drawing.Point(131, 184);
            this.samplerateComboBox.Name = "samplerateComboBox";
            this.samplerateComboBox.Size = new System.Drawing.Size(157, 21);
            this.samplerateComboBox.TabIndex = 3;
            this.samplerateComboBox.SelectedIndexChanged += new System.EventHandler(this.sample_rateBoxSelectedIndexChanged);
            // 
            // TrackBar_LNA
            // 
            this.TrackBar_LNA.LargeChange = 1;
            this.TrackBar_LNA.Location = new System.Drawing.Point(74, 218);
            this.TrackBar_LNA.Maximum = 3;
            this.TrackBar_LNA.Name = "TrackBar_LNA";
            this.TrackBar_LNA.Size = new System.Drawing.Size(204, 45);
            this.TrackBar_LNA.TabIndex = 21;
            this.TrackBar_LNA.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar_LNA.Scroll += new System.EventHandler(this.lnaTrackBar_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "LNA Gain";
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(213, 313);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 7;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TrackBar_VGA
            // 
            this.TrackBar_VGA.LargeChange = 1;
            this.TrackBar_VGA.Location = new System.Drawing.Point(74, 251);
            this.TrackBar_VGA.Maximum = 15;
            this.TrackBar_VGA.Name = "TrackBar_VGA";
            this.TrackBar_VGA.Size = new System.Drawing.Size(204, 45);
            this.TrackBar_VGA.TabIndex = 23;
            this.TrackBar_VGA.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar_VGA.Scroll += new System.EventHandler(this.TrackBar_VGA_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "VGA Gain";
            // 
            // TextBox_API
            // 
            this.TextBox_API.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TextBox_API.Location = new System.Drawing.Point(50, 52);
            this.TextBox_API.Name = "TextBox_API";
            this.TextBox_API.Size = new System.Drawing.Size(238, 20);
            this.TextBox_API.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "API";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Board";
            // 
            // TextBox_Board
            // 
            this.TextBox_Board.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TextBox_Board.Location = new System.Drawing.Point(50, 78);
            this.TextBox_Board.Name = "TextBox_Board";
            this.TextBox_Board.Size = new System.Drawing.Size(238, 20);
            this.TextBox_Board.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Serial";
            // 
            // TextBox_Serial
            // 
            this.TextBox_Serial.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TextBox_Serial.Location = new System.Drawing.Point(50, 104);
            this.TextBox_Serial.Name = "TextBox_Serial";
            this.TextBox_Serial.Size = new System.Drawing.Size(238, 20);
            this.TextBox_Serial.TabIndex = 28;
            // 
            // CheckBox_ExternalClock
            // 
            this.CheckBox_ExternalClock.AutoSize = true;
            this.CheckBox_ExternalClock.Location = new System.Drawing.Point(13, 316);
            this.CheckBox_ExternalClock.Name = "CheckBox_ExternalClock";
            this.CheckBox_ExternalClock.Size = new System.Drawing.Size(93, 17);
            this.CheckBox_ExternalClock.TabIndex = 31;
            this.CheckBox_ExternalClock.Text = "External clock";
            this.CheckBox_ExternalClock.UseVisualStyleBackColor = true;
            this.CheckBox_ExternalClock.CheckedChanged += new System.EventHandler(this.CheckBox_ExternalClock_CheckedChanged);
            // 
            // button_SetCenterFrequency
            // 
            this.button_SetCenterFrequency.Location = new System.Drawing.Point(247, 155);
            this.button_SetCenterFrequency.Name = "button_SetCenterFrequency";
            this.button_SetCenterFrequency.Size = new System.Drawing.Size(41, 23);
            this.button_SetCenterFrequency.TabIndex = 32;
            this.button_SetCenterFrequency.Text = "Set";
            this.toolTip1.SetToolTip(this.button_SetCenterFrequency, "Set center frequency explicitly");
            this.button_SetCenterFrequency.UseVisualStyleBackColor = true;
            this.button_SetCenterFrequency.Click += new System.EventHandler(this.Button_SetCenterFrequency);
            // 
            // TextBox_CenterFrequency
            // 
            this.TextBox_CenterFrequency.Location = new System.Drawing.Point(104, 158);
            this.TextBox_CenterFrequency.Name = "TextBox_CenterFrequency";
            this.TextBox_CenterFrequency.Size = new System.Drawing.Size(137, 20);
            this.TextBox_CenterFrequency.TabIndex = 33;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Central freq, Hz";
            // 
            // ComboBox_Input
            // 
            this.ComboBox_Input.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Input.FormattingEnabled = true;
            this.ComboBox_Input.Items.AddRange(new object[] {
            "RF",
            "IQ (HF1+HF2) direct sampling",
            "HF1 direct sampling",
            "HF2 direct sampling"});
            this.ComboBox_Input.Location = new System.Drawing.Point(104, 130);
            this.ComboBox_Input.Name = "ComboBox_Input";
            this.ComboBox_Input.Size = new System.Drawing.Size(184, 21);
            this.ComboBox_Input.TabIndex = 35;
            this.ComboBox_Input.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Input_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Input";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 284);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "GPO:";
            // 
            // checkBox_GPO0
            // 
            this.checkBox_GPO0.AutoSize = true;
            this.checkBox_GPO0.Location = new System.Drawing.Point(42, 283);
            this.checkBox_GPO0.Name = "checkBox_GPO0";
            this.checkBox_GPO0.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO0.TabIndex = 38;
            this.checkBox_GPO0.Text = "0";
            this.checkBox_GPO0.UseVisualStyleBackColor = true;
            this.checkBox_GPO0.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO1
            // 
            this.checkBox_GPO1.AutoSize = true;
            this.checkBox_GPO1.Location = new System.Drawing.Point(73, 283);
            this.checkBox_GPO1.Name = "checkBox_GPO1";
            this.checkBox_GPO1.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO1.TabIndex = 39;
            this.checkBox_GPO1.Text = "1";
            this.checkBox_GPO1.UseVisualStyleBackColor = true;
            this.checkBox_GPO1.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO2
            // 
            this.checkBox_GPO2.AutoSize = true;
            this.checkBox_GPO2.Location = new System.Drawing.Point(104, 283);
            this.checkBox_GPO2.Name = "checkBox_GPO2";
            this.checkBox_GPO2.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO2.TabIndex = 40;
            this.checkBox_GPO2.Text = "2";
            this.checkBox_GPO2.UseVisualStyleBackColor = true;
            this.checkBox_GPO2.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO3
            // 
            this.checkBox_GPO3.AutoSize = true;
            this.checkBox_GPO3.Location = new System.Drawing.Point(134, 283);
            this.checkBox_GPO3.Name = "checkBox_GPO3";
            this.checkBox_GPO3.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO3.TabIndex = 41;
            this.checkBox_GPO3.Text = "3";
            this.checkBox_GPO3.UseVisualStyleBackColor = true;
            this.checkBox_GPO3.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO4
            // 
            this.checkBox_GPO4.AutoSize = true;
            this.checkBox_GPO4.Location = new System.Drawing.Point(165, 283);
            this.checkBox_GPO4.Name = "checkBox_GPO4";
            this.checkBox_GPO4.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO4.TabIndex = 42;
            this.checkBox_GPO4.Text = "4";
            this.checkBox_GPO4.UseVisualStyleBackColor = true;
            this.checkBox_GPO4.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO5
            // 
            this.checkBox_GPO5.AutoSize = true;
            this.checkBox_GPO5.Location = new System.Drawing.Point(197, 283);
            this.checkBox_GPO5.Name = "checkBox_GPO5";
            this.checkBox_GPO5.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO5.TabIndex = 43;
            this.checkBox_GPO5.Text = "5";
            this.checkBox_GPO5.UseVisualStyleBackColor = true;
            this.checkBox_GPO5.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO6
            // 
            this.checkBox_GPO6.AutoSize = true;
            this.checkBox_GPO6.Location = new System.Drawing.Point(231, 283);
            this.checkBox_GPO6.Name = "checkBox_GPO6";
            this.checkBox_GPO6.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO6.TabIndex = 44;
            this.checkBox_GPO6.Text = "6";
            this.checkBox_GPO6.UseVisualStyleBackColor = true;
            this.checkBox_GPO6.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // checkBox_GPO7
            // 
            this.checkBox_GPO7.AutoSize = true;
            this.checkBox_GPO7.Location = new System.Drawing.Point(264, 283);
            this.checkBox_GPO7.Name = "checkBox_GPO7";
            this.checkBox_GPO7.Size = new System.Drawing.Size(32, 17);
            this.checkBox_GPO7.TabIndex = 45;
            this.checkBox_GPO7.Text = "7";
            this.checkBox_GPO7.UseVisualStyleBackColor = true;
            this.checkBox_GPO7.CheckedChanged += new System.EventHandler(this.checkBox_GPO0_CheckedChanged);
            // 
            // FobosSDRControllerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 345);
            this.Controls.Add(this.checkBox_GPO7);
            this.Controls.Add(this.checkBox_GPO6);
            this.Controls.Add(this.checkBox_GPO5);
            this.Controls.Add(this.checkBox_GPO4);
            this.Controls.Add(this.checkBox_GPO3);
            this.Controls.Add(this.checkBox_GPO2);
            this.Controls.Add(this.checkBox_GPO1);
            this.Controls.Add(this.checkBox_GPO0);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ComboBox_Input);
            this.Controls.Add(this.close);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TextBox_CenterFrequency);
            this.Controls.Add(this.button_SetCenterFrequency);
            this.Controls.Add(this.CheckBox_ExternalClock);
            this.Controls.Add(this.deviceComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TextBox_Serial);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TextBox_Board);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBox_API);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TrackBar_VGA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TrackBar_LNA);
            this.Controls.Add(this.samplerateComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FobosSDRControllerDialog";
            this.Text = "Fobos SDR Controller";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FobosSDRControllerDialog_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.FobosSDRControllerDialog_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_LNA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar_VGA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox samplerateComboBox;
        private System.Windows.Forms.TrackBar TrackBar_LNA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar TrackBar_VGA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBox_API;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TextBox_Board;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBox_Serial;
        private System.Windows.Forms.CheckBox CheckBox_ExternalClock;
        private System.Windows.Forms.Button button_SetCenterFrequency;
        private System.Windows.Forms.TextBox TextBox_CenterFrequency;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ComboBox_Input;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_GPO0;
        private System.Windows.Forms.CheckBox checkBox_GPO1;
        private System.Windows.Forms.CheckBox checkBox_GPO2;
        private System.Windows.Forms.CheckBox checkBox_GPO3;
        private System.Windows.Forms.CheckBox checkBox_GPO4;
        private System.Windows.Forms.CheckBox checkBox_GPO5;
        private System.Windows.Forms.CheckBox checkBox_GPO6;
        private System.Windows.Forms.CheckBox checkBox_GPO7;
    }
}