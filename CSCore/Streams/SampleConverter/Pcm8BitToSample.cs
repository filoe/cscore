using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class Pcm8BitToSample : WaveToSampleBase
    {
        public Pcm8BitToSample(IWaveSource source)
            : base(source, 8, AudioEncoding.Pcm)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsPCM() && source.WaveFormat.BitsPerSample != 8)
                throw new InvalidOperationException("Invalid format. Format has to 8 bit Pcm.");
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            _buffer = _buffer.CheckBuffer(count);
            int read = _source.Read(_buffer, 0, count);
            unsafe
            {
                fixed (float* ptrBuffer = buffer)
                {
                    float* ppbuffer = ptrBuffer + offset;
                    for (int i = 0; i < read; i++)
                    {
                        *(ppbuffer++) = _buffer[i] / 128f - 1.0f;
                    }
                }
            }
            return read;
        }
    }
}