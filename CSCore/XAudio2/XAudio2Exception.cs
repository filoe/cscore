using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.XAudio2
{
    /// <summary>
    /// XAudio2-COMException.
    /// </summary>
    public class XAudio2Exception : COMException
    {
        /// <summary>
        /// Throws an <see cref="XAudio2Exception"/> if the <see cref="result"/> is not <see cref="HResult.S_OK"/>.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <see cref="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <see cref="result"/>.</param>
        public static void Try(int result, string interfaceName, string member)
        {
            if (result != (int)Win32.HResult.S_OK)
                throw new XAudio2Exception(result, interfaceName, member);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAudio2Exception"/> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <see cref="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <see cref="result"/>.</param>
        public XAudio2Exception(int result, string interfaceName, string member)
            : base(String.Format("{0}::{1} returned 0x{2}", interfaceName, member, result.ToString("x")), result)
        {
        }
    }
}