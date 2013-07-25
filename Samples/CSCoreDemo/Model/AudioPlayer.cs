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

        public event EventHandler Updated;

		public void SetupAudioPlayer(SoundOutType soundOutType)
		{
			SoundOutManager.CreateSoundOut(soundOutType);
		}

		public void OpenFile(string filename, Func<IWaveSource, IWaveSource> oninitcallback)
		{
			if (String.IsNullOrWhiteSpace(filename))
				throw new ArgumentException("filename");

			var source = CodecFactory.Instance.GetCodec(filename);
            source = new LoopStream(source);

            if (source.WaveFormat.Channels == 1)
                source = new MonoToStereoSource(source).ToWaveSource(16);
			_panSource = new PanSource(source){ Pan = this.Pan };
            var _notification = new SimpleNotificationSource(_panSource);
            _notification.DataRead += OnNotification;

            if (oninitcallback != null)
                SoundOutManager.Initialize(oninitcallback(_notification.ToWaveSource(16)));
            else
			    SoundOutManager.Initialize(_notification.ToWaveSource(16));

            RaiseUpdated();
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

        public TimeSpan Position
        {
            get
            {
                if (_panSource != null)
                    return _panSource.GetPosition();
                else
                    return TimeSpan.FromMilliseconds(0);
            }
            set
            {
                if (_panSource != null)
                    _panSource.SetPosition(value);
            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_panSource != null)
                    return _panSource.GetLength();
                else
                    return TimeSpan.FromMilliseconds(0);
            }
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
