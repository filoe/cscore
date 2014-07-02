using System;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Defines a provider for recording audio.
    /// </summary>
    public interface ISoundIn : IDisposable
    {
        /// <summary>
        /// Occurs when new data got captured and is available. 
        /// </summary>
        event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// Occurs when capturing stopped.
        /// </summary>
        event EventHandler Stopped;

        /// <summary>
        /// Gets the OutputFormat.
        /// </summary>
        WaveFormat WaveFormat { get; }

        /// <summary>
        /// Initializes the <see cref="ISoundIn"/> instance and prepares all resources recording.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts recording.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops recording. 
        /// </summary>
        void Stop();
    }
}