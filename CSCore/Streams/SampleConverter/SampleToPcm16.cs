using CSCore.Utils.Buffer;
using CSCore;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class SampleToPcm16 : SampleToWaveBase
    {
        private float[] _buffer;

        public SampleToPcm16(ISampleSource source)
            : base(source, 16, AudioEncoding.Pcm)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            _buffer = _buffer.CheckBuffer(count / 2);

            int read = _source.Read(_buffer, 0, count / 2);
            int bufferOffset = offset;
            for (int i = 0; i < read; i++)
            {
                short value = (short)(_buffer[i] * short.MaxValue);
                var bytes = BitConverter.GetBytes(value);

                buffer[bufferOffset++] = bytes[0];
                buffer[bufferOffset++] = bytes[1];
            }

            return read * 2;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}