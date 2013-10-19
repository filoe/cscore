using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSCore.SoundOut.DirectSound
{
    public sealed class DirectSoundNotifyManager : IDisposable
    {
        private readonly DirectSoundBufferBase _buffer;
        private readonly Object _lockObject;
        private readonly Predicate<DirectSoundNotifyManager> _stopNotificationExpression;
        private readonly int _latency;
        private readonly int _bufferSize;

        private DirectSoundNotify _notify;
        private WaitHandle[] _waitHandles;
        private Thread _notifyThread;
        private DSBPositionNotify[] _positionNotifies;
        private bool _isinitialized;
        private bool _stopping;

        public event EventHandler<DirectSoundNotifyEventArgs> NotificationReceived;
        public event EventHandler Stopped;


        public DirectSoundNotifyManager(DirectSoundBufferBase buffer, int latency, int bufferSize)
            : this(buffer, latency, bufferSize, null)
        {
        }

        public DirectSoundNotifyManager(DirectSoundBufferBase buffer, int latency, int bufferSize, Predicate<DirectSoundNotifyManager> stopNotificationExpression)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if((buffer.BufferCaps.dwFlags & DSBufferFlags.ControlPositionNotify) != DSBufferFlags.ControlPositionNotify)
                throw new ArgumentException("Buffer does not support PositionNotify.");

            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            if (stopNotificationExpression == null)
                _stopNotificationExpression = new Predicate<DirectSoundNotifyManager>(x => _stopping);
            else
                _stopNotificationExpression = new Predicate<DirectSoundNotifyManager>(x => stopNotificationExpression(this) || _stopping);

            _buffer = buffer;
            _latency = latency;
            _bufferSize = bufferSize;
            _lockObject = new Object();
        }

        public void Start()
        {
            lock (_lockObject)
            {
                CheckForInitialize();

                if (_notifyThread != null)
                    throw new InvalidOperationException("NotifyManager is already running"); //todo:

                _notifyThread = new Thread(NotifyProc);
                _notifyThread.Name = "DirectSoundNotifyManager Thread: ID = 0x" + _notify.BasePtr.ToInt64().ToString("x");
                _notifyThread.Priority = ThreadPriority.AboveNormal;
                _notifyThread.Start();

                Debug.WriteLine("DirectSoundNotifyManager started");
            }
        }

        public void Stop()
        {
            Stop(5 * _latency); //waithandles.length
        }

        public void Stop(int timeout)
        {
            lock (_lockObject)
            {
                _stopping = true;
                if (_notifyThread != null)
                {
                    if (_notifyThread != Thread.CurrentThread)
                    {
                        bool r = _notifyThread.Join(timeout);
                        if (r)
                            return;
                    }

                    _notifyThread.Abort();
                    _notifyThread = null;
                }
            }
        }

        public void Initialize()
        {
            lock (_lockObject)
            {
                Uninitialize();

                var notify = _buffer.QueryInterface<DirectSoundNotify>();

                var waitHandleNull = new EventWaitHandle(false, EventResetMode.AutoReset);
                var waitHandle0 = new EventWaitHandle(false, EventResetMode.AutoReset);
                /*var waitHandle1 = new EventWaitHandle(false, EventResetMode.AutoReset);
                var waitHandle2 = new EventWaitHandle(false, EventResetMode.AutoReset);*/
                var waitHandleEnd = new EventWaitHandle(false, EventResetMode.AutoReset);

                /*DSBPositionNotify[] positionNotifies = new DSBPositionNotify[5];
                positionNotifies[0] = new DSBPositionNotify(0, waitHandleNull.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[1] = new DSBPositionNotify((uint)_bufferSize / 2, waitHandle0.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[2] = new DSBPositionNotify((uint)_bufferSize, waitHandle1.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[3] = new DSBPositionNotify((uint)(_bufferSize + _bufferSize / 2), waitHandle2.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[4] = new DSBPositionNotify(0xFFFFFFFF, waitHandleEnd.SafeWaitHandle.DangerousGetHandle());*/

                DSBPositionNotify[] positionNotifies = new DSBPositionNotify[3];
                positionNotifies[0] = new DSBPositionNotify(0, waitHandleNull.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[1] = new DSBPositionNotify((uint)_bufferSize, waitHandle0.SafeWaitHandle.DangerousGetHandle());
                positionNotifies[2] = new DSBPositionNotify(0xFFFFFFFF, waitHandleEnd.SafeWaitHandle.DangerousGetHandle());

                var result = notify.SetNotificationPositions(positionNotifies);
                DirectSoundException.Try(result, "IDirectSoundNotify", "SetNotificationPositions");

                _positionNotifies = positionNotifies;
                //_waitHandles = new WaitHandle[] { waitHandleNull, waitHandle0, waitHandle1, waitHandle2, waitHandleEnd };
                _waitHandles = new WaitHandle[] { waitHandleNull, waitHandle0, waitHandleEnd };

                _notify = notify;
                _isinitialized = true;
            }
        }

        private void NotifyProc()
        {
            try
            {
                _stopping = false;
                while (!_stopNotificationExpression(this))
                {
                    int handleIndex = WaitHandle.WaitAny(_waitHandles, _waitHandles.Length * _latency, true);
                    if (!RaiseNotificationReceived(handleIndex))
                        break;
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                Debug.WriteLine("DirectSoundNotifyManager::NotifyProc: " + ex.ToString(), "WasapiOut.PlaybackProc");
                if (Debugger.IsAttached)
                    throw new Exception("Unhandled exception in directsound playback proc. See innerexception for details.", ex);
            }
            finally
            {
                RaiseStopped();
                _notifyThread = null;
                Debug.WriteLine("DirectSoundNotifyManager stopped successfully.");
            }
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
                if (_waitHandles != null)
                {
                    foreach (var w in _waitHandles)
                    {
                        w.Close();
                    }
                    _waitHandles = null;
                }

                _isinitialized = false;
            }
        }

        private bool RaiseNotificationReceived(int handleIndex)
        {
            if (NotificationReceived != null)
            {
                bool isTimeOut = handleIndex == WaitHandle.WaitTimeout;
                bool bufferStopped = handleIndex == _waitHandles.Length - 1;
                int sampleOffset = (handleIndex == 0 ? 1 : 0) * _bufferSize;

                var args = new DirectSoundNotifyEventArgs(handleIndex, sampleOffset, _bufferSize, isTimeOut, bufferStopped);

                NotificationReceived(this, args);
                return !args.RequestStopPlayback;
            }
            return false;
        }

        private void RaiseStopped()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }

        private void CheckForInitialize()
        {
            if (!_isinitialized)
                throw new InvalidOperationException("DirectSoundNotifyManager is not initialized.");
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
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
