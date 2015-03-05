using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     Enables a client to read input data from a capture endpoint buffer. For more information, see
    ///     <see href="http://msdn.microsoft.com/en-us/library/dd370858(v=vs.85).aspx" />.
    /// </summary>
    [Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317")]
    public class AudioCaptureClient : ComObject
    {
        private const string InterfaceName = "IAudioCaptureClient";
        // ReSharper disable once InconsistentNaming
        private static readonly Guid IID_IAudioCaptureClient = new Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317");

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioCaptureClient" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the IAudioCaptureClient COM object.</param>
        public AudioCaptureClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets the size of the next packet in frames (the size of one frame equals the blockalign value of the waveformat).
        /// </summary>
        public int NextPacketSize
        {
            get { return GetNextPacketSize(); }
        }

        /// <summary>
        ///     Creates a new <see cref="AudioCaptureClient" /> by calling the <see cref="AudioClient.GetService" /> method of the
        ///     specified <paramref name="audioClient" />.
        /// </summary>
        /// <param name="audioClient">
        ///     The <see cref="AudioClient" /> which should be used to create the <see cref="AudioCaptureClient" />-instance
        ///     with.
        /// </param>
        /// <returns>A new instance of the <see cref="AudioCaptureClient"/> class.</returns>
        public static AudioCaptureClient FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new AudioCaptureClient(audioClient.GetService(IID_IAudioCaptureClient));
        }

        /// <summary>
        ///     Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
        ///     For more information see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370859%28v=vs.85%29.aspx" />.
        /// </summary>
        /// <param name="data">
        ///     A pointer variable into which the method writes the starting address of the next data
        ///     packet that is available for the client to read.
        /// </param>
        /// <param name="numFramesRead">
        ///     Variable into which the method writes the frame count (the number of audio frames
        ///     available in the data packet). The client should either read the entire data packet or none of it.
        /// </param>
        /// <param name="flags">Variable into which the method writes the buffer-status flags.</param>
        /// <param name="devicePosition">
        ///     Variable into which the method writes the device position of the first audio frame in the
        ///     data packet. The device position is expressed as the number of audio frames from the start of the stream.
        /// </param>
        /// <param name="qpcPosition">
        ///     Variable into which the method writes the value of the performance counter at the time that
        ///     the audio endpoint device recorded the device position of the first audio frame in the data packet.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferNative(out IntPtr data, out Int32 numFramesRead, out AudioClientBufferFlags flags,
            out Int64 devicePosition, out Int64 qpcPosition)
        {
            fixed (void* d = &data, p0 = &numFramesRead, p1 = &flags)
            {
                fixed (void* p2 = &devicePosition, p3 = &qpcPosition)
                {
                    return InteropCalls.CallI(UnsafeBasePtr, d, p0, p1, p2, p3, ((void**) (*(void**) UnsafeBasePtr))[3]);
                }
            }
        }

        /// <summary>
        ///     Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
        ///     For more information see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370859%28v=vs.85%29.aspx" />.
        /// </summary>
        /// <param name="framesRead">
        ///     Variable into which the method writes the frame count (the number of audio frames available in
        ///     the data packet). The client should either read the entire data packet or none of it.
        /// </param>
        /// <param name="flags">Variable into which the method writes the buffer-status flags.</param>
        /// <param name="devicePosition">
        ///     Variable into which the method writes the device position of the first audio frame in the
        ///     data packet. The device position is expressed as the number of audio frames from the start of the stream.
        /// </param>
        /// <param name="qpcPosition">
        ///     Variable into which the method writes the value of the performance counter at the time that
        ///     the audio endpoint device recorded the device position of the first audio frame in the data packet.
        /// </param>
        /// <returns>
        ///     Pointer to a variable which stores the starting address of the next data packet that is available for the
        ///     client to read.
        /// </returns>
        /// <remarks>
        ///     Use Marshal.Copy to convert the pointer to the buffer into an array.
        /// </remarks>
        public IntPtr GetBuffer(out int framesRead, out AudioClientBufferFlags flags, out Int64 devicePosition,
            out Int64 qpcPosition)
        {
            IntPtr data;
            int result = GetBufferNative(out data, out framesRead, out flags, out devicePosition, out qpcPosition);
            CoreAudioAPIException.Try(result, InterfaceName, "GetBuffer");
            return data;
        }

        /// <summary>
        ///     Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
        ///     For more information see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370859%28v=vs.85%29.aspx" />.
        /// </summary>
        /// <param name="framesRead">
        ///     Variable into which the method writes the frame count (the number of audio frames available in
        ///     the data packet). The client should either read the entire data packet or none of it.
        /// </param>
        /// <param name="flags">Variable into which the method writes the buffer-status flags.</param>
        /// <returns>
        ///     Pointer to a variable which stores the starting address of the next data packet that is available for the
        ///     client to read.
        /// </returns>
        /// <remarks>
        ///     Use Marshal.Copy to convert the pointer to the buffer into an array.
        /// </remarks>
        public IntPtr GetBuffer(out int framesRead, out AudioClientBufferFlags flags)
        {
            Int64 p0, p1;
            return GetBuffer(out framesRead, out flags, out p0, out p1);
        }

        /// <summary>
        ///     The ReleaseBuffer method releases the buffer. For more information, see <see href="http://msdn.microsoft.com/en-us/library/dd370861(v=vs.85).aspx"/>.
        /// </summary>
        /// <param name="framesRead">
        ///     The number of audio frames that the client read from the
        ///     capture buffer. This parameter must be either equal to the number of frames in the
        ///     previously acquired data packet or 0.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int ReleaseBufferNative(int framesRead)
        {
            return InteropCalls.CallI(UnsafeBasePtr, framesRead, ((void**) (*(void**) UnsafeBasePtr))[4]);
        }

        /// <summary>
        ///     The ReleaseBuffer method releases the buffer. For more information, see <see href="http://msdn.microsoft.com/en-us/library/dd370861(v=vs.85).aspx"/>.
        /// </summary>
        /// <param name="framesRead">
        ///     The number of audio frames that the client read from the
        ///     capture buffer. This parameter must be either equal to the number of frames in the
        ///     previously acquired data packet or 0.
        /// </param>
        public void ReleaseBuffer(int framesRead)
        {
            CoreAudioAPIException.Try(ReleaseBufferNative(framesRead), InterfaceName, "ReleaseBuffer");
        }

        /// <summary>
        ///     The GetNextPacketSize method retrieves the number of frames in the next data packet in
        ///     the capture endpoint buffer.
        /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/dd370860(v=vs.85).aspx"/>.
        /// </summary>
        /// <param name="numFramesInNextPacket">
        ///     Variable into which the method writes the frame count (the number of audio
        ///     frames in the next capture packet).
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetNextPacketSizeNative(out int numFramesInNextPacket)
        {
            fixed (void* p = &numFramesInNextPacket)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        ///     The GetNextPacketSize method retrieves the number of frames in the next data packet in
        ///     the capture endpoint buffer.
        /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/dd370860(v=vs.85).aspx"/>.
        /// </summary>
        /// <returns>The number of the audio frames in the next capture packet.</returns>
        public int GetNextPacketSize()
        {
            int t;
            CoreAudioAPIException.Try(GetNextPacketSizeNative(out t), InterfaceName, "GetNextPacketSize");
            return t;
        }
    }
}