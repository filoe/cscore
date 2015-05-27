using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.SoundOut.MMInterop;

namespace CSCore.SoundOut
{
    internal sealed class WaveOutBuffer : IDisposable
    {
        private readonly IntPtr _waveOutHandle;
        private readonly WaveHeader _waveHeader;
        private readonly byte[] _buffer;
        //don't make _bufferHandle readonly in order to disable warning "ImpureMethodCallOnReadonlyValueField"
        private GCHandle _bufferHandle;
        private GCHandle _waveHeaderHandle;
        private int _previousBufferSize;
        private bool _isDisposed;

        public WaveOutBuffer(IntPtr waveOutHandle, int bufferSize, IntPtr userData)
        {
            if(waveOutHandle == IntPtr.Zero)
                throw new ArgumentNullException("waveOutHandle");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            _waveOutHandle = waveOutHandle;

            _buffer = new byte[bufferSize];
            _bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);

            _waveHeader = new WaveHeader()
            {
                bufferLength = bufferSize,
                dataBuffer = _bufferHandle.AddrOfPinnedObject(),
                loops = 1,
                userData = userData
            };
            _waveHeaderHandle = GCHandle.Alloc(_waveHeaderHandle, GCHandleType.Pinned); //do we really need the waveheaderhandle?
        }

        public bool Refill(IWaveSource source)
        {
            int read = source.Read(_buffer, 0, _buffer.Length);
            if (read > 0)
            {
                Array.Clear(_buffer, read, _buffer.Length - read);

                if (_previousBufferSize != read)
                {
                    //"Unpreparing a buffer that has not been prepared has no effect, and the function returns zero."
                    MmException.Try(
                        NativeMethods.waveOutUnprepareHeader(_waveOutHandle, _waveHeader, Marshal.SizeOf(_waveHeader)),
                        "waveOutUnprepareHeader");

                    /*
                     * The lpData, dwBufferLength, and dwFlags members must be set before calling 
                     * the waveInPrepareHeader or waveOutPrepareHeader function. (For either function
                     * the dwFlags member must be set to zero.)
                     */
                    _waveHeader.bufferLength = read;
                    _waveHeader.flags = WaveHeaderFlags.NONE;
                    MmException.Try(
                        NativeMethods.waveOutPrepareHeader(_waveOutHandle, _waveHeader, Marshal.SizeOf(_waveHeader)),
                        "waveOutPrepareHeader");
                    Debug.WriteLine(_waveHeader.flags);
                    _previousBufferSize = read;
                }
                MmException.Try(
                    NativeMethods.waveOutWrite(_waveOutHandle, _waveHeader, Marshal.SizeOf(_waveHeader)),
                    "waveOutWrite");
                return true;
            }

            return false;
        }

        public bool IsQueued
        {
            get { return (_waveHeader.flags & WaveHeaderFlags.WHDR_INQUEUE) == WaveHeaderFlags.WHDR_INQUEUE; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                        NativeMethods.waveOutUnprepareHeader(_waveOutHandle, _waveHeader, Marshal.SizeOf(_waveHeader))) ==
                    MmResult.StillPlaying &&
                    counter++ < 3)
                {
                    Thread.Sleep(20);
                }

                if(_bufferHandle.IsAllocated)
                    _bufferHandle.Free();
                if(_waveHeaderHandle.IsAllocated)
                    _waveHeaderHandle.Free();

                MmException.Try(
                    result,
                    "waveOutUnprepareHeader");
            }
        }

        ~WaveOutBuffer()
        {
            Dispose(false);
        }
    }
}