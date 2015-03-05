using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioSessionEvents.SessionDisconnected"/> event.
    /// </summary>
    public class AudioSessionDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the reason that the audio session was disconnected.
        /// </summary>
        public AudioSessionDisconnectReason DisconnectReason { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionDisconnectedEventArgs"/>  class.
        /// </summary>
        /// <param name="disconnectReason">The reason that the audio session was disconnected.</param>
        public AudioSessionDisconnectedEventArgs(AudioSessionDisconnectReason disconnectReason)
        {
            DisconnectReason = disconnectReason;
        }
    }
}