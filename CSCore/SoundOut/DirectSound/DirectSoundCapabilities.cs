using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DirectSoundCapabilities
    {
        public int Size;
        public DSCapabilitiesFlags Flags;
        public int MinSecondarySampleRate;
        public int MaxSecondarySampleRate;
        public int PrimaryBuffers;
        public int MaxHardwareMixingAllBuffers;
        public int MaxHardwareMixingStaticBuffers;
        public int MaxHardwareMixingStreamingBuffers;
        public int FreeHardwareMixingAllBuffers;
        public int FreeHardwareMixingStaticBuffers;
        public int FreeHardwareMixingStreamingBuffers;
        public int MaxHardware3DAllBuffers;
        public int MaxHardware3DStaticBuffers;
        public int MaxHardware3DStreamingBuffers;
        public int FreeHardware3DAllBuffers;
        public int FreeHardware3DStaticBuffers;
        public int FreeHardware3DStreamingBuffers;
        public int TotalHardwareMemBytes;
        public int FreeHardwareMemBytes;
        public int MaxContigFreeHardwareMemBytes;
        public int UnlockTransferRateHardwareBuffers;
        public int PlayCpuOverheadSwBuffers;
        public int Reserved1;
        public int Reserved2;
    }
}