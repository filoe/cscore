using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Win32
{
    /// <summary>
    /// Exception for Com Exceptions.
    /// </summary>
    [Serializable]
    public class Win32ComException : COMException
    {
        /// <summary>
        /// Throws an <see cref="Win32ComException"/> if the <paramref name="result"/> is not <see cref="HResult.S_OK"/>.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <paramref name="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result"/>.</param>

        public static void Try(int result, string interfaceName, string member)
        {
            if (result != 0)
                throw new Win32ComException(result, interfaceName, member);
        }

        /// <summary>
        /// Name of the Cominterface which caused the error.
        /// </summary>
        public string InterfaceName { get; private set; }

        /// <summary>
        /// Name of the member of the Cominterface which caused the error.
        /// </summary>
        public string Member { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32ComException"/> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">Name of the interface which contains the COM-function which returned the specified <paramref name="result"/>.</param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result"/>.</param>
        public Win32ComException(int result, string interfaceName, string member)
            : base(GetErrorMessage(result, interfaceName, member), result)
        {
            InterfaceName = interfaceName;
            Member = member;
        }

        private static string GetErrorMessage(int result, string interfaceName, string member)
        {
            return String.Format("{0}::{1} caused an error: 0x{2}, \"{3}\".", interfaceName, member, result.ToString("x8"), GetMessage(result));
        }

        private static string GetMessage(int result)
        {
            StringBuilder stringBuilder = new StringBuilder(512);
            int num = InteropFunctions.FormatMessage(12800, IntPtr.Zero, result, 0, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
            if (num != 0)
            {
                return stringBuilder.ToString().Trim();
            }

            return "Unknown HRESULT";
        }
    }
}