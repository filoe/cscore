using System;
using CSCore.SoundOut.AL;
using CSCore.Streams;

namespace CSCore.SoundOut
{
    public class ALSoundOut : ISoundOut
    {
        public float Volume
        {
            get
            {
                if (_volumeSource != null)
                {
                    return _volumeSource.Volume;
                }

                return 0;
            }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (_volumeSource != null)
                {
                    _volumeSource.Volume = value;
                }
            }
        }

        public IWaveSource WaveSource { get; private set; }

        public PlaybackState PlaybackState
        {
            get
            {
                if (_alPlayback != null)
                {
                    return _alPlayback.PlaybackState;
                }

                return PlaybackState.Stopped;
            }
        }

        public event EventHandler<PlaybackStoppedEventArgs> Stopped;

        private ALPlayback _alPlayback;
        private VolumeSource _volumeSource;
        private ALDevice _alDevice;

        /// <summary>
        /// Initializes a new ALSoundOut class
        /// </summary>
        public ALSoundOut()
        {
            _alDevice = ALDevice.DefaultDevice;
        }

        ~ALSoundOut()
        {
            Dispose(false);
        }

        static ALSoundOut()
        {
            if (ALInterops.IsSupported())
            {
                ALDevice.DefaultDevice.Initialize();
            }
        }

        /// <summary>
        /// Plays the stream
        /// </summary>
        public void Play()
        {
            if (_alPlayback != null)
            {
                _alPlayback.Play();
            }
        }

        /// <summary>
        /// Resumes the stream
        /// </summary>
        public void Resume()
        {
            if (_alPlayback != null)
            {
                _alPlayback.Resume();
            }
        }

        /// <summary>
        /// Pause the stream
        /// </summary>
        public void Pause()
        {
            if (_alPlayback != null)
            {
                _alPlayback.Pause();
            }
        }

        /// <summary>
        /// Stops the stream
        /// </summary>
        public void Stop()
        {
            if (_alPlayback != null)
            {
                _alPlayback.Stop();
            }
        }

        public void Initialize(IWaveSource source)
        {
            WaveSource = source;
            _volumeSource = new VolumeSource(source.ToSampleSource());

            if (_alPlayback != null)
            {
                _alPlayback.Stop();
                _alPlayback.Dispose();
            }

            _alPlayback = new ALPlayback(_alDevice);
            _alPlayback.PlaybackChanged += PlaybackChanged;
            _alPlayback.Initialize(_volumeSource.ToWaveSource(), source.WaveFormat);
        }

        private void PlaybackChanged(object sender, EventArgs e)
        {
            if (_alPlayback != null && _alPlayback.PlaybackState == PlaybackState.Stopped)
            {
                if (Stopped != null)
                {
                    Stopped(this, new PlaybackStoppedEventArgs());
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_alPlayback != null)
                {
                    _alPlayback.Dispose();
                }
            }
        }
    }
}
