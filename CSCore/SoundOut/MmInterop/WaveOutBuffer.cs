using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.Interop
{
    public class WaveOutBuffer : IDisposable
    {
        WaveOut _waveOut;
        int _bufferSize;

        byte[] _buffer;

        GCHandle _userDataHandle, _bufferHandle, _headerHandle;

        WaveHeader _header;

        public bool IsInQueue { get { return _header.flags.HasFlag(WaveHeaderFlags.WHDR_INQUEUE); } }

        public WaveOutBuffer(WaveOut waveOut, int bufferSize)
        {
            if (waveOut == null) throw new ArgumentNullException("waveOut");
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException("bufferSize");

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
                Context.Current.Logger.MMResult(MMInterops.waveOutPrepareHeader(_waveOut.WaveOutHandle, header, Marshal.SizeOf(header)),
                    "waveOutPrepareHeader", "WaveOutBuffer.Initialize()");
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
                        Context.Current.Logger.MMResult(result, "waveOutWrite", "WaveOutBuffer.WriteData()");
                    }
                    return result == MmResult.MMSYSERR_NOERROR;
                }
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
			GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
			if(disposing)
			{
				//dispose managed
			}

            lock (_waveOut.LockObj)
            {
                if (_header == null) return;
                Context.Current.Logger.MMResult(MMInterops.waveOutUnprepareHeader(_waveOut.WaveOutHandle, _header, Marshal.SizeOf(_header)),
                    "waveOutUnprepareHeader", "WaveOutBuffer.Dispose()");

                if (_bufferHandle.IsAllocated) _bufferHandle.Free();
                if (_headerHandle.IsAllocated) _headerHandle.Free();
                if (_userDataHandle.IsAllocated) _userDataHandle.Free();
                _header = null;
            }
        }

        ~WaveOutBuffer()
        {
            Dispose(false);
        }
    }
}
