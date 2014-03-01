using CSCore.Utils;
using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    [Guid("d8ad0f58-5494-4102-97c5-ec798e59bcf4")]
    public class MediaObject : ComObject
    {
        private const string n = "MediaObject1";

        public static MediaObject FromComObject(ComObject comObj)
        {
            if (comObj == null)
                throw new ArgumentNullException("comObj");
            return comObj.QueryInterface<MediaObject>();
        }

        public MediaObject(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Retrieves the number of input and output streams.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamCountNative(out int inputStreams, out int outputStreams)
        {
            inputStreams = outputStreams = 0;
            fixed(void* i0 = &inputStreams, i1 = &outputStreams)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, i0, i1, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        /// <summary>
        /// Retrieves the number of input and output streams.
        /// </summary>
        public void GetStreamCount(out int inputStreams, out int outputStreams)
        {
            DmoException.Try(GetStreamCountNative(out inputStreams, out outputStreams), n, "GetStreamCount");
        }

        //--

        /// <summary>
        /// Retrieves information about a specified input stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputStreamInfoNative(int inputStreamIndex, out DmoInputStreamInfoFlags flags)
        {
            flags = DmoInputStreamInfoFlags.None;
            fixed(void* p = &flags)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, p, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        /// <summary>
        /// Retrieves information about a specified input stream.
        /// </summary>
        public DmoInputStreamInfoFlags GetInputStreamInfo(int inputStreamIndex)
        {
            DmoInputStreamInfoFlags flags;
            DmoException.Try(GetInputStreamInfoNative(inputStreamIndex, out flags), n, "GetInputSreamInfo");
            return flags;
        }

        //--

        /// <summary>
        /// Retrieves information about a specified output stream.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputStreamInfoNative(int outputStreamIndex, out DmoOutputStreamInfoFlags flags)
        {
            flags = DmoOutputStreamInfoFlags.None;
            fixed (void* p = &flags)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, outputStreamIndex, p, ((void**)(*(void**)_basePtr))[5]);
            }
        }

        /// <summary>
        /// Retrieves information about a specified output stream.
        /// </summary>
        public DmoOutputStreamInfoFlags GetOutputStreamInfo(int outputStreamIndex)
        {
            DmoOutputStreamInfoFlags flags;
            DmoException.Try(GetOutputStreamInfoNative(outputStreamIndex, out flags), n, "GetOutputSreamInfo");
            return flags;
        }

        //--

        /// <summary>
        /// Retrieves a preferred media type for a specified input stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="mediaType">Can be null to check whether the typeIndex argument is in range. If not, the errorcode will be DMO_E_NO_MORE_ITEMS (0x80040206).</param>
        /// <returns>RESULT</returns>
        public unsafe int GetInputTypeNative(int streamIndex, int typeIndex, out MediaType mediaType)
        {
            fixed(void* p = &mediaType)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, typeIndex, p, ((void**)(*(void**)_basePtr))[6]);
            }
        }

        public unsafe int SetInputTypeNative(int streamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, 
                streamIndex, ((void*)(&mediaType)), flags, ((void**)(*(void**)_basePtr))[8]);
        }

        public void SetInputType(int streamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            DmoException.Try(SetInputTypeNative(streamIndex, mediaType, flags), n, "SetInputType");
        }

        public void SetInputType(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            SetInputType(streamIndex, mediaType, SetTypeFlags.None);
            mediaType.Free();
        }

        public bool SupportsInputFormat(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            bool result = SupportsInputFormat(streamIndex, mediaType);
            mediaType.Free();
            return result;
        }

        public bool SupportsInputFormat(int streamIndex, MediaType mediaType)
        {
            int result = SetInputTypeNative(streamIndex, mediaType, SetTypeFlags.TestOnly);
            switch ((DmoResult)result)
            {
                case DmoResult.S_OK:
                    return true;

                case DmoResult.DMO_E_INVALIDSTREAMINDEX:
                    throw new ArgumentOutOfRangeException("streamIndex");
                case DmoResult.DMO_E_INVALIDTYPE:
                case DmoResult.DMO_E_TYPE_NOT_SET:
                case DmoResult.DMO_E_NOTACCEPTING:
                case DmoResult.DMO_E_TYPE_NOT_ACCEPTED:
                case DmoResult.DMO_E_NO_MORE_ITEMS:
                default:
                    return false;
            }
        }

        //----

        public unsafe void SetOutputType(int streamIndex, MediaType mediaType, SetTypeFlags flags, out int result)
        {
            //void* pvalue = mediaType.HasValue ? &value : IntPtr.Zero.ToPointer();
            result = InteropCalls.CalliMethodPtr(_basePtr, streamIndex, (void*)&mediaType, flags, ((void**)(*(void**)_basePtr))[9]);
        }

        public void SetOutputType(int streamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            int result;
            SetOutputType(streamIndex, mediaType, flags, out result);
            if ((flags & SetTypeFlags.TestOnly) != SetTypeFlags.TestOnly)
                DmoException.Try(result, n, "SetOutputType");
        }

        public void SetOutputType(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            SetOutputType(streamIndex, mediaType, SetTypeFlags.None);
            mediaType.Free();
        }

        public bool SupportsOutputFormat(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            bool result = SupportsOutputFormat(streamIndex, mediaType);
            mediaType.Free();
            return result;
        }

        public bool SupportsOutputFormat(int streamIndex, MediaType mediaType)
        {
            int result;
            SetOutputType(streamIndex, mediaType, SetTypeFlags.TestOnly, out result);
            switch ((DmoResult)result)
            {
                case DmoResult.S_OK:
                    return true;

                case DmoResult.DMO_E_INVALIDSTREAMINDEX:
                    throw new ArgumentOutOfRangeException("streamIndex");
                case DmoResult.DMO_E_INVALIDTYPE:
                case DmoResult.DMO_E_TYPE_NOT_SET:
                case DmoResult.DMO_E_NOTACCEPTING:
                case DmoResult.DMO_E_TYPE_NOT_ACCEPTED:
                case DmoResult.DMO_E_NO_MORE_ITEMS:
                default:
                    return false;
            }
        }

        //---

        public InputStatusFlags GetInputStatus(int streamIndex)
        {
            InputStatusFlags flags;
            int result = GetInputStatus(streamIndex, out flags);
            DmoException.Try(result, n, "GetInputStatus");
            return flags;
        }

        public unsafe int GetInputStatus(int streamIndex, out InputStatusFlags flags)
        {
            fixed (void* pflags = &flags)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, pflags, ((void**)(*(void**)_basePtr))[20]);
            }
        }

        public bool IsReadyForInput(int streamIndex)
        {
            return (GetInputStatus(streamIndex) & (InputStatusFlags.AcceptData)) == InputStatusFlags.AcceptData;
        }

        //----

        public void ProcessInput(int streamIndex, IMediaBuffer mediaBuffer)
        {
            ProcessInput(streamIndex, mediaBuffer, InputdataBufferFlags.None, 0, 0);
        }

        public void ProcessInput(int streamIndex, IMediaBuffer mediaBuffer, InputdataBufferFlags flags)
        {
            ProcessInput(streamIndex, mediaBuffer, flags, 0, 0);
        }

        public unsafe void ProcessInput(int streamIndex, IMediaBuffer mediaBuffer, InputdataBufferFlags flags, long timestamp, long timeduration)
        {
            int result = InteropCalls.CalliMethodPtr(_basePtr, streamIndex, mediaBuffer, flags, timestamp, timeduration, ((void**)(*(void**)_basePtr))[21]);
            DmoException.Try(result, n, "ProcessInput");
        }

        //------
        public void ProcessOutput(ProcessOutputFlags flags, params DmoOutputDataBuffer[] buffers)
        {
            ProcessOutput(flags, buffers, buffers.Length);
        }

        public unsafe void ProcessOutput(ProcessOutputFlags flags, DmoOutputDataBuffer[] buffers, int bufferCount)
        {
            int status = -1;

            int result = ProcessOutput(flags, bufferCount, buffers, out status);
            DmoException.Try(result, n, "ProcessOutput");
        }

        public unsafe int ProcessOutput(ProcessOutputFlags flags, int bufferCount, DmoOutputDataBuffer[] buffers, out int status)
        {
            fixed (void* pstatus = &status)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, flags, bufferCount, buffers, pstatus, ((void**)(*(void**)_basePtr))[22]);
            }
        }
    }
}