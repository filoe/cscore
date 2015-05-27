using System;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.SoundOut.MMInterop;

namespace CSCore.SoundIn
{
    internal sealed class WaveInBuffer : IDisposable
    {
        private readonly byte[] _buffer;
        private readonly WaveHeader _waveHeader;
        private readonly IntPtr _waveInHandle;
        private GCHandle _bufferHandle;
        private GCHandle _waveHeaderHandle;
        private bool _isDisposed;

        public WaveInBuffer(IntPtr waveInHandle, int bufferSize, IntPtr userData)
        {
            if (waveInHandle == IntPtr.Zero)
                throw new ArgumentNullException("waveInHandle");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            _waveInHandle = waveInHandle;

            _buffer = new byte[bufferSize];
            _bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);

            _waveHeader = new WaveHeader
            {
                bufferLength = bufferSize,
                dataBuffer = _bufferHandle.AddrOfPinnedObject(),
                loops = 1,
                userData = userData
            };
            _waveHeaderHandle = GCHandle.Alloc(_waveHeader);
        }

        public WaveHeader WaveHeader
        {
            get { return _waveHeader; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        public bool IsInQueue
        {
            get { return (WaveHeader.flags & WaveHeaderFlags.WHDR_INQUEUE) == WaveHeaderFlags.WHDR_INQUEUE; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddBufferToQueue()
        {
            MmException.Try(
                NativeMethods.waveInUnprepareHeader(_waveInHandle, _waveHeader, Marshal.SizeOf(_waveHeader)),
                "waveInUnprepareHeader");
            MmException.Try(NativeMethods.waveInPrepareHeader(_waveInHandle, _waveHeader, Marshal.SizeOf(_waveHeader)),
                "waveInPrepareHeader");
            MmException.Try(NativeMethods.waveInAddBuffer(_waveInHandle, _waveHeader, Marshal.SizeOf(_waveHeader)),
                "waveInAddBuffer");
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                MmResult result;
                int counter = 0;
                while (
                    (result =
                        NativeMethods.waveInUnprepareHeader(_waveInHandle, _waveHeader, Marshal.SizeOf(_waveHeader))) ==
                    MmResult.StillPlaying
                    && counter++ < 3)
                {
                    Thread.Sleep(20);
                }
                MmException.Try(result, "waveInUnprepareHeader");

                if (_bufferHandle.IsAllocated)
                    _bufferHandle.Free();
                if(_waveHeaderHandle.IsAllocated)
                    _waveHeaderHandle.Free();
            }
        }

        ~WaveInBuffer()
        {
            Dispose(false);
        }
    }
}