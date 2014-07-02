using System;

namespace CSCore
{
    /// <summary>
    ///     Exception class for all MM-APIs like waveOut or ACM.
    /// </summary>
    public class MmException : Exception
    {
        /// <summary>
        ///     Throws an <see cref="MmException" /> if the <paramref name="result" /> is not
        ///     <see cref="MmResult.MMSYSERR_NOERROR" />.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="function">Name of the function which returned the specified <paramref name="result" />.</param>
        public static void Try(MmResult result, string function)
        {
            if (result != MmResult.MMSYSERR_NOERROR)
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
    }
}