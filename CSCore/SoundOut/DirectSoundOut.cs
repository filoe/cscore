using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CSCore.SoundOut.DirectSound;

namespace CSCore.SoundOut
{
    public class DirectSoundOut : ISoundOut
    {
        protected IWaveSource _waveSource;
        protected DirectSound8 _directSound;
        protected DirectSoundPrimaryBuffer _primaryBuffer;
        protected DirectSoundSecondaryBuffer _secondaryBuffer;
        protected DirectSoundNotifyManager _notifyManager;
        protected PlaybackState _playbackState = PlaybackState.Stopped;
        private SynchronizationContext _syncContext;
        private Guid _device;
        private int _latency;
        private bool _isinitialized;
        protected object _lockObj;

        public event EventHandler Stopped;
        private byte[] _buffer;

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

        public Guid Device
        {
            get { return _device; }
            set
            {
                _device = value;
            }
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
            if (_playbackState == SoundOut.PlaybackState.Stopped)
            {
                _secondaryBuffer.SetCurrentPosition(0);
                _secondaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);
                PlaybackState = SoundOut.PlaybackState.Playing;
                _notifyManager.Start();
                Context.Current.Logger.Info("DirectSoundOut playback started", "DirectSoundOut.Play()");
            }

            PlaybackState = SoundOut.PlaybackState.Playing;
        }

        public void Pause()
        {
            lock (_lockObj)
            {
                PlaybackState = SoundOut.PlaybackState.Paused;
            }
        }

        public void Resume()
        {
            Play();
        }

        public void Stop()
        {
            if (_notifyManager != null && !_notifyManager.GotStarted)
            {
                StopInternal();
                _notifyManager.Dispose();
                _notifyManager = null;
            }

            if (Monitor.TryEnter(_lockObj, 50))
            {
                _playbackState = SoundOut.PlaybackState.Stopped;
                Monitor.Exit(_lockObj);
            }
            else
            {
                if (_notifyManager != null)
                {
                    if (!_notifyManager.Stop())
                    {
                        _notifyManager.Abort(); //playbackstate set by Stopped-Eventhandler(NotifyManager)
                    }
                    _notifyManager = null;
                }
            }
        }

        public void Initialize(IWaveSource source)
        {
            lock (_lockObj)
            {
                if (source == null) throw new ArgumentNullException("source");
                StopInternal();

                IntPtr handle = DSInterop.DirectSoundUtils.GetDesktopWindow();

                Guid device = Device;
                IntPtr ptrdsound;
                DirectSoundException.Try(DSInterop.DirectSoundCreate8(ref device, out ptrdsound, IntPtr.Zero), "DSInterop", "DirectSoundCreate8(ref Guid, out IntPtr, IntPtr)");

                _directSound = new DirectSound8(ptrdsound);
                DirectSoundException.Try(_directSound.SetCooperativeLevel(handle, DSCooperativeLevelType.DSSCL_EXCLUSIVE),
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

                _primaryBuffer = new DirectSoundPrimaryBuffer(_directSound);
                _secondaryBuffer = new DirectSoundSecondaryBuffer(_directSound, waveFormat, bufferSize, false); //remove true

                _primaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);

                DSBufferCaps bufferCaps;
                DirectSoundException.Try(_secondaryBuffer.GetCaps(out bufferCaps), "IDirectSoundBuffer", "GetCaps");
                _buffer = _buffer.CheckBuffer(bufferCaps.dwBufferBytes); //[bufferCaps.dwBufferBytes];

                _notifyManager = new DirectSoundNotifyManager(_secondaryBuffer, waveFormat,
                    bufferSize);
                _notifyManager.NotifyAnyRaised += OnNotify;
                _notifyManager.Stopped += (s, e) =>
                    {
                        StopInternal();
                        PlaybackState = SoundOut.PlaybackState.Stopped;
                        RaiseStopped();
                    };

                _isinitialized = true;

                Context.Current.Logger.Info("DirectSoundOut initialized", "DirectSoundOut.Initialize(IWaveSource)");
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
            private set { lock (_lockObj) { _playbackState = value; } }
        }

        protected void CheckForInitialize()
        {
            if (!_isinitialized)
                throw new InvalidOperationException("DirectSound is not initialized");
        }

        private bool _disposed;
		public void Dispose()
        {
			if(!_disposed)
			{
				_disposed = true;
				
				Dispose(true);
				GC.SuppressFinalize(this);
			}
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                Stop();
                //var mgr = _notifyManager;
                //if (mgr != null)
                //{
                //    mgr.WaitForStopped();
                //}
            }
        }

        ~DirectSoundOut()
        {
            Dispose(false);
        }
    }
}
