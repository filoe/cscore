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

        /// <summary>
        ///     Gets a value which indicates whether the playback stopped due to an error. True means that that the playback
        ///     stopped due to an error. False means that the playback did not stop due to an error.
        /// </summary>
        public override bool HasError
        {
            get { return base.HasError; }
        }

        /// <summary>
        ///     Gets the associated <see cref="Exception" /> which caused the playback to stop.
        /// </summary>
        /// <value>Can be null.</value>
        public override Exception Exception
        {
            get { return base.Exception; }
        }
    }
}