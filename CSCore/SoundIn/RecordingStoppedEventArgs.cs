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

        /// <summary>
        ///     Gets a value which indicates whether the recording _stopped due to an error. True means that that the recording
        ///     _stopped due to an error. False means that the recording did not stop due to an error.
        /// </summary>
        public override bool HasError
        {
            get { return base.HasError; }
        }

        /// <summary>
        ///     Gets the associated <see cref="Exception" /> which caused the recording to stop.
        /// </summary>
        /// <value>Can be null.</value>
        public override Exception Exception
        {
            get { return base.Exception; }
        }
    }
}