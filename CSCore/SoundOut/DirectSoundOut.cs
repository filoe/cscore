using CSCore.SoundOut.DirectSound;
using System;
using System.Linq;
using System.Threading;

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
        protected byte[] _buffer;

        bool _isinitialized = false;

        object lockObj = new object();
        object lockObj1;

        public event EventHandler Stopped;

        int _latency = 100;
        public int Latency
        {
            get { return _latency; }
            set { _latency = value; }
        }

        Guid _device;
        public Guid Device
        {
            get { return _device; }
            set { _device = value; }
        }

        public float Volume
        {
            get { return GetVolume(); }
            set { SetVolume(value); }
        }

        public IWaveSource WaveSource
        {
            get { return _waveSource; }
        }

        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
        }

        public DirectSoundOut()
        {
            Device = DirectSoundDevice.DefaultPlaybackGuid;

            lockObj1 = lockObj;
        }

        public void Play()
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                _secondaryBuffer.SetCurrentPosition(0);
                _secondaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);

                lock (lockObj1)
                {
                    _playbackState = SoundOut.PlaybackState.Playing;
                }

                _notifyManager.Start();
                Context.Current.Logger.Info("DirectSoundOut playback started", "DirectSoundOut.Play()");
            }
            else if (_playbackState == SoundOut.PlaybackState.Paused)
            {
                lock (lockObj1)
                {
                    _playbackState = SoundOut.PlaybackState.Playing;
                }
            }
        }

        public void Stop()
        {
            if (_playbackState != SoundOut.PlaybackState.Stopped)
            {
                if (Monitor.TryEnter(lockObj1, 50))
                {
                    _playbackState = PlaybackState.Stopped;
                    Monitor.Exit(lockObj1);
                }
                else
                {
                    StopInternal();
                }
            }
        }

        public void Pause()
        {
            lock (lockObj1)
            {
                _playbackState = PlaybackState.Paused;
            }
        }

        public void Resume()
        {
            Play();
        }

        public void Initialize(IWaveSource source)
        {
            lock (lockObj)
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
                _buffer = new byte[bufferCaps.dwBufferBytes];

                _notifyManager = new DirectSoundNotifyManager(_secondaryBuffer, waveFormat,
                    bufferSize);
                _notifyManager.NotifyAnyRaised += OnNotify;
                _notifyManager.Stopped += (s, e) => StopInternal();

                _isinitialized = true;

                Context.Current.Logger.Info("DirectSoundOut initialized", "DirectSoundOut.Initialize(IWaveSource)");
            }
        }

        private void OnNotify(object sender, DirectSoundNotifyEventArgs e)
        {
            lock (lockObj)
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
                        }
                    }
                    else
                    {
                        //dsound stopped
                        StopInternal();
                    }
                }
                else
                {
                    //timeout
                    StopInternal();
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
                else return false;
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

        private void StopInternal()
        {
            lock (lockObj)
            {
                if (_primaryBuffer != null)
                {
                    _primaryBuffer.Stop();
                    _primaryBuffer.Dispose();
                    _primaryBuffer = null;
                }
                if (_secondaryBuffer != null)
                {
                    _secondaryBuffer.Stop();
                    _secondaryBuffer.Dispose();
                    _secondaryBuffer = null;
                }
                StopNotification();
                if (_directSound != null)
                {
                    _directSound.Dispose();
                    _directSound = null;
                }

                _isinitialized = false;
            }
            Context.Current.Logger.Info("DirectSoundOut playback stopped", "DirectSoundOut.StopInternal()");
        }

        private void StopNotification()
        {
            lock (lockObj)
            {
                if (_notifyManager == null)
                    return;

                if (!_notifyManager.Stop(100))
                {
                    _notifyManager.Stopped += (s, e) =>
                    {
                        (s as DirectSoundNotifyManager).Dispose();
                        lock (lockObj1)
                        {
                            _playbackState = SoundOut.PlaybackState.Stopped;
                        }
                        RaiseStopped();
                    };
                }
                else
                {
                    _notifyManager.Dispose();
                    lock (lockObj1)
                    {
                        _playbackState = SoundOut.PlaybackState.Stopped;
                    }
                    RaiseStopped();
                }
                _notifyManager = null;
            }
        }

        private void SetVolume(float volume)
        {
            CheckForInitialize();
            DirectSoundException.Try(_secondaryBuffer.SetVolume(volume), "IDirectSoundBuffer", "SetVolume");
        }

        private float GetVolume()
        {
            CheckForInitialize();
            float volume;
            DirectSoundException.Try(_secondaryBuffer.GetVolume(out volume), "IDirectSoundBuffer", "GetVolume");
            return volume;
        }

        protected void CheckForInitialize()
        {
            if (!_isinitialized)
                throw new InvalidOperationException("DirectSound is not initialized");
        }

        private void RaiseStopped()
        {
            if (Stopped != null)
            {
                Stopped(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            lock (lockObj)
            {
                GC.SuppressFinalize(this);
                Dispose(true);
            }
        }

        private void Dispose(bool disposing)
        {
            Stop();
        }

        ~DirectSoundOut()
        {
            Dispose(false);
        }
    }
}
