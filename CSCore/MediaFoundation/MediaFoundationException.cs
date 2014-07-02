using CSCore.Win32;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Mediafoundation COM Exception
    /// </summary>
    public class MediaFoundationException : Win32ComException
    {
        /// <summary>
        /// Throws an <see cref="MediaFoundationException"/> if the <paramref name="result"/> is not <see cref="HResult.S_OK"/>.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <paramref name="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result"/>.</param>
        public new static void Try(int result, string interfaceName, string member)
        {
            if (result != 0)
                throw new MediaFoundationException(result, interfaceName, member);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFoundationException"/> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <paramref name="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result"/>.</param>
        public MediaFoundationException(int result, string interfaceName, string member)
            : base(result, interfaceName, member)
        {
        }
    }
}