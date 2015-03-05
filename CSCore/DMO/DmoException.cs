using System;
using System.Runtime.Serialization;
using CSCore.Win32;

namespace CSCore.DMO
{
    /// <summary>
    ///     DirectX Media Object COM Exception
    /// </summary>
    [Serializable]
    public class DmoException : Win32ComException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DmoException" /> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public DmoException(int result, string interfaceName, string member)
            : base(result, interfaceName, member)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DmoException" /> class from serialization data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> object that holds the serialized object data.</param>
        /// <param name="context">
        ///     The StreamingContext object that supplies the contextual information about the source or
        ///     destination.
        /// </param>
        public DmoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Throws an <see cref="DmoException" /> if the <paramref name="result" /> is not <see cref="HResult.S_OK" />.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public new static void Try(int result, string interfaceName, string member)
        {
            if (result != 0)
                throw new DmoException(result, interfaceName, member);
        }

        /*private static string TryGetFriendlyName(int result)
        {
            if (Enum.IsDefined(typeof(DmoErrorCodes), result))
                return String.Format(" ({0})", Enum.GetName(typeof(DmoErrorCodes), result));
            else if (Enum.IsDefined(typeof(Win32.HResult), result))
                return String.Format(" ({0})", Enum.GetName(typeof(Win32.HResult), result));
            else
                return String.Empty;
        }*/
    }
}