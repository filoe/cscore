using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams.SampleConverter
{
    public class Pcm24BitToSample : WaveToSampleBase
    {
        public Pcm24BitToSample(IWaveSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsPCM() && source.WaveFormat.BitsPerSample != 24)
                throw new InvalidOperationException("Invalid format. Format has to 24 bit Pcm.");
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int bytesToRead = count * 3;
            Buffer = Buffer.CheckBuffer(bytesToRead);
            int read = Source.Read(Buffer, 0, bytesToRead);
            unsafe
            {
                fixed (float* ptrBuffer = buffer)
                {
                    float* ppbuffer = ptrBuffer + offset;
                    for (int i = 0; i < read; i += 3)
                    {
                        *(ppbuffer++) = (((sbyte)Buffer[i + 2] << 16) | (Buffer[i + 1] << 8) | Buffer[i]) / 8388608f;
                    }
                }
            }
            return read / 3;
        }
    }
}