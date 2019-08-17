using System;
using System.Runtime.InteropServices;
using CSCore.Ffmpeg.Interops;

namespace CSCore.Ffmpeg
{
    /// <summary>
    /// FFmpeg Exception
    /// </summary>
    public class FfmpegException : Exception
    {
        /// <summary>
        /// Throws an <see cref="FfmpegException"/> if the <paramref name="errorCode"/> is less than zero.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="function">The name of the function that returned the <paramref name="errorCode"/>.</param>
        /// <exception cref="CSCore.Ffmpeg.FfmpegException"><see cref="FfmpegException"/> with some details (including the <paramref name="errorCode"/> and the <paramref name="function"/>).</exception>
        public static void Try(int errorCode, string function)
        {
            if (errorCode < 0)
                throw new FfmpegException(errorCode, function);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegException"/> class with an <paramref name="errorCode"/> that got returned by any ffmpeg <paramref name="function"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="function">The name of the function that returned the <paramref name="errorCode"/>.</param>
        public FfmpegException(int errorCode, string function)
            : base(String.Format("{0} returned 0x{1:x8}: {2}", function, errorCode, FfmpegCalls.AvStrError(errorCode)))
        {
            ErrorCode = errorCode;
            Function = function;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegException"/> class with a <paramref name="message"/> describing an error that occurred by calling any ffmpeg <paramref name="function"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="function">The name of the function that caused the error.</param>
        public FfmpegException(string message, string function)
            : base(String.Format("{0} failed: {1}", message, function))
        {
            Function = function;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegException"/> class with a message that describes the error.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FfmpegException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Gets the ffmpeg function which caused the error.
        /// </summary>
        public string Function { get; private set; }
    }
}