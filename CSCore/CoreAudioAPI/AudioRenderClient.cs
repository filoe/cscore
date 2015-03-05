using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     The <see cref="AudioRenderClient" /> class enables a client to write output data to a rendering endpoint buffer.
    /// </summary>
    /// <remarks>
    ///     For more information, see
    ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd368242(v=vs.85).aspx" />.
    /// </remarks>
    [Guid("F294ACFC-3146-4483-A7BF-ADDCA7C260E2")]
    public class AudioRenderClient : ComObject
    {
        private const string InterfaceName = "IAudioRenderClient";
        // ReSharper disable once InconsistentNaming
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
        ///     The <see cref="AudioClient" /> instance which should be used to create the new
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
        /// <param name="numFramesRequested">
        ///     The number of audio frames in the data packet that the caller plans to write to the requested space in the buffer.
        ///     If the call succeeds, the size of the buffer area pointed to by return value matches the size specified in
        ///     <paramref name="numFramesRequested" />.
        /// </param>
        /// <returns>
        ///     A pointer variable into which the method writes the starting address of the buffer area into which the caller
        ///     will write the data packet.
        /// </returns>
        public IntPtr GetBuffer(int numFramesRequested)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetBufferNative(numFramesRequested, out ptr), InterfaceName, "GetBuffer");
            return ptr;
        }

        /// <summary>
        ///     Retrieves a pointer to the next available space in the rendering endpoint buffer into
        ///     which the caller can write a data packet.
        /// </summary>
        /// <param name="numFramesRequested">
        ///     The number of audio frames in the data packet that the caller plans to write to the requested space in the buffer.
        ///     If the call succeeds, the size of the buffer area pointed to by <paramref name="buffer" /> matches the size
        ///     specified in <paramref name="numFramesRequested" />.
        /// </param>
        /// <param name="buffer">
        ///     Pointer variable into which the method writes the starting address of the buffer area into which
        ///     the caller will write the data packet.
        /// </param>
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
        ///     Releases the buffer space acquired in the previous call to the
        ///     <see cref="GetBuffer" /> method.
        /// </summary>
        /// <param name="numFramesWritten">
        ///     The number of audio frames written by the client to the data packet.
        ///     The value of this parameter must be less than or equal to the size of the data packet, as specified in the
        ///     <c>numFramesRequested</c> parameter passed to the <see cref="GetBuffer" /> method.
        /// </param>
        /// <param name="flags">The buffer-configuration flags.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ReleaseBufferNative(int numFramesWritten, AudioClientBufferFlags flags)
        {
            return InteropCalls.CallI(UnsafeBasePtr, unchecked(numFramesWritten), unchecked(flags),
                ((void**) (*(void**) UnsafeBasePtr))[4]);
        }

        /// <summary>
        ///     Releases the buffer space acquired in the previous call to the
        ///     <see cref="GetBuffer" /> method.
        /// </summary>
        /// <param name="numFramesWritten">
        ///     The number of audio frames written by the client to the data packet.
        ///     The value of this parameter must be less than or equal to the size of the data packet, as specified in the
        ///     <c>numFramesRequested</c> parameter passed to the <see cref="GetBuffer" /> method.
        /// </param>
        /// <param name="flags">The buffer-configuration flags.</param>
        public void ReleaseBuffer(int numFramesWritten, AudioClientBufferFlags flags)
        {
            CoreAudioAPIException.Try(ReleaseBufferNative(numFramesWritten, flags), InterfaceName, "ReleaseBuffer");
        }
    }
}