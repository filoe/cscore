using System;

namespace CSCore.DSP
{
    public class PeakMeter : SampleSourceBase
    {
        private float _maxLeftPeak;
        private float _maxRightPeak;
        private float _rmsSumLeft;
        private float _rmsSumRight;

        private int _blocksRead;
        private int _blockSize;

        public event EventHandler<PeakCalculatedEventArgs> PeakCalculated;

        public int BlockSize
        {
            get { return _blockSize; }
            set { _blockSize = value; }
        }

        public float MaxLeftPeak 
        { 
            get { return _maxLeftPeak; } 
        }

        public float MaxRightPeak 
        { 
            get { return _maxRightPeak; } 
        }

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
                _rmsSumLeft += (buffer[i] * buffer[i]);
                if (channels == 2)
                {
                    _maxRightPeak = Math.Max(buffer[i + 1], _maxRightPeak);
                    _rmsSumRight += (buffer[i + 1] * buffer[i + 1]);
                }
                if (_blocksRead >= BlockSize)
                {
                    _rmsSumLeft = (float)Math.Sqrt(_rmsSumLeft / (float)BlockSize);
                    _rmsSumRight = (float)Math.Sqrt(_rmsSumRight / (float)BlockSize);

                    RaiseBlockRead(_maxLeftPeak, _maxRightPeak, _rmsSumLeft, _rmsSumRight);
                    Reset();
                }
                _blocksRead++;
            }

            return read;
        }

        protected virtual void Reset()
        {
            _maxLeftPeak = 0f;
            _maxRightPeak = 0f;
            _rmsSumLeft = 0f;
            _rmsSumRight = 0f;
            _blocksRead = 0;
        }

        protected virtual void RaiseBlockRead(float maxleft, float maxright, float rmsLeft, float rmsRight)
        {
            maxleft = Math.Abs(maxleft);
            maxleft = Math.Max(Math.Min(1, maxleft), 0);
            maxright = Math.Abs(maxright);
            maxright = Math.Max(Math.Min(1, maxright), 0);
            if (PeakCalculated != null)
                PeakCalculated(this, new PeakCalculatedEventArgs(maxleft, maxright, rmsLeft, rmsRight));
        }
    }
}