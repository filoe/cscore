using CSCore.SoundOut.DirectSound;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSCore.SoundOut
{
    public class DirectSoundOut : ISoundOut
    {
        protected IWaveSource _waveSource;
        protected volatile DirectSound8 _directSound;
        protected volatile DirectSoundPrimaryBuffer _primaryBuffer;
        protected volatile DirectSoundSecondaryBuffer _secondaryBuffer;
        protected volatile DirectSoundNotifyManager _notifyManager;
        protected volatile PlaybackState _playbackState = PlaybackState.Stopped;
        private SynchronizationContext _syncContext;
        private Guid _device;
        private int _latency;
        private bool _isinitialized;
        protected object _lockObj;

        public event EventHandler Stopped;

        private byte[] _buffer;

        public int Latency
        {
            get
            {
                return _latency;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _latency = value;
            }
        }

        public Guid Device
        {
            get { return _device; }
            set { _device = value; }
        }

        public DirectSoundOut()
            : this(DirectSoundDevice.DefaultPlaybackGuid)
        {
        }

        public DirectSoundOut(Guid device)
            : this(device, 100)
        {
        }

        public DirectSoundOut(Guid device, int latency)
        {
            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");

            Device = device;
            Latency = latency;
            _lockObj = new Object();
            _syncContext = SynchronizationContext.Current;
        }

        public void Play()
        {
            CheckForInitialize();
            if (_playbackState == SoundOut.PlaybackState.Stopped)
            {
                //lock (_lockObj)
                //{
                    _secondaryBuffer.SetCurrentPosition(0);
                    _secondaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);
                    PlaybackState = SoundOut.PlaybackState.Playing;
                    _notifyManager.Start();
                    Debug.WriteLine("DirectSoundOut playback started", "DirectSoundOut.Play()");
                //}
            }

            PlaybackState = SoundOut.PlaybackState.Playing;
        }

        public void Pause()
        {
            CheckForInitialize();
            PlaybackState = SoundOut.PlaybackState.Paused;
        }

        public void Resume()
        {
            Play();
        }

        public void Stop()
        {
            CheckForInitialize();

            if (Monitor.TryEnter(_lockObj, 50))
            {
                PlaybackState = SoundOut.PlaybackState.Stopped;
                Monitor.Exit(_lockObj);
                return;
            }

            if (_notifyManager != null && _notifyManager.GotStarted)
            {
                /*
                 * Will call stop event which will call StopInternal
                 */
                if (_notifyManager.Stop() == false) //if (_notifyManager.Stop(Int32.MaxValue) == false)
                {
                    _notifyManager.Abort();
                    StopInternal(); //abort manually 
                }

                _notifyManager.Dispose();
                _notifyManager = null;
                PlaybackState = SoundOut.PlaybackState.Stopped;
            }
            //StopInternal();
        }

        public void Initialize(IWaveSource source)
        {
            lock (_lockObj)
            {
                if (source == null) 
                    throw new ArgumentNullException("source");
                StopInternal();

                IntPtr handle = DSUtils.GetDesktopWindow();

                Guid device = Device;
                IntPtr ptrdsound;
                DirectSoundException.Try(NativeMethods.DirectSoundCreate8(ref device, out ptrdsound, IntPtr.Zero), "DSInterop", "DirectSoundCreate8(ref Guid, out IntPtr, IntPtr)");

                //create directsound
                _directSound = new DirectSound8(ptrdsound);
                DirectSoundException.Try(_directSound.SetCooperativeLevel(handle, DSCooperativeLevelType.DSSCL_NORMAL),
                    "IDirectSound8", "SetCooperativeLevel");
                if (!_directSound.SupportsFormat(source.WaveFormat))
                {
                    if (source.WaveFormat.WaveFormatTag == AudioEncoding.IeeeFloat)
                        source = source.ToSampleSource().ToWaveSource(16);
                    if (_directSound.SupportsFormat(new WaveFormat(source.WaveFormat.SampleRate, 16, source.WaveFormat.Channels, source.WaveFormat.WaveFormatTag)))
                        source = source.ToSampleSource().ToWaveSource(16);
                    else if (_directSound.SupportsFormat(new WaveFormat(source.WaveFormat.SampleRate, 8, source.WaveFormat.Channels, source.WaveFormat.WaveFormatTag)))
                        source = source.ToSampleSource().ToWaveSource(8);
                    else
                        throw new FormatException("Invalid WaveFormat. WaveFormat specified by parameter {source} is not supported by this DirectSound-Device");
                }

                _waveSource = source;
                WaveFormat waveFormat = _waveSource.WaveFormat;
                int bufferSize = (int)waveFormat.MillisecondsToBytes(_latency);

                //create buffers
                _primaryBuffer = new DirectSoundPrimaryBuffer(_directSound);
                _secondaryBuffer = new DirectSoundSecondaryBuffer(_directSound, waveFormat, bufferSize, false); //remove true

                _primaryBuffer.Play(DSBPlayFlags.None);

                DSBufferCaps bufferCaps;
                DirectSoundException.Try(_secondaryBuffer.GetCaps(out bufferCaps), "IDirectSoundBuffer", "GetCaps");
                _buffer = _buffer.CheckBuffer(bufferCaps.dwBufferBytes); //[bufferCaps.dwBufferBytes];

                _notifyManager = new DirectSoundNotifyManager(_secondaryBuffer, waveFormat, bufferSize);
                _notifyManager.NotifyAnyRaised += OnNotify;
                _notifyManager.Stopped += (s, e) =>
                    {
                        StopInternal();
                        Debug.WriteLine("Stopped");
                        PlaybackState = SoundOut.PlaybackState.Stopped;
                        RaiseStopped();
                    };

                _isinitialized = true;
                //Interlocked.Exchange(ref _disposeCount, 0);

                //Debug.WriteLine("DirectSoundOut initialized");
            }
        }

        private void StopInternal()
        {
            lock (_lockObj)
            {
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
            }
        }

        private void OnNotify(object sender, DirectSoundNotifyEventArgs e)
        {
            lock (_lockObj)
            {
                int handleIndex = e.HandleIndex;
                if (e.IsTimeOut == false && _secondaryBuffer != null && PlaybackState != SoundOut.PlaybackState.Stopped)
                {
                    if (!e.DSoundBufferStopped)
                    {
                        if (_secondaryBuffer.IsBufferLost())
                        {
                            DirectSoundException.Try(_secondaryBuffer.Restore(), "IDirectSoundBuffer", "Restore");
                        }

                        if (!OnRefill(e))
                        {
                            //no more data available
                            StopInternal();
                            e.StopPlayback = true;
                        }
                    }
                    else
                    {
                        //dsound stopped
                        StopInternal();
                        e.StopPlayback = true;
                    }
                }
                else
                {
                    //timeout
                    StopInternal();
                    e.StopPlayback = true;
                }
            }
        }

        private bool OnRefill(DirectSoundNotifyEventArgs e)
        {
            int bufferSize = e.BufferSize;
            int read;
            if (_playbackState == PlaybackState.Paused)
            {
                Array.Clear(_buffer, 0, _buffer.Length);
                read = bufferSize;
            }
            else
            {
                if (_waveSource != null)
                    read = _waveSource.Read(_buffer, 0, bufferSize);
                else
                    return false;
            }

            if (read > 0 && _secondaryBuffer != null)
            {
                if (_secondaryBuffer != null)
                {
                    return _secondaryBuffer.Write(_buffer, e.SampleOffset, bufferSize);
                }
            }

            return false;
        }

        private void RaiseStopped()
        {
            if (Stopped != null)
            {
                if (_syncContext != null)
                {
                    _syncContext.Post(c => Stopped(this, EventArgs.Empty), null);
                }
                else
                {
                    Stopped(this, EventArgs.Empty);
                }
            }
        }

        public float Volume
        {
            get
            {
                CheckForInitialize();
                return _secondaryBuffer.GetVolume();
            }
            set
            {
                CheckForInitialize();
                DirectSoundException.Try(_secondaryBuffer.SetVolume(value), "IDirectSoundBuffer", "SetVolume");
            }
        }

        public IWaveSource WaveSource
        {
            get { return _waveSource; }
        }

        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
            private set {  _playbackState = value;  }
        }

        protected void CheckForInitialize()
        {
            if (!_isinitialized)
                throw new InvalidOperationException("DirectSound is not initialized");
        }

        private volatile bool _disposed;
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && _isinitialized)
            {
                Stop();
            }
            _disposed = true;
            _isinitialized = false;
        }

        ~DirectSoundOut()
        {
            Dispose(false);
        }
    }
}