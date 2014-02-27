using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class FadeInOut : VolumeSource
    {
        public event EventHandler FadeFinished;

        public override float Volume
        {
            get
            {
                return base.Volume;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public float TargetVolume
        {
            get { return _targetVolume; }
        }

        public int Duration
        {
            get { return _duration; }
        }

        private bool _fade;
        private int _duration;

        private int _samplesRead = 0;
        private float _step = 0;
        private int _blockSize = 0;
        private float _targetVolume = 1f;

        public FadeInOut(IWaveStream source, float initialVolume)
            : base(source)
        {
            base.Volume = initialVolume;
        }

        public void StartFading(int duration, float targetVolume)
        {
            StartFading(duration, targetVolume, -1, 10);
        }

        public void StartFading(int duration, float targetVolume, float startVolume, int resolution)
        {
            if (duration <= 0)
                throw new ArgumentOutOfRangeException("duration");
            if (targetVolume < 0 || targetVolume > 1)
                throw new ArgumentOutOfRangeException("targetVolume");
            if (resolution < 0)
                throw new ArgumentOutOfRangeException("resolution");

            if (startVolume == -1)
                startVolume = base.Volume;

            base.Volume = startVolume;

            _fade = true;

            this._duration = duration;
            this._targetVolume = targetVolume;
            _blockSize = (WaveFormat.SampleRate * WaveFormat.Channels) / resolution;
            _blockSize -= (_blockSize % WaveFormat.BlockAlign);
            if (_blockSize <= 0)
                throw new InvalidOperationException("c");

            _step = (targetVolume - Volume) / (WaveFormat.SampleRate * WaveFormat.Channels * duration) * _blockSize;
        }

        public void StopFading()
        {
            _fade = false;
            _samplesRead = 0;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (_fade && Volume == _targetVolume)
            {
                if (FadeFinished != null)
                    FadeFinished(this, new EventArgs());
                _fade = false;
            }

            if (!_fade)
                return base.Read(buffer, offset, count);

            int read = 0;

            while (read < count)
            {
                int r = base.Read(buffer, offset, Math.Min(_blockSize, count));
                read += r;
                offset += r;
                _samplesRead += r;
                count -= r;

                if (_samplesRead >= _blockSize)
                {
                    base.Volume = Math.Max(0, Math.Min(1, base.Volume + _step));
                    _samplesRead = 0;
                }
            }

            return read;
        }
    }
}