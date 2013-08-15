using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [Guid("F294ACFC-3146-4483-A7BF-ADDCA7C260E2")]
    public class AudioRenderClient : ComObject
    {
        private static readonly Guid IID_IAudioRenderClient = new Guid("F294ACFC-3146-4483-A7BF-ADDCA7C260E2");
        private const string c = "IAudioRenderClient";

        public static AudioRenderClient FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new AudioRenderClient(audioClient.GetService(IID_IAudioRenderClient));
        }

        public AudioRenderClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Retrieves a pointer to the next available space in the rendering endpoint buffer into
        /// which the caller can write a data packet.
        /// </summary>
        /// <returns>Buffer</returns>
        public IntPtr GetBuffer(int numFramesRequested)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetBuffer(numFramesRequested, out ptr), c, "GetBuffer");
            return ptr;
        }

        /// <summary>
        /// Retrieves a pointer to the next available space in the rendering endpoint buffer into
        /// which the caller can write a data packet.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetBuffer(int numFramesRequested, out IntPtr buffer)
        {
            fixed (void* pbuffer = &buffer)
            {
                return InteropCalls.CallI(_basePtr, unchecked(numFramesRequested), pbuffer, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        /// <summary>
        /// The ReleaseBuffer method releases the buffer space acquired in the previous call to the
        /// IAudioRenderClient::GetBuffer method.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ReleaseBufferInternal(int numFramesWritten, AudioClientBufferFlags flags)
        {
            return InteropCalls.CallI(_basePtr, unchecked(numFramesWritten), unchecked(flags), ((void**)(*(void**)_basePtr))[4]);
        }

        /// <summary>
        /// The ReleaseBuffer method releases the buffer space acquired in the previous call to the
        /// IAudioRenderClient::GetBuffer method.
        /// </summary>
        public void ReleaseBuffer(int numFramesWritter, AudioClientBufferFlags flags)
        {
            CoreAudioAPIException.Try(ReleaseBufferInternal(numFramesWritter, flags), c, "ReleaseBuffer");
        }
    }

    /// <summary>
    /// Defines flags that indicate the status of an audio endpoint buffer.
    /// </summary>
    [Flags]
    public enum AudioClientBufferFlags
    {
        None = 0x0,

        /// <summary>
        /// The data in the packet is not correlated with the previous packet's device position;
        /// this is possibly due to a stream state transition or timing glitch.
        /// </summary>
        DataDiscontinuity = 0x1,

        /// <summary>
        /// Treat all of the data in the packet as silence and ignore the actual data values.
        /// </summary>
        Silent = 0x2,

        /// <summary>
        /// The time at which the device's stream position was recorded is uncertain. Thus, the
        /// client might be unable to accurately set the time stamp for the current data packet.
        /// </summary>
        TimestampError = 0x3
    }
}