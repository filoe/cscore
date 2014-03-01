using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class IeeeFloatToSample : WaveToSampleBase
    {
        public IeeeFloatToSample(IWaveSource source)
            : base(source, 32, AudioEncoding.IeeeFloat)
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
            _buffer = _buffer.CheckBuffer(bytesToRead);
            int read = _source.Read(_buffer, 0, bytesToRead);

            int startIndex = offset;
            for (int i = 0; i < read; i += 4)
            {
                buffer[startIndex] = BitConverter.ToSingle(_buffer, i);
                startIndex++;
            }

            return read / 4;
        }
    }
}