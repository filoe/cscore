using System;

namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// OpenAL Exception
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class ALException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ALException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ALException(string message)
            : base(message)
        {
            LastError = ALInterops.alGetError();
        }

        /// <summary>
        /// Gets the last OpenAL Error.
        /// </summary>
        public ALErrorCode LastError { get; private set; }
    }
}
