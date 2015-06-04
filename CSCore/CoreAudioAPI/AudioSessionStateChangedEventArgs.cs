using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioSessionEvents.StateChanged"/> event.
    /// </summary>
    public class AudioSessionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new session state.
        /// </summary>
        public AudioSessionState NewState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newState"></param>
        public AudioSessionStateChangedEventArgs(AudioSessionState newState)
        {
            NewState = newState;
        }
    }
}