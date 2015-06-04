using System;
using System.IO;
using System.Runtime.InteropServices;
using comtypes = System.Runtime.InteropServices.ComTypes;

namespace CSCore.Win32
{
    /// <summary>
    /// Managed implementation of the <see cref="IStream"/> interface. See <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms752876(v=vs.85).aspx"/>.
    /// </summary>
    public class ComStream : Stream, IStream, IWriteable
    {
        private Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComStream"/> class.
        /// </summary>
        /// <param name="stream">Underlying <see cref="Stream"/>.</param>
        public ComStream(Stream stream)
            : this(stream, true)
        {
        }

        internal ComStream(Stream stream, bool synchronizeStream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (synchronizeStream)
            {
                stream = Synchronized(stream);
            }
            _stream = stream;
        }

        /// <summary>
        /// Creates a new stream object with its own seek pointer that references the same bytes as the original stream.
        /// </summary>
        /// <param name="ppstm">When this method returns, contains the new stream object. This parameter is passed uninitialized.</param>
        /// <returns>HRESULT</returns>
        HResult IStream.Clone(out IStream ppstm)
        {
            ppstm = null;
            return HResult.E_NOTIMPL;
        }

        /// <summary>
        /// Ensures that any changes made to a stream object that is open in transacted mode are reflected in the parent storage.
        /// </summary>
        /// <param name="grfCommitFlags">A value that controls how the changes for the stream object are committed. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.Commit(int grfCommitFlags)
        {
            //ignore flags... just implement this method because MediaSinkWriter throws exceptions on finalize
            _stream.Flush();
            return HResult.S_OK;
        }

        /// <summary>
        /// Copies a specified number of bytes from the current seek pointer in the stream to the current seek pointer in another stream.
        /// </summary>
        /// <param name="pstm">A reference to the destination stream. </param>
        /// <param name="cb">The number of bytes to copy from the source stream. </param>
        /// <param name="pcbRead">On successful return, contains the actual number of bytes read from the source. </param>
        /// <param name="pcbWritten">On successful return, contains the actual number of bytes written to the destination. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            return HResult.E_NOTIMPL;
        }

        /// <summary>
        /// Restricts access to a specified range of bytes in the stream.
        /// </summary>
        /// <param name="libOffset">The byte offset for the beginning of the range. </param>
        /// <param name="cb">The length of the range, in bytes, to restrict. </param>
        /// <param name="dwLockType">The requested restrictions on accessing the range. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.LockRegion(long libOffset, long cb, int dwLockType)
        {
            return HResult.E_NOTIMPL;
        }

        /// <summary>
        /// Reads a specified number of bytes from the stream object into memory starting at the current seek pointer.
        /// </summary>
        /// <param name="pv">When this method returns, contains the data read from the stream. This parameter is passed uninitialized.</param>
        /// <param name="cb">The number of bytes to read from the stream object. </param>
        /// <param name="pcbRead">A pointer to a ULONG variable that receives the actual number of bytes read from the stream object. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            if (!CanRead)
                throw new InvalidOperationException("Stream is not readable.");

            int read = Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero)
                Marshal.WriteInt32(pcbRead, read);
            return HResult.S_OK;
        }

        /// <summary>
        /// Discards all changes that have been made to a transacted stream since the last Commit call.
        /// </summary>
        /// <returns>HRESULT</returns>
        HResult IStream.Revert()
        {
            return HResult.E_NOTIMPL;
        }

        /// <summary>
        /// Changes the seek pointer to a new location relative to the beginning of the stream, to the end of the stream, or to the current seek pointer.
        /// </summary>
        /// <param name="dlibMove">The displacement to add to dwOrigin. </param>
        /// <param name="dwOrigin">The origin of the seek. The origin can be the beginning of the file, the current seek pointer, or the end of the file. </param>
        /// <param name="plibNewPosition">On successful return, contains the offset of the seek pointer from the beginning of the stream. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            SeekOrigin origin = (SeekOrigin)dwOrigin; //hope that the SeekOrigin enumeration won't change
            long pos = Seek(dlibMove, origin);
            if (plibNewPosition != IntPtr.Zero)
                Marshal.WriteInt64(plibNewPosition, pos);
            return HResult.S_OK;
        }

        /// <summary>
        /// Changes the size of the stream object.
        /// </summary>
        /// <param name="libNewSize">The new size of the stream as a number of bytes. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.SetSize(long libNewSize)
        {
            SetLength(libNewSize);
            return HResult.S_OK;
        }

        /// <summary>
        /// Retrieves the STATSTG structure for this stream.
        /// </summary>
        /// <param name="pstatstg">When this method returns, contains a STATSTG structure that describes this stream object. This parameter is passed uninitialized.</param>
        /// <param name="grfStatFlag">Members in the STATSTG structure that this method does not return, thus saving some memory allocation operations. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.Stat(out comtypes.STATSTG pstatstg, int grfStatFlag)
        {
            const int STGM_READ = 0x00000000;
            const int STGM_WRITE = 0x00000001;
            const int STGM_READWRITE = 0x00000002;

            var tmp = new comtypes.STATSTG {type = 2, cbSize = Length, grfMode = 0};

            if (CanWrite && CanRead)
                tmp.grfMode |= STGM_READWRITE;
            else if (CanRead)
                tmp.grfMode |= STGM_READ;
            else if (CanWrite)
                tmp.grfMode |= STGM_WRITE;
            else
                throw new ObjectDisposedException("Stream");

            pstatstg = tmp;
            return HResult.S_OK;
        }

        /// <summary>
        /// Removes the access restriction on a range of bytes previously restricted with the LockRegion method.
        /// </summary>
        /// <param name="libOffset">The byte offset for the beginning of the range. </param>
        /// <param name="cb">The length, in bytes, of the range to restrict. </param>
        /// <param name="dwLockType">The access restrictions previously placed on the range. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            return HResult.E_NOTIMPL;
        }

        /// <summary>
        /// Writes a specified number of bytes into the stream object starting at the current seek pointer.
        /// </summary>
        /// <param name="pv">The buffer to write this stream to. </param>
        /// <param name="cb">he number of bytes to write to the stream. </param>
        /// <param name="pcbWritten">On successful return, contains the actual number of bytes written to the stream object. If the caller sets this pointer to Zero, this method does not provide the actual number of bytes written. </param>
        /// <returns>HRESULT</returns>
        HResult IStream.Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            if (!CanWrite)
                throw new InvalidOperationException("Stream is not writeable.");

            Write(pv, 0, cb);
            if (pcbWritten != IntPtr.Zero)
                Marshal.WriteInt32(pcbWritten, cb);
            return HResult.S_OK;
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            _stream.Flush();
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length
        {
            get { return _stream.Length; }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get
            {
                return _stream.Position;
            }
            set
            {
                _stream.Position = value;
            }
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source. </param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream. </param>
        /// <param name="count">The maximum number of bytes to be read from the current stream. </param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter. </param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position. </param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes. </param>
        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream. </param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream. </param>
        /// <param name="count">The number of bytes to be written to the current stream. </param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Stream and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        public override void Close()
        {
            base.Close();
            if (_stream != null)
            {
                _stream.Close();
                _stream = null;
            }
        }
    }
}