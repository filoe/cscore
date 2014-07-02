using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    [Guid("F294ACFC-3146-4483-A7BF-ADDCA7C260E2")]
    public class AudioRenderClient : ComObject
    {
// ReSharper disable once InconsistentNaming
        private const string c = "IAudioRenderClient";
        private static readonly Guid IID_IAudioRenderClient = new Guid("F294ACFC-3146-4483-A7BF-ADDCA7C260E2");

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioRenderClient" /> class.
        /// </summary>
        /// <param name="ptr">Pointer to the <see cref="AudioRenderClient" /> instance.</param>
        public AudioRenderClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="AudioRenderClient" /> class. This is done by calling the
        ///     <see cref="AudioClient.GetService" /> method of the <see cref="AudioClient" /> class.
        /// </summary>
        /// <param name="audioClient">
        ///     <see cref="AudioClient" /> instance which should be used to create the new
        ///     <see cref="AudioRenderClient" /> instance.
        /// </param>
        /// <returns>A new instance of the <see cref="AudioRenderClient" /> class.</returns>
        public static AudioRenderClient FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new AudioRenderClient(audioClient.GetService(IID_IAudioRenderClient));
        }

        /// <summary>
        ///     Retrieves a pointer to the next available space in the rendering endpoint buffer into
        ///     which the caller can write a data packet.
        /// </summary>
        /// <returns>Buffer</returns>
        public IntPtr GetBuffer(int numFramesRequested)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetBufferNative(numFramesRequested, out ptr), c, "GetBuffer");
            return ptr;
        }

        /// <summary>
        ///     Retrieves a pointer to the next available space in the rendering endpoint buffer into
        ///     which the caller can write a data packet.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferNative(int numFramesRequested, out IntPtr buffer)
        {
            fixed (void* pbuffer = &buffer)
            {
                return InteropCalls.CallI(UnsafeBasePtr, unchecked(numFramesRequested), pbuffer,
                    ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        ///     The ReleaseBuffer method releases the buffer space acquired in the previous call to the
        ///     IAudioRenderClient::GetBuffer method.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ReleaseBufferNative(int numFramesWritten, AudioClientBufferFlags flags)
        {
            return InteropCalls.CallI(UnsafeBasePtr, unchecked(numFramesWritten), unchecked(flags),
                ((void**) (*(void**) UnsafeBasePtr))[4]);
        }

        /// <summary>
        ///     The ReleaseBuffer method releases the buffer space acquired in the previous call to the
        ///     IAudioRenderClient::GetBuffer method.
        /// </summary>
        public void ReleaseBuffer(int numFramesWritten, AudioClientBufferFlags flags)
        {
            CoreAudioAPIException.Try(ReleaseBufferNative(numFramesWritten, flags), c, "ReleaseBuffer");
        }
    }
}