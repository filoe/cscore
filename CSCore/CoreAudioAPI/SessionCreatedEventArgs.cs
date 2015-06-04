using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioSessionNotification.SessionCreated"/> event.
    /// </summary>
    public class SessionCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="AudioSessionControl"/> object of the audio session that was created.
        /// </summary>
        public AudioSessionControl NewSession { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCreatedEventArgs"/> class.
        /// </summary>
        /// <param name="newSession">The <see cref="AudioSessionControl"/> object of the audio session that was created.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="newSession"/> must not be null.</exception>
        public SessionCreatedEventArgs(AudioSessionControl newSession)
        {
            if (newSession == null)
                throw new ArgumentNullException("newSession");
            NewSession = newSession;
        }
    }
}