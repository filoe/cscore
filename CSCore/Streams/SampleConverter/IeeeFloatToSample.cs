using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class IeeeFloatToSample : WaveToSampleBase
    {
        public IeeeFloatToSample(IWaveSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsIeeeFloat() ||
                source.WaveFormat.BitsPerSample != 32)
                throw new InvalidOperationException("Invalid format. Format has to be 32 bit IeeeFloat");
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int bytesToRead = count * 4;
            Buffer = Buffer.CheckBuffer(bytesToRead);
            int read = Source.Read(Buffer, 0, bytesToRead);

            int startIndex = offset;
            for (int i = 0; i < read; i += 4)
            {
                buffer[startIndex] = BitConverter.ToSingle(Buffer, i);
                startIndex++;
            }

            return read / 4;
        }
    }
}