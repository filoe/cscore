using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioSessionEvents.IconPathChanged"/> event.
    /// </summary>
    public class AudioSessionIconPathChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// Gets the path for the new display icon for the session.
        /// </summary>
        public string NewIconPath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionIconPathChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newIconPath">The path for the new display icon for the session.</param>
        /// <param name="eventContext">The event context value.</param>
        public AudioSessionIconPathChangedEventArgs(string newIconPath, Guid eventContext)
            : base(eventContext)
        {
            NewIconPath = newIconPath;
        }
    }
}