using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// SessionCreatedEventArgs
    /// </summary>
    public class SessionCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// AudioSessionControl interface of the audio session that was created.
        /// </summary>
        public AudioSessionControl NewSession { get; private set; }

        public SessionCreatedEventArgs(AudioSessionControl newSession)
        {
            if (newSession == null)
                throw new ArgumentNullException("newSession");
            NewSession = newSession;
        }
    }
}