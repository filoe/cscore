using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.DMO
{
    /// <summary>
    /// The <see cref="IMediaBuffer"/> interface provides methods for manipulating a data buffer.
    /// </summary>
    /// <remarks>For more information, <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd390166(v=vs.85).aspx"/>.</remarks>
    [ComImport]
    [Guid("59eff8b9-938c-4a26-82f2-95cb84cdc837")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IMediaBuffer
    {
        /// <summary>
        /// The SetLength method specifies the length of the data currently in the buffer.
        /// </summary>
        /// <param name="length">Size of the data, in bytes. The value must not exceed the buffer's maximum size. Call the <see cref="GetMaxLength"/> method to obtain the maximum size.</param>
        /// <returns>HRESULT</returns>
        [PreserveSig]
        int SetLength(int length);

        /// <summary>
        /// The <see cref="GetMaxLength"/> method retrieves the maximum number of bytes this buffer can hold.
        /// </summary>
        /// <param name="length">A variable that receives the buffer's maximum size, in bytes.</param>
        /// <returns>HRESULT</returns>
        [PreserveSig]
        int GetMaxLength(out int length);

        /// <summary>
        /// The <see cref="GetBufferAndLength"/> method retrieves the buffer and the size of the valid data in the buffer.
        /// </summary>
        /// <param name="ppBuffer">Address of a pointer that receives the buffer array. Can be <see cref="IntPtr.Zero"/> if <paramref name="validDataByteLength"/> is not <see cref="IntPtr.Zero"/>.</param>
        /// <param name="validDataByteLength">Pointer to a variable that receives the size of the valid data, in bytes. Can be <see cref="IntPtr.Zero"/> if <paramref name="ppBuffer"/> is not <see cref="IntPtr.Zero"/>.</param>
        /// <returns>HRESULT</returns>
        [PreserveSig]
        int GetBufferAndLength(IntPtr ppBuffer, IntPtr validDataByteLength);
    }
}