using CSCore.Utils.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams.SampleConverter
{
    public class SampleToPcm24 : SampleToWaveBase
    {
        private float[] _buffer;

        public SampleToPcm24(ISampleSource source)
            : base(source, 24, AudioEncoding.Pcm)
        {
        }

        public unsafe override int Read(byte[] buffer, int offset, int count)
        {
            int sourceCount = count / 3;
            _buffer = _buffer.CheckBuffer(sourceCount);
            int read = _source.Read(_buffer, 0, sourceCount);

            int bufferOffset = offset;
            for (int i = 0; i < read; i++)
            {
                uint sample32 = (uint)(_buffer[i] * 8388608f);
                byte* psample32 = (byte*)&sample32;
                buffer[bufferOffset++] = psample32[0];
                buffer[bufferOffset++] = psample32[1];
                buffer[bufferOffset++] = psample32[2];
            }

            return read * 3;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}