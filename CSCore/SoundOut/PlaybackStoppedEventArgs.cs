using System;

namespace CSCore.SoundOut
{
    /// <summary>
    ///     Provides data for the <see cref="ISoundOut.Stopped" /> event.
    /// </summary>
    public class PlaybackStoppedEventArgs : StoppedEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PlaybackStoppedEventArgs" /> class.
        /// </summary>
        public PlaybackStoppedEventArgs()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlaybackStoppedEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The associated exception. Can be null.</param>
        public PlaybackStoppedEventArgs(Exception exception)
            : base(exception)
        {
        }
    }
}