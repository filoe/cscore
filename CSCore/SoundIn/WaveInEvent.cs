using CSCore.SoundOut.MMInterop;
using System;
using System.Threading;

namespace CSCore.SoundIn
{
#pragma warning disable 612
    public class WaveInEvent : WaveIn
#pragma warning restore 612
    {
        private readonly AutoResetEvent _event;
        private Thread _thread;

        public WaveInEvent()
            : this(new WaveFormat())
        {
        }

        public WaveInEvent(WaveFormat waveFormat)
            : base(waveFormat)
        {
            _event = new AutoResetEvent(false);
        }

        protected override void OpenWaveDevice(int device)
        {
            var result = MMInterops.waveInOpenWithWindow(out InternalHandle, (IntPtr)device, WaveFormat, _event.SafeWaitHandle.DangerousGetHandle(), IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_EVENT);
            MmException.Try(result, "waveInOpen");
        }

        protected override void OnStarted()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(EventProc));
            _thread = new Thread(EventProc);
            _thread.Start();
        }

        protected override void OnStopped()
        {
            _event.Set();
        }

        protected override void OnStopping()
        {
            _event.Set();
            _thread.Join(100);
            _thread = null;
        }

        private void EventProc(object o)
        {
            while (!IsStopped)
            {
                if (_event.WaitOne())
                {
                    foreach (var buffer in Buffers)
                    {
                        if (buffer.Done)
                        {
                            RaiseDataAvailable(buffer);
                            buffer.Reset();
                        }
                    }
                }
            }
            IsStopped = true;
            RaiseStopped();
        }
    }
}