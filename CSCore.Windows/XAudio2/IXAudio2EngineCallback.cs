using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Internal used IXAudio2EngineCallback-wrapper. The default implementation of this interface is
    ///     <see cref="XAudio2EngineCallback" />.
    /// </summary>
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IXAudio2EngineCallback
    {
        /// <summary>
        ///     OnProcessingPassStart
        /// </summary>
        void OnProcessingPassStart();

        /// <summary>
        ///     OnProcessingPassEnd
        /// </summary>
        void OnProcessingPassEnd();

        /// <summary>
        ///     OnCriticalError
        /// </summary>
        /// <param name="error">Errorcode</param>
        void OnCriticalError(int error);
    }
}