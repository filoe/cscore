using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CSCore.Win32;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Represents a byte stream from some data source, which might be a local file, a network file, or some other source. The <see cref="MFByteStream"/> interface supports the typical stream operations, such as reading, writing, and seeking.
    /// </summary>
    [Guid("ad4c1b00-4bf7-422f-9175-756693d9130d")]
    public class MFByteStream : ComObject
    {
        private const string InterfaceName = "IMFByteStream";

        /// <summary>
        /// Gets the characteristics of the <see cref="MFByteStream"/>.
        /// </summary>
        public MFByteStreamCapsFlags Capabilities
        {
            get
            {
                MFByteStreamCapsFlags value;
                MediaFoundationException.Try(GetCapabilitiesNative(out value), InterfaceName, "GetCapabilities");
                return value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the stream in bytes.
        /// </summary>
        public long Length
        {
            get
            {
                long value;
                MediaFoundationException.Try(GetLengthNative(out value), InterfaceName, "GetLength");
                return value;
            }
            set { MediaFoundationException.Try(SetLengthNative(value), InterfaceName, "SetLength"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFByteStream"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MFByteStream(IntPtr ptr)
            : base(ptr)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFByteStream"/> class which acts as a wrapper for the specified <paramref name="stream"/> to use it in a media foundation context.
        /// </summary>
        /// <param name="stream">The stream to wrap for media foundation usage.</param>
        public MFByteStream(Stream stream)
            : this(MediaFoundationCore.StreamToByteStreamNative(stream))
        {
        }

        /// <summary>
        /// Retrieves the characteristics of the byte stream.
        /// </summary>
        /// <param name="capabilities">Receives a bitwise OR of zero or more flags.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Use the <see cref="Capabilities"/> property for easier usage with automated error handling.</remarks>
        public unsafe int GetCapabilitiesNative(out MFByteStreamCapsFlags capabilities)
        {
            fixed (void* p = &capabilities)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// Retrieves the length of the stream.
        /// </summary>
        /// <param name="length">Receives the length of the stream, in bytes. If the length is unknown, this value is -1.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Use the <see cref="Length"/> property for easier usage with automated error handling.</remarks>        
        public unsafe int GetLengthNative(out long length)
        {
            fixed (void* p = &length)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// Sets the length of the stream.
        /// </summary>
        /// <param name="length">The length of the stream in bytes.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Use the <see cref="Length"/> property for easier usage with automated error handling.</remarks>        
        public unsafe int SetLengthNative(long length)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, length, ((void**) (*(void**) UnsafeBasePtr))[5]);
        }

        /// <summary>
        /// Retrieves the current read or write position in the stream.
        /// </summary>
        /// <param name="position">The current position, in bytes.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetCurrentPositionNative(out long position)
        {
            fixed (void* p = &position)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        /// Sets the current read or write position.
        /// </summary>
        /// <param name="position">New position in the stream, as a byte offset from the start of the stream.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetCurrentPositionNative(long position)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, position, ((void**) (*(void**) UnsafeBasePtr))[7]);
        }

        /// <summary>
        /// Gets or sets the current read/write position in bytes.
        /// </summary>
        public long CurrentPosition
        {
            get
            {
                long position;
                MediaFoundationException.Try(GetCurrentPositionNative(out position), InterfaceName, "GetCurrentPosition");
                return position;
            }
            set { MediaFoundationException.Try(SetCurrentPositionNative(value), InterfaceName, "SetCurrentPosition"); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CurrentPosition"/> has reached the end of the stream.
        /// </summary>
        public bool IsEndOfStream
        {
            get
            {
                NativeBool value;
                MediaFoundationException.Try(IsEndOfStreamNative(out value), InterfaceName, "IsEndOfStream");
                return value;
            }
        }

        /// <summary>
        /// Queries whether the current position has reached the end of the stream.
        /// </summary>
        /// <param name="isEndOfStream">Receives the value <see cref="NativeBool.True"/> if the end of the stream has been reached, or <see cref="NativeBool.False"/> otherwise.</param>
        /// <returns>HREUSLT</returns>
        public unsafe int IsEndOfStreamNative(out NativeBool isEndOfStream)
        {
            fixed (void* p = &isEndOfStream)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[8]);
            }
        }

        /// <summary>
        /// Reads data from the stream.
        /// </summary>
        /// <param name="buffer">Pointer to a buffer that receives the data. The caller must allocate the buffer.</param>
        /// <param name="count">Size of the buffer in bytes.</param>
        /// <param name="read">Receives the number of bytes that are copied into the buffer.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ReadNative(IntPtr buffer, int count, out int read)
        {
            fixed (void* pRead = &read)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*) buffer, count, pRead,
                    ((void**) (*(void**) UnsafeBasePtr))[9]);
            }
        }

        /// <summary>
        /// Reads data from the stream.
        /// </summary>
        /// <param name="buffer">The buffer that receives the data.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>HRESULT</returns>
        /// <exception cref="ArgumentNullException">buffer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is bigger than the length of the buffer.</exception>
        public unsafe int Read(byte[] buffer, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (count > buffer.Length)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return 0;

            int read;

            fixed (void* pBuffer = &buffer[0])
            {
                int result = ReadNative((IntPtr) pBuffer, count, out read);
                MediaFoundationException.Try(result, InterfaceName, "Read");
            }

            return read;
        }

        /// <summary>
        /// Begins an asynchronous read operation from the stream.
        /// </summary>
        /// <param name="buffer">Pointer to a buffer that receives the data. The caller must allocate the buffer.</param>
        /// <param name="count">Size of the buffer in bytes.</param>
        /// <param name="callback">Pointer to the IMFAsyncCallback interface of a callback object. The caller must implement this interface.</param>
        /// <param name="unkState">Pointer to the IUnknown interface of a state object, defined by the caller. Can be Zero.</param>
        /// <returns>HRESULT</returns>
        public unsafe int BeginReadNative(IntPtr buffer, int count, IntPtr callback, IntPtr unkState)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, buffer, count, callback, unkState,
                ((void**) (*(void**) UnsafeBasePtr))[10]);
        }

        /// <summary>
        /// Completes an asynchronous read operation.
        /// </summary>
        /// <param name="result">Pointer to the IMFAsyncResult interface. Pass in the same pointer that your callback object received in the IMFAsyncCallback::Invoke method.</param>
        /// <param name="read">Receives the number of bytes that were read.</param>
        /// <returns>HRESULT</returns>
        public unsafe int EndReadNative(IntPtr result, out int read)
        {
            fixed (void* p = &read)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, result, p, ((void**) (*(void**) UnsafeBasePtr))[11]);
            }
        }
        //TODO: Provide a better implementation for the BeginRead/EndRead methods

        /// <summary>
        /// Writes data to the stream.
        /// </summary>
        /// <param name="buffer">Pointer to a buffer that contains the data to write.</param>
        /// <param name="count">Size of the buffer in bytes.</param>
        /// <param name="written">Receives the number of bytes that are written.</param>
        /// <returns>HRESULT</returns>
        public unsafe int WriteNative(IntPtr buffer, int count, out int written)
        {
            fixed (void* p = &written)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*) buffer, count, p,
                    ((void**) (*(void**) UnsafeBasePtr))[12]);
            }
        }

        /// <summary>
        /// Writes data to the stream.
        /// </summary>
        /// <param name="buffer">Buffer that contains the data to write.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <returns>The number of bytes that were written.</returns>
        /// <exception cref="ArgumentNullException">buffer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is bigger than the length of the buffer.</exception>
        public unsafe int Write(byte[] buffer, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (count > buffer.Length)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return 0;

            int written;
            fixed (void* p = &buffer[0])
            {
                int result = WriteNative((IntPtr) p, count, out written);
                MediaFoundationException.Try(result, InterfaceName, "Write");
            }
            return written;
        }

        /// <summary>
        /// Begins an asynchronous write operation to the stream.
        /// </summary>
        /// <param name="buffer">Pointer to a buffer containing the data to write.</param>
        /// <param name="count">Size of the buffer in bytes.</param>
        /// <param name="callback">Pointer to the IMFAsyncCallback interface of a callback object. The caller must implement this interface.</param>
        /// <param name="unkState">Pointer to the IUnknown interface of a state object, defined by the caller. Can be Zero.</param>
        /// <returns>HRESULT</returns>
        public unsafe int BeginWriteNative(IntPtr buffer, int count, IntPtr callback, IntPtr unkState)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, buffer, count, callback, unkState,
                ((void**)(*(void**)UnsafeBasePtr))[13]);
        }

        /// <summary>
        /// Completes an asynchronous write operation.
        /// </summary>
        /// <param name="result">Pointer to the IMFAsyncResult interface. Pass in the same pointer that your callback object received in the IMFAsyncCallback::Invoke method.</param>
        /// <param name="written">Receives the number of bytes that were written.</param>
        /// <returns>HRESULT</returns>
        public unsafe int EndWriteNative(IntPtr result, out int written)
        {
            fixed (void* p = &written)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, result, p, ((void**)(*(void**)UnsafeBasePtr))[14]);
            }
        }
        //TODO: Provide a better implementation for the BeginWrite/EndWrite methods


        /// <summary>
        /// Moves the current position in the stream by a specified offset.
        /// </summary>
        /// <param name="seekOrigin">Specifies the origin of the seek as a member of the <see cref="MFByteStreamSeekOrigin"/> enumeration. The offset is calculated relative to this position.</param>
        /// <param name="seekOffset">Specifies the new position, as a byte offset from the seek origin.</param>
        /// <param name="cancelPendingIO">Specifies whether all pending I/O requests are canceled after the seek request completes successfully.</param>
        /// <param name="currentPosition">Receives the new position after the seek.</param>
        /// <returns>The new position after the seek.</returns>
        public unsafe int SeekNative(MFByteStreamSeekOrigin seekOrigin, long seekOffset, bool cancelPendingIO, out long currentPosition)
        {
            // ReSharper disable once InconsistentNaming
            const int MFBYTESTREAM_SEEK_FLAG_CANCEL_PENDING_IO = 0x00000001;
            int flags = cancelPendingIO ? MFBYTESTREAM_SEEK_FLAG_CANCEL_PENDING_IO : 0x0;

            fixed (void* p = &currentPosition)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (int) seekOrigin, seekOffset, flags, p,
                    ((void**) (*(void**) UnsafeBasePtr))[15]);
            }
        }

        /// <summary>
        /// Moves the current position in the stream by a specified offset.
        /// </summary>
        /// <param name="seekOrigin">Specifies the origin of the seek as a member of the <see cref="MFByteStreamSeekOrigin"/> enumeration. The offset is calculated relative to this position.</param>
        /// <param name="seekOffset">Specifies the new position, as a byte offset from the seek origin.</param>
        /// <param name="cancelPendingIO">Specifies whether all pending I/O requests are canceled after the seek request completes successfully.</param>
        /// <returns>The new position after the seek.</returns>
        public long Seek(MFByteStreamSeekOrigin seekOrigin, long seekOffset, bool cancelPendingIO)
        {
            long v;
            MediaFoundationException.Try(SeekNative(seekOrigin, seekOffset, cancelPendingIO, out v), InterfaceName,
                "Seek");
            return v;
        }

        /// <summary>
        /// Clears any internal buffers used by the stream. If you are writing to the stream, the buffered data is written to the underlying file or device.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int FlushNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[16]);
        }

        /// <summary>
        /// Clears any internal buffers used by the stream. If you are writing to the stream, the buffered data is written to the underlying file or device.
        /// </summary>
        public void Flush()
        {
            MediaFoundationException.Try(FlushNative(), InterfaceName, "Flush");
        }

        /// <summary>
        /// Closes the stream and releases any resources associated with the stream, such as sockets or file handles. This method also cancels any pending asynchronous I/O requests.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int CloseNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[17]);
        }

        /// <summary>
        /// Closes the stream and releases any resources associated with the stream, such as sockets or file handles. This method also cancels any pending asynchronous I/O requests.
        /// </summary>
        public void Close()
        {
            MediaFoundationException.Try(CloseNative(), InterfaceName, "Close");
        }

        private bool _disposed;

        /// <summary>
        /// Releases the COM object.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            Close();
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
