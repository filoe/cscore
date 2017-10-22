using System.Runtime.InteropServices;
using System.Security;
using CSCore.Win32;

namespace CSCore.DMO
{
    [ComImport]
    [Guid("d8ad0f58-5494-4102-97c5-ec798e59bcf4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMediaObject : IUnknown
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

        int ProcessInput(int inputStreamIndex, [In] IMediaBuffer mediaBuffer, InputDataBufferFlags flags,
            long referenceTimeTimestamp, long referenceTimeDuration);

        int ProcessOutput(ProcessOutputFlags flags,
            int outputBufferCount,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DmoOutputDataBuffer[] outputBuffers,
            out int statusReserved);

        int Lock(bool acquireLock);
    }
}