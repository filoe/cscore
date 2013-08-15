using System;

namespace CSCore.SoundIn
{
    public interface ISoundIn : IDisposable
    {
        event EventHandler<DataAvailableEventArgs> DataAvailable;

        event EventHandler Stopped;

        WaveFormat WaveFormat { get; }

        void Initialize();

        void Start();

        void Stop();
    }
}