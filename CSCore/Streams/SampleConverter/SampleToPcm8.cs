using CSCore.Utils.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams.SampleConverter
{
    public class SampleToPcm8 : SampleToWaveBase
    {
        private float[] _buffer;

        public SampleToPcm8(ISampleSource source)
            : base(source, 8, AudioEncoding.Pcm)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int sourceCount = count;
            _buffer = _buffer.CheckBuffer(sourceCount);

            int read = _source.Read(_buffer, 0, sourceCount);
            for (int i = offset; i < read; i++)
            {
                byte value = (byte)((_buffer[i] + 1) * 128f);
                buffer[i] = unchecked((byte)value);
            }

            return read;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}