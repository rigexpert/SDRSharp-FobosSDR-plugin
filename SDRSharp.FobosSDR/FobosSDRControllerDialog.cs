using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SDRSharp.FobosSDR
{
    public partial class FobosSDRControllerDialog : Form
    {
        private readonly FobosSDRIO _owner;
        private bool _initialized;
        public FobosSDRControllerDialog(FobosSDRIO owner)
        {

            //ConsoleHelper.CreateConsole();

            try
            {
                InitializeComponent();
                _owner = owner;

                var devices = _owner.GetActiveDevices();
                deviceComboBox.Items.AddRange(devices);

                if (devices.Length > 0)
                {
                    deviceComboBox.SelectedIndex = 0;
                }

                samplerateComboBox.SelectedIndex = Utils.GetIntSetting("FobosSDR.SampleRateIdx", 4);

                ComboBox_Input.SelectedIndex = Utils.GetIntSetting("FobosSDR.SamplingMode", 0);

                CheckBox_ExternalClock.Checked = Utils.GetBooleanSetting("FobosSDR.ExternalClock", false);

                TrackBar_LNA.Value = Utils.GetIntSetting("FobosSDR.LNA", 0);

                TrackBar_VGA.Value = Utils.GetIntSetting("FobosSDR.VGA", 0);

                _initialized = true;

                ComboBox_Input_SelectedIndexChanged(null, null);
                deviceComboBox_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            
        }


        private bool Initialized
        {
            get
            {
                return _initialized && _owner != null;
            }
        }

        public void ConfigureGUI()
        {
            deviceComboBox.SelectedIndex = _owner.Index;
        }

        public void ConfigureDevice()
        {
            sample_rateBoxSelectedIndexChanged(null, null);
            deviceComboBox_SelectedIndexChanged(null, null);
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EOTAControllerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void sample_rateBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var samplerateString = samplerateComboBox.Items[samplerateComboBox.SelectedIndex].ToString().Split(' ')[0];
            var sampleRate = double.Parse(samplerateString, CultureInfo.InvariantCulture);
            _owner.Samplerate = sampleRate * 1000000.0;
            Utils.SaveSetting("FobosSDR.SampleRateIdx", samplerateComboBox.SelectedIndex);
        }

        private void lnaTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                int gain = TrackBar_LNA.Value;
                if (_owner != null)
                {
                    _owner.LNAgain = gain;
                }
                Utils.SaveSetting("FobosSDR.LNA", _owner.LNAgain);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }
            try
            {
                _owner.SelectDevice(deviceComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                deviceComboBox.SelectedIndex = -1;
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            TextBox_API.Text = _owner.ApiInfo;
            TextBox_Board.Text = _owner.BoardInfo;
            TextBox_Serial.Text = _owner.Serial;
        }

        private void FobosSDRControllerDialog_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                samplerateComboBox.Enabled = !_owner.Device.IsStreaming;
                deviceComboBox.Enabled = !_owner.Device.IsStreaming;

                if (!_owner.Device.IsStreaming)
                {
                    var devices = _owner.GetActiveDevices();
                    deviceComboBox.Items.Clear();
                    deviceComboBox.Items.AddRange(devices);

                    for (var i = 0; i < devices.Length; i++)
                    {
                        if (i == _owner.Device.Index)
                        {
                            _initialized = false;
                            deviceComboBox.SelectedIndex = i;
                            _initialized = true;
                            break;
                        }
                    }
                }
            }
        }

        private void TrackBar_VGA_Scroll(object sender, EventArgs e)
        {
            try
            {
                int gain = TrackBar_VGA.Value;
                if (_owner != null)
                {
                    _owner.VGAgain = gain;
                }
                Console.WriteLine("VGA " + Convert.ToString(gain) + "\n");
                Utils.SaveSetting("FobosSDR.VGA", _owner.VGAgain);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        private void CheckBox_DirectSampling_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void CheckBox_ExternalClock_CheckedChanged(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                _owner.ExternalClock = Convert.ToInt32(CheckBox_ExternalClock.Checked);
            }
            Utils.SaveSetting("FobosSDR.ExternalClock", CheckBox_ExternalClock.Checked);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                samplerateComboBox.Enabled = !_owner.IsStreaming;
                deviceComboBox.Enabled = !_owner.IsStreaming;
                TextBox_CenterFrequency.Enabled = (_owner.SamplingMode == 0) || (_owner.SamplingMode == 1);
                button_SetCenterFrequency.Enabled = (_owner.SamplingMode == 0) || (_owner.SamplingMode == 1);
                TrackBar_LNA.Enabled = (_owner.SamplingMode == 0);
                TrackBar_VGA.Enabled = (_owner.SamplingMode == 0);
                if ((!TextBox_CenterFrequency.Focused) && (!this.Focused) && (!button_SetCenterFrequency.Focused))
                {
                    TextBox_CenterFrequency.Text = Convert.ToString(_owner.Frequency);
                }
            }
        }

        private void Button_SetCenterFrequency(object sender, EventArgs e)
        {
            _owner._main.CenterFrequency = Convert.ToInt64(TextBox_CenterFrequency.Text);
        }

        private void ComboBox_Input_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                _owner.SamplingMode = ComboBox_Input.SelectedIndex;
            }
            Utils.SaveSetting("FobosSDR.SamplingMode", ComboBox_Input.SelectedIndex);

        }
    }
}
