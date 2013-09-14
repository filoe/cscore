using System;
using System.Runtime.InteropServices;

namespace CSCore.ACM
{
    internal static class AcmInterop
    {
        private const string msacm = "msacm32.dll";

        [DllImport(msacm)]
        public static extern MmResult acmStreamOpen(
            out IntPtr acmStreamHandle,
            IntPtr driver,
            [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "CSCore.Win32.WaveFormatMarshaler")] WaveFormat sourceFormat,
            [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "CSCore.Win32.WaveFormatMarshaler")] WaveFormat destinationFormat,
            [In]AcmWaveFilter waveFilter,
            IntPtr callback,
            IntPtr instance,
            AcmStreamOpenFlags flags);

        [DllImport(msacm)]
        public static extern MmResult acmStreamClose(IntPtr acmStreamHandle, int reserved = 0);

        [DllImport(msacm)]
        public static extern MmResult acmStreamPrepareHeader(
            IntPtr acmStreamHandle,
            [In, Out]NativeAcmHeader header,
            int reserved = 0);

        [DllImport(msacm)]
        public static extern MmResult acmStreamUnprepareHeader(
            IntPtr acmStreamHandle,
            [In, Out]NativeAcmHeader header,
            int reserved = 0);

        [DllImport(msacm)]
        public static extern MmResult acmStreamConvert(
            IntPtr acmStreamHandle,
            [In, Out]NativeAcmHeader header,
            AcmConvertFlags flags);

        [DllImport(msacm)]
        public static extern MmResult acmStreamSize(
            IntPtr acmStreamHandle,
            int inputCount,
            out int outputCount,
            AcmStreamSizeFlags flags);

        [DllImport(msacm)]
        public static extern MmResult acmFormatSuggest(
            IntPtr acmStreamHandle,
            [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "CSCore.Win32.WaveFormatMarshaler")]WaveFormat sourceFormat,
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "CSCore.Win32.WaveFormatMarshaler")]WaveFormat destinationFormat,
            int cbDestinationFormat,
            AcmFormatSuggestFlags flags);

        [DllImport(msacm)]
        public static extern MmResult acmDriverEnum(
            AcmDriverDetailsSupport callback,
            IntPtr instance, 
            AcmDriverEnumFlags flags);

        [DllImport(msacm)]
        public static extern MmResult acmDriverDetails(
            IntPtr acmDriverHandle, 
            ref AcmDriverDetails details, 
            IntPtr reserved);
    }
}