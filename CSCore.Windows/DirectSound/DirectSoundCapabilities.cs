using System.Runtime.InteropServices;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Describes the capabilities of a device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DirectSoundCapabilities
    {
        /// <summary>
        /// Size of the structure, in bytes. This member must be initialized before the structure is used.
        /// </summary>
        public int Size;
        /// <summary>
        /// Flags describing device capabilities.
        /// </summary>
        public DSCapabilitiesFlags Flags;
        /// <summary>
        /// Minimum sample rate specification that is supported by this device's hardware secondary sound buffers.
        /// </summary>
        public int MinSecondarySampleRate;
        /// <summary>
        /// Maximum sample rate specification that is supported by this device's hardware secondary sound buffers.
        /// </summary>
        public int MaxSecondarySampleRate;
        /// <summary>
        /// Number of primary buffers supported. This value will always be 1.
        /// </summary>
        public int PrimaryBuffers;
        /// <summary>
        /// Number of buffers that can be mixed in hardware. This member can be less than the sum of <see cref="MaxHardwareMixingStaticBuffers"/> and <see cref="MaxHardwareMixingStreamingBuffers"/>. Resource tradeoffs frequently occur.
        /// </summary>
        public int MaxHardwareMixingAllBuffers;
        /// <summary>
        /// Maximum number of static buffers.
        /// </summary>
        public int MaxHardwareMixingStaticBuffers;
        /// <summary>
        /// Maximum number of streaming sound buffers.
        /// </summary>
        public int MaxHardwareMixingStreamingBuffers;
        /// <summary>
        /// Number of unallocated buffers. On WDM drivers, this includes <see cref="FreeHardware3DAllBuffers"/>.
        /// </summary>
        public int FreeHardwareMixingAllBuffers;
        /// <summary>
        /// Number of unallocated static buffers.
        /// </summary>
        public int FreeHardwareMixingStaticBuffers;
        /// <summary>
        /// Number of unallocated streaming buffers.
        /// </summary>
        public int FreeHardwareMixingStreamingBuffers;
        /// <summary>
        /// Maximum number of 3D buffers.
        /// </summary>
        public int MaxHardware3DAllBuffers;
        /// <summary>
        /// Maximum number of static 3D buffers.
        /// </summary>
        public int MaxHardware3DStaticBuffers;
        /// <summary>
        /// Maximum number of streaming 3D buffers.
        /// </summary>
        public int MaxHardware3DStreamingBuffers;
        /// <summary>
        /// Number of unallocated 3D buffers.
        /// </summary>
        public int FreeHardware3DAllBuffers;
        /// <summary>
        /// Number of unallocated static 3D buffers.
        /// </summary>
        public int FreeHardware3DStaticBuffers;
        /// <summary>
        /// Number of unallocated streaming 3D buffers.
        /// </summary>
        public int FreeHardware3DStreamingBuffers;
        /// <summary>
        /// Size, in bytes, of the amount of memory on the sound card that stores static sound buffers.
        /// </summary>
        public int TotalHardwareMemBytes;
        /// <summary>
        /// Size, in bytes, of the free memory on the sound card.
        /// </summary>
        public int FreeHardwareMemBytes;
        /// <summary>
        /// Size, in bytes, of the largest contiguous block of free memory on the sound card.
        /// </summary>
        public int MaxContigFreeHardwareMemBytes;
        /// <summary>
        /// The rate, in kilobytes per second, at which data can be transferred to hardware static sound buffers. This and the number of bytes transferred determines the duration of a call to the <see cref="DirectSoundBuffer.Unlock"/> method.
        /// </summary>
        public int UnlockTransferRateHardwareBuffers;
        /// <summary>
        /// The processing overhead, as a percentage of main processor cycles, needed to mix software buffers. This varies according to the bus type, the processor type, and the clock speed.
        /// </summary>
        public int PlayCpuOverheadSwBuffers;
        internal int Reserved1;
        internal int Reserved2;
    }
}