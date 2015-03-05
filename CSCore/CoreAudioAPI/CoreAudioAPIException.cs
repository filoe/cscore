using System;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// CoreAudioAPI COM Exception
    /// </summary>
// ReSharper disable once InconsistentNaming
    [Serializable]
    public class CoreAudioAPIException : Win32ComException
    {
        /// <summary>
        /// Throws an <see cref="CoreAudioAPIException"/> if the <paramref name="result"/> is not <see cref="HResult.S_OK"/>.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <paramref name="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result"/>.</param>
        public new static void Try(int result, string interfaceName, string member)
        {
            if (result != 0)
                throw new CoreAudioAPIException(result, interfaceName, member);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreAudioAPIException"/> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <paramref name="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result"/>.</param>
        public CoreAudioAPIException(int result, string interfaceName, string member)
            : base(result, interfaceName, member)
        {
        }
    }
}