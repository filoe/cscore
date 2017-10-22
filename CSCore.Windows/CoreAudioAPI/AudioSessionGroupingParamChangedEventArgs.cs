using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioSessionEvents.GroupingParamChanged"/> event.
    /// </summary>
    public class AudioSessionGroupingParamChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// Gets the new grouping parameter for the session.
        /// </summary>
        public Guid NewGroupingParam { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionGroupingParamChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newGroupingParam">The new grouping parameter for the session.</param>
        /// <param name="eventContext">The event context value.</param>
        public AudioSessionGroupingParamChangedEventArgs(Guid newGroupingParam, Guid eventContext)
            : base(eventContext)
        {
            NewGroupingParam = newGroupingParam;
        }
    }
}