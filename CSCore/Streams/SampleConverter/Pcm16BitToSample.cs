using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class Pcm16BitToSample : WaveToSampleBase
    {
        private Object syncLock = new Object();

        public Pcm16BitToSample(IWaveSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsPCM() && source.WaveFormat.BitsPerSample != 16)
                throw new InvalidOperationException("Invalid format. Format has to 16 bit Pcm.");
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int bytesToRead = count * 2;
            Buffer = Buffer.CheckBuffer(bytesToRead);
            int read = Source.Read(Buffer, 0, bytesToRead);

            int startIndex = offset;
            for (int i = 0; i < read; i += 2)
            {
                buffer[startIndex] = BitConverter.ToInt16(Buffer, i) / (true ? 32768f : 1.0f);
                startIndex++;
            }

            return read / 2;
        }
    }
}