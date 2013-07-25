using CSCore.SoundOut.MmInterop;
using System;
using System.Threading;

namespace CSCore.SoundIn
{
    public class WaveInEvent : WaveIn
    {
        AutoResetEvent _event;
        Thread _thread;

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
            var result = MMInterops.waveInOpenWithWindow(out handle, (IntPtr)device, WaveFormat, _event.SafeWaitHandle.DangerousGetHandle(), IntPtr.Zero, MMInterops.WaveInOutOpenFlags.CALLBACK_EVENT);
            Context.Current.Logger.MMResult(result, "waveInOpen", "WaveInEvent.OpenWaveDevice", Utils.Logger.LogDispatcher.MMLogFlag.LogAlways | Utils.Logger.LogDispatcher.MMLogFlag.ThrowOnError);
        }

        protected override void OnStart()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(EventProc));
            _thread = new Thread(new ParameterizedThreadStart(EventProc));
            _thread.Start();
        }

        protected override void OnStop()
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
            while (!stopped)
            {
                if (_event.WaitOne())
                {
                    foreach (var buffer in _buffers)
                    {
                        if (buffer.Done)
                        {
                            RaiseDataAvailable(buffer);
                            buffer.Reset();
                        }
                    }
                }
            }
            stopped = true;
            RaiseStopped();
        }
    }
}
