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
        /// Checks whether the OpenAL Error-Stack contains errors. 
        /// If an error occurs, the a <see cref="ALException"/> gets thrown.
        /// </summary>
        /// <param name="functionName">The name of the OpenAL function which caused the error.</param>
        /// <remarks>
        /// If the <paramref name="functionName"/> starts with "alc" use the <see cref="Try(string, IntPtr)"/> 
        /// method instead and provide a contextHandle.
        /// </remarks>
        public static void Try(string functionName)
        {
            Try(functionName, IntPtr.Zero);
        }

        /// <summary>
        /// Checks whether the OpenAL Error-Stack contains errors. 
        /// If an error occurs, the a <see cref="ALException"/> gets thrown.
        /// </summary>
        /// <param name="functionName">The name of the OpenAL function which caused the error.</param>
        /// <param name="contextHandle">The handle of the <see cref="ALContext"/> if the <paramref name="functionName"/> starts with "alc".</param>
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

        /// <summary>
        /// Resets the OpenAL Error-Stack, executes the <paramref name="alAction"/> and checks
        /// for an OpenAL Error afterwards. If an error occurs, the a <see cref="ALException"/>
        /// gets thrown.
        /// </summary>
        /// <param name="alAction">The action to execute.</param>
        /// <param name="functionName">The name of the OpenAL function which gets executed by the <paramref name="alAction"/>.</param>
        /// <remarks>If a OpenAL function, starting with "alc" gets executed, use the <see cref="Try(Action, string, IntPtr)"/> method instead.</remarks>
        public static void Try(Action alAction, string functionName)
        {
            Try(alAction, functionName, IntPtr.Zero);
        }

        /// <summary>
        /// Resets the OpenAL Error-Stack, executes the <paramref name="alAction"/> and checks
        /// for an OpenAL Error afterwards. If an error occurs, the an <see cref="ALException"/>
        /// gets thrown. 
        /// </summary>
        /// <param name="alAction">The action to execute.</param>
        /// <param name="functionName">The name of the OpenAL function which gets executed by the <paramref name="alAction"/>.</param>
        /// <param name="contextHandle">The handle of the <see cref="ALContext"/> if the <paramref name="functionName"/> starts with "alc".</param>
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
