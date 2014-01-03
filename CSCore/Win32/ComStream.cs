using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using comtypes = System.Runtime.InteropServices.ComTypes;

namespace CSCore.Win32
{
    /// <summary>
    /// see http://msdn.microsoft.com/en-us/library/windows/desktop/ms752876(v=vs.85).aspx
    /// </summary>
    public class ComStream : Stream, IStream, IWritable
    {
        private Stream _stream;

        public ComStream(Stream stream)
            : this(stream, true)
        {
        }

        internal ComStream(Stream stream, bool sync)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (sync)
            {
                stream = Stream.Synchronized(stream);
            }
            _stream = stream;
        }

        HResult IStream.Clone(out IStream ppstm)
        {
            ppstm = null;
            return HResult.E_NOTIMPL;
        }

        HResult IStream.Commit(int grfCommitFlags)
        {
            //ignore flags... just implement this method because MediaSinkWriter throws exceptions on finalize
            _stream.Flush();
            return HResult.S_OK;
        }

        HResult IStream.CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            return HResult.E_NOTIMPL;
        }

        HResult IStream.LockRegion(long libOffset, long cb, int dwLockType)
        {
            return HResult.E_NOTIMPL;
        }

        HResult IStream.Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            if (!CanRead)
                throw new InvalidOperationException("Stream not readable");

            int read = Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero)
                Marshal.WriteInt64(pcbRead, read);
            return HResult.S_OK;
        }

        HResult IStream.Revert()
        {
            return HResult.E_NOTIMPL;
        }

        HResult IStream.Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            SeekOrigin origin = (SeekOrigin)dwOrigin; //hope that the SeekOrigin enumeration won't change
            long pos = Seek(dlibMove, origin);
            if (plibNewPosition != IntPtr.Zero)
                Marshal.WriteInt64(plibNewPosition, pos);
            return HResult.S_OK;
        }

        HResult IStream.SetSize(long libNewSize)
        {
            SetLength(libNewSize);
            return HResult.S_OK;
        }

        HResult IStream.Stat(out comtypes.STATSTG pstatstg, int grfStatFlag)
        {
            const int STGM_READ = 0x00000000;
            const int STGM_WRITE = 0x00000001;
            const int STGM_READWRITE = 0x00000002;

            var tmp = new comtypes.STATSTG();
            tmp.type = 2; //STGTY_STREAM
            tmp.cbSize = Length;
            if (CanWrite && CanRead)
                tmp.grfMode = STGM_READWRITE;
            else if (CanRead)
                tmp.grfMode = STGM_READ;
            else if (CanWrite)
                tmp.grfMode = STGM_WRITE;
            else
                throw new ObjectDisposedException("Stream");

            pstatstg = tmp;
            return HResult.S_OK;
        }

        HResult IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            return HResult.E_NOTIMPL;
        }

        HResult IStream.Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            if (!CanWrite)
                throw new InvalidOperationException("Stream is not writeable.");

            Write(pv, 0, cb);
            if (pcbWritten != null)
                Marshal.WriteInt64(pcbWritten, (long)cb);
            return HResult.S_OK;
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }
    }
}