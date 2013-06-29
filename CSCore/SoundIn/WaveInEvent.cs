using CSCore.SoundOut.Interop;
using System;
using System.Threading;

namespace CSCore.SoundIn
{
    public class WaveInEvent : WaveIn
    {
        AutoResetEvent _event;

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
            ThreadPool.QueueUserWorkItem(new WaitCallback(EventProc));
        }

        protected override void OnStop()
        {
            _event.Set();
        }

        private void EventProc(object o)
        {
            try
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
            }
            catch (Exception)
            {
            }
            finally
            {
                stopped = true;
                RaiseStopped();
            }
        }
    }
}
