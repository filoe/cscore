using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// http: //msdn.microsoft.com/en-us/library/dd370858(v=vs.85).aspx
    /// </summary>
    [Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317")]
    public class AudioCaptureClient : ComObject
    {
        public static readonly Guid IID_IAudioCaptureClient = new Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317");
        private const string c = "IAudioCaptureClient";

        public static AudioCaptureClient FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new AudioCaptureClient(audioClient.GetService(IID_IAudioCaptureClient));
        }

        /// <summary>
        /// Size of the next packet in frames (the size of one frame equals the blockalign value of the waveformat).
        /// </summary>
        public int NextPacketSize
        {
            get { return (int)GetNextPacketSize(); }
        }

        public AudioCaptureClient(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370859(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferNative(out IntPtr ppData, out UInt32 pNumFramesRead, out AudioClientBufferFlags flags, out UInt64 devicePosition, out UInt64 qpcPosition)
        {
            fixed (void* d = &ppData, p0 = &pNumFramesRead, p1 = &flags)
            fixed (void* p2 = &devicePosition, p3 = &qpcPosition)
            {
                return InteropCalls.CallI(_basePtr, d, p0, p1, p2, p3, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        /// <summary>
        /// Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370859(v=vs.85).aspx
        /// </summary>
        /// <remarks>
        /// Use Marshal.Copy to convert the pointer to the buffer into an array.
        /// </remarks>
        public IntPtr GetBuffer(out UInt32 framesRead, out AudioClientBufferFlags flags, out UInt64 devicePosition, out UInt64 qpcPosition)
        {
            IntPtr data;
            int result = GetBufferNative(out data, out framesRead, out flags, out devicePosition, out qpcPosition);
            CoreAudioAPIException.Try(result, c, "GetBuffer");
            return data;
        }

        /// <summary>
        /// Retrieves a pointer to the next available packet of data in the capture endpoint buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370859(v=vs.85).aspx
        /// </summary>
        /// <remarks>
        /// Use Marshal.Copy to convert the pointer to the buffer into an array.
        /// </remarks>
        public IntPtr GetBuffer(out UInt32 framesRead, out AudioClientBufferFlags flags)
        {
            UInt64 p0, p1;
            return GetBuffer(out framesRead, out flags, out p0, out p1);
        }

        /// <summary>
        /// The ReleaseBuffer method releases the buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370861(v=vs.85).aspx
        /// </summary>
        /// <param name="framesRead">The number of audio frames that the client read from the
        /// capture buffer. This parameter must be either equal to the number of frames in the
        /// previously acquired data packet or 0.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ReleaseBufferNative(UInt32 framesRead)
        {
            return InteropCalls.CallI(_basePtr, framesRead, ((void**)(*(void**)_basePtr))[4]);
        }

        /// <summary>
        /// The ReleaseBuffer method releases the buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370861(v=vs.85).aspx
        /// </summary>
        /// <param name="framesRead">The number of audio frames that the client read from the
        /// capture buffer. This parameter must be either equal to the number of frames in the
        /// previously acquired data packet or 0.</param>
        public void ReleaseBuffer(UInt32 framesRead)
        {
            CoreAudioAPIException.Try(ReleaseBufferNative(framesRead), c, "ReleaseBuffer");
        }

        /// <summary>
        /// The GetNextPacketSize method retrieves the number of frames in the next data packet in
        /// the capture endpoint buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370860(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetNextPacketSizeNative(out UInt32 numFramesInNextPacket)
        {
            fixed (void* p = &numFramesInNextPacket)
            {
                return InteropCalls.CallI(_basePtr, p, ((void**)(*(void**)_basePtr))[5]);
            }
        }

        /// <summary>
        /// The GetNextPacketSize method retrieves the number of frames in the next data packet in
        /// the capture endpoint buffer.
        /// http: //msdn.microsoft.com/en-us/library/dd370860(v=vs.85).aspx
        /// </summary>
        public uint GetNextPacketSize()
        {
            uint t;
            CoreAudioAPIException.Try(GetNextPacketSizeNative(out t), c, "GetNextPacketSize");
            return t;
        }
    }
}