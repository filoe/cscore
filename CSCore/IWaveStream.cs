using System;

namespace CSCore
{
    public interface IWaveStream : IDisposable
    {
        WaveFormat WaveFormat { get; }

        long Position { get; set; }

        long Length { get; }
    }
}