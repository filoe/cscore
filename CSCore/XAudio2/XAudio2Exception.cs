using System;
using System.Runtime.Serialization;
using CSCore.Win32;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     XAudio2-COMException.
    /// </summary>
    [Serializable]
    public class XAudio2Exception : Win32ComException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2Exception" /> class.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public XAudio2Exception(int result, string interfaceName, string member)
            : base(result, interfaceName, member)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2Exception" /> class from serialization data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> object that holds the serialized object data.</param>
        /// <param name="context">
        ///     The StreamingContext object that supplies the contextual information about the source or
        ///     destination.
        /// </param>
        public XAudio2Exception(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Throws an <see cref="XAudio2Exception" /> if the <paramref name="result" /> is not <see cref="HResult.S_OK" />.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public new static void Try(int result, string interfaceName, string member)
        {
            if (result != (int) Win32.HResult.S_OK)
                throw new XAudio2Exception(result, interfaceName, member);
        }
    }
}