using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionDisconnectedEventArgs
    /// </summary>
    public class AudioSessionDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// The reason that the audio session was disconnected.
        /// </summary>
        public AudioSessionDisconnectReason DisconnectReason { get; private set; }

        public AudioSessionDisconnectedEventArgs(AudioSessionDisconnectReason disconnectReason)
        {
            DisconnectReason = disconnectReason;
        }
    }
}