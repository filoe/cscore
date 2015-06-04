using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// A base class for all event-args classes which specify an <see cref="EventContext"/> value.
    /// </summary>
    public abstract class AudioSessionEventContextEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the event context value.
        /// </summary>
        public Guid EventContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionEventContextEventArgs"/> class.
        /// </summary>
        /// <param name="eventContext">The event context value.</param>
        protected AudioSessionEventContextEventArgs(Guid eventContext)
        {
            EventContext = eventContext;
        }
    }
}