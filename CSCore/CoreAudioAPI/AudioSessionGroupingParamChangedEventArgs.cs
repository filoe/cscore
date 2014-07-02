using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionGroupingParamChangedEventArgs
    /// </summary>
    public class AudioSessionGroupingParamChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The new grouping parameter for the session.
        /// </summary>
        public Guid NewGroupingParam { get; private set; }

        public AudioSessionGroupingParamChangedEventArgs(Guid newGroupingParam, Guid eventContext)
            : base(eventContext)
        {
            NewGroupingParam = newGroupingParam;
        }
    }
}