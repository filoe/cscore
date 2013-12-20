using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.MMInterop
{
    public class WaveOutBuffer : IDisposable
    {
        private WaveOut _waveOut;
        private int _bufferSize;

        private byte[] _buffer;

        private GCHandle _userDataHandle, _bufferHandle, _headerHandle;

        private WaveHeader _header;

        public bool IsInQueue 
        { 
            get 
            { 
                return (_header.flags & WaveHeaderFlags.WHDR_INQUEUE) == WaveHeaderFlags.WHDR_INQUEUE; 
            } 
        }

        public WaveOutBuffer(WaveOut waveOut, int bufferSize)
        {
            if (waveOut == null)
                throw new ArgumentNullException("waveOut");
            if (bufferSize <= 0) 
                throw new ArgumentOutOfRangeException("bufferSize");

            _waveOut = waveOut;
            _bufferSize = bufferSize;
        }

        public void Initialize()
        {
            _buffer = new byte[_bufferSize];

            WaveHeader header = new WaveHeader();
            _headerHandle = GCHandle.Alloc(header);
            _userDataHandle = GCHandle.Alloc(this);
            _bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);

            header.userData = (IntPtr)_userDataHandle;
            header.loops = 1;
            header.dataBuffer = _bufferHandle.AddrOfPinnedObject();
            header.bufferLength = _bufferSize;

            _header = header;
            lock (_waveOut.LockObj)
            {
                MmException.Try(MMInterops.waveOutPrepareHeader(_waveOut.WaveOutHandle, header, Marshal.SizeOf(header)),
                    "waveOutPrepareHeader");
            }
        }

        public bool WriteData()
        {
            int read;
            lock (_waveOut.WaveSource)
            {
                read = _waveOut.WaveSource.Read(_buffer, 0, _buffer.Length);
            }
            if (read > 0)
            {
                Array.Clear(_buffer, read, _buffer.Length - read);
                lock (_waveOut.LockObj)
                {
                    MmResult result = MMInterops.waveOutWrite(_waveOut.WaveOutHandle, _header, Marshal.SizeOf(_header));
                    if (result != MmResult.MMSYSERR_NOERROR)
                    {
                        MmException.Try(result, "waveOutWrite");
                    }
                    return result == MmResult.MMSYSERR_NOERROR;
                }
            }
            return false;
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            _disposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_waveOut.LockObj)
            {
                if (_header == null || _waveOut.WaveOutHandle == IntPtr.Zero) 
                    return;
                try
                {
                    MmException.Try(MMInterops.waveOutUnprepareHeader(_waveOut.WaveOutHandle, _header, Marshal.SizeOf(_header)),
                        "waveOutUnprepareHeader"); //don't throw?
                }
                catch (MmException ex)
                {
                    if (ex.Result != MmResult.WAVERR_STILLPLAYING)
                        throw; //can't fix bug
                }
                if (_bufferHandle.IsAllocated) 
                    _bufferHandle.Free();
                if (_headerHandle.IsAllocated) 
                    _headerHandle.Free();
                if (_userDataHandle.IsAllocated) 
                    _userDataHandle.Free();
                _header = null;
            }
        }

        ~WaveOutBuffer()
        {
            Dispose(false);
        }
    }
}