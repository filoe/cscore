using CSCore.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.DMO
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd406926(v=vs.85).aspx
    //mediaobj.h line 312
    [ComImport]
    [Guid("d8ad0f58-5494-4102-97c5-ec798e59bcf4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IMediaObject : IUnknown
    {
        int GetStreamCount([Out] out int inputStreams, [Out] out int outputStreams);

        int GetInputStreamInfo(int inputStreamIndex, [Out] out InputStreamInfoFlags flags);

        int GetOutputStreamInfo(int outputStreamIndex, [Out] out OutputStreamInfoFlags flags);

        int GetInputType(int inputStreamIndex, int typeIndex, out MediaType mediaType);

        int GetOutputType(int outputStreamIndex, int typeIndex, out MediaType mediaType);

        [PreserveSig]
        int SetInputType(int inputStreamIndex, [In] ref MediaType mediaType, SetTypeFlags flags);

        int SetOutputType(int outputStreamIndex, [In] ref MediaType mediaType, SetTypeFlags flags);

        int GetInputCurrentType(int inputStreamIndex, out MediaType mediaType);

        int GetOutputCurrentType(int outputStreamIndex, out MediaType mediaType);

        int GetInputSizeInfo(int inputStreamIndex, out int size, out int maxLookahead, out int alignment);

        int GetOutputSizeInfo(int outputStreamIndex, out int size, out int alignment);

        int GetInputMaxLatency(int inputStreamIndex, out long referenceTimeMaxLatency);

        int SetInputMaxLatency(int inputStreamIndex, long referenceTimeMaxLatency);

        int Flush();

        int Discontinuity(int inputStreamIndex);

        int AllocateStreamingResources();

        int FreeStreamingResources();

        [PreserveSig]
        int GetInputStatus(int inputStreamIndex, out InputStatusFlags flags);

        int ProcessInput(int inputStreamIndex, [In] IMediaBuffer mediaBuffer, InputdataBufferFlags flags, long referenceTimeTimestamp, long referenceTimeDuration);

        int ProcessOutput(ProcessOutputFlags flags,
            int outputBufferCount,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DmoOutputDataBuffer[] outputBuffers,
            out int statusReserved);

        int Lock(bool acquireLock);
    }



    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd375508(v=vs.85).aspx
    [Flags]
    public enum OutputDataBufferFlags
    {
        None,

        /// <summary>
        /// The beginning of the data is a synchronization point. A synchronization point is a
        /// random access point. For encoded video, this a sample that can be used as a decoding
        /// start point (key frame). For uncompressed audio or video, every sample is a
        /// synchronization point.
        /// </summary>
        SyncPoint = 0x1,

        /// <summary>
        /// The buffer's time stamp is valid. The buffer's indicated time length is valid.
        /// </summary>
        Time = 0x2,

        /// <summary>
        /// The buffer's indicated time length is valid.
        /// </summary>
        TimeLength = 0x4,

        /// <summary>
        /// There is still input data available for processing, but the output buffer is full.
        /// </summary>
        Incomplete = 0x8
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd375502(v=vs.85).aspx
    [Flags]
    public enum InputStreamInfoFlags
    {
        /// <summary>
        /// The stream contains whole samples. Samples do not span multiple buffers, and buffers do
        /// not contain partial samples.
        /// </summary>
        WholeSamples = 0x1,

        /// <summary>
        /// Each buffer contains exactly one sample.
        /// </summary>
        SingleSamplePerBuffer = 0x2,

        /// <summary>
        /// The stream is discardable. Within calls to IMediaObject::ProcessOutput, the DMO can
        /// discard data for this stream without copying it to an output buffer.
        /// </summary>
        FixedSampleSize = 0x4,

        /// <summary>
        /// The DMO performs lookahead on the incoming data, and may hold multiple input buffers for
        /// this stream.
        /// </summary>
        HoldsBuffers = 0x8
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd375509(v=vs.85).aspx
    [Flags]
    public enum OutputStreamInfoFlags
    {
        /// <summary>
        /// The stream contains whole samples. Samples do not span multiple buffers, and buffers do
        /// not contain partial samples.
        /// </summary>
        WholeSamples = 0x1,

        /// <summary>
        /// Each buffer contains exactly one sample.
        /// </summary>
        SingleSamplePerBuffer = 0x2,

        /// <summary>
        /// All the samples in this stream are the same size.
        /// </summary>
        FixedSampleSize = 0x4,

        /// <summary>
        /// The stream is discardable. Within calls to IMediaObject::ProcessOutput, the DMO can
        /// discard data for this stream without copying it to an output buffer.
        /// </summary>
        Discardable = 0x8,

        /// <summary>
        /// The stream is optional. An optional stream is discardable. Also, the application can
        /// ignore this stream entirely; it does not have to set the media type for the stream.
        /// Optional streams generally contain additional information, or data not needed by all
        /// applications.
        /// </summary>
        Optional = 0x10
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd375514(v=vs.85).aspx
    /// <summary>
    /// The SetTypeFlags enumeration defines flags for setting the media type on a stream.
    /// </summary>
    [Flags]
    public enum SetTypeFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Test the media type but do not set it.
        /// </summary>
        TestOnly = 0x1,

        /// <summary>
        /// Clear the media type that was set for the stream.
        /// </summary>
        Clear = 0x2
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd406950(v=vs.85).aspx
    [Flags]
    public enum InputStatusFlags
    {
        None = 0x0,
        AcceptData = 0x1
    }

    /// <summary>
    /// Defines flags that describe an input buffer. 
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375501(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum InputdataBufferFlags
    {
        None,

        /// <summary>
        /// The beginning of the data is a synchronization point.
        /// </summary>
        SyncPoint = 0x1,

        /// <summary>
        /// The buffer's time stamp is valid. The buffer's indicated time length is valid.
        /// </summary>
        Time = 0x2,

        /// <summary>
        /// The buffer's indicated time length is valid.
        /// </summary>
        TimeLength = 0x4
    }

    /// <summary>
    /// Defines flags that specify output processing requests. 
    /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/dd375511(v=vs.85).aspx"/>
    /// </summary>
    [Flags]
    public enum ProcessOutputFlags
    {
        None,

        /// <summary>
        /// Discard the output when the pointer to the output buffer is NULL.
        /// </summary>
        DiscardWhenNoBuffer = 0x1
    }
}