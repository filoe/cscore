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

    //mftransform.h 178

    //mfobjects 4979

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms704067(v=vs.85).aspx

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms696974(v=vs.85).aspx

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms703975(v=vs.85).aspx

    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms705618(v=vs.85).aspx
}