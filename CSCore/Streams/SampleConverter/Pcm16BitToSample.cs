using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class Pcm16BitToSample : WaveToSampleBase
    {
        private Object syncLock = new Object();

        public Pcm16BitToSample(IWaveSource source)
            : base(source, 16, AudioEncoding.Pcm)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsPCM() && source.WaveFormat.BitsPerSample != 16)
                throw new InvalidOperationException("Invalid format. Format has to 16 bit Pcm.");
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int bytesToRead = count * 2;
            _buffer = _buffer.CheckBuffer(bytesToRead);
            int read = _source.Read(_buffer, 0, bytesToRead);

            int startIndex = offset;
            for (int i = 0; i < read; i += 2)
            {
                buffer[startIndex] = CSCore.Utils.CSMath.Bit16ToFloat(_buffer, i, true);
                startIndex++;
            }

            return read / 2;
        }
    }
}