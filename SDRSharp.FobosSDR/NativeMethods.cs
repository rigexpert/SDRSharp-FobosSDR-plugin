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
using System.Runtime.InteropServices;

namespace SDRSharp.FobosSDR
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void fobos_rx_cb_t(float* buf, int buf_length, IntPtr ctx);

    public class NativeMethods
    {

        private const string library_name = "fobos";

        [DllImport(library_name, EntryPoint = "fobos_rx_get_api_info", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int fobos_rx_get_api_info(byte * lib_version, byte* drv_version);


        [DllImport(library_name, EntryPoint = "fobos_rx_get_device_count", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_get_device_count();

        [DllImport(library_name, EntryPoint = "fobos_rx_list_devices", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int fobos_rx_list_devices(byte* serials);

        [DllImport(library_name, EntryPoint = "fobos_rx_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_open(out IntPtr dev, uint index);

        [DllImport(library_name, EntryPoint = "fobos_rx_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_close(IntPtr dev);

        [DllImport(library_name, EntryPoint = "fobos_rx_get_board_info", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int fobos_rx_get_board_info(IntPtr dev, byte * hw_revision, byte * fw_version, byte* manufacturer, byte* product, byte*  serial);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_frequency", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern int fobos_rx_set_frequency(IntPtr dev, double freq, double* actual);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_direct_sampling", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_set_direct_sampling(IntPtr dev, int value);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_lna_gain", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_set_lna_gain(IntPtr dev, int value);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_vga_gain", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_set_vga_gain(IntPtr dev, int value);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_samplerate", CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern uint fobos_rx_set_samplerate(IntPtr dev, double rate, double* actual);

        [DllImport(library_name, EntryPoint = "fobos_rx_read_async", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_read_async(IntPtr dev, fobos_rx_cb_t cb, IntPtr ctx, UInt32 buf_count, UInt32 buf_length);

        [DllImport(library_name, EntryPoint = "fobos_rx_cancel_async", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_cancel_async(IntPtr dev);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_user_gpo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_set_user_gpo(IntPtr dev, uint value);

        [DllImport(library_name, EntryPoint = "fobos_rx_set_clk_source", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fobos_rx_set_clk_source(IntPtr dev, int value);
    }
}
