using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public class FFTDataProvider : WaveAggregatorBase
    {
        public event EventHandler<FFTCalculatedEventArgs> FFTCalculated;

        private FFTAggregator _fftaggregator;

        public int Bands
        {
            get { return _fftaggregator.BandCount; }
            set { _fftaggregator.BandCount = value; }
        }

        public FFTDataProvider(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            CreateFFTAggregator(source);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            return read;
        }

        private void CreateFFTAggregator(IWaveSource source)
        {
            if (_fftaggregator != null)
            {
                _fftaggregator.FFTCalculated -= OnNewData;
                _fftaggregator = null;
            }

            _fftaggregator = new FFTAggregator(source);
            _fftaggregator.FFTCalculated += OnNewData;
            BaseStream = _fftaggregator;
        }

        private void OnNewData(object sender, FFTCalculatedEventArgs e)
        {
            RaiseFFTCalculated(e);
        }

        private void RaiseFFTCalculated(FFTCalculatedEventArgs e)
        {
            if (FFTCalculated != null)
                FFTCalculated(this, e);
        }
    }
}