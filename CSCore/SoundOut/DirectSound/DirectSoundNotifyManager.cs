using System;
using System.Threading;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundNotifyManager : IDisposable
    {
        public event EventHandler<DirectSoundNotifyEventArgs> NotifyAnyRaised;
        public event EventHandler Stopped;

        DirectSoundSecondaryBuffer _buffer;
        WaveFormat _waveFormat;
        int _bufferSize;
        DSBPositionNotify[] _positionNotifies;
        WaitHandle[] _waitHandles;
        bool _disposing;
        Func<object, bool> _hasToStop;
        int _latency;

        Thread _thread;

        DirectSoundNotify _notify;

        public DirectSoundNotifyManager(DirectSoundSecondaryBuffer buffer, WaveFormat waveFormat, int bufferSize)
            : this(buffer, waveFormat, bufferSize, null)
        {
        }

        public DirectSoundNotifyManager(DirectSoundSecondaryBuffer buffer, WaveFormat waveFormat, int bufferSize, Func<object, bool> stopEvaluation = null)
        {
            if (stopEvaluation == null) _hasToStop = new Func<object, bool>((sender) => _disposing);
            else
            {
                _hasToStop = new Func<object, bool>((s) =>
                {
                    return stopEvaluation(this) || _disposing;
                });
            }

            _buffer = buffer;
            _waveFormat = waveFormat;
            _bufferSize = bufferSize;

            var notify = buffer.QueryInterface<DirectSoundNotify>();

            var waitHandleNull = new EventWaitHandle(false, EventResetMode.AutoReset);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            var waitHandleEnd = new EventWaitHandle(false, EventResetMode.AutoReset);

            DSBPositionNotify[] positionNotifies = new DSBPositionNotify[3];
            positionNotifies[0] = new DSBPositionNotify(0, waitHandleNull.SafeWaitHandle.DangerousGetHandle());
            positionNotifies[1] = new DSBPositionNotify((uint)bufferSize, waitHandle.SafeWaitHandle.DangerousGetHandle());
            positionNotifies[2] = new DSBPositionNotify(0xFFFFFFFF, waitHandleEnd.SafeWaitHandle.DangerousGetHandle());

            var result = notify.SetNotificationPositions(positionNotifies);
            DirectSoundException.Try(result, "IDirectSoundNotify", "SetNotificationPositions");

            _positionNotifies = positionNotifies;
            _waitHandles = new WaitHandle[] { waitHandleNull, waitHandle, waitHandleEnd };

            _latency = (int)(_bufferSize / (float)_waveFormat.BytesPerSecond * 1000);

            _notify = notify;

            Context.Current.Logger.Debug("DirectSoundNotifyManager initialized", "DirectSoundNotifyManager.ctor(DirectSoundSecondaryBuffer, WaveFormat, int, Func<object, bool>)");
        }

        public void Start()
        {
            if (_thread != null) return;

            _thread = new Thread(NotifyProc);
            _thread.Name = "DirectSoundNotifyManager Thread: ID = 0x" + _notify.BasePtr.ToInt64().ToString("x");
            _thread.Priority = ThreadPriority.AboveNormal;
            _thread.Start();
            Context.Current.Logger.Debug("DirectSoundNotifyManager started", "DirectSoundNotifyManager.Start()");
        }

        private void NotifyProc()
        {
            while (true)
            {
                int handleIndex = WaitHandle.WaitAny(_waitHandles, _waitHandles.Length * _latency, false);
                if (_hasToStop(this)) 
                    break;
                RaiseNotifyAnyRaised(handleIndex);
            }

            RaiseStopped();
            _thread = null;
        }

        public void Stop()
        {
            Stop(_waitHandles.Length * _latency);
        }

        public bool Stop(int timeout)
        {
            _disposing = true;
            if (_thread != null)
            {
                if (!_thread.Join(timeout))
                {
                    Context.Current.Logger.Error(String.Format("DirectSoundNotifyManager stop failed: timeout after {0} ms", timeout), "DirectSoundNotifyManager.Stop()");
                    return false;
                }
                _thread = null;
            }
            Context.Current.Logger.Debug("DirectSoundNotifyManager stopped successfully", "DirectSoundNotifyManager.Stop(int)");
            return true;
        }

        protected void RaiseNotifyAnyRaised(int handleIndex)
        {
            if (NotifyAnyRaised != null)
                NotifyAnyRaised(this, new DirectSoundNotifyEventArgs(handleIndex, _bufferSize));
        }

        protected void RaiseStopped()
        {
            if (Stopped != null)
                Stopped(this, new EventArgs());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();
            _notify.Dispose();
        }

        ~DirectSoundNotifyManager()
        {
            Dispose(false);
        }
    }
}
