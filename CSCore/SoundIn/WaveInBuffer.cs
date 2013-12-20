using CSCore.SoundOut.MMInterop;
using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundIn
{
    public class WaveInBuffer : IDisposable
    {
        private int _bufferSize;

        private byte[] _buffer;
        private WaveHeader _header;

        private GCHandle _bufferHandle, _headerHandle, _userDataHandle;

        private IntPtr _waveInHandle;

        public IntPtr WaveInHandle 
        { 
            get { return _waveInHandle; } 
        }

        public Byte[] Buffer 
        { 
            get { return _buffer; } 
        }

        public int Recorded 
        { 
            get { return _header.bytesRecorded; } 
        }

        public int BufferSize 
        { 
            get { return _buffer.Length; } 
        }

        public bool IsInQueue 
        { 
            get { return (_header.flags & WaveHeaderFlags.WHDR_INQUEUE) == WaveHeaderFlags.WHDR_INQUEUE; } 
        }

        public bool Done 
        {
            get { return (_header.flags & WaveHeaderFlags.WHDR_DONE) == WaveHeaderFlags.WHDR_DONE; }
        }

        public WaveInBuffer(WaveIn waveIn, int bufferSize)
        {
            if (waveIn == null) throw new ArgumentNullException("waveOut");
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException("bufferSize");
            if (waveIn.Handle == IntPtr.Zero)
                throw new InvalidOperationException("Invalid WaveIn-Handle");

            _waveInHandle = waveIn.Handle;
            _bufferSize = bufferSize;
        }

        public void Initialize()
        {
            _buffer = new byte[_bufferSize];
            _header = new WaveHeader();

            _headerHandle = GCHandle.Alloc(_header, GCHandleType.Pinned);
            _bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
            _userDataHandle = GCHandle.Alloc(this);

            _header.bufferLength = _bufferSize;
            _header.dataBuffer = _bufferHandle.AddrOfPinnedObject();
            _header.loops = 1;
            _header.userData = (IntPtr)_userDataHandle;

            Prepare();
            AddBuffer();
        }

        public void Reset()
        {
            Unprepare();
            Prepare();
            AddBuffer();
        }

        private void AddBuffer()
        {
            var result = MMInterops.waveInAddBuffer(_waveInHandle, _header, Marshal.SizeOf(_header));
            MmException.Try(result, "waveInAddBuffer");
        }

        private void Prepare()
        {
            var result = MMInterops.waveInPrepareHeader(_waveInHandle, _header, Marshal.SizeOf(_header));
            MmException.Try(result, "waveInPrepareHeader");
        }

        private void Unprepare()
        {
            var result = MMInterops.waveInUnprepareHeader(_waveInHandle, _header, Marshal.SizeOf(_header));
            MmException.Try(result, "waveInUnprepareHeader");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }

            if (_header != null)
            {
                try
                {
                    Unprepare();
                }
                catch (Exception) { }
                finally
                {
                    _header = null;
                }
            }

            if (_bufferHandle.IsAllocated)
                _bufferHandle.Free();
            if (_headerHandle.IsAllocated)
                _headerHandle.Free();
            if (_userDataHandle.IsAllocated)
                _userDataHandle.Free();
        }

        ~WaveInBuffer()
        {
            Dispose(false);
        }
    }
}