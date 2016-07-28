using System;

namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// OpenAL Exception
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class ALException : Exception
    {
        public static void Try(string functionName)
        {
            Try(functionName, IntPtr.Zero);
        }

        public static void Try(string functionName, IntPtr contextHandle)
        {
            ALErrorCode errorCode;
            if (functionName.StartsWith("alc"))
                errorCode = ALInterops.alcGetError(contextHandle);
            else
                errorCode = ALInterops.alGetError();

            if (errorCode != ALErrorCode.NoError)
                throw new ALException(String.Format("{0} returned {1}.", functionName, errorCode));
        }

        public static void Try(Action alAction, string functionName)
        {
            Try(alAction, functionName, IntPtr.Zero);
        }

        public static void Try(Action alAction, string functionName, IntPtr contextHandle)
        {
            ALErrorCode errorCode;
            if (functionName.StartsWith("alc"))
                ALInterops.alcGetError(contextHandle);
            else
                ALInterops.alGetError();

            alAction();

            if (functionName.StartsWith("alc"))
                errorCode = ALInterops.alcGetError(contextHandle);
            else
                errorCode = ALInterops.alGetError();

            if (errorCode != ALErrorCode.NoError)
                throw new ALException(String.Format("{0} returned {1}.", functionName, errorCode));
        }

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
        /// Initializes a new instance of the <see cref="ALException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ALException(string message, ALErrorCode errorCode)
            : base(message)
        {
            LastError = errorCode;
        }

        /// <summary>
        /// Gets the last OpenAL Error.
        /// </summary>
        public ALErrorCode LastError { get; private set; }
    }
}
