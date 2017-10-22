using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using CSCore.Win32;

namespace CSCore.DirectSound
{
    /// <summary>
    ///     Exception class which represents all DirectSound related exceptions.
    /// </summary>
    [Serializable]
    public class DirectSoundException : Win32ComException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectSoundException" /> class.
        /// </summary>
        /// <param name="result">The Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public DirectSoundException(DSResult result, string interfaceName, string member)
            : this((int) result, interfaceName, member)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Win32ComException" /> class.
        /// </summary>
        /// <param name="result">The Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public DirectSoundException(int result, string interfaceName, string member)
            : base(result, interfaceName, member)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectSoundException" /> class from serialization data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> object that holds the serialized object data.</param>
        /// <param name="context">
        ///     The StreamingContext object that supplies the contextual information about the source or
        ///     destination.
        /// </param>
        public DirectSoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Gets the <see cref="DSResult" /> which got associated with the specified <see cref="ExternalException.ErrorCode" />.
        /// </summary>
        public DSResult Result
        {
            get { return (DSResult) ErrorCode; }
        }

        /// <summary>
        ///     Throws an <see cref="DirectSoundException" /> if the <paramref name="result" /> is not
        ///     <see cref="DSResult.Ok" />.
        /// </summary>
        /// <param name="result">Errorcode.</param>
        /// <param name="interfaceName">
        ///     Name of the interface which contains the COM-function which returned the specified
        ///     <paramref name="result" />.
        /// </param>
        /// <param name="member">Name of the COM-function which returned the specified <paramref name="result" />.</param>
        public static void Try(DSResult result, string interfaceName, string member)
        {
            if (result != DSResult.Ok)
                throw new DirectSoundException(result, interfaceName, member);
        }
    }
}