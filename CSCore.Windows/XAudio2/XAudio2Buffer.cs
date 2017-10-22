using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Represents an audio data buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XAudio2Buffer : IDisposable
    {
        /// <summary>
        ///     Maximum non-infinite LoopCount.
        /// </summary>
        public const int MaxLoopCount = 254;

        /// <summary>
        ///     Infinite Loop.
        /// </summary>
        public const int LoopInfinite = 255;

        /// <summary>
        ///     MaxBufferBytes. See <see cref="AudioBytes" />.
        /// </summary>
        public const int MaxBufferBytes = unchecked((int) 2147483648);

        /// <summary>
        ///     Flags that provide additional information about the audio buffer.
        ///     May be <see cref="XAudio2BufferFlags.None" /> or <see cref="XAudio2BufferFlags.EndOfStream" />.
        /// </summary>
        public XAudio2BufferFlags Flags;

        /// <summary>
        ///     Size of the audio data, in bytes. Must be no larger than <see cref="MaxBufferBytes" /> for PCM data.
        ///     For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_buffer(v=vs.85).aspx.
        /// </summary>
        public int AudioBytes;

        /// <summary>
        ///     Pointer to the audio data.
        /// </summary>
        public IntPtr AudioDataPtr;

        /// <summary>
        ///     First sample in the buffer that should be played.
        ///     For XMA buffers this value must be a multiple of 128 samples.
        /// </summary>
        public int PlayBegin;

        /// <summary>
        ///     Length of the region to be played, in samples.
        ///     A value of zero means to play the entire buffer, and, in this case, <see cref="PlayBegin" /> must be zero as well.
        ///     For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_buffer(v=vs.85).aspx.
        /// </summary>
        public int PlayLength;

        /// <summary>
        ///     First sample of the region to be looped. The value of <see cref="LoopBegin" /> must be less than
        ///     <see cref="PlayBegin" /> + <see cref="PlayLength" />.
        ///     <see cref="LoopBegin" /> can be less than <see cref="PlayBegin" />. <see cref="LoopBegin" /> must be 0 if
        ///     <see cref="LoopCount" /> is 0.
        /// </summary>
        public int LoopBegin;

        /// <summary>
        ///     Length of the loop region, in samples.
        ///     The value of <see cref="LoopBegin" /> + <see cref="LoopLength" /> must be greater than <see cref="PlayBegin" /> and
        ///     less than <see cref="PlayBegin" /> + <see cref="PlayLength" />.
        ///     <see cref="LoopLength" /> must be zero if <see cref="LoopLength" /> is 0.
        ///     If <see cref="LoopLength" /> is not 0 then a loop length of zero indicates the entire sample should be looped.
        /// </summary>
        public int LoopLength;

        /// <summary>
        ///     Number of times to loop through the loop region.
        ///     This value can be between 0 and <see cref="MaxLoopCount" />.
        ///     If LoopCount is zero no looping is performed and <see cref="LoopBegin" /> and <see cref="LoopLength" /> must be 0.
        ///     To loop forever, set <see cref="LoopCount" /> to <see cref="LoopInfinite" />.
        /// </summary>
        public int LoopCount;

        /// <summary>
        ///     Context value to be passed back in callbacks to the client. This may be <see cref="IntPtr.Zero" />.
        /// </summary>
        public IntPtr ContextPtr;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2Buffer" /> structure.
        /// </summary>
        /// <param name="bufferSize"></param>
        public XAudio2Buffer(int bufferSize)
        {
            AudioDataPtr = Marshal.AllocHGlobal(bufferSize); //todo: may need to align buffer?
            //AudioDataPtr = AllocateMemory(bufferSize);
            AudioBytes = bufferSize;

            Flags = XAudio2BufferFlags.None;
            ContextPtr = IntPtr.Zero;
            LoopBegin = 0;
            LoopLength = 0;
            LoopCount = 0;
            PlayBegin = 0;
            PlayLength = 0;
        }

        /// <summary>
        ///     Returns a <see cref="UnmanagedMemoryStream" /> instance for the underlying <see cref="AudioDataPtr" />.
        /// </summary>
        /// <remarks>Call </remarks>
        /// <returns>
        ///     <see cref="UnmanagedMemoryStream" />
        /// </returns>
        public unsafe Stream GetStream()
        {
            return new UnmanagedMemoryStream((byte*) AudioDataPtr.ToPointer(), AudioBytes, AudioBytes, FileAccess.Write);
        }

        /// <summary>
        ///     Frees the allocated memory.
        /// </summary>
        [Obsolete("Use the Dispose method instead.")]
        public void Free()
        {
            Free();
        }

        /// <summary>
        /// Frees the allocated memory.
        /// </summary>
        public void Dispose()
        {
            if (AudioDataPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(AudioDataPtr);
                AudioDataPtr = IntPtr.Zero;
                AudioBytes = 0;
            }
        }
    }
}