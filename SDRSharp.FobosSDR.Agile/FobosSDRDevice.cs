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
using SDRSharp.Radio;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
//==============================================================================
namespace SDRSharp.FobosSDR.Agile
{
    public unsafe sealed class FobosSDRDevice : IDisposable
    {
        public const double DefaultFrequency = 100000000.0;
        public const double MinFrequency =   50000000.0;
        public const double MaxFrequency = 6000000000.0;

        public const double DefaultSamplerate = 25000000.0;

        private readonly uint _index;
        private IntPtr _dev;
        private bool _isStreaming;
        private double _frequency = DefaultFrequency;
        private double _sampleRate = DefaultSamplerate;
        private double _abw = 0.9;
        private int _sampling_mode = 0;
        private double _before_direct_sampling_frequency = 0.0;
        private int _external_clock = 0;
        private int _LNA_Gain = 0;
        private int _VGA_Gain = 0;
        private int _UserGPO = 0;

        private GCHandle _gcHandle;
        private Thread _worker;
        private readonly SamplesAvailableEventArgs _eventArgs = new SamplesAvailableEventArgs();

        public string lib_version;
        public string drv_version;
        public string hw_revision;
        public string fw_version;
        public string serial;
        //======================================================================
        public FobosSDRDevice(uint index)
        {
            _index = index;
            _frequency = DefaultFrequency;
            _before_direct_sampling_frequency = _frequency;
            var r = NativeMethods.fobos_sdr_open(out _dev, _index);
            if (r != 0)
            {
                throw new ApplicationException("Cannot open Fobos SDR device. Is the device locked somewhere?");
            }
            byte[] buf0 = new byte[128];
            byte[] buf1 = new byte[128];
            byte[] buf2 = new byte[128];
            byte[] buf3 = new byte[128];
            byte[] buf4 = new byte[128];
            unsafe
            {
                fixed (byte* p_buf0 = &buf0[0]) fixed (byte* p_buf1 = &buf1[0])
                {
                    NativeMethods.fobos_sdr_get_api_info(p_buf0, p_buf1);
                }
            }
            lib_version = System.Text.Encoding.UTF8.GetString(buf0).TrimEnd('\0');
            drv_version = System.Text.Encoding.UTF8.GetString(buf1).TrimEnd('\0');
            Array.Clear(buf0, 0, buf0.Length);
            Array.Clear(buf1, 0, buf1.Length);
            unsafe
            {
                fixed(byte* p_buf0 = &buf0[0]) fixed(byte* p_buf1 = &buf1[0]) fixed(byte* p_buf2 = &buf2[0]) fixed(byte* p_buf3 = &buf3[0]) fixed(byte* p_buf4 = &buf4[0])
                {
                    NativeMethods.fobos_sdr_get_board_info(_dev, p_buf0, p_buf1, p_buf2, p_buf3, p_buf4);
                }
            }
            hw_revision = System.Text.Encoding.UTF8.GetString(buf0).TrimEnd('\0');
            fw_version = System.Text.Encoding.UTF8.GetString(buf1).TrimEnd('\0');

            serial = System.Text.Encoding.UTF8.GetString(buf4).TrimEnd('\0');

            Frequency = DefaultFrequency;
            Samplerate = DefaultSamplerate;

            _gcHandle = GCHandle.Alloc(this);
        }
        //======================================================================
        ~FobosSDRDevice()
        {
            Dispose();
        }
        //======================================================================
        public void Dispose()
        {
            Stop();
            NativeMethods.fobos_sdr_close(_dev);
            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }
            _dev = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }
        //======================================================================
        public void Start()
        {
            if (_worker != null)
            {
                return;
            }
            _worker = new Thread(StreamProc);
            _worker.Priority = ThreadPriority.Highest;
            _worker.Start();
        }
        //======================================================================
        public void Stop()
        {
            if (_worker == null)
            {
                return;
            }
            NativeMethods.fobos_sdr_cancel_async(_dev);
            if (_worker.ThreadState == ThreadState.Running)
            {
                _worker.Join();
            }
            _worker = null;
        }
        //======================================================================
        public double Samplerate
        {
            get
            {
                return _sampleRate;
            }
            set
            {
                _sampleRate = value;
                if (_dev != IntPtr.Zero)
                {
                    var r = NativeMethods.fobos_sdr_set_samplerate(_dev, _sampleRate);
                    if (r != 0)
                    {
                        throw new ApplicationException("Cannot access Fobos SDR device");
                    }
                    
                }
            }
        }
        //======================================================================
        public double AutoBandWidth
        {
            get
            {
                return _abw;
            }
            set
            {
                _abw = value;
                if (_dev != IntPtr.Zero)
                {
                    var r = NativeMethods.fobos_sdr_set_auto_bandwidth(_dev, _abw);
                    if (r != 0)
                    {
                        throw new ApplicationException("Cannot access Fobos SDR device");
                    }

                }
            }
        }
        //======================================================================
        public double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                if ((_sampling_mode == 0) || (_sampling_mode == 1))
                {
                    if ((value < MinFrequency) || (value > MaxFrequency))
                    {
                        return;
                    }
                    int result = 0;
                    _frequency = value;
                    if (_dev != IntPtr.Zero)
                    {
                        unsafe
                        {
                            result = NativeMethods.fobos_sdr_set_frequency(_dev, _frequency);
                        }
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
                int result = 0;
                int direct = 1;
                if (_sampling_mode != value)
                {
                    if (_dev != IntPtr.Zero)
                    {
                        if (value == 0) direct = 0;
                        result = NativeMethods.fobos_sdr_set_direct_sampling(_dev, direct);
                    }
                }
                if (result == 0)
                {
                    _sampling_mode = value;
                    if ((_sampling_mode == 2) || (_sampling_mode == 3))
                    {
                        _before_direct_sampling_frequency = _frequency;
                        _frequency = _sampleRate / 2;
                    }
                    else
                    {
                        _frequency = _before_direct_sampling_frequency;
                    }

                }
            }
        }
        //======================================================================
        unsafe public int ExternalClock
        {
            get { return _external_clock ; }
            set
            {
                _external_clock = value;
                if (_dev != IntPtr.Zero)
                {
                    NativeMethods.fobos_sdr_set_clk_source(_dev, _external_clock);
                }
            }
        }
        //======================================================================
        unsafe public int LNAgain
        {
            get { return _LNA_Gain; }
            set
            {
                _LNA_Gain = value;
                if (_dev != IntPtr.Zero)
                {
                    NativeMethods.fobos_sdr_set_lna_gain(_dev, _LNA_Gain);
                }
            }
        }
        //======================================================================
        unsafe public int VGAgain
        {
            get { return _VGA_Gain; }
            set
            {
                _VGA_Gain = value;
                if (_dev != IntPtr.Zero)
                {
                    NativeMethods.fobos_sdr_set_vga_gain(_dev, _VGA_Gain);
                }
            }
        }
        //======================================================================
        unsafe public int UserGPO
        {
            get { return _UserGPO; }
            set
            {
                _UserGPO = value;
                if (_dev != IntPtr.Zero)
                {
                    NativeMethods.fobos_sdr_set_user_gpo(_dev, (uint)_UserGPO);
                }
            }
        }
        //======================================================================
        public bool IsStreaming
        {
            get { return _worker != null; }
        }
        //======================================================================
        private void StreamProc()
        {
            _isStreaming = true;
            double latency = 0.02;
            double p = Math.Log(_sampleRate * latency, 2.0);
            double r = Math.Round(p);
            double l = Math.Pow(2, r);
            uint buffer_length = (uint)l;
            NativeMethods.fobos_sdr_read_async(_dev, callback, (IntPtr)_gcHandle, 32, buffer_length);
            _isStreaming = false;
        }
        //======================================================================
        public event SamplesAvailableDelegate SamplesAvailable;
        private void ComplexSamplesAvailable(Complex* buffer, int length)
        {
            if (SamplesAvailable != null)
            {
                if (_sampling_mode == 2)
                {
                    int count = length / 4;

                    for (int i = 0; i < count; i++)
                    {
                        buffer[i * 4 + 0].Imag = 0.0f;

                        buffer[i * 4 + 1].Real = - buffer[i * 4 + 1].Real;
                        buffer[i * 4 + 1].Imag = 0.0f;

                        buffer[i * 4 + 2].Imag = 0.0f;

                        buffer[i * 4 + 3].Real = -buffer[i * 4 + 3].Real;
                        buffer[i * 4 + 3].Imag = 0.0f;
                    }
                }
                if (_sampling_mode == 3)
                {
                    int count = length / 4;
                    for (int i = 0; i < count; i++)
                    {
                        buffer[i * 4 + 0].Real = buffer[i * 4 + 0].Imag;
                        buffer[i * 4 + 0].Imag = 0.0f;

                        buffer[i * 4 + 1].Real = -buffer[i * 4 + 1].Imag;
                        buffer[i * 4 + 1].Imag = 0.0f;

                        buffer[i * 4 + 2].Real = buffer[i * 4 + 2].Imag;
                        buffer[i * 4 + 2].Imag = 0.0f;

                        buffer[i * 4 + 3].Real = -buffer[i * 4 + 3].Imag;
                        buffer[i * 4 + 3].Imag = 0.0f;
                    }
                }
                _eventArgs.Buffer = buffer;
                _eventArgs.Length = length;
                SamplesAvailable(this, _eventArgs);
            }
        }
        //======================================================================
        private static void callback(float* buf, uint buf_length, IntPtr sender, IntPtr user)
        {
            var gcHandle = GCHandle.FromIntPtr(user);
            if (!gcHandle.IsAllocated)
            {
                return;
            }
            var instance = (FobosSDRDevice)gcHandle.Target;
            Complex* _iqPtr = (Complex*)buf;

            instance.ComplexSamplesAvailable(_iqPtr, (int)buf_length);
        }
        //======================================================================
        public uint Index { get { return _index;  } }
        //======================================================================
    }

    public delegate void SamplesAvailableDelegate(object sender, SamplesAvailableEventArgs e);

    public unsafe sealed class SamplesAvailableEventArgs : EventArgs
    {
        public int Length { get; set; }
        public Complex* Buffer { get; set; }
    }
}
//==============================================================================