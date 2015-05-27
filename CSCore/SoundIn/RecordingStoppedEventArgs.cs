using System;

namespace CSCore.SoundIn
{
    /// <summary>
    ///     Provides data for the <see cref="ISoundIn.Stopped" /> event.
    /// </summary>
    public class RecordingStoppedEventArgs : StoppedEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordingStoppedEventArgs" /> class.
        /// </summary>
        public RecordingStoppedEventArgs()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordingStoppedEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The associated exception. Can be null.</param>
        public RecordingStoppedEventArgs(Exception exception)
            : base(exception)
        {
        }
    }
}