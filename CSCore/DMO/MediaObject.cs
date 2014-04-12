using CSCore.Utils;
using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    /// <summary>
    /// Represents a DMO MediaObject.
    /// </summary>
    [Guid("d8ad0f58-5494-4102-97c5-ec798e59bcf4")]
    public class MediaObject : ComObject
    {
        private const string n = "MediaObject";

        /// <summary>
        /// Creates a MediaObject from any ComObjects which derives from MediaObject.
        /// </summary>
        /// <remarks>
        /// Internally they IUnknown::QueryInterface method of the passed COM Object gets called.
        /// </remarks>
        /// <param name="comObj">ComObjects which has to get casted to a MediaObject.</param>
        /// <returns>MediaObject</returns>
        public static MediaObject FromComObject(ComObject comObj)
        {
            if (comObj == null)
                throw new ArgumentNullException("comObj");
            return comObj.QueryInterface<MediaObject>();
        }

        /// <summary>
        /// Gets the number of input streams.
        /// </summary>
        public int InputStreamCount
        {
            get
            {
                int i1, i2;
                GetStreamCount(out i1, out i2);
                return i1;
            }
        }

        /// <summary>
        /// Gets the number of output streams.
        /// </summary>
        public int OutputStreamCount
        {
            get
            {
                int i1, i2;
                GetStreamCount(out i1, out i2);
                return i2;
            }
        }

        /// <summary>
        /// Creates a MediaObject from its pointer.
        /// </summary>
        /// <param name="ptr">Pointer of a MediaObject.</param>
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
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
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
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
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
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
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
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
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
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputTypeNative(int inputStreamIndex, int typeIndex, ref MediaType? mediaType)
        {
            void* ptr = (void*)IntPtr.Zero;
            MediaType mt = new MediaType();

            if(mediaType != null)
                ptr = &mt;

            int result = InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, typeIndex, ptr, ((void**)(*(void**)_basePtr))[6]);

            if (mediaType != null)
                mediaType = mt;

            return result;
        }

        /// <summary>
        /// Retrieves a preferred media type for a specified input stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        public MediaType GetInputType(int inputStreamIndex, int typeIndex)
        {
            MediaType? mediaType = new MediaType();
            DmoException.Try(GetInputTypeNative(inputStreamIndex, typeIndex, ref mediaType), n, "GetInputType");
            return mediaType.Value;
        }

        //--

        /// <summary>
        /// Retrieves a preferred media type for a specified output stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="mediaType">Can be null to check whether the typeIndex argument is in range. If not, the errorcode will be DMO_E_NO_MORE_ITEMS (0x80040206).</param>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputTypeNative(int outputStreamIndex, int typeIndex, ref MediaType? mediaType)
        {
            void* ptr = (void*)IntPtr.Zero;
            MediaType mt = new MediaType();

            if (mediaType != null)
                ptr = &mt;

            int result = InteropCalls.CalliMethodPtr(_basePtr, outputStreamIndex, typeIndex, ptr, ((void**)(*(void**)_basePtr))[7]);

            if (mediaType != null)
                mediaType = mt;

            return result;
        }

        /// <summary>
        /// Retrieves a preferred media type for a specified output stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        public MediaType GetOutputType(int outputStreamIndex, int typeIndex)
        {
            MediaType? mediaType = new MediaType();
            DmoException.Try(GetOutputTypeNative(outputStreamIndex, typeIndex, ref mediaType), n, "GetOutputType");
            return mediaType.Value;
        }

        //--

        /// <summary>
        /// The SetInputType method sets the media type on an input stream, or tests whether a media type is acceptable.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">The new mediatype.</param>
        /// <param name="flags">Flags for setting the mediatype.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetInputTypeNative(int inputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            return InteropCalls.CalliMethodPtr(_basePtr,
                inputStreamIndex, ((void*)(&mediaType)), flags, ((void**)(*(void**)_basePtr))[8]);
        }

        /// <summary>
        /// Clears the inputtype for a specific inputStreamIndex.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        public unsafe void ClearInputType(int inputStreamIndex)
        {
            DmoException.Try(InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, IntPtr.Zero.ToPointer(), SetTypeFlags.Clear, ((void**)(*(void**)_basePtr))[8]), n, "SetInputType");
        }

        /// <summary>
        /// The SetInputType method sets the media type on an input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">The new mediatype.</param>
        /// <param name="flags">Flags for setting the mediatype.</param>
        public void SetInputType(int inputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            DmoException.Try(SetInputTypeNative(inputStreamIndex, mediaType, flags), n, "SetInputType");
        }

        /// <summary>
        /// The SetInputType method sets the media type on an input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="waveFormat">The waveformat which gets converted to the new mediatype.</param>
        public void SetInputType(int inputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            SetInputType(inputStreamIndex, mediaType, SetTypeFlags.None);
            mediaType.Free();
        }

        /// <summary>
        /// Tests whether the given waveformat is supported.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="waveFormat">Waveformat</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsInputFormat(int inputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            bool result = SupportsInputFormat(inputStreamIndex, mediaType);
            mediaType.Free();
            return result;
        }

        /// <summary>
        /// Tests whehter the given mediatype is supported.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">Mediatype</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsInputFormat(int inputStreamIndex, MediaType mediaType)
        {
            int result = SetInputTypeNative(inputStreamIndex, mediaType, SetTypeFlags.TestOnly);
            switch ((DmoErrorCodes)result)
            {
                case (DmoErrorCodes)(HResult.S_OK):
                    return true;

                case DmoErrorCodes.DMO_E_INVALIDSTREAMINDEX:
                    throw new ArgumentOutOfRangeException("inputStreamIndex");
                case DmoErrorCodes.DMO_E_INVALIDTYPE:
                case DmoErrorCodes.DMO_E_TYPE_NOT_SET:
                case DmoErrorCodes.DMO_E_NOTACCEPTING:
                case DmoErrorCodes.DMO_E_TYPE_NOT_ACCEPTED:
                case DmoErrorCodes.DMO_E_NO_MORE_ITEMS:
                default:
                    return false;
            }
        }

        //----

        /// <summary>
        /// The SetOutputType method sets the media type on an output stream, or tests whether a media type is acceptable.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">The new mediatype.</param>
        /// <param name="flags">Flags for setting the mediatype.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetOutputTypeNative(int outputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, outputStreamIndex, (void*)&mediaType, flags, ((void**)(*(void**)_basePtr))[9]);
        }

        /// <summary>
        /// Clears the outputtype for a specific mediatype
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        public unsafe void ClearOutputType(int outputStreamIndex)
        {
            DmoException.Try(InteropCalls.CalliMethodPtr(_basePtr, outputStreamIndex, IntPtr.Zero.ToPointer(), SetTypeFlags.Clear, ((void**)(*(void**)_basePtr))[9]), n, "SetOutputType");
        }

        /// <summary>
        /// The SetOutputType method sets the media type on an output stream, or tests whether a media type is acceptable.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">The new mediatype.</param>
        /// <param name="flags">Flags for setting the mediatype.</param>
        public void SetOutputType(int outputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            int result = SetOutputTypeNative(outputStreamIndex, mediaType, flags);
            if ((flags & SetTypeFlags.TestOnly) != SetTypeFlags.TestOnly)
                DmoException.Try(result, n, "SetOutputType");
        }

        /// <summary>
        /// The SetOutputType method sets the media type on an output stream, or tests whether a media type is acceptable.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="waveFormat">The new waveformat.</param>
        public void SetOutputType(int outputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            SetOutputType(outputStreamIndex, mediaType, SetTypeFlags.None);
            mediaType.Free();
        }

        /// <summary>
        /// Tests whether the given WaveFormat is supported as OutputFormat.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="waveFormat">WaveFormat</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsOutputFormat(int outputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            bool result = SupportsOutputFormat(outputStreamIndex, mediaType);
            mediaType.Free();
            return result;
        }

        /// <summary>
        /// Tests whether the given MediaType is supported as OutputFormat.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">MediaType</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsOutputFormat(int outputStreamIndex, MediaType mediaType)
        {
            int result = SetOutputTypeNative(outputStreamIndex, mediaType, SetTypeFlags.TestOnly);
            switch ((DmoErrorCodes)result)
            {
                case (DmoErrorCodes)(HResult.S_OK):
                    return true;

                case DmoErrorCodes.DMO_E_INVALIDSTREAMINDEX:
                    throw new ArgumentOutOfRangeException("outputStreamIndex");
                case DmoErrorCodes.DMO_E_INVALIDTYPE:
                case DmoErrorCodes.DMO_E_TYPE_NOT_SET:
                case DmoErrorCodes.DMO_E_NOTACCEPTING:
                case DmoErrorCodes.DMO_E_TYPE_NOT_ACCEPTED:
                case DmoErrorCodes.DMO_E_NO_MORE_ITEMS:
                default:
                    return false;
            }
        }

        //---

        /// <summary>
        /// The GetInputCurrentType method retrieves the media type that was set for an input stream, if any.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">MediaType</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputCurrentType(int inputStreamIndex, out MediaType mediaType)
        {
            fixed(void* p = &mediaType)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, p, ((void**)(*(void**)_basePtr))[10]);
            }
        }

        /// <summary>
        /// The GetInputCurrentType method retrieves the media type that was set for an input stream, if any.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        public MediaType GetInputCurrentType(int inputStreamIndex)
        {
            MediaType mediaType;
            DmoException.Try(GetInputCurrentType(inputStreamIndex, out mediaType), n, "GetInputCurrentType");
            return mediaType;
        }

        //---

        /// <summary>
        /// The GetOutputCurrentType method retrieves the media type that was set for an output stream, if any.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">MediaType</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputCurrentType(int outputStreamIndex, out MediaType mediaType)
        {
            fixed (void* p = &mediaType)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, outputStreamIndex, p, ((void**)(*(void**)_basePtr))[11]);
            }
        }

        /// <summary>
        /// The GetOutputCurrentType method retrieves the media type that was set for an output stream, if any.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        public MediaType GetOutputCurrentType(int outputStreamIndex)
        {
            MediaType mediaType;
            DmoException.Try(GetOutputCurrentType(outputStreamIndex, out mediaType), n, "GetOutputCurrentType");
            return mediaType;
        }

        //---

        /// <summary>
        /// This method retrieves the buffer requirements for a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="minSize">Minimum size of an input buffer for this stream, in bytes.</param>
        /// <param name="maxLookahead">The maximum amount of data that the DMO will hold for a lookahead, in bytes. If the DMO does not perform a lookahead on the stream, the value is zero.</param>
        /// <param name="alignment">The required buffer alignment, in bytes. If the input stream has no alignment requirement, the value is 1.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputSizeInfoNative(int inputStreamIndex, out int minSize, out int maxLookahead, out int alignment)
        {
            fixed(void* p0 = &minSize, p1 = &maxLookahead, p2 = &alignment)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, p0, p1, p2, ((void**)(*(void**)_basePtr))[12]);
            }
        }

        /// <summary>
        /// This method retrieves the buffer requirements for a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        public DmoInputSizeInfo GetInputSizeInfo(int inputStreamIndex)
        {
            int i0, i1, i2;
            DmoException.Try(GetInputSizeInfoNative(inputStreamIndex, out i0, out i1, out i2), n, "GetInputSizeInfo");
            return new DmoInputSizeInfo(i0, i2, i1);
        }

        //---

        /// <summary>
        /// This method retrieves the buffer requirements for a specified output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="minSize">Minimum size of an output buffer for this stream, in bytes.</param>
        /// <param name="alignment">The required buffer alignment, in bytes. If the output stream has no alignment requirement, the value is 1.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputSizeInfoNative(int outputStreamIndex, out int minSize, out int alignment)
        {
            fixed (void* p0 = &minSize, p2 = &alignment)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, outputStreamIndex, p0, p2, ((void**)(*(void**)_basePtr))[13]);
            }
        }

        /// <summary>
        /// This method retrieves the buffer requirements for a specified output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        public DmoSizeInfo GetOutputSizeInfo(int outputStreamIndex)
        {
            int i0, i2;
            DmoException.Try(GetOutputSizeInfoNative(outputStreamIndex, out i0, out i2), n, "GetInputSizeInfo");
            return new DmoSizeInfo(i0, i2);
        }

        //---

        /// <summary>
        /// Retrieves the maximum latency on a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="maxLatency">Receives the maximum latency. Unit = REFERENCE_TIME = 100 nanoseconds</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputMaxLatencyNative(int inputStreamIndex, out long maxLatency)
        {
            fixed(void* p = &maxLatency)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, p, ((void**)(*(void**)_basePtr))[14]);
            }
        }

        /// <summary>
        /// Retrieves the maximum latency on a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>Maximum latency. Unit = REFERENCE_TIME = 100 nanoseconds</returns>
        public long GetInputMaxLatency(int inputStreamIndex)
        {
            long l0;
            DmoException.Try(GetInputMaxLatencyNative(inputStreamIndex, out l0), n, "GetInputMaxLatency");
            return l0;
        }

        //---

        /// <summary>
        /// Sets the maximum latency on a specified input stream. For the definition of maximum latency, see IMediaObject::GetInputMaxLatency.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="maxLatency">Maximum latency. Unit = REFERENCE_TIME = 100 nanoseconds</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetInputMaxLatencyNative(int inputStreamIndex, long maxLatency)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, maxLatency, ((void**)(*(void**)_basePtr))[15]);
        }

        /// <summary>
        /// Sets the maximum latency on a specified input stream. For the definition of maximum latency, see IMediaObject::GetInputMaxLatency.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="maxLatency">Maximum latency. Unit = REFERENCE_TIME = 100 nanoseconds</param>
        public void SetInputMaxLatency(int inputStreamIndex, long maxLatency)
        {
            DmoException.Try(SetInputMaxLatencyNative(inputStreamIndex, maxLatency), n, "SetInputMaxLatency");
        }

        //---

        /// <summary>
        /// This method flushes all internally buffered data.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int FlushNative()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[16]);
        }

        /// <summary>
        /// This method flushes all internally buffered data.
        /// </summary>
        public void Flush()
        {
            DmoException.Try(FlushNative(), n, "Flush");
        }

        //---

        /// <summary>
        /// The Discontinuity method signals a discontinuity on the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>HRESULT</returns>
        public unsafe int DiscontinuityNative(int inputStreamIndex)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, ((void**)(*(void**)_basePtr))[17]);
        }

        /// <summary>
        /// The Discontinuity method signals a discontinuity on the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        public void Discontinuity(int inputStreamIndex)
        {
            DmoException.Try(DiscontinuityNative(inputStreamIndex), n, "Discontinuity");
        }

        //---

        /// <summary>
        /// The AllocateStreamingResources method allocates any resources needed by the DMO. Calling this method is always optional.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd406943(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int AllocateStreamingResourcesNative()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[18]);
        }

        /// <summary>
        /// The AllocateStreamingResources method allocates any resources needed by the DMO. Calling this method is always optional.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd406943(v=vs.85).aspx
        /// </summary>
        public void AllocateStreamingResources()
        {
            DmoException.Try(AllocateStreamingResourcesNative(), n, "AllocateStreamingResources");
        }

        //---

        /// <summary>
        /// The FreeStreamingResources method frees resources allocated by the DMO. Calling this method is always optional.
        /// </summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd406946(v=vs.85).aspx
        /// <returns>HREUSLT</returns>
        public unsafe int FreeStreamingResourcesNative()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[19]);
        }

        /// <summary>
        /// The FreeStreamingResources method frees resources allocated by the DMO. Calling this method is always optional.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd406946(v=vs.85).aspx
        /// </summary>
        public void FreeStreamingResources()
        {
            DmoException.Try(FreeStreamingResourcesNative(), n, "FreeStreamingResources");
        }

        //---

        /// <summary>
        /// The GetInputStatus method queries whether an input stream can accept more input data.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd406950(v=vs.85).aspx
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>InputStatusFlags</returns>
        public InputStatusFlags GetInputStatus(int inputStreamIndex)
        {
            InputStatusFlags flags;
            int result = GetInputStatusNative(inputStreamIndex, out flags);
            DmoException.Try(result, n, "GetInputStatus");
            return flags;
        }

        /// <summary>
        /// The GetInputStatus method queries whether an input stream can accept more input data.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd406950(v=vs.85).aspx
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="flags">InputStatusFlags of the inputstream specified by the inputStreamIndex parameter.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputStatusNative(int inputStreamIndex, out InputStatusFlags flags)
        {
            fixed (void* pflags = &flags)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, pflags, ((void**)(*(void**)_basePtr))[20]);
            }
        }

        //---

        /// <summary>
        /// Queries whether an input stream can accept more input data.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>If the return value is True, the input stream can accept more input data.</returns>
        public bool IsReadyForInput(int inputStreamIndex)
        {
            return (GetInputStatus(inputStreamIndex) & (InputStatusFlags.AcceptData)) == InputStatusFlags.AcceptData;
        }

        //----

        /// <summary>
        /// Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The mediabuffer which has to be processed.</param>
        public void ProcessInput(int inputStreamIndex, IMediaBuffer mediaBuffer)
        {
            ProcessInput(inputStreamIndex, mediaBuffer, InputDataBufferFlags.None, 0, 0);
        }

        /// <summary>
        /// Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The mediabuffer which has to be processed.</param>
        /// <param name="flags">Flags to describe the mediabuffer.</param>
        public void ProcessInput(int inputStreamIndex, IMediaBuffer mediaBuffer, InputDataBufferFlags flags)
        {
            ProcessInput(inputStreamIndex, mediaBuffer, flags, 0, 0);
        }

        /// <summary>
        /// Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The mediabuffer which has to be processed.</param>
        /// <param name="flags">Flags to describe the mediabuffer.</param>
        /// <param name="timestamp">Time stamp that specifies the start time of the data in the buffer. If the buffer has a valid time stamp, set the Time flag in the flags parameter.</param>
        /// <param name="timeduration">Reference time specifying the duration of the data in the buffer. If the buffer has a valid time stamp, set the TimeLength flag in the flags parameter.</param>
        public unsafe void ProcessInput(int inputStreamIndex, IMediaBuffer mediaBuffer, InputDataBufferFlags flags, long timestamp, long timeduration)
        {
            int result = ProcessInputNative(inputStreamIndex, mediaBuffer, flags, timestamp, timeduration);
            if (result == (int)HResult.S_FALSE)
                return;

            DmoException.Try(result, n, "ProcessInput");
        }

        /// <summary>
        /// Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The mediabuffer which has to be processed.</param>
        /// <param name="flags">Flags to describe the mediabuffer.</param>
        /// <param name="timestamp">Time stamp that specifies the start time of the data in the buffer. If the buffer has a valid time stamp, set the Time flag in the flags parameter.</param>
        /// <param name="timeduration">Reference time specifying the duration of the data in the buffer. If the buffer has a valid time stamp, set the TimeLength flag in the flags parameter.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ProcessInputNative(int inputStreamIndex, IMediaBuffer mediaBuffer, InputDataBufferFlags flags, long timestamp, long timeduration)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, inputStreamIndex, mediaBuffer, flags, timestamp, timeduration, ((void**)(*(void**)_basePtr))[21]);
        }

        //---

        /// <summary>
        /// Generates output from the current input data.
        /// </summary>
        /// <param name="flags">Flags that specify output processing requests</param>
        /// <param name="buffers">Array which contains the output buffers.</param>
        public void ProcessOutput(ProcessOutputFlags flags, params DmoOutputDataBuffer[] buffers)
        {
            ProcessOutput(flags, buffers, buffers.Length);
        }

        /// <summary>
        /// Generates output from the current input data.
        /// </summary>
        /// <param name="flags">Flags that specify output processing requests</param>
        /// <param name="buffers">Array which contains the output buffers.</param>
        /// <param name="bufferCount">Number of output buffers.</param>
        public unsafe void ProcessOutput(ProcessOutputFlags flags, DmoOutputDataBuffer[] buffers, int bufferCount)
        {
            int status = -1;

            int result = ProcessOutputNative(flags, bufferCount, buffers, out status);
            if (result == (int)HResult.S_FALSE)
                return;
            DmoException.Try(result, n, "ProcessOutput");
        }

        /// <summary>
        /// Generates output from the current input data.
        /// </summary>
        /// <param name="flags">Flags that specify output processing requests</param>
        /// <param name="buffers">Array which contains the output buffers.</param>
        /// <param name="bufferCount">Number of output buffers.</param>
        /// <returns>HREUSLT</returns>
        public unsafe int ProcessOutputNative(ProcessOutputFlags flags, int bufferCount, DmoOutputDataBuffer[] buffers, out int status)
        {
            fixed (void* pstatus = &status)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, flags, bufferCount, buffers, pstatus, ((void**)(*(void**)_basePtr))[22]);
            }
        }

        //---

        /// <summary>
        /// acquires or releases a lock on the DMO. Call this method to keep the DMO serialized when performing multiple operations.
        /// </summary>
        /// <param name="bLock">Value that specifies whether to acquire or release the lock. If the value is non-zero, a lock is acquired. If the value is zero, the lock is released.</param>
        /// <returns>HRESULT</returns>
        public unsafe int LockNative(long bLock)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, bLock, ((void**)(*(void**)_basePtr))[23]);
        }

        /// <summary>
        /// acquires or releases a lock on the DMO. Call this method to keep the DMO serialized when performing multiple operations.
        /// </summary>
        /// <param name="bLock">Value that specifies whether to acquire or release the lock. If the value is non-zero, a lock is acquired. If the value is zero, the lock is released.</param>
        public void Lock(long bLock)
        {
            DmoException.Try(LockNative(bLock), n, "Lock");
        }
    }
}