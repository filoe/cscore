using System;

namespace CSCore.DSP
{
    public class PeakMeter : SampleSourceBase
    {
        private float _maxLeftPeak;
        private float _maxRightPeak;
        private int _blocksRead;
        private int _blockSize;

        public event EventHandler<PeakCalculatedEventArgs> PeakCalculated;

        public int BlockSize
        {
            get { return _blockSize; }
            set { _blockSize = value; }
        }

        public float MaxLeftPeak { get { return _maxLeftPeak; } }

        public float MaxRightPeak { get { return _maxRightPeak; } }

        public PeakMeter(IWaveStream source)
            : base(source)
        {
            _blockSize = 2000;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            int channels = WaveFormat.Channels;
            channels = Math.Min(2, channels);

            for (int i = 0; i < read; i += channels)
            {
                _maxLeftPeak = Math.Max(buffer[i], _maxLeftPeak);
                if (channels == 2)
                    _maxRightPeak = Math.Max(buffer[i + 1], _maxRightPeak);

                if (_blocksRead >= _blockSize)
                {
                    RaiseBlockRead(_maxLeftPeak, _maxRightPeak);
                    Reset();
                }
                _blocksRead++;
            }

            return read;
        }

        protected virtual void Reset()
        {
            _maxLeftPeak = float.MinValue;
            _maxRightPeak = float.MinValue;
            _blocksRead = 0;
        }

        protected virtual void RaiseBlockRead(float maxleft, float maxright)
        {
            maxleft = Math.Abs(maxleft);
            maxleft = Math.Max(Math.Min(1, maxleft), 0);
            maxright = Math.Abs(maxright);
            maxright = Math.Max(Math.Min(1, maxright), 0);
            if (PeakCalculated != null)
                PeakCalculated(this, new PeakCalculatedEventArgs(maxleft, maxright));
        }
    }
}