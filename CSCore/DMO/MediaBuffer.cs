using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    /// <summary>
    /// Default-Implementation of the IMediaBuffer interface.
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd376684(v=vs.85).aspx.
    /// </summary>
    public class MediaBuffer : IMediaBuffer, IDisposable, IWritable
    {
        private const string n = "IMediaBuffer";

        private IntPtr _buffer;
        private int _maxlength;
        private int _length;

        /// <summary>
        /// Creates a MediaBuffer and allocates the specified number of bytes in the memory.
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
        /// Gets the maximum number of bytes this buffer can hold.
        /// </summary>
        public int MaxLength 
        { 
            get 
            { 
                return _maxlength; 
            } 
        }

        /// <summary>
        /// Gets the length of the data currently in the buffer.
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (_length > MaxLength || value < 0)
                    throw new ArgumentOutOfRangeException("value", "Length can not be less than zero or greater than maxlength.");
                _length = value;
            }
        }

        /// <summary>
        /// Writes a sequence of bytes to the internally used buffer.
        /// </summary>
        /// <param name="buffer">Array of bytes. The Write method copies data from the specified array of bytes to the internally used buffer.</param>
        /// <param name="offset">Zero-based bytes offset in the specified buffer at which to begin copying bytes to the internally used buffer.</param>
        /// <param name="count">The number of bytes to be copied.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            Length = count;
            Marshal.Copy(buffer, offset, _buffer, count);
        }

        /// <summary>
        /// Reads a sequence of bytes from the internally used buffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">Zero-based byte offset in the specified buffer at which to begin storing the data read from the buffer.</param>
        public void Read(byte[] buffer, int offset)
        {
            Read(buffer, offset, Length);
        }

        /// <summary>
        /// Reads a sequence of bytes from the buffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">Zero-based byte offset in the specified buffer at which to begin storing the data read from the buffer.</param>
        /// <param name="count">The maximum number of bytes to read from the buffer.</param>
        public void Read(byte[] buffer, int offset, int count)
        {
            if (count > Length)
            {
                throw new ArgumentOutOfRangeException("count", "count is greater than MaxLength");
            }
            Marshal.Copy(_buffer, buffer, offset, count);
        }

        public unsafe void Read(byte[] buffer, int offset, int count, int sourceOffset)
        {
            if (count > Length)
            {
                throw new ArgumentOutOfRangeException("count", "count is greater than MaxLength");
            }

            byte* p = (byte*)_buffer.ToPointer();
            p += sourceOffset;

            Marshal.Copy(new IntPtr(p), buffer, offset, count);
        }

        int IMediaBuffer.SetLength(int length)
        {
            if (length > MaxLength)
            {
                return (int)HResult.E_INVALIDARG;
            }
            _length = length;
            return (int)HResult.S_OK;
        }

        int IMediaBuffer.GetMaxLength(out int length)
        {
            length = _maxlength;
            return (int)HResult.S_OK;
        }

        int IMediaBuffer.GetBufferAndLength(IntPtr ppBuffer, IntPtr validDataByteLength)
        {
            //if (ppBuffer == IntPtr.Zero && validDataByteLength == IntPtr.Zero)
            //    return (int)Utils.HResult.E_POINTER;
            if (ppBuffer != IntPtr.Zero)
            {
                Marshal.WriteIntPtr(ppBuffer, _buffer);
            }
            if (validDataByteLength != IntPtr.Zero)
            {
                Marshal.WriteInt32(validDataByteLength, _length);
            }
            return (int)HResult.S_OK;
        }

        /// <summary>
        /// Frees the allocated memory of the internally used buffer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees the allocated memory of the internally used buffer.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_buffer != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(_buffer);
                _buffer = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Frees the allocated memory of the internally used buffer.
        /// </summary>
        ~MediaBuffer()
        {
            Dispose(false);
        }
    }
}