using System;
using CSCore.SoundOut;
using CSCore.SoundOut.MMInterop;
using System.Runtime.InteropServices;

namespace CSCore.SoundIn
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct WaveInCaps
    {
        public readonly short wMid;
        public readonly short wPid;
        public readonly int DriverVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public readonly string Name;

        public readonly MmDeviceFormats Formats;
        public readonly short Channels;
        public readonly short wReserved1;

        public WaveFormat[] GetSupportedFormats()
        {
            return SoundOut.MMInterop.Utils.SupportedFormatsFlagsToWaveFormats(Formats);
        }
    }
}