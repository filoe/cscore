using System;

namespace CSCore
{
    /// <summary>
    ///     Provides data for any stopped operations.
    /// </summary>
    public class StoppedEventArgs : EventArgs
    {
        private readonly Exception _exception;


        /// <summary>
        ///     Initializes a new instance of the <see cref="StoppedEventArgs" /> class.
        /// </summary>
        public StoppedEventArgs()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoppedEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The associated exception. Can be null.</param>
        public StoppedEventArgs(Exception exception)
        {
            _exception = exception;
        }

        /// <summary>
        ///     Gets a value which indicates whether the operation stopped due to an error. True means that that the operation
        ///     stopped due to an error. False means that the operation did not stop due to an error.
        /// </summary>
        public virtual bool HasError
        {
            get { return _exception != null; }
        }

        /// <summary>
        ///     Gets the associated <see cref="Exception" /> which caused the operation to stop.
        /// </summary>
        /// <value>Can be null.</value>
        public virtual Exception Exception
        {
            get { return _exception; }
        }
    }
}