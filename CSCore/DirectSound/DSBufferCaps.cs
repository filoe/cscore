using System.Runtime.InteropServices;

namespace CSCore.DirectSound
{
    //http://msdn.microsoft.com/en-us/library/ms897756.aspx
    /// <summary>
    /// Describes the capabilities of a DirectSound buffer object. It is used by the <see cref="DirectSoundBuffer.BufferCaps"/> property. 
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.dsbcaps%28v=vs.85%29.aspx"/>.</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct DSBufferCaps
    {
        /// <summary>
        /// Size of the structure, in bytes. This member must be initialized before the structure is used.
        /// </summary>
        /// <remarks>Use the <see cref="Marshal.SizeOf(object)"/> method to determine the size.</remarks>
        public int Size;
        /// <summary>
        /// Flags that specify buffer-object capabilities.
        /// </summary>
        public DSBufferCapsFlags Flags;
        /// <summary>
        /// Size of this buffer, in bytes.
        /// </summary>
        public int BufferBytes;
        /// <summary>
        /// The rate, in kilobytes per second, at which data is transferred to the buffer memory when <see cref="DirectSoundBuffer.Unlock"/> is called. High-performance applications can use this value to determine the time required for <see cref="DirectSoundBuffer.Unlock"/> to execute. For software buffers located in system memory, the rate will be very high because no processing is required. For hardware buffers, the rate might be slower because the buffer might have to be downloaded to the sound card, which might have a limited transfer rate.
        /// </summary>
        public int UnlockTransferRate;
        /// <summary>
        /// The processing overhead as a percentage of main processor cycles needed to mix this sound buffer. For hardware buffers, this member will be zero because the mixing is performed by the sound device. For software buffers, this member depends on the buffer format and the speed of the system processor.
        /// </summary>
        public int PlayCpuOverhead;
    }
}