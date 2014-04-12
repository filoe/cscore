using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.DMO
{
    public abstract class DmoAggregator : DmoStream, IWaveAggregator
    {
        private IWaveSource _source;

        public DmoAggregator(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
            //Initialize();
        }

        protected override int GetInputData(ref byte[] inputDataBuffer, int requested)
        {
            inputDataBuffer = inputDataBuffer.CheckBuffer(requested);
            return BaseStream.Read(inputDataBuffer, 0, requested);
        }

        protected override WaveFormat GetInputFormat()
        {
            return BaseStream.WaveFormat;
        }

        public override long Position
        {
            get
            {
                return InputToOutput(_source.Position);
            }
            set
            {
                 _source.Position = OutputToInput(value);
            }
        }

        public override long Length
        {
            get
            {
                return OutputToInput(_source.Position);
            }
        }

        public IWaveSource BaseStream
        {
            get { return _source; }
        }
    }
}
