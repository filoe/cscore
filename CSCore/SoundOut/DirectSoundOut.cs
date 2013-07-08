using CSCore.SoundOut.DirectSound;
using System;
using System.Linq;

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
        //http://www.flexheader.net/download.html
        protected byte[] _buffer;

        object lockObj = new object();

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
            var devices = DirectSoundDevice.EnumerateDevices();
            if (devices == null || devices.Count <= 0)
                throw new InvalidOperationException("No aviable devices");
            Device = (Guid)devices.First();
        }

        public void Play()
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                //new DSEchoEffect(_secondaryBuffer.BasePtr);
                _secondaryBuffer.Play(DSBPlayFlags.DSBPLAY_LOOPING);
                _notifyManager.Start();
                _playbackState = SoundOut.PlaybackState.Playing;
                Context.Current.Logger.Info("DirectSoundOut playback started", "DirectSoundOut.Play()");
            }
            else if (_playbackState == PlaybackState.Paused)
            {
                _playbackState = PlaybackState.Playing;
            }
        }

        public void Stop()
        {
            StopInternal();
            _playbackState = PlaybackState.Stopped;
        }

        public void Pause()
        {
            _playbackState = PlaybackState.Paused;
        }

        public void Resume()
        {
            Play();
        }

        public void Initialize(IWaveSource source)
        {
            if (source == null) throw new ArgumentNullException("source");
            StopInternal();

            IntPtr handle = DSInterop.DirectSoundUtils.GetDesktopWindow();

            Guid device = Device;
            IntPtr pDirectSound;
            DirectSoundException.Try(DSInterop.DirectSoundCreate8(ref device, out pDirectSound, IntPtr.Zero), "DSInterop", "DirectSoundCreate8(ref Guid, out IntPtr, IntPtr)");

            _directSound = new DirectSound8(pDirectSound);
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

            Context.Current.Logger.Info("DirectSoundOut initialized", "DirectSoundOut.Initialize(IWaveSource)");
        }

        protected virtual void OnNotify(object sender, DirectSoundNotifyEventArgs e)
        {
            lock (lockObj)
            {
                int handleIndex = e.HandleIndex;
                if (e.IsTimeOut == false && _secondaryBuffer != null && PlaybackState != SoundOut.PlaybackState.Stopped)
                {
                    if (handleIndex != 2)
                    {
                        if (_secondaryBuffer.IsBufferLost())
                        {
                            DirectSoundException.Try(_secondaryBuffer.Restore(), "IDirectSoundBuffer", "Restore");
                        }

                        if (!OnRefill(e))
                        {
                            StopInternal();
                        }
                    }
                    else
                    {
                        StopInternal();
                    }
                }
                else
                {
                    StopInternal();
                }
            }
        }

        protected virtual bool OnRefill(DirectSoundNotifyEventArgs e)
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

            if (read > 0)
            {
                if(_secondaryBuffer != null)
                    return _secondaryBuffer.Write(_buffer, e.SampleOffset, bufferSize);
            }

            return false;
        }

        protected virtual void StopInternal()
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
                if (_notifyManager != null)
                {
                    if (!_notifyManager.Stop(100))
                    {
                        _notifyManager.Stopped += (s, e) =>
                        {
                            (s as DirectSoundNotifyManager).Dispose();
                        };
                    }
                    else
                    {
                        _notifyManager.Dispose();
                    }
                    _notifyManager = null;
                }
                if (_directSound != null)
                {
                    _directSound.Dispose();
                    _directSound = null;
                }
            }
            Context.Current.Logger.Info("DirectSoundOut playback stopped", "DirectSoundOut.StopInternal()");
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
            if(_secondaryBuffer == null)
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

        protected virtual void Dispose(bool disposing)
        {
            Stop();
            if (_directSound != null)
            {
                _directSound.Dispose();
                _directSound = null;
                _notifyManager.Dispose();
            }
        }

        ~DirectSoundOut()
        {
            Dispose(false);
        }
    }
}
