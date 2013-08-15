using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    //mfobjects.h
    [ComImport]
    [Guid("c40a00f2-b93a-4d80-ae8c-5a1c634f58e4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IMFSample : IMFAttributes
    {
        /// <summary>
        /// Retrieves flags associated with the sample.
        /// </summary>
        void GetSampleFlags(out int pdwSampleFlags);

        /// <summary>
        /// Sets flags associated with the sample.
        /// </summary>
        void SetSampleFlags(int dwSampleFlags);

        /// <summary>
        /// Retrieves the presentation time of the sample.
        /// </summary>
        void GetSampleTime(out long phnsSampletime);

        /// <summary>
        /// Sets the presentation time of the sample.
        /// </summary>
        void SetSampleTime(long hnsSampleTime);

        /// <summary>
        /// Retrieves the duration of the sample.
        /// </summary>
        void GetSampleDuration(out long phnsSampleDuration);

        /// <summary>
        /// Sets the duration of the sample.
        /// </summary>
        void SetSampleDuration(long hnsSampleDuration);

        /// <summary>
        /// Retrieves the number of buffers in the sample.
        /// </summary>
        void GetBufferCount(out int pdwBufferCount);

        /// <summary>
        /// Retrieves a buffer from the sample.
        /// </summary>
        void GetBufferByIndex(int dwIndex, out IMFMediaBuffer ppBuffer);

        /// <summary>
        /// Converts a sample with multiple buffers into a sample with a single buffer.
        /// </summary>
        void ConvertToContiguousBuffer(out IMFMediaBuffer ppBuffer);

        /// <summary>
        /// Adds a buffer to the end of the list of buffers in the sample.
        /// </summary>
        void AddBuffer(IMFMediaBuffer pBuffer);

        /// <summary>
        /// Removes a buffer at a specified index from the sample.
        /// </summary>
        void RemoveBufferByIndex(int dwIndex);

        /// <summary>
        /// Removes all buffers from the sample.
        /// </summary>
        void RemoveAllBuffers();

        /// <summary>
        /// Retrieves the total length of the valid data in all of the buffers in the sample.
        /// </summary>
        void GetTotalLength(out int pcbTotalLength);

        /// <summary>
        /// Copies the sample data to a buffer.
        /// </summary>
        void CopyToBuffer(IMFMediaBuffer pBuffer);
    }
}