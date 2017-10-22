using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.DMO
{
    /// <summary>
    ///     Default-Implementation of the IMediaBuffer interface.
    ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd376684(v=vs.85).aspx"/>.
    /// </summary>
    public sealed class MediaBuffer : IMediaBuffer, IDisposable, IWriteable
    {
        private readonly int _maxlength;
        private IntPtr _buffer;
        private int _length;
        private bool _disposed;

        /// <summary>
        ///     Creates a MediaBuffer and allocates the specified number of bytes in the memory.
        /// </summary>
        /// <param name="maxlength">The number of bytes which has to be allocated in the memory.</param>
        public MediaBuffer(int maxlength)
        {
            if (maxlength < 1)
                throw new ArgumentOutOfRangeException("maxlength");
            _maxlength = maxlength;

            _buffer = Marshal.AllocCoTaskMem(maxlength);

            if (_buffer == IntPtr.Zero)
                throw new OutOfMemoryException("Could not allocate memory");
        }

        /// <summary>
        ///     Gets the maximum number of bytes this buffer can hold.
        /// </summary>
        public int MaxLength
        {
            get { return _maxlength; }
        }

        /// <summary>
        ///     Gets the length of the data currently in the buffer.
        /// </summary>
        public int Length
        {
            get { return _length; }
            set
            {
                if (_length > MaxLength || value < 0)
                {
                    throw new ArgumentOutOfRangeException("value",
                        "Length can not be less than zero or greater than maxlength.");
                }
                _length = value;
            }
        }

        /// <summary>
        ///     Frees the allocated memory of the internally used buffer.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        /// <summary>
        /// The SetLength method specifies the length of the data currently in the buffer.
        /// </summary>
        /// <param name="length">Size of the data, in bytes. The value must not exceed the buffer's maximum size. Call the <see cref="IMediaBuffer.GetMaxLength"/> method to obtain the maximum size.</param>
        /// <returns>HRESULT</returns>
        int IMediaBuffer.SetLength(int length)
        {
            if (length > MaxLength)
                return (int) HResult.E_INVALIDARG;
            _length = length;
            return (int) HResult.S_OK;
        }

        /// <summary>
        /// The <see cref="IMediaBuffer.GetMaxLength"/> method retrieves the maximum number of bytes this buffer can hold.
        /// </summary>
        /// <param name="length">A variable that receives the buffer's maximum size, in bytes.</param>
        /// <returns>HRESULT</returns>
        int IMediaBuffer.GetMaxLength(out int length)
        {
            length = _maxlength;
            return (int) HResult.S_OK;
        }

        /// <summary>
        /// The <see cref="IMediaBuffer.GetBufferAndLength"/> method retrieves the buffer and the size of the valid data in the buffer.
        /// </summary>
        /// <param name="ppBuffer">Address of a pointer that receives the buffer array. Can be <see cref="IntPtr.Zero"/> if <paramref name="validDataByteLength"/> is not <see cref="IntPtr.Zero"/>.</param>
        /// <param name="validDataByteLength">Pointer to a variable that receives the size of the valid data, in bytes. Can be <see cref="IntPtr.Zero"/> if <paramref name="ppBuffer"/> is not <see cref="IntPtr.Zero"/>.</param>
        /// <returns>HRESULT</returns>
        int IMediaBuffer.GetBufferAndLength(IntPtr ppBuffer, IntPtr validDataByteLength)
        {
            //if (ppBuffer == IntPtr.Zero && validDataByteLength == IntPtr.Zero)
            //    return (int)Utils.HResult.E_POINTER;
            if (ppBuffer != IntPtr.Zero)
                Marshal.WriteIntPtr(ppBuffer, _buffer);
            if (validDataByteLength != IntPtr.Zero)
                Marshal.WriteInt32(validDataByteLength, _length);
            return (int) HResult.S_OK;
        }

        /// <summary>
        ///     Writes a sequence of bytes to the internally used buffer.
        /// </summary>
        /// <param name="buffer">
        ///     Array of bytes. The Write method copies data from the specified array of bytes to the internally
        ///     used buffer.
        /// </param>
        /// <param name="offset">
        ///     Zero-based bytes offset in the specified buffer at which to begin copying bytes to the internally
        ///     used buffer.
        /// </param>
        /// <param name="count">The number of bytes to be copied.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            Length = count;
            Marshal.Copy(buffer, offset, _buffer, count);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the internally used buffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">
        ///     Zero-based byte offset in the specified buffer at which to begin storing the data read from the
        ///     buffer.
        /// </param>
        public void Read(byte[] buffer, int offset)
        {
            Read(buffer, offset, Length);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the buffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">
        ///     Zero-based byte offset in the specified buffer at which to begin storing the data read from the
        ///     buffer.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the buffer.</param>
        public void Read(byte[] buffer, int offset, int count)
        {
            if (count > Length)
                throw new ArgumentOutOfRangeException("count", "count is greater than MaxLength");
            Marshal.Copy(_buffer, buffer, offset, count);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the buffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">
        ///     Zero-based byte offset in the specified buffer at which to begin storing the data read from the
        ///     buffer.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the buffer.</param>
        /// <param name="sourceOffset">Zero-based offset inside of the source buffer at which to begin copying data.</param>
        internal unsafe void Read(byte[] buffer, int offset, int count, int sourceOffset)
        {
            if (count > Length)
                throw new ArgumentOutOfRangeException("count", "count is greater than MaxLength");

            var p = (byte*) _buffer.ToPointer();
            p += sourceOffset;

            Marshal.Copy(new IntPtr(p), buffer, offset, count);
        }

        /// <summary>
        ///     Frees the allocated memory of the internally used buffer.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (_buffer != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(_buffer);
                _buffer = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Frees the allocated memory of the internally used buffer.
        /// </summary>
        ~MediaBuffer()
        {
            Dispose(false);
        }
    }
}