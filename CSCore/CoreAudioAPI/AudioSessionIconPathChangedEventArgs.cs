using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionIconPathChangedEventArgs
    /// </summary>
    public class AudioSessionIconPathChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The path for the new display icon for the session.
        /// </summary>
        public string NewIconPath { get; private set; }

        public AudioSessionIconPathChangedEventArgs(string newIconPath, Guid eventContext)
            : base(eventContext)
        {
            NewIconPath = newIconPath;
        }
    }
}