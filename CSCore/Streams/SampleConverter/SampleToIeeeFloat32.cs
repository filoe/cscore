using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using CSCore.Utils;

namespace CSCore.Streams.SampleConverter
{
    public class SampleToIeeeFloat32 : SampleToWaveBase
    {
        private float[] _decoderBuffer;

        public SampleToIeeeFloat32(ISampleSource source)
            : base(source, 32, AudioEncoding.IeeeFloat)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            _decoderBuffer = _decoderBuffer.CheckBuffer(count / 4);
            int read = Source.Read(_decoderBuffer, offset / 4, count / 4);
            ILUtils.MemoryCopy(buffer, _decoderBuffer, read * 4);
            return read * 4;
        }
    }
}