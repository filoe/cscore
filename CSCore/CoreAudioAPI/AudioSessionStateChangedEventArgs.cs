using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionStateChanged
    /// </summary>
    public class AudioSessionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The new session state.
        /// </summary>
        public AudioSessionState NewState { get; private set; }

        public AudioSessionStateChangedEventArgs(AudioSessionState newState)
        {
            NewState = newState;
        }
    }
}