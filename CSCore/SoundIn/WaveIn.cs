using CSCore.SoundOut.MMInterop;
using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundIn
{
    [Obsolete("Use the WasapiCapture class instead.")]
    public class WaveIn : ISoundRecorder
    {
        internal static readonly WaveFormat DefaultFormat = new WaveFormat(44100, 16, 1, AudioEncoding.Pcm);

        public static WaveInCaps[] Devices
        {
            get { return WaveInCaps.GetCaps(); }
        }

        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        public event EventHandler<RecordingStoppedEventArgs> Stopped;

        protected internal IntPtr InternalHandle;
        protected WaveCallback CallbackProc;
        private WaveFormat _waveFormat;
        private int _bufferSizeMs = 100;
        protected bool IsStopped = true;

        private const int BufferCount = 3;

        protected WaveInBuffer[] Buffers;

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
            get { return IsStopped ? SoundIn.RecordingState.Stopped : SoundIn.RecordingState.Recording; }
        }

        public IntPtr Handle { get { return InternalHandle; } }

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
            if (!IsStopped)
                throw new InvalidOperationException("Recording has to be stopped");

            CloseWaveDevice();
            OpenWaveDevice(Device);
            CreateBuffers();
        }

        public void Start()
        {
            if (!IsStopped)
                throw new InvalidOperationException("Recording has to be stopped");
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Not initialized");

            ResetBuffers();
            var result = MMInterops.waveInStart(Handle);
            MmException.Try(result, "waveInStart");
            IsStopped = false;

            OnStarted();
        }

        protected virtual void OnStarted()
        {
        }

        public void Stop()
        {
            if (!IsStopped)
            {
                IsStopped = true;
                var result = MMInterops.waveInStop(Handle);
                MmException.Try(result, "waveInStop");
                OnStopping();
                foreach (var buffer in Buffers)
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
            CallbackProc = new WaveCallback(Callback);
            var result = MMInterops.waveInOpen(out InternalHandle, (IntPtr)device, _waveFormat, CallbackProc, IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_FUNCTION);
            MmException.Try(result, "waveInOpen");
        }

        private void CloseWaveDevice()
        {
            if (Handle == IntPtr.Zero) return;
            var result = MMInterops.waveInReset(Handle);
            MmException.Try(result, "waveInReset");
            if (Buffers != null)
            {
                for (int i = 0; i < Buffers.Length; i++)
                {
                    Buffers[i].Dispose();
                }
            }
            result = MMInterops.waveInClose(Handle);
            MmException.Try(result, "waveInClose");
            InternalHandle = IntPtr.Zero;
        }

        private void ResetBuffers()
        {
            foreach (var item in Buffers)
            {
                if (!item.IsInQueue)
                {
                    item.Reset();
                }
            }
        }

        private void CreateBuffers()
        {
            int count = BufferCount;
            int size = (int)WaveFormat.MillisecondsToBytes(BufferSizeMs);

            WaveInBuffer[] buffers = new WaveInBuffer[count];
            for (int i = 0; i < count; i++)
            {
                WaveInBuffer buffer = new WaveInBuffer(this, size);
                buffer.Initialize();
                buffers[i] = buffer;
            }
            Buffers = buffers;
        }

        protected virtual void Callback(IntPtr handle, WaveMsg msg, IntPtr user, WaveHeader header, IntPtr reserved)
        {
            if (msg == WaveMsg.WIM_DATA)
            {
                if (!IsStopped)
                {
                    var buffer = ((GCHandle)header.userData).Target as WaveInBuffer;
                    RaiseDataAvailable(buffer);
                    try
                    {
                        buffer.Reset();
                    }
                    catch (MmException)
                    {
                        IsStopped = true;
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

        protected void RaiseStopped() //todo: add possibility to add exceptions.
        {
            if (Stopped != null)
            {
                Stopped(this, new RecordingStoppedEventArgs(null));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsStopped)
                Stop();
            CloseWaveDevice();
        }

        ~WaveIn()
        {
            Dispose(false);
        }
    }
}