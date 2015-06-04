using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Represents a media sample, which is a container object for media data. For video, a sample typically contains one video frame. For audio data, a sample typically contains multiple audio samples, rather than a single sample of audio.
    /// </summary>
    public class MFSample : MFAttributes
    {
        private const string InterfaceName = "IMFSample";

        /// <summary>
        /// Initializes a new instance of the <see cref="MFSample"/> class.
        /// </summary>
        /// <remarks>Calls the MFCreateSample function.</remarks>
        public MFSample()
            : this(MediaFoundationCore.CreateEmptySample())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFSample"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MFSample(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To get attibutes from a sample, use the <see cref="MFAttributes"/> object, which
        /// <see cref="MFSample"/> inherits.
        /// </summary>
        /// <param name="sampleFlags">Receives the value <see cref="MFSampleFlags.None"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSampleFlagsNative(out MFSampleFlags sampleFlags)
        {
            fixed (void* ptr = &sampleFlags)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[33]);
            }
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To get attibutes from a sample, use the <see cref="MFAttributes"/> object, which
        /// <see cref="MFSample"/> inherits.
        /// </summary>
        /// <returns>Returns the <see cref="MFSampleFlags.None"/>.</returns>
        public MFSampleFlags GetSampleFlags()
        {
            MFSampleFlags flags;
            MediaFoundationException.Try(GetSampleFlagsNative(out flags), InterfaceName, "GetSampleFlags");
            return flags;
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To set attibutes on a sample, use the <see cref="MFAttributes"/> object, which
        /// IMFSample inherits.
        /// </summary>
        /// <param name="flags">Must be <see cref="MFSampleFlags.None"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetSampleFlagsNative(MFSampleFlags flags)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, flags, ((void**)(*(void**)UnsafeBasePtr))[34]);
        }

        /// <summary>
        /// Currently no flags are defined. Instead, metadata for samples is defined using
        /// attributes. To set attibutes on a sample, use the <see cref="MFAttributes"/> object, which
        /// IMFSample inherits.
        /// </summary>
        /// <param name="flags">Must be <see cref="MFSampleFlags.None"/>.</param>
        public void SetSampleFlags(MFSampleFlags flags)
        {
            MediaFoundationException.Try(SetSampleFlagsNative(flags), InterfaceName, "SetSampleFlags");
        }

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        /// <param name="hnsSampleTime">Presentation time, in 100-nanosecond units.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSampleTimeNative(out long hnsSampleTime)
        {
            fixed (void* ptr = &hnsSampleTime)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[35]);
            }
        }

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        /// <returns>Presentation time, in 100-nanosecond units.</returns>
        public long GetSampleTime()
        {
            long res;
            MediaFoundationException.Try(GetSampleTimeNative(out res), InterfaceName, "GetSampleTime");
            return res;
        }

        /// <summary>
        /// Sets the presentation time of the sample.
        /// </summary>
        /// <param name="hnsSampleTime">The presentation time, in 100-nanosecond units.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetSampleTimeNative(long hnsSampleTime)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, hnsSampleTime, ((void**)(*(void**)UnsafeBasePtr))[36]);
        }

        /// <summary>
        /// Sets the presentation time of the sample.
        /// </summary>
        /// <param name="hnsSampleTime">The presentation time, in 100-nanosecond units.</param>
        public void SetSampleTime(long hnsSampleTime)
        {
            MediaFoundationException.Try(SetSampleTimeNative(hnsSampleTime), InterfaceName, "SetSampleTime");
        }

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        /// <param name="hnsSampleDuration">Receives the presentation time, in 100-nanosecond units.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSampleDurationNative(out long hnsSampleDuration)
        {
            fixed (void* ptr = &hnsSampleDuration)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[37]);
            }
        }

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        /// <returns>The presentation time, in 100-nanosecond units.</returns>
        public long GetSampleDuration()
        {
            long res;
            MediaFoundationException.Try(GetSampleDurationNative(out res), InterfaceName, "GetSampleDuration");
            return res;
        }

        /// <summary>
        /// Sets the duration of the sample.
        /// </summary>
        /// <param name="hnsSampleDuration">Duration of the sample, in 100-nanosecond units.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetSampleDurationNative(long hnsSampleDuration)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, hnsSampleDuration, ((void**)(*(void**)UnsafeBasePtr))[38]);
        }

        /// <summary>
        /// Sets the duration of the sample.
        /// </summary>
        /// <param name="hnsSampleDuration">Duration of the sample, in 100-nanosecond units.</param>
        public void SetSampleDuration(long hnsSampleDuration)
        {
            MediaFoundationException.Try(SetSampleDurationNative(hnsSampleDuration), InterfaceName, "SetSampleDuration");
        }

        /// <summary>
        /// Retrieves the number of buffers in the sample.
        /// </summary>
        /// <param name="bufferCount">Receives the number of buffers in the sample. A sample might contain zero buffers.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetBufferCountNative(out int bufferCount)
        {
            fixed (void* ptr = &bufferCount)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[39]);
            }
        }

        /// <summary>
        /// Retrieves the number of buffers in the sample.
        /// </summary>
        /// <returns>The number of buffers in the sample. A sample might contain zero buffers.</returns>
        public int GetBufferCount()
        {
            int res;
            MediaFoundationException.Try(GetBufferCountNative(out res), InterfaceName, "GetBufferCount");
            return res;
        }


        /// <summary>
        /// Gets a buffer from the sample, by index.
        /// </summary>
        /// <param name="index">Index of the buffer. To find the number of buffers in the sample, call <see cref="GetBufferCount"/>. Buffers are indexed from zero. </param>
        /// <param name="buffer">Receives the <see cref="MFMediaBuffer"/> instance. The caller must release the object. </param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// Note: In most cases, it is safer to use the <see cref="ConvertToContiguousBuffer"/> method. 
        /// If the sample contains more than one buffer, the <see cref="ConvertToContiguousBuffer"/> method replaces them with a single buffer, copies the original data into that buffer, and returns the new buffer to the caller. 
        /// The copy operation occurs at most once. On subsequent calls, no data is copied.
        /// </remarks>
        public unsafe int GetBufferByIndexNative(int index, out MFMediaBuffer buffer)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, index, &ptr, ((void**)(*(void**)UnsafeBasePtr))[40]);
            buffer = ptr == IntPtr.Zero ? null : new MFMediaBuffer(ptr);
            return result;
        }

        /// <summary>
        /// Gets a buffer from the sample, by index.
        /// </summary>
        /// <param name="index">Index of the buffer. To find the number of buffers in the sample, call <see cref="GetBufferCount"/>. Buffers are indexed from zero. </param>
        /// <returns>The <see cref="MFMediaBuffer"/> instance. The caller must release the object.</returns>
        /// <remarks>
        /// Note: In most cases, it is safer to use the <see cref="ConvertToContiguousBuffer"/> method. 
        /// If the sample contains more than one buffer, the <see cref="ConvertToContiguousBuffer"/> method replaces them with a single buffer, copies the original data into that buffer, and returns the new buffer to the caller. 
        /// The copy operation occurs at most once. On subsequent calls, no data is copied.
        /// </remarks>
        public MFMediaBuffer GetBufferByIndex(int index)
        {
            MFMediaBuffer buffer;
            MediaFoundationException.Try(GetBufferByIndexNative(index, out buffer), InterfaceName, "GetBufferByIndex");
            return buffer;
        }

        /// <summary>
        /// Converts a sample with multiple buffers into a sample with a single buffer. 
        /// </summary>
        /// <param name="buffer">Receives a <see cref="MFMediaBuffer"/> instance. The caller must release the instance.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ConvertToContiguousBufferNative(out MFMediaBuffer buffer)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, &ptr, ((void**)(*(void**)UnsafeBasePtr))[41]);
            buffer = ptr == IntPtr.Zero ? null : new MFMediaBuffer(ptr);
            return result;
        }

        /// <summary>
        /// Converts a sample with multiple buffers into a sample with a single buffer. 
        /// </summary>
        /// <returns>A <see cref="MFMediaBuffer"/> instance. The caller must release the instance.</returns>
        public MFMediaBuffer ConvertToContiguousBuffer()
        {
            MFMediaBuffer buffer;
            MediaFoundationException.Try(ConvertToContiguousBufferNative(out buffer), InterfaceName, "ConvertToContiguousBuffer");
            return buffer;
        }

        /// <summary>
        /// Adds a buffer to the end of the list of buffers in the sample. 
        /// </summary>
        /// <param name="buffer">The <see cref="MFMediaBuffer"/> to add.</param>
        /// <returns>HRESULT</returns>
        public unsafe int AddBufferNative(MFMediaBuffer buffer)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((buffer == null) ? IntPtr.Zero : buffer.BasePtr), ((void**)(*(void**)UnsafeBasePtr))[42]);
        }

        /// <summary>
        /// Adds a buffer to the end of the list of buffers in the sample. 
        /// </summary>
        /// <param name="buffer">The <see cref="MFMediaBuffer"/> to add.</param>
        public void AddBuffer(MFMediaBuffer buffer)
        {
            MediaFoundationException.Try(AddBufferNative(buffer), InterfaceName, "AddBuffer");
        }

        /// <summary>
        /// Removes a buffer at a specified index from the sample.
        /// </summary>
        /// <param name="index">Index of the buffer. To find the number of buffers in the sample, call <see cref="GetBufferCount"/>. Buffers are indexed from zero.</param>
        /// <returns>HRESULT</returns>
        public unsafe int RemoveBufferByIndexNative(int index)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, index, ((void**)(*(void**)UnsafeBasePtr))[43]);
        }

        /// <summary>
        /// Removes a buffer at a specified index from the sample.
        /// </summary>
        /// <param name="index">Index of the buffer. To find the number of buffers in the sample, call <see cref="GetBufferCount"/>. Buffers are indexed from zero.</param>
        public void RemoveBufferByIndex(int index)
        {
            MediaFoundationException.Try(RemoveBufferByIndexNative(index), InterfaceName, "RemoveBufferByIndex");
        }

        /// <summary>
        /// Removes all of the buffers from the sample.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int RemoveAllBuffersNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[44]);
        }

        /// <summary>
        /// Removes all of the buffers from the sample.
        /// </summary>
        public void RemoveAllBuffers()
        {
            MediaFoundationException.Try(RemoveAllBuffersNative(), InterfaceName, "RemoveAllBuffers");
        }

        /// <summary>
        /// Retrieves the total length of the valid data in all of the buffers in the sample. The length is calculated as the sum of the values retrieved by the <see cref="MFMediaBuffer.GetCurrentLength"/> method.
        /// </summary>
        /// <param name="totalLength">Receives the total length of the valid data, in bytes.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetTotalLengthNative(out int totalLength)
        {
            fixed (void* ptr = &totalLength)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ptr, ((void**)(*(void**)UnsafeBasePtr))[45]);
            }
        }

        /// <summary>
        /// Retrieves the total length of the valid data in all of the buffers in the sample. The length is calculated as the sum of the values retrieved by the <see cref="MFMediaBuffer.GetCurrentLength"/> method.
        /// </summary>
        /// <returns>The total length of the valid data, in bytes.</returns>
        public int GetTotalLength()
        {
            int res;
            MediaFoundationException.Try(GetTotalLengthNative(out res), InterfaceName, "GetTotalLength");
            return res;
        }

        /// <summary>
        /// Copies the sample data to a buffer. This method concatenates the valid data from all of the buffers of the sample, in order.
        /// </summary>
        /// <param name="buffer">The <see cref="MFMediaBuffer"/> object of the destination buffer. 
        /// The buffer must be large enough to hold the valid data in the sample. 
        /// To get the size of the data in the sample, call <see cref="GetTotalLength"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int CopyToBufferNative(MFMediaBuffer buffer)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((buffer == null) ? IntPtr.Zero : buffer.BasePtr), ((void**)(*(void**)UnsafeBasePtr))[46]);
        }

        /// <summary>
        /// Copies the sample data to a buffer. This method concatenates the valid data from all of the buffers of the sample, in order.
        /// </summary>
        /// <param name="buffer">The <see cref="MFMediaBuffer"/> object of the destination buffer. 
        /// The buffer must be large enough to hold the valid data in the sample. 
        /// To get the size of the data in the sample, call <see cref="GetTotalLength"/>.</param>
        public void CopyToBuffer(MFMediaBuffer buffer)
        {
            MediaFoundationException.Try(CopyToBufferNative(buffer), InterfaceName, "CopyToBuffer");
        }
    }
}