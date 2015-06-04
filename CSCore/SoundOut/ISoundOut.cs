using System;

namespace CSCore.SoundOut
{
    /// <summary>
    /// Defines a interface for audio playbacks.
    /// </summary>
    public interface ISoundOut : IDisposable
    {
        /// <summary>
        /// Starts the audio playback.
        /// </summary>
        void Play();

        /// <summary>
        /// Pauses the audio playback.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the audio playback.
        /// </summary>
        void Resume();

        /// <summary>
        /// Stops the audio playback.
        /// </summary>
        void Stop();

        /// <summary>
        /// Initializes the <see cref="ISoundOut"/> for playing a <paramref name="source"/>.
        /// </summary>
        /// <param name="source"><see cref="IWaveSource"/> which provides waveform-audio data to play.</param>
        void Initialize(IWaveSource source);

        /// <summary>
        /// Gets or sets the volume of the playback. The value of this property must be within the range from 0.0 to 1.0 where 0.0 equals 0% (muted) and 1.0 equals 100%.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Gets the <see cref="IWaveSource"/> which provides the waveform-audio data and was used to <see cref="Initialize"/> the <see cref="ISoundOut"/>.
        /// </summary>
        IWaveSource WaveSource { get; }

        /// <summary>
        /// Gets the <see cref="SoundOut.PlaybackState"/> of the <see cref="ISoundOut"/>. The playback state indicates whether the playback is currently playing, paused or stopped.
        /// </summary>
        PlaybackState PlaybackState { get; }

        /// <summary>
        /// Occurs when the playback stops. 
        /// </summary>
        event EventHandler<PlaybackStoppedEventArgs> Stopped;
    }
}