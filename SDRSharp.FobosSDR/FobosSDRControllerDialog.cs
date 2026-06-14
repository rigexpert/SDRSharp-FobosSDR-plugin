//==============================================================================
//       _____     __           _______
//      /  __  \  /_/          /  ____/                                __
//     /  /_ / / _   ____     / /__  __  __   ____    ____    ____   _/ /_
//    /    __ / / / /  _  \  / ___/  \ \/ /  / __ \  / __ \  / ___\ /  _/
//   /  /\ \   / / /  /_/ / / /___   /   /  / /_/ / /  ___/ / /     / /_
//  /_ /  \_\ /_/  \__   / /______/ /_/\_\ / ____/  \____/ /_/      \___/
//               /______/                 /_/             
//  Fobos SDR API library
//  C# .Net API wrapper for SDR# plugin
//  Copyright (C) Rig Expert Ukraine Ltd.
//  2024.04.04
//  2024.04.25 - update
//  2025.01.22 - update
//==============================================================================
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
        private string[] _lna = { "none", "0dB", "16dB", "33dB" };
        public FobosSDRControllerDialog(FobosSDRIO owner)
        {
            InitializeComponent();
            _owner = owner;
        }

        public void LoadSettings()
        {
            samplerateComboBox.Items.Clear();
            foreach (var sr in _owner.Samplerates)
            {
                string str = Convert.ToString(sr);
                samplerateComboBox.Items.Add(str);
            }

            samplerateComboBox.SelectedIndex = Utils.GetIntSetting("FobosSDR.SampleRateIdx", 4);
            if (samplerateComboBox.SelectedIndex < 0)
            {
                samplerateComboBox.SelectedIndex = 0;
            }
            ComboBox_Input.SelectedIndex = Utils.GetIntSetting("FobosSDR.SamplingMode", 0);
            if (ComboBox_Input.SelectedIndex < 0)
            {
                ComboBox_Input.SelectedIndex = 0;
            }
            CheckBox_ExternalClock.Checked = Utils.GetBooleanSetting("FobosSDR.ExternalClock", false);
            TrackBar_LNA.Value = Utils.GetIntSetting("FobosSDR.LNA", 0);
            TrackBar_VGA.Value = Utils.GetIntSetting("FobosSDR.VGA", 0);
        }
        public void SelectDevice()
        {
            deviceComboBox.Items.Clear();
            deviceComboBox.Items.AddRange(_owner.Devices);
            if (_owner.Devices.Length > 0)
            {
                deviceComboBox.SelectedIndex = _owner.Index;
            }
            TextBox_API.Text = _owner.ApiInfo;
            TextBox_Board.Text = _owner.BoardInfo;
            TextBox_Serial.Text = _owner.Serial;
        }
        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FobosSDRControllerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void sample_rateBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                var samplerateString = samplerateComboBox.Items[samplerateComboBox.SelectedIndex].ToString().Split(' ')[0];
                var sampleRate = double.Parse(samplerateString, CultureInfo.InvariantCulture);
                _owner.Samplerate = sampleRate;
            }
            Utils.SaveSetting("FobosSDR.SampleRateIdx", samplerateComboBox.SelectedIndex);
        }

        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _owner.SelectDevice(deviceComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            TextBox_API.Text = _owner.ApiInfo;
            TextBox_Board.Text = _owner.BoardInfo;
            TextBox_Serial.Text = _owner.Serial;
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
                if (_owner.IsOpen)
                {
                    ComboBox_Input.Enabled = true;
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
                    CheckBox_ExternalClock.Enabled = true;
                }
                else
                {
                    ComboBox_Input.Enabled = false;
                    samplerateComboBox.Enabled = false;
                    TextBox_CenterFrequency.Enabled = false;
                    button_SetCenterFrequency.Enabled = false;
                    TrackBar_LNA.Enabled = false;
                    TrackBar_VGA.Enabled = false;
                    CheckBox_ExternalClock.Enabled = false;
                }
            }
        }

        private void Button_SetCenterFrequency(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                _owner._main.CenterFrequency = Convert.ToInt64(TextBox_CenterFrequency.Text);
            }
        }

        private void ComboBox_Input_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox_Input.SelectedIndex < 0)
            {
                ComboBox_Input.SelectedIndex = 0;
            }
            if (_owner != null)
            {
                _owner.SamplingMode = ComboBox_Input.SelectedIndex;
            }
            Utils.SaveSetting("FobosSDR.SamplingMode", ComboBox_Input.SelectedIndex);
        }

        private void checkBox_GPO0_CheckedChanged(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                int gpo_value = 0;
                gpo_value |= (Convert.ToInt32(checkBox_GPO0.Checked) << 0);
                gpo_value |= (Convert.ToInt32(checkBox_GPO1.Checked) << 1);
                gpo_value |= (Convert.ToInt32(checkBox_GPO2.Checked) << 2);
                gpo_value |= (Convert.ToInt32(checkBox_GPO3.Checked) << 3);
                gpo_value |= (Convert.ToInt32(checkBox_GPO4.Checked) << 4);
                gpo_value |= (Convert.ToInt32(checkBox_GPO5.Checked) << 5);
                gpo_value |= (Convert.ToInt32(checkBox_GPO6.Checked) << 6);
                gpo_value |= (Convert.ToInt32(checkBox_GPO7.Checked) << 7);
                _owner.SetUserGPO(gpo_value);
            }
        }

        private void TrackBar_LNA_ValueChanged(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                _owner.LNAgain = TrackBar_LNA.Value;
            }
            Utils.SaveSetting("FobosSDR.LNA", TrackBar_LNA.Value);
        }

        private void TrackBar_VGA_ValueChanged(object sender, EventArgs e)
        {
            if (_owner != null)
            {
                _owner.VGAgain = TrackBar_VGA.Value;
            }
            Utils.SaveSetting("FobosSDR.VGA", TrackBar_VGA.Value);
        }
    }
}
