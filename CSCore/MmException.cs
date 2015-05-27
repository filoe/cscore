using System;
using System.Runtime.Serialization;

namespace CSCore
{
    /// <summary>
    ///     Exception class for all MM-APIs like waveOut or ACM.
    /// </summary>
    [Serializable]
    public class MmException : Exception
    {
        /// <summary>
        ///     Throws an <see cref="MmException" /> if the <paramref name="result" /> is not
        ///     <see cref="MmResult.NoError" />.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="function">Name of the function which returned the specified <paramref name="result" />.</param>
        public static void Try(MmResult result, string function)
        {
            if (result != MmResult.NoError)
                throw new MmException(result, function);
        }

        /// <summary>
        ///     Gets the <see cref="MmResult" /> which describes the error.
        /// </summary>
        public MmResult Result { get; private set; }

        /// <summary>
        ///     Gets the name of the function which caused the error.
        /// </summary>
        [Obsolete("Use the Function property instead.")]
        public string Target { get; private set; }

        /// <summary>
        ///     Gets the name of the function which caused the error.
        /// </summary>
#pragma warning disable 618
        public string Function
        {
            get { return Target; }
        }
#pragma warning restore 618

        /// <summary>
        ///     Initializes a new instance of the <see cref="MmException" /> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="function">Name of the function which returned the specified <paramref name="result" />.</param>
        public MmException(MmResult result, string function)
        {
            Result = result;
#pragma warning disable 618
            Target = function;
#pragma warning restore 618
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MmException"/> class from serialization data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The StreamingContext object that supplies the contextual information about the source or destination.</param>
        public MmException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
#pragma warning disable 618
            Target = info.GetString("Target");
#pragma warning restore 618
            Result = (MmResult) info.GetInt32("Result");
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see StreamingContext) for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
#pragma warning disable 618
            info.AddValue("Target", Target);
#pragma warning restore 618
            info.AddValue("Result", (int) Result);
        }
    }
}