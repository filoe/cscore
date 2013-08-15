using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams.SampleConverter
{
    public class SampleToIeeeFloat32 : SampleToWaveBase
    {
        public SampleToIeeeFloat32(ISampleSource source)
            : base(source, 32, AudioEncoding.IeeeFloat)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var ub = new Utils.Buffer.UnsafeBuffer(buffer);
            int read = _source.Read(ub.FloatBuffer, offset / 4, count / 4);
            return read * 4;
        }
    }
}