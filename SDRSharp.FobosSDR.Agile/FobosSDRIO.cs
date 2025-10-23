//==============================================================================
//       _____     __           _______
//      /  __  \  /_/          /  ____/                                __
//     /  /_ / / _   ____     / /__  __  __   ____    ____    ____   _/ /_
//    /    __ / / / /  _  \  / ___/  \ \/ /  / __ \  / __ \  / ___\ /  _/
//   /  /\ \   / / /  /_/ / / /___   /   /  / /_/ / /  ___/ / /     / /_
//  /_ /  \_\ /_/  \__   / /______/ /_/\_\ / ____/  \____/ /_/      \___/
//               /______/                 /_/             
//  Fobos SDR (agile) special API library
//  C# .Net API wrapper for SDR# plugin
//  Copyright (C) Rig Expert Ukraine Ltd.
//  2025.01.19
//==============================================================================
using System;
using System.Diagnostics;
using System.Windows.Forms;
using SDRSharp;
using SDRSharp.Common;
using SDRSharp.Radio;
//==============================================================================
namespace SDRSharp.FobosSDR.Agile
{
    public unsafe class FobosSDRIO : 
        IFrontendController, 
        IIQStreamController, 
        IFloatingConfigDialogProvider, 
        IDisposable, 
        ITunableSource
    {
        private readonly FobosSDRControllerDialog _gui = null;
        private int _index = -1;
        private FobosSDRDevice _dev;
        private Radio.SamplesAvailableDelegate _callback;
        private double _frequency = 100000000;
        private double _sampleRate = 25000000.0;
        private double _abw = 0.9;
        private int _sampling_mode = 0;
        private bool _can_tune = true;
        private int _external_clock = 0;
        private int _LNA_Gain = 0;
        private int _VGA_Gain = 0;
        public SDRSharp.MainForm _main;
        public string ApiInfo;
        public string BoardInfo;
        public string Serial;
        public string[] Devices;
        //======================================================================
        public FobosSDRIO()
        {
            //ConsoleHelper.CreateConsole();
            Enumerate();
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
        public int Enumerate()
        {
            int count = 0;
            byte[] buf = new byte[256];
            unsafe
            {
                fixed (byte* p_buf = &buf[0])
                {
                    count = NativeMethods.fobos_sdr_list_devices(p_buf);
                }
            }
            string serials_str = System.Text.Encoding.UTF8.GetString(buf).TrimEnd('\0');
            var serials_list = serials_str.Split();
            Array.Resize(ref Devices, count);
            for (var i = 0; i < count; i++)
            {
                if (i == this._index)
                {
                    Devices[i] = "Fobos SDR #" + Convert.ToString(i) + " " + this.Serial;
                }
                else
                {
                    Devices[i] = "Fobos SDR #" + Convert.ToString(i) + " " + serials_list[i];
                }
            }
            return count;
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
                    _dev.AutoBandWidth = _abw;
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
            if (_dev == null)
            {
                if (Enumerate() > 0)
                {
                    try
                    {
                        SelectDevice(0);
                        _gui.LoadSettings();
                        _gui.SelectDevice();
                        return;
                    }
                    catch (ApplicationException)
                    {
                        MessageBox.Show("FobosSDR: Could not open device" + _index);
                    }
                }
            }
        }
        //======================================================================
        public void ShowSettingGUI(IWin32Window parent)
        {
            _main = (SDRSharp.MainForm)parent;
            Console.WriteLine("parent: "+parent.ToString());
            UpdateMainForm();
            Open();
            _gui.Show();
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
        public double AutoBandWidth
        {
            get
            {
                return _dev == null ? 0.0 : _dev.AutoBandWidth;
            }
            set
            {
                _abw = value;
                if (_dev != null)
                {
                    _dev.AutoBandWidth = value;
                }
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
                }
                return 6200000000;
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
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    try
                    {
                        var form = Application.OpenForms[i];
                        if (form is SDRSharp.MainForm)
                        {
                            _main = (SDRSharp.MainForm)form;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.ToString());
                    }
                }
            }
        }
        //======================================================================
        public void UpdateMainForm()
        {
            GetMainForm();
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
