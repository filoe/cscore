using System;
using System.Diagnostics;
using System.Threading;

namespace CSCore.SoundOut.DirectSound
{
    public class DirectSoundNotifyManager : IDisposable
    {
        public event EventHandler<DirectSoundNotifyEventArgs> NotifyAnyRaised;

        public event EventHandler Stopped;

        private DirectSoundSecondaryBuffer _buffer;
        private WaveFormat _waveFormat;
        private int _bufferSize;
        private DSBPositionNotify[] _positionNotifies;
        private WaitHandle[] _waitHandles;
        private bool _disposing;
        private Func<object, bool> _hasToStop;
        private int _latency;
        private Object _lockObject = new Object();

        private Thread _thread;

        private DirectSoundNotify _notify;

        /// <summary>
        /// Was the notifymanager ever started?
        /// </summary>
        public bool GotStarted { get; private set; }

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
        }

        public void Initialize()
        {
            lock (_lockObject)
            {
                if (_notify != null)
                {
                    _notify.Dispose();
                    _notify = null;
                }

                var notify = _buffer.QueryInterface<DirectSoundNotify>();

                var waitHandleNull = new EventWaitHandle(false, EventResetMode.AutoReset);
                var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                var waitHandleEnd = new EventWaitHandle(false, EventResetMode.AutoReset);

                DSBPositionNotify[] positionNotifies = new DSBPositionNotify[3];
                positionNotifies[0] = new DSBPositionNotify(0, waitHandleNull.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[1] = new DSBPositionNotify((uint)_bufferSize, waitHandle.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[2] = new DSBPositionNotify(0xFFFFFFFF, waitHandleEnd.SafeWaitHandle.DangerousGetHandle());

                var result = notify.SetNotificationPositions(positionNotifies);
                DirectSoundException.Try(result, "IDirectSoundNotify", "SetNotificationPositions");

                _positionNotifies = positionNotifies;
                _waitHandles = new WaitHandle[] { waitHandleNull, waitHandle, waitHandleEnd };

                _latency = (int)(_bufferSize / (float)_waveFormat.BytesPerSecond * 1000);

                _notify = notify;
            }

            Debug.WriteLine("DirectSoundNotifyManager initialized.");
        }

        public void Start()
        {
            lock (_lockObject)
            {
                if (_thread != null)
                    return;

                _thread = new Thread(NotifyProc);
                _thread.Name = "DirectSoundNotifyManager Thread: ID = 0x" + _notify.BasePtr.ToInt64().ToString("x");
                _thread.Priority = ThreadPriority.AboveNormal;
                //_thread.IsBackground = true;
                _thread.Start();
                Debug.WriteLine("DirectSoundNotifyManager started");
            }
        }

        private void NotifyProc()
        {
            try
            {
                GotStarted = true;
                _disposing = false;
                while (true)
                {
                    if (_hasToStop(this))
                        break;

                    int handleIndex = WaitHandle.WaitAny(_waitHandles, _waitHandles.Length * _latency, true);

                    if (!RaiseNotifyAnyRaised(handleIndex))
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DirectSoundNotifyManager::NotifyProc: " + ex.ToString(), "WasapiOut.PlaybackProc");
                if (Debugger.IsAttached)
                    throw new Exception("Unhandled exception in directsound playback proc. See innerexception for details.", ex);
            }
            finally
            {
                RaiseStopped();
                _thread = null;
                Debug.WriteLine("DirectSoundNotifyManager stopped successfully");
            }
        }

        public bool Stop()
        {
            int timeout = 3 * _latency; //_waitHandles.Length??
            return Stop(timeout);
        }

        public bool Stop(int timeout)
        {
            _disposing = true;
            lock (_lockObject)
            {
                if (_thread != null)
                {
                    if (_thread != Thread.CurrentThread && !_thread.Join(timeout))
                    {
                        Debug.WriteLine(String.Format("DirectSoundNotifyManager stop failed: timeout after {0} ms", timeout));
                        return false;
                    }
                    else
                    {
                        _thread.Abort();
                    }
                    _thread = null;
                }
                GotStarted = false;
                return true;
            }
        }

        public void Abort()
        {
            lock (_lockObject)
            {
                if (_thread != null)
                {
                    if (!_thread.IsAlive)
                    {
                        _disposing = true;
                        _thread.Abort();
                    }
                    GotStarted = false;
                    _thread = null;
                }
            }
        }

        protected bool RaiseNotifyAnyRaised(int handleIndex)
        {
            if (NotifyAnyRaised != null)
            {
                var e = new DirectSoundNotifyEventArgs(handleIndex, _bufferSize);
                NotifyAnyRaised(this, e);
                return !e.RequestStopPlayback;
            }
            return false;
        }

        protected void RaiseStopped()
        {
            if (Stopped != null)
                Stopped(this, new EventArgs());
        }

        public void WaitForStopped()
        {
            try
            {
                if (_thread != null && _thread.IsAlive)
                    _thread.Join();
            }
            catch (Exception)
            {
            }
        }

        public bool WaitForStopped(int timeout)
        {
            try
            {
                if (_thread != null && _thread.IsAlive)
                    return _thread.Join(timeout);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsNotifyThread(Thread thread)
        {
            if (thread == null)
                throw new ArgumentNullException("thread");
            return thread.ManagedThreadId == thread.ManagedThreadId;
        }

        private void Uninitialize()
        {
            lock (_lockObject)
            {
                if (_notify != null)
                {
                    _notify.Dispose();
                    _notify = null;
                }
                /*if (_waitHandles != null)
                {
                    foreach (var w in _waitHandles)
                    {
                        w.Close();
                    }
                }*/
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Stop();
                Uninitialize();
            }
            _disposed = true;
        }

        ~DirectSoundNotifyManager()
        {
            Dispose(false);
        }
    }
}