using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.Compression.ACM;

namespace CSCore.Streams
{
    public class AcmWaveFormatConverterSource : WaveAggregatorBase
    {
        AcmBufferConverter _converter;

        public AcmWaveFormatConverterSource(IWaveSource source, WaveFormat destinationFormat)
            : base(source)
        {
            if (destinationFormat == null)
                throw new ArgumentNullException("destinationFormat");

            _converter = new AcmBufferConverter(source.WaveFormat, destinationFormat);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, count);
        }
    }
}
