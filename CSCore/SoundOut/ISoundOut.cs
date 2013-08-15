using System;

namespace CSCore.SoundOut
{
    public interface ISoundOut : IDisposable
    {
        void Play();

        void Pause();

        void Resume();

        void Stop();

        void Initialize(IWaveSource source);

        float Volume { get; set; }

        IWaveSource WaveSource { get; }

        PlaybackState PlaybackState { get; }

        event EventHandler Stopped;
    }
}