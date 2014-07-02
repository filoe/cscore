using System;
using System.Diagnostics;
using System.Threading;
using CSCore.SoundOut.DirectSound;

namespace CSCore.SoundOut
{
    /// <summary>
    ///     Provides audioplayback through DirectSound.
    /// </summary>
    public class DirectSoundOut : ISoundOut
    {
        private readonly ThreadPriority _playbackThreadPriority;
        private readonly SynchronizationContext _syncContext;
        private Guid _device;

        private DirectSound8 _directSound;

        private DirectSoundNotify _directSoundNotify;
        private bool _isDisposed;

        private int _latency;
        private volatile PlaybackState _playbackState;
        private Thread _playbackThread;
        private DirectSoundPrimaryBuffer _primaryBuffer;
        private DirectSoundSecondaryBuffer _secondaryBuffer;
        private IWaveSource _source;

        /// <summary>
        ///     Initializes an new instance of <see cref="DirectSoundOut"/> class.
        ///     Latency = 100.
        ///     EventSyncContext = SynchronizationContext.Current.
        ///     PlaybackThreadPriority = AboveNormal.
        /// </summary>
        public DirectSoundOut()
            : this(100)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="DirectSoundOut"/> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        ///     PlaybackThreadPriority = AboveNormal.
        /// </summary>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        public DirectSoundOut(int latency)
            : this(latency, ThreadPriority.AboveNormal)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="DirectSoundOut"/> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        /// </summary>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <param name="playbackThreadPriority">
        ///     ThreadPriority of the playbackthread which runs in background and feeds the device
        ///     with data.
        /// </param>
        public DirectSoundOut(int latency, ThreadPriority playbackThreadPriority)
            : this(latency, playbackThreadPriority, SynchronizationContext.Current)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="DirectSoundOut"/> class.
        /// </summary>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <param name="playbackThreadPriority">
        ///     ThreadPriority of the playbackthread which runs in background and feeds the device
        ///     with data.
        /// </param>
        /// <param name="eventSyncContext">
        ///     The synchronizationcontext which is used to raise any events like the "Stopped"-event.
        ///     If the passed value is not null, the events will be called async through the SynchronizationContext.Post() method.
        /// </param>
        public DirectSoundOut(int latency, ThreadPriority playbackThreadPriority,
            SynchronizationContext eventSyncContext)
        {
            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");

            Latency = latency;
            _playbackThreadPriority = playbackThreadPriority;
            _syncContext = eventSyncContext;
            Device = DirectSoundDevice.DefaultDevice.Guid;
        }

        /// <summary>
        ///     Random ID based on the internal directsounds memory address for debugging purposes.
        /// </summary>
        public long DebuggingId
        {
            get { return _directSound != null ? _directSound.BasePtr.ToInt64() : -1; }
        }

        /// <summary>
        ///     Latency of the playback specified in milliseconds.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _latency = value;
            }
        }

        /// <summary>
        /// Gets or sets the device to use for the playing the waveform-audio data. Note that the <see cref="Initialize"/> method has to get called
        /// </summary>
        public Guid Device
        {
            get { return _device; }
            set { _device = value; }
        }

        /// <summary>
        ///     Occurs when the playback gets stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        ///     Initializes <see cref="DirectSoundOut"/> and prepares all resources for playback.
        ///     Note that all properties like <see cref="Device"/>, <see cref="Latency"/>,... won't affect <see cref="DirectSoundOut"/> after calling <see cref="Initialize"/>.
        /// </summary>
        /// <param name="source">The source to prepare for playback.</param>
        public void Initialize(IWaveSource source)
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (source == null)
                throw new ArgumentNullException("source");

            if (_playbackState != PlaybackState.Stopped)
            {
                throw new InvalidOperationException(
                    "PlaybackState has to be Stopped. Call DirectSoundOut::Stop to stop the playback.");
            }

            _playbackThread.WaitForExit();

            //todo: implement _isInitialized behaviour. note that _isInitialized = ... has to be uncommented in the whole document.
            //if (_isInitialized)
            //    throw new InvalidOperationException("DirectSoundOut is already initialized. Call DirectSoundOut::Stop to uninitialize DirectSoundOut.");


            _source = source;

            CleanupRessources();
            InitializeInternal();
            //_isInitialized = true;

            Volume = 1.0f;
        }

        /// <summary>
        ///     Starts the playback.
        ///     Note: <see cref="Initialize"/> has to get called before calling Play.
        ///     If PlaybackState is Paused, Resume() will be called automatically.
        /// </summary>
        public void Play()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (PlaybackState == PlaybackState.Stopped)
            {
                using (var waitHandle = new AutoResetEvent(false))
                {
                    _playbackThread = new Thread(PlaybackProc)
                    {
                        Name = "DirectSound Playback-Thread; ID = " + DebuggingId,
                        Priority = _playbackThreadPriority
                    };
                    _playbackThread.Start(waitHandle);
                    waitHandle.WaitOne();
                }
            }
            else if (PlaybackState == PlaybackState.Paused)
                Resume();
        }

        /// <summary>
        ///     Stops the playback and frees all allocated resources.
        ///     After calling <see cref="Stop"/> the caller has to call <see cref="Initialize"/> again before another playback can be started.
        /// </summary>
        public void Stop()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (_playbackState != PlaybackState.Stopped)
            {
                _playbackState = PlaybackState.Stopped;
                _playbackThread.WaitForExit();
                _playbackThread = null;
            }
            else if (_playbackThread != null)
            {
                _playbackThread.WaitForExit();
                _playbackThread = null;
            }
            else
                Debug.WriteLine("DirectSoundOut is already stopped.");
        }

        /// <summary>
        ///     Resumes the paused playback.
        /// </summary>
        public void Resume()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (_playbackState == PlaybackState.Paused)
                _playbackState = PlaybackState.Playing;
        }

        /// <summary>
        ///     Pauses the playback.
        /// </summary>
        public void Pause()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (PlaybackState == PlaybackState.Playing)
                _playbackState = PlaybackState.Paused;
        }

        /// <summary>
        ///     Gets the current <see cref="SoundOut.PlaybackState"/> of the playback.
        /// </summary>
        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
        }

        /// <summary>
        ///     The volume of the playback. Valid values are from 0.0 (0%) to 1.0 (100%).
        /// </summary>
        /// <remarks>
        ///     Note that the if you for example set a volume of 33% => 0.33, the actual volume will be something like 0.33039999.
        /// </remarks>
        public float Volume
        {
            get
            {
                CheckForDisposed();
                return _secondaryBuffer != null ? (float) _secondaryBuffer.GetVolume() : 1;
            }
            set
            {
                CheckForDisposed();

                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("value");
                if (_secondaryBuffer != null)
                    _secondaryBuffer.SetVolume(value);
            }
        }

        /// <summary>
        ///     The currently initialized source.
        ///     To change the WaveSource property, call <see cref="Initialize"/>.
        /// </summary>
        public IWaveSource WaveSource
        {
            get { return _source; }
        }

        /// <summary>
        /// Disposes the <see cref="DirectSoundOut"/> instance and stops the playbacks.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void InitializeInternal()
        {
            //Use Desktophandle as default handle
            IntPtr handle = DSUtils.GetDesktopWindow();

            IntPtr pdsound;
            DirectSoundException.Try(NativeMethods.DirectSoundCreate8(ref _device, out pdsound, IntPtr.Zero),
                "DSInterop", "DirectSoundCreate8");

            _directSound = new DirectSound8(pdsound);
            _directSound.SetCooperativeLevel(handle, DSCooperativeLevelType.DSSCL_NORMAL); //use normal as default
            if (!_directSound.SupportsFormat(_source.WaveFormat))
            {
                if (_source.WaveFormat.WaveFormatTag == AudioEncoding.IeeeFloat)
                    _source = _source.ToSampleSource().ToWaveSource(16);
                if (
                    _directSound.SupportsFormat(new WaveFormat(_source.WaveFormat.SampleRate, 16,
                        _source.WaveFormat.Channels, _source.WaveFormat.WaveFormatTag)))
                    _source = _source.ToSampleSource().ToWaveSource(16);
                else if (
                    _directSound.SupportsFormat(new WaveFormat(_source.WaveFormat.SampleRate, 8,
                        _source.WaveFormat.Channels, _source.WaveFormat.WaveFormatTag)))
                    _source = _source.ToSampleSource().ToWaveSource(8);
                else
                {
                    throw new InvalidOperationException(
                        "Invalid WaveFormat. WaveFormat specified by parameter {_source} is not supported by this DirectSound-Device.");
                }

                if (!_directSound.SupportsFormat(_source.WaveFormat))
                {
                    throw new InvalidOperationException(
                        "Invalid WaveFormat. WaveFormat specified by parameter {_source} is not supported by this DirectSound-Device.");
                }
            }

            WaveFormat waveFormat = _source.WaveFormat;
            var bufferSize = (int) waveFormat.MillisecondsToBytes(_latency);

            _primaryBuffer = new DirectSoundPrimaryBuffer(_directSound);
            _secondaryBuffer = new DirectSoundSecondaryBuffer(_directSound, waveFormat, bufferSize);
        }

        private void PlaybackProc(object o)
        {
            var waitHandle = o as EventWaitHandle;
            WaitHandle[] waitHandles = null;

            try
            {
                //004
                //bool flag = true;
                int bufferSize = _secondaryBuffer.BufferCaps.dwBufferBytes;
                var latencyBytes = (int) _source.WaveFormat.MillisecondsToBytes(_latency);
                var buffer = new byte[bufferSize];

                _primaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING); //default flags: looping

                //003
                /*if (flag) //could refill buffer
                {*/
                /*
                     * Setup notify
                     */
                var waitHandleNull = new EventWaitHandle(false, EventResetMode.AutoReset);
                var waitHandle0 = new EventWaitHandle(false, EventResetMode.AutoReset);
                var waitHandleEnd = new EventWaitHandle(false, EventResetMode.AutoReset);

                waitHandles = new WaitHandle[] {waitHandleNull, waitHandle0, waitHandleEnd};

                _directSoundNotify = _secondaryBuffer.QueryInterface<DirectSoundNotify>();
                DSBPositionNotify[] positionNotifies =
                {
                    new DSBPositionNotify
                    {
                        dwOffset = DSBPositionNotify.OffsetZero,
                        hEventNotify = waitHandleNull.SafeWaitHandle.DangerousGetHandle()
                    },
                    new DSBPositionNotify
                    {
                        dwOffset = (uint) _source.WaveFormat.MillisecondsToBytes(_latency),
                        hEventNotify = waitHandle0.SafeWaitHandle.DangerousGetHandle()
                    },
                    new DSBPositionNotify
                    {
                        dwOffset = DSBPositionNotify.OffsetEnd,
                        hEventNotify = waitHandleEnd.SafeWaitHandle.DangerousGetHandle()
                    }
                };
                _directSoundNotify.SetNotificationPositions(positionNotifies);
                int waitHandleTimeout = waitHandles.Length * _latency;

                //001
                /*if (PlaybackState == SoundOut.PlaybackState.Stopped)
                    {
                        _secondaryBuffer.SetCurrentPosition(0);
                        flag = RefillBuffer(buffer, 0, bufferSize);
                    }*/
                //002
                _secondaryBuffer.SetCurrentPosition(0);

                _secondaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING); //default flags: looping

                _playbackState = PlaybackState.Playing;

                if (waitHandle != null)
                    waitHandle.Set();


                while (PlaybackState != PlaybackState.Stopped)
                {
                    int waitHandleIndex = WaitHandle.WaitAny(waitHandles, waitHandleTimeout, true);
                    bool isTimeOut = waitHandleIndex == WaitHandle.WaitTimeout;

                    //dsound stopped
                    //case of end of buffer or Stop() called: http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.dsbpositionnotify(v=vs.85).aspx
                    bool isBufferStopped = waitHandleIndex == (waitHandles.Length - 1);

                    int sampleOffset = (waitHandleIndex == 0 ? 1 : 0) * latencyBytes;
                    //bug: sampleOffset = count

                    if (isTimeOut == false && isBufferStopped == false)
                    {
                        /*
                             * Refill the buffer
                             */
                        if (RefillBuffer(buffer, sampleOffset, latencyBytes) == false)
                            _playbackState = PlaybackState.Stopped;
                    }
                    else
                        _playbackState = PlaybackState.Stopped;
                }
                /*}
                else
                {
                    _playbackState = SoundOut.PlaybackState.Stopped;
                }*/
            }
            finally
            {
                if (_directSoundNotify != null)
                {
                    _directSoundNotify.Dispose();
                    _directSoundNotify = null;
                }
                if (_secondaryBuffer != null)
                    _secondaryBuffer.Stop();
                if (_primaryBuffer != null)
                    _primaryBuffer.Stop();

                if (waitHandles != null)
                {
                    foreach (var waitHandle1 in waitHandles)
                    {
                        var wh = (EventWaitHandle) waitHandle1;
                        wh.Close();
                    }
                }

                RaiseStopped();
            }
        }

        private bool RefillBuffer(byte[] buffer, int sampleOffset, int bufferSize)
        {
            int read;

            if (_secondaryBuffer.IsBufferLost())
                _secondaryBuffer.Restore();

            if (_playbackState == PlaybackState.Paused)
            {
                Array.Clear(buffer, 0, buffer.Length);
                read = bufferSize;
            }
            else
            {
                if (_source != null)
                    read = _source.Read(buffer, 0, bufferSize);
                else
                    return false;
            }

            if (read > 0)
                return _secondaryBuffer.Write(buffer, sampleOffset, bufferSize);
            return false;
        }

        private void RaiseStopped()
        {
            if (Stopped == null)
                return;

            if (_syncContext != null)
                _syncContext.Post(x => Stopped(this, EventArgs.Empty), null); //maybe post?
            else
                Stopped(this, EventArgs.Empty);
        }

        private void CleanupRessources()
        {
            if (_directSoundNotify != null)
            {
                _directSoundNotify.Dispose();
                _directSoundNotify = null;
            }
            if (_secondaryBuffer != null)
            {
                _secondaryBuffer.Stop();
                _secondaryBuffer.Dispose();
                _secondaryBuffer = null;
            }
            if (_primaryBuffer != null)
            {
                _primaryBuffer.Stop();
                _primaryBuffer.Dispose();
                _primaryBuffer = null;
            }

            if (_directSound != null)
            {
                _directSound.Dispose();
                _directSound = null;
            }

            //_isInitialized = false;
        }

        private void CheckForInvalidThreadCall()
        {
            if (Thread.CurrentThread == _playbackThread)
                throw new InvalidOperationException("You must not access this method from the PlaybackThread.");
        }

        private void CheckForDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("WasapiOut");
        }

        /// <summary>
        /// Disposes and stops the <see cref="DirectSoundOut"/> instance.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                Stop();
                CleanupRessources();
            }
            _isDisposed = true;
        }

        /// <summary>
        /// Destructor which calls the <see cref="Dispose(bool)"/> method.
        /// </summary>
        ~DirectSoundOut()
        {
            Dispose(false);
        }
    }
}