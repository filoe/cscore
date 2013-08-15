using CSCore.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.MediaFoundation
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms696260(v=vs.85).aspx
    //mftransform.h line 223
    /// <summary>
    /// Implemented by all Media Foundation Transforms (MFTs).
    /// </summary>
    [ComImport]
    [Guid("bf94c121-5b05-4e6f-8000-ba598961414d")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IMFTransform : IUnknown
    {
        /// <summary>
        /// Retrieves the minimum and maximum number of input and output streams.
        /// </summary>
        int GetStreamLimits([Out] out int inputMinimum, [Out] out int inputMaximum,
                            [Out] out int outputMinimum, [Out] out int outputMaximum);

        /// <summary>
        /// Retrieves the current number of input and output streams on this MFT.
        /// </summary>
        int GetStreamCount([Out] out int inputStreams, [Out] out int outputStream);

        /// <summary>
        /// Retrieves the stream identifiers for the input and output streams on this MFT.
        /// </summary>
        int GetStreamIDs([In] int inputIDArraySize, [In, Out] int[] inputIDs, [In] int outputIDArraySize, [In, Out] int[] outputIDs);

        /// <summary>
        /// Retrieves the buffer requirements and other information for an input stream.
        /// </summary>
        int GetInputStreamInfo(int inputStreamID, [Out] out MFInputStreamInfo streaminfo);

        /// <summary>
        /// Gets the buffer requirements and other information for an output stream on this Media
        /// Foundation transform (MFT).
        /// </summary>
        int GetOutputStreamInfo(int outputStreamID, [Out] out MFOutputStreamInfo streaminfo);

        /// <summary>
        /// Retrieves the attribute store for this MFT.
        /// </summary>
        int GetAttributes([Out] out IMFAttributes attributes);

        /// <summary>
        /// Retrieves the attribute store for an input stream on this MFT.
        /// </summary>
        int GetInputStreamAttributes([Out] out IMFAttributes attributes);

        /// <summary>
        /// Retrieves the attribute store for an output stream on this MFT.
        /// </summary>
        int GetOutputStreamAttributes([Out] out IMFAttributes attributes);

        /// <summary>
        /// Removes an input stream from this MFT.
        /// </summary>
        int DeleteInputStream(int streamID);

        /// <summary>
        /// Adds one or more new input streams to this MFT.
        /// </summary>
        int AddInputStreams(int streamCount, [In] int[] streamIDs);

        int GetInputAvailableType(int streamID, int typeIndex, [Out, MarshalAs(UnmanagedType.Interface)] out IMFMediaType type);

        int GetOutputAvailableType(int streamID, int typeIndex, [Out, MarshalAs(UnmanagedType.Interface)] out IMFMediaType type);

        int SetInputType(int streamID, [In, MarshalAs(UnmanagedType.Interface)] IMFMediaType type, MFTransformSetTypeFlags flags);

        int SetOutputType(int streamID, [In, MarshalAs(UnmanagedType.Interface)] IMFMediaType type, MFTransformSetTypeFlags flags);

        int GetInputCurrentType(int streamID, [Out, MarshalAs(UnmanagedType.Interface)] out IMFMediaType type);

        int GetOutputCurrentType(int streamID, [Out, MarshalAs(UnmanagedType.Interface)] out IMFMediaType type);

        int GetInputStatus(int streamID, [Out] out MFTransformSetTypeFlags flags);

        int GetOutputStatus(int streamID, [Out] out MFTransformSetTypeFlags flags);

        /// <summary>
        /// Sets the range of time stamps the client needs for output.
        /// </summary>
        int SetOutputBounds(long hnsLowerBound, long hnsUpperBound);

        int ProcessEvent(int inputStreamID, [In] IMFMediaEvent @event);

        int ProcessMessage(MFTMessageType messageType, IntPtr param);

        int ProcessInput(int inputStream, IMFSample sample, int flags);

        [PreserveSig]
        int ProcessOutput(int flags, int outputBufferCount, [Out, In, MarshalAs(UnmanagedType.LPArray)] MFTOutputDataBuffer[] outputSamples, [Out] out MFTProcessOutputStatus status);
    }

    public enum MFTProcessOutputStatus
    {
        None = 0x0,
        NewStreams = 0x100
    }

    //mftransform.h 178
    [StructLayout(LayoutKind.Sequential)]
    public struct MFTOutputDataBuffer
    {
        public int StreamID;

        [MarshalAs(UnmanagedType.IUnknown)]
        public IMFSample Sample;

        public int Status;

        [MarshalAs(UnmanagedType.IUnknown)]
        public IMFCollection Events;
    }

    //mfobjects 4979
    [ComImport]
    [Guid("5BC8A76B-869A-46a3-9B03-FA218A66AEBE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IMFCollection
    {
        int GetElementCount([Out] out int count);

        int GetElement([In] int elementIndex, [Out] out IntPtr element);

        int AddElement([In] IntPtr element);

        int RemoveElement([In] int elementIndex, [Out] out IntPtr element);

        int InsertElementAt([In] int index, [In] IntPtr element);

        int RemoveAllElements();
    }

    [Flags]
    public enum MFTransformStatusFlags
    {
        Default = 0,
        AcceptData = 1
    }

    [Flags]
    public enum MFTransformSetTypeFlags
    {
        Default = 0,

        /// <summary>
        /// Test the proposed media type, but do not set it.
        /// </summary>
        TestOnly = 1
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms704067(v=vs.85).aspx
    /// <summary>
    /// Contains information about an input stream on a Media Foundation transform (MFT). To get these values, call IMFTransform::GetInputStreamInfo.
    /// </summary>
    /// <remarks>
    /// Before the media types are set, the only values that should be considered valid are the MFT_INPUT_STREAM_REMOVABLE and MFT_INPUT_STREAM_OPTIONAL flags in the dwFlags member.
    /// -The MFT_INPUT_STREAM_REMOVABLE flag indicates that the stream can be deleted.
    /// -The MFT_INPUT_STREAM_OPTIONAL flag indicates that the stream is optional and does not require a media type.
    /// After you set a media type on all of the input and output streams (not including optional streams), all of the values returned by the GetInputStreamInfo method are valid. They might change if you set different media types.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MFInputStreamInfo
    {
        /// <summary>
        /// Maximum amount of time between an input sample and the corresponding output sample, in
        /// 100-nanosecond units. For example, an MFT that buffers two samples, each with a duration
        /// of 1 second, has a maximum latency of two seconds. If the MFT always turns input samples
        /// directly into output samples, with no buffering, the latency is zero.
        /// </summary>
        public long HnsMaxLatency;

        /// <summary>
        /// Bitwise OR of zero or more flags from the InputStreamInfoFlags enumeration.
        /// </summary>
        public MFInputStreamInfoFlags Flags;

        /// <summary>
        /// The minimum size of each input buffer, in bytes. If the size is variable or the MFT does
        /// not require a specific size, the value is zero. For uncompressed audio, the value should
        /// be the audio frame size, which you can get from the MF_MT_AUDIO_BLOCK_ALIGNMENT
        /// attribute in the media type.
        /// </summary>
        public int Size;

        /// <summary>
        /// Maximum amount of input data, in bytes, that the MFT holds to perform lookahead.
        /// Lookahead is the action of looking forward in the data before processing it. This value
        /// should be the worst-case value. If the MFT does not keep a lookahead buffer, the value
        /// is zero.
        /// </summary>
        public int cbMaxLookahead;

        /// <summary>
        /// The memory alignment required for input buffers. If the MFT does not require a specific
        /// alignment, the value is zero.
        /// </summary>
        public int cbAlignment;

        /*public InputStreamInfo()
        {
            HnsMaxLatency = 0;
            Flags = (InputStreamInfoFlags)0;
            cbMaxLookahead = 0;
            cbAlignment = 0;
            Size = Marshal.SizeOf(typeof(InputStreamInfo));
        }*/
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms696974(v=vs.85).aspx
    /// <summary>
    /// Contains information about an output stream on a Media Foundation transform (MFT). To get these values, call IMFTransform::GetOutputStreamInfo.
    /// </summary>
    /// <remarks>
    /// Before the media types are set, the only values that should be considered valid is the MFT_OUTPUT_STREAM_OPTIONAL flag in the dwFlags member. This flag indicates that the stream is optional and does not require a media type.
    /// After you set a media type on all of the input and output streams (not including optional streams), all of the values returned by the GetOutputStreamInfo method are valid. They might change if you set different media types.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MFOutputStreamInfo
    {
        /// <summary>
        /// Bitwise OR of zero or more flags from the _MFT_OUTPUT_STREAM_INFO_FLAGS enumeration.
        /// </summary>
        public MFOutputStreamInfoFlags Flags;

        /// <summary>
        /// Minimum size of each output buffer, in bytes. If the MFT does not require a specific
        /// size, the value is zero. For uncompressed audio, the value should be the audio frame
        /// size, which you can get from the MF_MT_AUDIO_BLOCK_ALIGNMENT attribute in the media
        /// type. If the dwFlags member contains the MFT_OUTPUT_STREAM_PROVIDES_SAMPLES flag, the
        /// value is zero, because the MFT allocates the output buffers.
        /// </summary>
        public int Size;

        /// <summary>
        /// The memory alignment required for output buffers. If the MFT does not require a specific
        /// alignment, the value is zero. If the dwFlags member contains the
        /// MFT_OUTPUT_STREAM_PROVIDES_SAMPLES flag, this value is the alignment that the MFT uses
        /// internally when it allocates samples. It is recommended, but not required, that MFTs
        /// allocate buffers with at least a 16-byte memory alignment.
        /// </summary>
        public int cbAlignment;

        /*public OutputStreamInfo()
        {
            Flags = (OutputStreamInfoFlags)0;
            cbAlignment = 0;
            Size = Marshal.SizeOf(typeof(OutputStreamInfo));
        }*/
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms703975(v=vs.85).aspx
    [Flags]
    public enum MFInputStreamInfoFlags
    {
        WholeSamples = 0x00000001,
        SingleSamplePerBuffer = 0x00000002,
        FixedSampleSize = 0x00000004,
        HoldsBuffers = 0x00000008,
        DoesNotAddRef = 0x00000100,
        Removable = 0x00000200,
        Optional = 0x00000400,
        ProcessInPlace = 0x00000800
    }

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms705618(v=vs.85).aspx
    [Flags]
    public enum MFOutputStreamInfoFlags
    {
        WholeSamples = 0x1,
        SingleSamplePerBuffer = 0x2,
        FixedSampleSize = 0x4,
        Discardable = 0x8,
        Optional = 0x10,
        ProvidesSamples = 0x100,
        CanProvideSamples = 0x200,
        LazyRead = 0x400,
        Removable = 0x800
    }
}