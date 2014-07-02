using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionEventContextEventArgs
    /// </summary>
    public abstract class AudioSessionEventContextEventArgs : EventArgs
    {
        /// <summary>
        /// The event context value.
        /// </summary>
        public Guid EventContext { get; private set; }

        public AudioSessionEventContextEventArgs(Guid eventContext)
        {
            EventContext = eventContext;
        }
    }
}