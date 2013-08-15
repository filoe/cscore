using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    //mfobjects.h line 764
    [ComImport]
    [Guid("045FA593-8799-42b8-BC8D-8968C6453507")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IMFMediaBuffer
    {
        /// <summary>
        /// Gives the caller access to the memory in the buffer.
        /// </summary>
        void Lock(out IntPtr ppbBuffer, out int pcbMaxLength, out int pcbCurrentLength);

        /// <summary>
        /// Unlocks a buffer that was previously locked.
        /// </summary>
        void Unlock();

        /// <summary>
        /// Retrieves the length of the valid data in the buffer.
        /// </summary>
        void GetCurrentLength(out int pcbCurrentLength);

        /// <summary>
        /// Sets the length of the valid data in the buffer.
        /// </summary>
        void SetCurrentLength(int cbCurrentLength);

        /// <summary>
        /// Retrieves the allocated size of the buffer.
        /// </summary>
        void GetMaxLength(out int pcbMaxLength);
    }
}