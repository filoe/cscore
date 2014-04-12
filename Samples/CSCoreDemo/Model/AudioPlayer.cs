using CSCore;
using CSCore.Codecs;
using CSCore.Streams;
using System;
using System.Collections.Generic;

namespace CSCoreDemo.Model
{
    public class AudioPlayer : IDisposable
    {
        private PanSource _panSource;
        private IWaveSource _source;

        public event EventHandler Updated;

        public void SetupAudioPlayer(SoundOutType soundOutType)
        {
            SoundOutManager.CreateSoundOut(soundOutType);
        }

        public bool OpenFile(string filename, Func<IWaveSource, IWaveSource> oninitcallback)
        {
            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("filename");

            try
            {
                var source = CodecFactory.Instance.GetCodec(filename);
                source = new LoopStream(source);
                (source as LoopStream).EnableLoop = false;

                if (source.WaveFormat.Channels == 1)
                    source = new MonoToStereoSource(source).ToWaveSource(16);
                _panSource = new PanSource(source) { Pan = this.Pan };
                var _notification = new SimpleNotificationSource(_panSource);
                _notification.DataRead += OnNotification;

                source = _notification.ToWaveSource(16);
                //source = new BufferSource(source, source.WaveFormat.BytesPerSecond * 2);

                _source = source;

                if (oninitcallback != null)
                    SoundOutManager.Initialize(oninitcallback(source));
                else
                    SoundOutManager.Initialize(source);
            }
            catch (Exception)
            {
                return false;
            }
            RaiseUpdated();
            return true;
        }

        public void Play()
        {
            SoundOutManager.Play();
        }

        public void Pause()
        {
            SoundOutManager.Pause();
        }

        public void Stop()
        {
            SoundOutManager.Stop();
            _panSource = null;
            _source = null;
            RaiseUpdated();
        }

        public bool CanPlay
        {
            get { return SoundOutManager.IsInitialized && !SoundOutManager.IsPlaying; }
        }

        public bool CanStop
        {
            get { return SoundOutManager.IsPlaying || SoundOutManager.IsPaused; }
        }

        public bool CanPause
        {
            get { return SoundOutManager.IsPlaying; }
        }

        public bool CanOpenFile
        {
            get { return SoundOutManager.IsCreated && SoundOutManager.IsStopped; }
        }

        private void OnNotification(object sender, EventArgs e)
        {
            RaiseUpdated();
        }

        private void RaiseUpdated()
        {
            if (Updated != null)
                Updated(this, EventArgs.Empty);
        }

        public IEnumerable<SoundOutDevice> Devices
        {
            get { return SoundOutManager.IsCreated ? SoundOutManager.GetDevices() : null; }
        }

        private SoundOutDevice _device;

        public SoundOutDevice Device
        {
            get
            {
                return _device;
            }
            set
            {
                if (value != null)
                {
                    _device = value;
                    SoundOutManager.SetDevice(value);
                }
            }
        }

        private SoundOutManager _soundOutManager;

        public SoundOutManager SoundOutManager
        {
            get { return _soundOutManager ?? (_soundOutManager = new SoundOutManager()); }
            set { _soundOutManager = value; }
        }

        public TimeSpan Position
        {
            get
            {
                if (_source != null)
                    return _source.GetPosition();
                else
                    return TimeSpan.FromMilliseconds(0);
            }
            set
            {
                if (_source != null)
                    _source.SetPosition(value);
            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_source != null)
                    return _source.GetLength();
                else
                    return TimeSpan.FromMilliseconds(0);
            }
        }

        public float Volume
        {
            get { return SoundOutManager.Volume; }
            set { SoundOutManager.Volume = value; }
        }

        private float _pan = 0f;

        public float Pan
        {
            get
            {
                return _pan;
            }
            set
            {
                _pan = value;
                if (_panSource != null)
                    _panSource.Pan = Pan;
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (SoundOutManager.IsCreated)
            {
                SoundOutManager.Destroy();
            }
        }

        ~AudioPlayer()
        {
            Dispose(false);
        }
    }
}