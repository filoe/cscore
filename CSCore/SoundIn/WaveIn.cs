using CSCore.SoundOut.MMInterop;
using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundIn
{
    [Obsolete()]
    public class WaveIn : ISoundRecorder
    {
        internal static readonly WaveFormat DefaultFormat = new WaveFormat(44100, 16, 1, AudioEncoding.Pcm);

        public static WaveInCaps[] Devices
        {
            get { return WaveInCaps.GetCaps(); }
        }

        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        public event EventHandler Stopped;

        protected IntPtr handle;
        protected MMInterops.WaveCallback _callback;
        private WaveFormat _waveFormat;
        private int _bufferSizeMs = 100;
        protected bool stopped = true;

        private int _bufferCount = 3;

        protected WaveInBuffer[] _buffers;

        private int _device;

        public int Device
        {
            get { return _device; }
            set { _device = value; }
        }

        public int BufferSizeMs
        {
            get { return _bufferSizeMs; }
            set { _bufferSizeMs = value; }
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
            set { _waveFormat = value; }
        }

        public RecordingState RecordingState
        {
            get { return stopped ? SoundIn.RecordingState.Stopped : SoundIn.RecordingState.Recording; }
        }

        public IntPtr Handle { get { return handle; } }

        public WaveIn()
            : this(DefaultFormat)
        {
        }

        public WaveIn(WaveFormat waveFormat)
        {
            //todo: check for supported format
            WaveFormat = waveFormat;
        }

        public void Initialize()
        {
            if (!stopped)
                throw new InvalidOperationException("Recording has to be stopped");

            CloseWaveDevice();
            OpenWaveDevice(Device);
            CreateBuffers();
        }

        public void Start()
        {
            if (!stopped)
                throw new InvalidOperationException("Recording has to be stopped");
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Not initialized");

            ResetBuffers();
            var result = MMInterops.waveInStart(Handle);
            MmException.Try(result, "waveInStart");
            stopped = false;

            OnStarted();
        }

        protected virtual void OnStarted()
        {
        }

        public void Stop()
        {
            if (!stopped)
            {
                stopped = true;
                var result = MMInterops.waveInStop(Handle);
                MmException.Try(result, "waveInStop");
                OnStopping();
                foreach (var buffer in _buffers)
                {
                    if (buffer.Done)
                        RaiseDataAvailable(buffer);
                }
                //RaiseStopped();
                OnStopped();
            }
        }

        protected virtual void OnStopped()
        {
        }

        protected virtual void OnStopping()
        {
        }

        protected virtual void OpenWaveDevice(int device)
        {
            _callback = new MMInterops.WaveCallback(Callback);
            var result = MMInterops.waveInOpen(out handle, (IntPtr)device, _waveFormat, _callback, IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_FUNCTION);
            MmException.Try(result, "waveInOpen");
        }

        private void CloseWaveDevice()
        {
            if (Handle == IntPtr.Zero) return;
            var result = MMInterops.waveInReset(Handle);
            MmException.Try(result, "waveInReset");
            if (_buffers != null)
            {
                for (int i = 0; i < _buffers.Length; i++)
                {
                    _buffers[i].Dispose();
                }
            }
            result = MMInterops.waveInClose(Handle);
            MmException.Try(result, "waveInClose");
            handle = IntPtr.Zero;
        }

        private void ResetBuffers()
        {
            foreach (var item in _buffers)
            {
                if (!item.IsInQueue)
                {
                    item.Reset();
                }
            }
        }

        private void CreateBuffers()
        {
            int count = _bufferCount;
            int size = (int)WaveFormat.MillisecondsToBytes(BufferSizeMs);

            WaveInBuffer[] buffers = new WaveInBuffer[count];
            for (int i = 0; i < count; i++)
            {
                WaveInBuffer buffer = new WaveInBuffer(this, size);
                buffer.Initialize();
                buffers[i] = buffer;
            }
            _buffers = buffers;
        }

        protected virtual void Callback(IntPtr handle, WaveMsg msg, UIntPtr user, WaveHeader header, UIntPtr reserved)
        {
            if (msg == WaveMsg.WIM_DATA)
            {
                if (!stopped)
                {
                    var buffer = ((GCHandle)header.userData).Target as WaveInBuffer;
                    RaiseDataAvailable(buffer);
                    try
                    {
                        buffer.Reset();
                    }
                    catch (MmException)
                    {
                        stopped = true;
                        RaiseStopped();
                    }
                }
            }
            else if (msg == WaveMsg.WIM_CLOSE)
            {
                RaiseStopped();
            }
        }

        protected void RaiseDataAvailable(WaveInBuffer buffer)
        {
            if (DataAvailable != null && buffer.Recorded > 0)
            {
                DataAvailable(this, new DataAvailableEventArgs(buffer.Buffer, 0, buffer.Recorded, WaveFormat));
            }
        }

        protected void RaiseStopped()
        {
            if (Stopped != null)
            {
                Stopped(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!stopped)
                Stop();
            CloseWaveDevice();
        }

        ~WaveIn()
        {
            Dispose(false);
        }
    }
}