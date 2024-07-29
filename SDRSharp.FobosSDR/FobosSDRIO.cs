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
//==============================================================================
using System;
using System.Threading;
using System.Windows.Forms;
using SDRSharp;
using SDRSharp.Common;
using SDRSharp.Radio;
using System.Runtime.InteropServices;

namespace SDRSharp.FobosSDR
{
    public unsafe class FobosSDRIO : IFrontendController, IIQStreamController, IFloatingConfigDialogProvider, IDisposable, ITunableSource
    {
        private readonly FobosSDRControllerDialog _gui = null;
        private int _index;
        private FobosSDRDevice _dev;
        public bool _isStreaming;
        private Radio.SamplesAvailableDelegate _callback;

        private double _frequency = 100000000;
        private double _sampleRate = 25000000.0;
        private int _sampling_mode = 0;
        private bool _can_tune = true;
        private int _external_clock = 0;
        private int _LNA_Gain = 0;
        private int _VGA_Gain = 0;

        public SDRSharp.MainForm _main;
        public string ApiInfo;
        public string BoardInfo;
        public string Serial;
        //======================================================================
        public FobosSDRIO()
        {
            //ConsoleHelper.CreateConsole();
            _index = -1;
            _gui = new FobosSDRControllerDialog(this);
        }
        //======================================================================
        ~FobosSDRIO()
        {
            Dispose();
        }
        //======================================================================
        public void Dispose()
        {
            if(_gui != null)
            {
                _gui.Dispose();
            }
            GC.SuppressFinalize(this);
        }
        //======================================================================
        public string[] GetActiveDevices()
        {
            var count = NativeMethods.fobos_rx_get_device_count();
            byte[] buf = new byte[256];
            unsafe
            {
                fixed (byte* p_buf = &buf[0])
                {
                    count = NativeMethods.fobos_rx_list_devices(p_buf);
                }
            }
            string serials_str = System.Text.Encoding.UTF8.GetString(buf).TrimEnd('\0');
            var serials_list = serials_str.Split();

            var result = new string[count];

            for (var i = 0; i < count; i++)
            {
                if (i == this._index)
                {
                    result[i] = "Fobos SDR #" + Convert.ToString(i) + " " + this.Serial;
                }
                else
                {
                    result[i] = "Fobos SDR #" + Convert.ToString(i) + " " + serials_list[i];
                }
            }
            return result;
        }
        //======================================================================
        public void SelectDevice(int index)
        {
            if (_index != index)
            {
                Close();
                _index = index;
                if (index > -1)
                {
                    _dev = new FobosSDRDevice((uint)index);
                    _dev.Frequency = _frequency;
                    _dev.Samplerate = _sampleRate;
                    _dev.SamplingMode = _sampling_mode;
                    _dev.ExternalClock = _external_clock;
                    _dev.LNAgain = _LNA_Gain;
                    _dev.VGAgain = _VGA_Gain;
                    ApiInfo = "lib v." + _dev.lib_version + " drv " + _dev.drv_version;
                    BoardInfo = "HW: r." + _dev.hw_revision + " FV: v." + _dev.fw_version;
                    Serial = _dev.serial;
                    _dev.SamplesAvailable += SamplesAvailableEvent;
                }
            }
        }
        //======================================================================
        public FobosSDRDevice Device
        { 
            get
            {
                return _dev;
            } 
        }
        //======================================================================
        public void Close()
        {
            if (_dev != null)
            {
                _dev.Stop();
                _dev.SamplesAvailable -= SamplesAvailableEvent;
                _dev.Dispose();
                _dev = null;
            }
            _index = -1;
        }
        //======================================================================
        public void Open()
        {
            if (_dev != null)
            {
                return;
            }
            if (_index >= 0)
            {
                try
                {
                    SelectDevice(_index);
                    return;
                }
                catch (ApplicationException)
                {
                    MessageBox.Show("FobosSDR: Could not open device" + _index);
                }
            }
        }
        //======================================================================
        public void ShowSettingGUI(IWin32Window parent)
        {
            _main = (SDRSharp.MainForm)parent;
            Console.WriteLine("parent: "+parent.ToString());
            UpdateMainForm();
            Console.WriteLine("show");
            _gui.Show();
            Console.WriteLine("ok");
        }
        //======================================================================
        public void HideSettingGUI()
        {
            _gui.Hide();
        }
        //======================================================================
        public void Start(Radio.SamplesAvailableDelegate callback)
        {
            GetMainForm();
            if (_dev == null)
            {
                throw new ApplicationException("No device selected");
            }
            _callback = callback;
            try
            {
                _main.CenterFrequency = (long)_dev.Frequency;
                _dev.Start();
            }
            catch
            {
                Open();
                _dev.Start();
            }
            UpdateMainForm();
        }
        //======================================================================
        public void Stop()
        {
            if (_dev != null)
            {
                _dev.Stop();
            }
        }
        //======================================================================
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                SelectDevice(value);
            }
        }
        //======================================================================
        public bool CanTune
        {
            get
            {
                return _can_tune;
            }
        }
        //======================================================================
        public bool IsSoundCardBased
        {
            get
            {
                return false;
            }
        }
        //======================================================================
        public double Samplerate
        {
            get
            {
                return _dev == null ? 0.0 : _dev.Samplerate; 
            }
            set
            {
                _sampleRate = value;
                if (_dev != null)
                {
                    _dev.Samplerate = value;
                }
                UpdateMainForm();
            }
        }
        //======================================================================
        public long Frequency
        {
            get
            {
                if ((_sampling_mode == 0) || (_sampling_mode == 1))
                {
                    return (long)_frequency;
                }
                return (long)_sampleRate / 2;
            }
            set
            {
                if ((_sampling_mode == 0) || (_sampling_mode == 1))
                {
                    if (_dev != null)
                    {
                        _dev.Frequency = value;
                        _frequency = value;
                    }
                }
            }
        }
        //======================================================================
        unsafe public int SamplingMode
        {
            get { return _sampling_mode; }
            set
            {
                if (_sampling_mode != value)
                {
                    if (_dev != null)
                    {
                        _dev.SamplingMode = value;
                    }
                    _sampling_mode = value;
                    UpdateMainForm();
                }
            }
        }
        //======================================================================
        unsafe public int ExternalClock
        {
            get { return _external_clock;}
            set
            {
                _external_clock = value;
                if (_dev != null)
                {
                    _dev.ExternalClock = _external_clock;
                }
            }
        }
        //======================================================================
        public int LNAgain
        {
            get
            {
                if (_dev != null)
                {
                    return _dev.LNAgain;
                }
                else
                    return 0;
            }
            set
            {
                _LNA_Gain = value;

                if (_dev != null)
                {
                    _dev.LNAgain = value;
                }
            }
        }
        //======================================================================
        public int VGAgain
        {
            get
            {
                if (_dev != null)
                {
                    return _dev.VGAgain;
                }
                else
                    return 0;
            }
            set
            {
                _VGA_Gain = value;

                if (_dev != null)
                {
                    _dev.VGAgain = _VGA_Gain;
                }
            }
        }
        //======================================================================
        public long MinimumTunableFrequency
        {
            get
            {
                if ((_sampling_mode == 2) || (_sampling_mode == 3))
                {
                    return (long)_sampleRate / 2;
                    //return 0;
                }
                return 50000000;
            }
        }
        //======================================================================
        public long MaximumTunableFrequency
        {
            get
            {
                if ((_sampling_mode == 2) || (_sampling_mode == 3))
                {
                    return (long)_sampleRate / 2;
                    //return 0;
                }
                return 6000000000;
            }
        }
        //======================================================================
        private void SamplesAvailableEvent(object sender, SamplesAvailableEventArgs e)
        {
            _callback(this, e.Buffer, e.Length);
        }
        //======================================================================
        public bool IsOpen
        {
            get
            {
                return (_dev != null);
            }
        }
        //======================================================================

        public bool IsStreaming
        {
            get
            {
                if (_dev != null)
                {
                    return _dev.IsStreaming;
                }
                return false;
            }
        }
        //======================================================================
        public void GetMainForm()
        {
            if (_main == null)
            {
                if (Application.OpenForms.Count > 0)
                {
                    _main = (SDRSharp.MainForm)Application.OpenForms[0];
                }
            }
        }
        //======================================================================
        public void UpdateMainForm()
        {
            if (_main == null)
            {
                if (Application.OpenForms.Count > 0)
                {
                    _main = (SDRSharp.MainForm)Application.OpenForms[0];
                }
            }
            if (_main != null)
            {
                if ((_sampling_mode == 2) || (_sampling_mode == 3))
                {
                    _can_tune = true;
                    _main.CenterFrequency = (long)_sampleRate / 2;
                    _can_tune = false;
                }
                else
                {
                    _can_tune = true;
                    _main.CenterFrequency = (long)_frequency;
                }
                Console.WriteLine("_main.CenterFrequency = " + _main.CenterFrequency);
            }
        }
        //======================================================================
        public void SetUserGPO(Int32 value)
        {
            if (_dev != null)
            {
                _dev.UserGPO = value;
            }
        }
        //======================================================================
    }
}
