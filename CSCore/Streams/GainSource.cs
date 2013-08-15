using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class GainSource : SampleSourceBase, System.ComponentModel.INotifyPropertyChanged
    {
        private float _gain;

        public float Gain
        {
            get
            {
                return _gain;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _gain = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Gain"));
            }
        }

        public GainSource(IWaveStream source)
            : base(source)
        {
            Gain = 1;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            if (Gain != 1)
                ApplyGain(buffer, offset, read, Gain);

            return read;
        }

        protected void ApplyGain(float[] buffer, int offset, int count, float gain)
        {
            for (int i = offset; i < offset + count; i++)
            {
                buffer[i] *= gain;

                if (buffer[i] < -1)
                    buffer[i] = 0;
                if (buffer[i] > 1)
                    buffer[i] = 1;
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}