using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.DMO
{
    /// <summary>
    ///     Represents a DMO MediaObject.
    /// </summary>
    [Guid("d8ad0f58-5494-4102-97c5-ec798e59bcf4")]
    public class MediaObject : ComObject
    {
        private const string n = "MediaObject";

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaObject"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MediaObject(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets the number of input streams.
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
        ///     Gets the number of output streams.
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
        ///     Creates a MediaObject from any ComObjects.
        /// </summary>
        /// <remarks>
        ///     Internally the IUnknown::QueryInterface method of the specified COM Object gets called.
        /// </remarks>
        /// <param name="comObject">The COM Object to cast to a <see cref="MediaObject"/>.</param>
        /// <returns>The <see cref="MediaObject"/>.</returns>
        public static MediaObject FromComObject(ComObject comObject)
        {
            if (comObject == null)
                throw new ArgumentNullException("comObject");
            return comObject.QueryInterface<MediaObject>();
        }

        /// <summary>
        ///     Retrieves the number of input and output streams.
        /// </summary>
        /// <param name="inputStreams">A variable that receives the number of input streams.</param>
        /// <param name="outputStreams">A variable that receives the number of output streams.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetStreamCountNative(out int inputStreams, out int outputStreams)
        {
            inputStreams = outputStreams = 0;
            fixed (void* i0 = &inputStreams, i1 = &outputStreams)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, i0, i1, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        ///     Retrieves the number of input and output streams.
        /// </summary>
        /// <param name="inputStreams">A variable that receives the number of input streams.</param>
        /// <param name="outputStreams">A variable that receives the number of output streams.</param>
        public void GetStreamCount(out int inputStreams, out int outputStreams)
        {
            DmoException.Try(GetStreamCountNative(out inputStreams, out outputStreams), n, "GetStreamCount");
        }

        //--

        /// <summary>
        ///     Retrieves information about a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="flags">Bitwise combination of zero or more <see cref="DmoInputStreamInfoFlags" /> flags.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputStreamInfoNative(int inputStreamIndex, out DmoInputStreamInfoFlags flags)
        {
            flags = DmoInputStreamInfoFlags.None;
            fixed (void* p = &flags)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, p, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        ///     Retrieves information about a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>The retrieved information about the specified input stream.</returns>
        public DmoInputStreamInfoFlags GetInputStreamInfo(int inputStreamIndex)
        {
            DmoInputStreamInfoFlags flags;
            DmoException.Try(GetInputStreamInfoNative(inputStreamIndex, out flags), n, "GetInputSreamInfo");
            return flags;
        }

        //--

        /// <summary>
        ///     Retrieves information about a specified output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="flags">Bitwise combination of zero or more <see cref="DmoOutputStreamInfoFlags" /> flags.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputStreamInfoNative(int outputStreamIndex, out DmoOutputStreamInfoFlags flags)
        {
            flags = DmoOutputStreamInfoFlags.None;
            fixed (void* p = &flags)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, outputStreamIndex, p, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        ///     Retrieves information about a specified output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <returns>The information about the specified output stream.</returns>
        public DmoOutputStreamInfoFlags GetOutputStreamInfo(int outputStreamIndex)
        {
            DmoOutputStreamInfoFlags flags;
            DmoException.Try(GetOutputStreamInfoNative(outputStreamIndex, out flags), n, "GetOutputSreamInfo");
            return flags;
        }

        //--

        /// <summary>
        ///     Retrieves a preferred media type for a specified input stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="mediaType">
        ///     Can be null to check whether the typeIndex argument is in range. If not, the errorcode will be
        ///     <see cref="DmoErrorCodes.DMO_E_NO_MORE_ITEMS"/> (0x80040206).
        /// </param>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputTypeNative(int inputStreamIndex, int typeIndex, ref MediaType? mediaType)
        {
            var ptr = (void*) IntPtr.Zero;
            var mt = new MediaType();

            if (mediaType != null)
                ptr = &mt;

            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, typeIndex, ptr,
                ((void**) (*(void**) UnsafeBasePtr))[6]);

            if (mediaType != null)
                mediaType = mt;

            return result;
        }

        /// <summary>
        ///     Retrieves a preferred media type for a specified input stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>The preferred media type for the specified input stream.</returns>
        public MediaType GetInputType(int inputStreamIndex, int typeIndex)
        {
            MediaType? mediaType = new MediaType();
            DmoException.Try(GetInputTypeNative(inputStreamIndex, typeIndex, ref mediaType), n, "GetInputType");
            Debug.Assert(mediaType != null, "No mediatype was returned.");
            return mediaType.Value;
        }

        //--

        /// <summary>
        ///     Retrieves a preferred media type for a specified output stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="mediaType">
        ///     Can be null to check whether the typeIndex argument is in range. If not, the errorcode will be
        ///     <see cref="DmoErrorCodes.DMO_E_NO_MORE_ITEMS"/> (0x80040206).
        /// </param>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputTypeNative(int outputStreamIndex, int typeIndex, ref MediaType? mediaType)
        {
            var ptr = (void*) IntPtr.Zero;
            var mt = new MediaType();

            if (mediaType != null)
                ptr = &mt;

            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, outputStreamIndex, typeIndex, ptr,
                ((void**) (*(void**) UnsafeBasePtr))[7]);

            if (mediaType != null)
                mediaType = mt;

            return result;
        }

        /// <summary>
        ///     Retrieves a preferred media type for a specified output stream.
        /// </summary>
        /// <param name="typeIndex">Zero-based index on the set of acceptable media types.</param>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <returns>The preferred media type for the specified output stream.</returns>
        public MediaType GetOutputType(int outputStreamIndex, int typeIndex)
        {
            MediaType? mediaType = new MediaType();
            DmoException.Try(GetOutputTypeNative(outputStreamIndex, typeIndex, ref mediaType), n, "GetOutputType");
            Debug.Assert(mediaType != null, "No mediatype was returned.");
            return mediaType.Value;
        }

        //--

        /// <summary>
        ///     Sets the media type on an input stream, or tests whether a media type is acceptable.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">The new mediatype.</param>
        /// <param name="flags">Bitwise combination of zero or more flags from the <see cref="SetTypeFlags"/> enumeration.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetInputTypeNative(int inputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr,
                inputStreamIndex, &mediaType, flags, ((void**) (*(void**) UnsafeBasePtr))[8]);
        }

        /// <summary>
        ///     Clears the inputtype for a specific input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        public unsafe void ClearInputType(int inputStreamIndex)
        {
            DmoException.Try(
                InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, IntPtr.Zero.ToPointer(), SetTypeFlags.Clear,
                    ((void**) (*(void**) UnsafeBasePtr))[8]), n, "SetInputType");
        }

        /// <summary>
        ///     Sets the media type on an input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">The new mediatype.</param>
        /// <param name="flags">Bitwise combination of zero or more flags from the <see cref="SetTypeFlags"/> enumeration.</param>
        public void SetInputType(int inputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            DmoException.Try(SetInputTypeNative(inputStreamIndex, mediaType, flags), n, "SetInputType");
        }

        /// <summary>
        ///     Sets the media type on an input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="waveFormat">The format to set as the new <see cref="MediaType"/> for the specified input stream.</param>
        public void SetInputType(int inputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            using (MediaType mediaType = MediaType.FromWaveFormat(waveFormat))
            {
                SetInputType(inputStreamIndex, mediaType, SetTypeFlags.None);
            }
        }

        /// <summary>
        ///     Tests whether the given <see cref="WaveFormat"/> is supported.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="waveFormat">The <see cref="WaveFormat"/> to test whether it is supported.</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsInputFormat(int inputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            using (MediaType mediaType = MediaType.FromWaveFormat(waveFormat))
            {
                return SupportsInputFormat(inputStreamIndex, mediaType);
            }
        }

        /// <summary>
        ///     Tests whether the given <see cref="MediaType"/> is supported.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">The <see cref="MediaType"/> to test whether it is supported.</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsInputFormat(int inputStreamIndex, MediaType mediaType)
        {
            int result = SetInputTypeNative(inputStreamIndex, mediaType, SetTypeFlags.TestOnly);
            switch ((DmoErrorCodes) result)
            {
                case (DmoErrorCodes) (HResult.S_OK):
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
        ///     Sets the <see cref="MediaType"/> on an output stream, or tests whether a <see cref="MediaType"/> is acceptable.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">The new <see cref="MediaType"/>.</param>
        /// <param name="flags">Bitwise combination of zero or more flags from the <see cref="SetTypeFlags"/> enumeration.</param>        
        /// <returns>HRESULT</returns>
        public unsafe int SetOutputTypeNative(int outputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, outputStreamIndex, &mediaType, flags,
                ((void**) (*(void**) UnsafeBasePtr))[9]);
        }

        /// <summary>
        ///     Clears the outputtype for a specific output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        public unsafe void ClearOutputType(int outputStreamIndex)
        {
            DmoException.Try(
                InteropCalls.CalliMethodPtr(UnsafeBasePtr, outputStreamIndex, IntPtr.Zero.ToPointer(), SetTypeFlags.Clear,
                    ((void**) (*(void**) UnsafeBasePtr))[9]), n, "SetOutputType");
        }

        /// <summary>
        ///     Sets the <see cref="MediaType"/> on an output stream, or tests whether a <see cref="MediaType"/> is acceptable.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">The new <see cref="MediaType"/>.</param>
        /// <param name="flags">Bitwise combination of zero or more flags from the <see cref="SetTypeFlags"/> enumeration.</param>        
        public void SetOutputType(int outputStreamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            int result = SetOutputTypeNative(outputStreamIndex, mediaType, flags);
            if ((flags & SetTypeFlags.TestOnly) != SetTypeFlags.TestOnly)
                DmoException.Try(result, n, "SetOutputType");
        }

        /// <summary>
        ///     Sets the <see cref="MediaType"/> on an output stream, or tests whether a <see cref="MediaType"/> is acceptable.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="waveFormat">The format to set as the new <see cref="MediaType"/> for the specified output stream.</param>        
        public void SetOutputType(int outputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            using (MediaType mediaType = MediaType.FromWaveFormat(waveFormat))
            {
                SetOutputType(outputStreamIndex, mediaType, SetTypeFlags.None);
            }
        }

        /// <summary>
        ///     Tests whether the given <see cref="WaveFormat"/> is supported as OutputFormat.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="waveFormat">WaveFormat</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsOutputFormat(int outputStreamIndex, WaveFormat waveFormat)
        {
            //experimental
            if (waveFormat is WaveFormatExtensible)
                waveFormat = (waveFormat as WaveFormatExtensible).ToWaveFormat();

            using (MediaType mediaType = MediaType.FromWaveFormat(waveFormat))
            {
                return SupportsOutputFormat(outputStreamIndex, mediaType);
            }
        }

        /// <summary>
        ///     Tests whether the given <see cref="WaveFormat"/> is supported.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">The <see cref="MediaType"/> to test whether it is supported.</param>
        /// <returns>True = supported, False = not supported</returns>
        public bool SupportsOutputFormat(int outputStreamIndex, MediaType mediaType)
        {
            int result = SetOutputTypeNative(outputStreamIndex, mediaType, SetTypeFlags.TestOnly);
            switch ((DmoErrorCodes) result)
            {
                case (DmoErrorCodes) (HResult.S_OK):
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
        ///     Retrieves the media type that was set for an input stream, if any.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaType">A variable that receives the retrieved media type of the specified input stream.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputCurrentType(int inputStreamIndex, out MediaType mediaType)
        {
            fixed (void* p = &mediaType)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, p, ((void**) (*(void**) UnsafeBasePtr))[10]);
            }
        }

        /// <summary>
        ///     Retrieves the media type that was set for an input stream, if any.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>The retrieved media type of the specified input stream.</returns>
        public MediaType GetInputCurrentType(int inputStreamIndex)
        {
            MediaType mediaType;
            DmoException.Try(GetInputCurrentType(inputStreamIndex, out mediaType), n, "GetInputCurrentType");
            return mediaType;
        }

        //---

        /// <summary>
        ///     Retrieves the media type that was set for an output stream, if any.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="mediaType">A variable that receives the retrieved media type of the specified output stream.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputCurrentType(int outputStreamIndex, out MediaType mediaType)
        {
            fixed (void* p = &mediaType)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, outputStreamIndex, p, ((void**) (*(void**) UnsafeBasePtr))[11]);
            }
        }

        /// <summary>
        ///     Retrieves the media type that was set for an output stream, if any.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <returns>The media type that was set for the specified output stream.</returns>
        public MediaType GetOutputCurrentType(int outputStreamIndex)
        {
            MediaType mediaType;
            DmoException.Try(GetOutputCurrentType(outputStreamIndex, out mediaType), n, "GetOutputCurrentType");
            return mediaType;
        }

        //---

        /// <summary>
        ///     Retrieves the buffer requirements for a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="minSize">Minimum size of an input buffer for this stream, in bytes.</param>
        /// <param name="maxLookahead">
        ///     The maximum amount of data that the DMO will hold for a lookahead, in bytes. If the DMO does
        ///     not perform a lookahead on the stream, the value is zero.
        /// </param>
        /// <param name="alignment">
        ///     The required buffer alignment, in bytes. If the input stream has no alignment requirement, the
        ///     value is 1.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputSizeInfoNative(int inputStreamIndex, out int minSize, out int maxLookahead,
            out int alignment)
        {
            fixed (void* p0 = &minSize, p1 = &maxLookahead, p2 = &alignment)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, p0, p1, p2,
                    ((void**) (*(void**) UnsafeBasePtr))[12]);
            }
        }

        /// <summary>
        ///     This method retrieves the buffer requirements for a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>The buffer requirements for the specified input stream.</returns>
        public DmoInputSizeInfo GetInputSizeInfo(int inputStreamIndex)
        {
            int i0, i1, i2;
            DmoException.Try(GetInputSizeInfoNative(inputStreamIndex, out i0, out i1, out i2), n, "GetInputSizeInfo");
            return new DmoInputSizeInfo(i0, i2, i1);
        }

        //---

        /// <summary>
        ///     This method retrieves the buffer requirements for a specified output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <param name="minSize">Minimum size of an output buffer for this stream, in bytes.</param>
        /// <param name="alignment">
        ///     The required buffer alignment, in bytes. If the output stream has no alignment requirement, the
        ///     value is 1.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetOutputSizeInfoNative(int outputStreamIndex, out int minSize, out int alignment)
        {
            fixed (void* p0 = &minSize, p2 = &alignment)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, outputStreamIndex, p0, p2,
                    ((void**) (*(void**) UnsafeBasePtr))[13]);
            }
        }

        /// <summary>
        ///     This method retrieves the buffer requirements for a specified output stream.
        /// </summary>
        /// <param name="outputStreamIndex">Zero-based index of an output stream on the DMO.</param>
        /// <returns>The buffer requirements for the specified output stream.</returns>
        public DmoSizeInfo GetOutputSizeInfo(int outputStreamIndex)
        {
            int i0, i2;
            DmoException.Try(GetOutputSizeInfoNative(outputStreamIndex, out i0, out i2), n, "GetInputSizeInfo");
            return new DmoSizeInfo(i0, i2);
        }

        //---

        /// <summary>
        ///     Retrieves the maximum latency on a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="maxLatency">Receives the maximum latency in reference type units. Unit = REFERENCE_TIME = 100 nanoseconds</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetInputMaxLatencyNative(int inputStreamIndex, out long maxLatency)
        {
            fixed (void* p = &maxLatency)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, p, ((void**) (*(void**) UnsafeBasePtr))[14]);
            }
        }

        /// <summary>
        ///     Retrieves the maximum latency on a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>The maximum latency in reference type units. Unit = REFERENCE_TIME = 100 nanoseconds</returns>
        public long GetInputMaxLatency(int inputStreamIndex)
        {
            long l0;
            DmoException.Try(GetInputMaxLatencyNative(inputStreamIndex, out l0), n, "GetInputMaxLatency");
            return l0;
        }

        //---

        /// <summary>
        ///     Sets the maximum latency on a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="maxLatency">Maximum latency in reference time units. Unit = REFERENCE_TIME = 100 nanoseconds</param>
        /// <returns>HRESULT</returns>
        /// <remarks>For the definition of maximum latency, see <see href="https://msdn.microsoft.com/en-us/Library/dd406948(v=vs.85).aspx"/>.</remarks>
        public unsafe int SetInputMaxLatencyNative(int inputStreamIndex, long maxLatency)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, maxLatency,
                ((void**) (*(void**) UnsafeBasePtr))[15]);
        }

        /// <summary>
        ///     Sets the maximum latency on a specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="maxLatency">Maximum latency in reference time units. Unit = REFERENCE_TIME = 100 nanoseconds</param>
        /// <returns>HRESULT</returns>
        /// <remarks>For the definition of maximum latency, see <see href="https://msdn.microsoft.com/en-us/Library/dd406948(v=vs.85).aspx"/>.</remarks>
        public void SetInputMaxLatency(int inputStreamIndex, long maxLatency)
        {
            DmoException.Try(SetInputMaxLatencyNative(inputStreamIndex, maxLatency), n, "SetInputMaxLatency");
        }

        //---

        /// <summary>
        ///     This method flushes all internally buffered data.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int FlushNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[16]);
        }

        /// <summary>
        ///     This method flushes all internally buffered data.
        /// </summary>
        public void Flush()
        {
            DmoException.Try(FlushNative(), n, "Flush");
        }

        //---

        /// <summary>
        ///     Signals a discontinuity on the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>A discontinuity represents a break in the input. A discontinuity might occur because no more data is expected, the format is changing, or there is a gap in the data. 
        /// After a discontinuity, the DMO does not accept further input on that stream until all pending data has been processed. 
        /// The application should call the <see cref="ProcessOutput(CSCore.DMO.ProcessOutputFlags,CSCore.DMO.DmoOutputDataBuffer[])"/> method until none of the streams returns the <see cref="OutputDataBufferFlags.Incomplete"/> (see <see cref="DmoOutputDataBuffer.Status"/>) flag. 
        /// This method might fail if it is called before the client sets the input and output types on the DMO.</remarks>
        public unsafe int DiscontinuityNative(int inputStreamIndex)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, ((void**) (*(void**) UnsafeBasePtr))[17]);
        }

        /// <summary>
        ///     Signals a discontinuity on the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <remarks>A discontinuity represents a break in the input. A discontinuity might occur because no more data is expected, the format is changing, or there is a gap in the data. 
        /// After a discontinuity, the DMO does not accept further input on that stream until all pending data has been processed. 
        /// The application should call the <see cref="ProcessOutput(CSCore.DMO.ProcessOutputFlags,CSCore.DMO.DmoOutputDataBuffer[])"/> method until none of the streams returns the <see cref="OutputDataBufferFlags.Incomplete"/> (see <see cref="DmoOutputDataBuffer.Status"/>) flag. 
        /// This method might fail if it is called before the client sets the input and output types on the DMO.</remarks>
        public void Discontinuity(int inputStreamIndex)
        {
            DmoException.Try(DiscontinuityNative(inputStreamIndex), n, "Discontinuity");
        }

        //---

        /// <summary>
        ///     Allocates any resources needed by the DMO. Calling this method is always
        ///     optional.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd406943(v=vs.85).aspx"/>.
        /// </remarks>
        public unsafe int AllocateStreamingResourcesNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[18]);
        }

        /// <summary>
        ///     Allocates any resources needed by the DMO. Calling this method is always
        ///     optional.
        /// </summary>
        /// <remarks>
        ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd406943(v=vs.85).aspx"/>.
        /// </remarks>
        public void AllocateStreamingResources()
        {
            DmoException.Try(AllocateStreamingResourcesNative(), n, "AllocateStreamingResources");
        }

        //---

        /// <summary>
        ///     Frees resources allocated by the DMO. Calling this method is always optional.
        /// </summary>
        /// <returns>HREUSLT</returns>
        /// <remarks>
        ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd406946(v=vs.85).aspx"/>.
        /// </remarks>
        public unsafe int FreeStreamingResourcesNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[19]);
        }

        /// <summary>
        ///     Frees resources allocated by the DMO. Calling this method is always optional.
        /// </summary>
        /// <remarks>
        ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd406946(v=vs.85).aspx"/>.
        /// </remarks>
        public void FreeStreamingResources()
        {
            DmoException.Try(FreeStreamingResourcesNative(), n, "FreeStreamingResources");
        }

        //---

        /// <summary>
        ///     Queries whether an input stream can accept more input data.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>The queried input status.</returns>
        /// <remarks>
        ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd406950(v=vs.85).aspx"/>.
        /// </remarks>
        public InputStatusFlags GetInputStatus(int inputStreamIndex)
        {
            InputStatusFlags flags;
            int result = GetInputStatusNative(inputStreamIndex, out flags);
            DmoException.Try(result, n, "GetInputStatus");
            return flags;
        }

        /// <summary>
        ///     Queries whether an input stream can accept more input data.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="flags">A variable that receives either <see cref="InputStatusFlags.None"/> or <see cref="InputStatusFlags.AcceptData"/>.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd406950(v=vs.85).aspx"/>.
        /// </remarks>
        public unsafe int GetInputStatusNative(int inputStreamIndex, out InputStatusFlags flags)
        {
            fixed (void* pflags = &flags)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, pflags,
                    ((void**) (*(void**) UnsafeBasePtr))[20]);
            }
        }

        //---

        /// <summary>
        ///     Queries whether an input stream can accept more input data.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <returns>If the return value is True, the input stream can accept more input data. Otherwise false.</returns>
        public bool IsReadyForInput(int inputStreamIndex)
        {
            return (GetInputStatus(inputStreamIndex) & (InputStatusFlags.AcceptData)) == InputStatusFlags.AcceptData;
        }

        //----

        /// <summary>
        ///     Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The <see cref="MediaBuffer"/> to process.</param>
        public void ProcessInput(int inputStreamIndex, IMediaBuffer mediaBuffer)
        {
            ProcessInput(inputStreamIndex, mediaBuffer, InputDataBufferFlags.None, 0, 0);
        }

        /// <summary>
        ///     Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The <see cref="MediaBuffer"/> to process.</param>
        /// <param name="flags">Bitwise combination of <see cref="InputDataBufferFlags.None"/> or more flags from the <see cref="InputDataBufferFlags"/> enumeration.</param>
        public void ProcessInput(int inputStreamIndex, IMediaBuffer mediaBuffer, InputDataBufferFlags flags)
        {
            ProcessInput(inputStreamIndex, mediaBuffer, flags, 0, 0);
        }

        /// <summary>
        ///     Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The <see cref="MediaBuffer"/> to process.</param>
        /// <param name="flags">Bitwise combination of <see cref="InputDataBufferFlags.None"/> or more flags from the <see cref="InputDataBufferFlags"/> enumeration.</param>
        /// <param name="timestamp">
        ///     Time stamp that specifies the start time of the data in the buffer. If the buffer has a valid
        ///     time stamp, set the Time flag in the flags parameter.
        /// </param>
        /// <param name="timeduration">
        ///     Reference time specifying the duration of the data in the buffer. If the buffer has a valid
        ///     time stamp, set the TimeLength flag in the flags parameter.
        /// </param>
        public void ProcessInput(int inputStreamIndex, IMediaBuffer mediaBuffer, InputDataBufferFlags flags,
            long timestamp, long timeduration)
        {
            int result = ProcessInputNative(inputStreamIndex, mediaBuffer, flags, timestamp, timeduration);
            if (result == (int) HResult.S_FALSE)
                return;

            DmoException.Try(result, n, "ProcessInput");
        }

        /// <summary>
        ///     Delivers a buffer to the specified input stream.
        /// </summary>
        /// <param name="inputStreamIndex">Zero-based index of an input stream on the DMO.</param>
        /// <param name="mediaBuffer">The <see cref="MediaBuffer"/> to process.</param>
        /// <param name="flags">Bitwise combination of <see cref="InputDataBufferFlags.None"/> or more flags from the <see cref="InputDataBufferFlags"/> enumeration.</param>
        /// <param name="timestamp">
        ///     Time stamp that specifies the start time of the data in the buffer. If the buffer has a valid
        ///     time stamp, set the Time flag in the flags parameter.
        /// </param>
        /// <param name="timeduration">
        ///     Reference time specifying the duration of the data in the buffer. If the buffer has a valid
        ///     time stamp, set the TimeLength flag in the flags parameter.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int ProcessInputNative(int inputStreamIndex, IMediaBuffer mediaBuffer, InputDataBufferFlags flags,
            long timestamp, long timeduration)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, inputStreamIndex, mediaBuffer, flags, timestamp, timeduration,
                ((void**) (*(void**) UnsafeBasePtr))[21]);
        }

        //---

        /// <summary>
        ///     Generates output from the current input data.
        /// </summary>
        /// <param name="flags">Bitwise combination of <see cref="ProcessOutputFlags.None"/> or more flags from the <see cref="ProcessOutputFlags"/> enumeration.</param>
        /// <param name="buffers">An array of output buffers to process.</param>
        public void ProcessOutput(ProcessOutputFlags flags, params DmoOutputDataBuffer[] buffers)
        {
            ProcessOutput(flags, buffers, buffers.Length);
        }

        /// <summary>
        ///     Generates output from the current input data.
        /// </summary>
        /// <param name="flags">Bitwise combination of <see cref="ProcessOutputFlags.None"/> or more flags from the <see cref="ProcessOutputFlags"/> enumeration.</param>
        /// <param name="buffers">An array of output buffers to process.</param>
        /// <param name="bufferCount">Number of output buffers.</param>
        public void ProcessOutput(ProcessOutputFlags flags, DmoOutputDataBuffer[] buffers, int bufferCount)
        {
            int status;

            int result = ProcessOutputNative(flags, bufferCount, buffers, out status);
            if (result == (int) HResult.S_FALSE)
                return;
            DmoException.Try(result, n, "ProcessOutput");
        }

        /// <summary>
        ///     Generates output from the current input data.
        /// </summary>
        /// <param name="flags">Bitwise combination of <see cref="ProcessOutputFlags.None"/> or more flags from the <see cref="ProcessOutputFlags"/> enumeration.</param>
        /// <param name="buffers">An array of output buffers to process.</param>
        /// <param name="bufferCount">Number of output buffers.</param>
        /// <param name="status">Receives a reserved value (zero). The application should ignore this value.</param>
        /// <returns>HREUSLT</returns>
        public unsafe int ProcessOutputNative(ProcessOutputFlags flags, int bufferCount, DmoOutputDataBuffer[] buffers,
            out int status)
        {
            fixed (void* pstatus = &status)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, flags, bufferCount, buffers, pstatus,
                    ((void**) (*(void**) UnsafeBasePtr))[22]);
            }
        }

        //---

        /// <summary>
        ///     Acquires or releases a lock on the DMO. Call this method to keep the DMO serialized when performing multiple
        ///     operations.
        /// </summary>
        /// <param name="bLock">
        ///     Value that specifies whether to acquire or release the lock. If the value is non-zero, a lock is
        ///     acquired. If the value is zero, the lock is released.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int LockNative(long bLock)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, bLock, ((void**) (*(void**) UnsafeBasePtr))[23]);
        }

        /// <summary>
        ///     Acquires or releases a lock on the DMO. Call this method to keep the DMO serialized when performing multiple
        ///     operations.
        /// </summary>
        /// <param name="bLock">
        ///     Value that specifies whether to acquire or release the lock. If the value is non-zero, a lock is
        ///     acquired. If the value is zero, the lock is released.
        /// </param>
        public void Lock(long bLock)
        {
            DmoException.Try(LockNative(bLock), n, "Lock");
        }

        /// <summary>
        ///     Acquires or releases a lock on the DMO. Call this method to keep the DMO serialized when performing multiple
        ///     operations.
        /// </summary>
        /// <returns>A disposable object which can be used to unlock the <see cref="MediaObject"/> by calling its <see cref="LockDisposable.Dispose"/> method.</returns>
        /// <example>
        /// This example shows how to use the <see cref="Lock()"/> method:
        /// <code>
        /// partial class TestClass
        /// {
        /// 	public void DoStuff(MediaObject mediaObject)
        /// 	{
        /// 		using(var lock = mediaObject.Lock())
        /// 		{
        /// 			//do some stuff
        /// 		}
        /// 		//the mediaObject gets automatically unlocked by the using statement after "doing your stuff"
        /// 	}
        /// }
        /// </code>
        /// </example>
        public LockDisposable Lock()
        {
            Lock(1);
            return new LockDisposable(this);
        }

        /// <summary>
        /// Used to unlock a <see cref="MediaObject"/> after locking it by calling the <see cref="MediaObject.Lock()"/> method.
        /// </summary>
        public class LockDisposable : IDisposable
        {
            private readonly MediaObject _mediaObject;
            private bool _disposed;

            internal LockDisposable(MediaObject mediaObject)
            {
                _mediaObject = mediaObject;
            }

            /// <summary>
            /// Unlocks the locked <see cref="MediaObject"/>.
            /// </summary>
            public void Dispose()
            {
                if (!_disposed)
                {
                    _mediaObject.Lock(0);
                    _disposed = true;
                }
            }
        }
    }
}