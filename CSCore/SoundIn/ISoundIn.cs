using System;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Defines a interface for capturing audio. 
    /// </summary>
    public interface ISoundIn : IDisposable
    {
        /// <summary>
        /// Occurs when new data got captured and is available. 
        /// </summary>
        event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// Occurs when the recording stopped.
        /// </summary>
        event EventHandler<RecordingStoppedEventArgs> Stopped;

        /// <summary>
        /// Gets the format of the captured audio data.
        /// </summary>
        WaveFormat WaveFormat { get; }

        /// <summary>
        /// Initializes the <see cref="ISoundIn"/> instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts capturing.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops capturing. 
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the current <see cref="SoundIn.RecordingState"/>.
        /// </summary>
        RecordingState RecordingState { get; }
    }
}