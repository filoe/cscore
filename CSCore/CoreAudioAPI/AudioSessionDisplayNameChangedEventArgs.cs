using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionDisplayNameChangedEventArgs
    /// </summary>
    public class AudioSessionDisplayNameChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The new display name for the session.
        /// </summary>
        public string NewDisplayName { get; private set; }

        public AudioSessionDisplayNameChangedEventArgs(string newDisplayName, Guid eventContext)
            : base(eventContext)
        {
            NewDisplayName = newDisplayName;
        }
    }
}