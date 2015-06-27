using System;
using System.Runtime.Serialization;

namespace CSCore.Codecs.AIFF
{
    /// <summary>
    ///     Represents errors that occur when decoding or encoding Aiff-streams/files.
    /// </summary>
    [Serializable]
    public class AiffException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffException" /> class.
        /// </summary>
        public AiffException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AiffException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The <see cref="Exception" /> that caused the <see cref="AiffException" />.</param>
        public AiffException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        protected AiffException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}