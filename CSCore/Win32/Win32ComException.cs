using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32ComException"/> class from serialization data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The StreamingContext object that supplies the contextual information about the source or destination.</param>
        protected Win32ComException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            InterfaceName = info.GetString("InterfaceName");
            Member = info.GetString("Member");
        }

        private static string GetErrorMessage(int result, string interfaceName, string member)
        {
            return String.Format("{0}::{1} caused an error: 0x{2}, \"{3}\".", interfaceName, member, result.ToString("x8"), GetMessage(result));
        }

        private static string GetMessage(int result)
        {
            StringBuilder stringBuilder = new StringBuilder(512);
            int num = NativeMethods.FormatMessage(12800, IntPtr.Zero, result, 0, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
            if (num != 0)
            {
                return stringBuilder.ToString().Trim();
            }

            return "Unknown HRESULT";
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see StreamingContext) for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Interface", InterfaceName);
            info.AddValue("Member", Member);
        }
    }
}