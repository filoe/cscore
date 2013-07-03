using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.Streams;
using CSCore.SoundOut;

namespace CSCoreDemo.Model
{
    public class AudioPlayer : IDisposable
    {
        PanSource _panSource;

        public void SetupAudioPlayer(SoundOutType soundOutType)
        {
            SoundOutManager.CreateSoundOut(soundOutType);
        }

        public void OpenFile(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("filename");

            var source = CodecFactory.Instance.GetCodec(filename);
            _panSource = new PanSource(source){ Pan = this.Pan };

            SoundOutManager.Initialize(_panSource.ToWaveSource(24));
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

        public IEnumerable<SoundOutDevice> Devices
        {
            get { return SoundOutManager.IsCreated ? SoundOutManager.GetDevices() : null; }
        }

        SoundOutDevice _device;
        public SoundOutDevice Device
        {
            get { return _device; }
            set
            {
                if (value != null)
                {
                    _device = value;
                    SoundOutManager.SetDevice(value);
                }
            }
        }

        SoundOutManager _soundOutManager;
        public SoundOutManager SoundOutManager
        {
            get { return _soundOutManager ?? (_soundOutManager = new SoundOutManager()); }
            set { _soundOutManager = value; }
        }

        public float Volume
        {
            get { return SoundOutManager.Volume; }
            set { SoundOutManager.Volume = value; }
        }

        float _pan = 0f;
        public float Pan
        {
            get { return _pan; }
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
			if(!_disposed)
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
