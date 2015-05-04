using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Is used to create buffer objects, manage devices, and set up the environment. This object supersedes <see cref="DirectSoundBase"/> and adds new methods.
    /// Obtain a instance by calling the <see cref="DirectSoundBase.Create8"/> method.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [Guid("C50A7E93-F395-4834-9EF6-7FA99DE50966")]
    public unsafe class DirectSound8 : DirectSoundBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSound8"/> class.
        /// </summary>
        /// <param name="directSound">The native pointer of the <see cref="DirectSound8"/> COM object.</param>
        public DirectSound8(IntPtr directSound)
            : base(directSound)
        {
        }

        /// <summary>
        /// Ascertains whether the device driver is certified for DirectX. 
        /// </summary>
        /// <param name="certified">Receives a value which indicates whether the device driver is certified for DirectX.</param>
        /// <returns>DSResult</returns>
        public DSResult VerifyCertificationNative(out DSCertification certified)
        {
            certified = DSCertification.Uncertified;
            fixed (void* pcertified = &certified)
            {
                var result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, pcertified, ((void**)(*(void**)UnsafeBasePtr))[11]);
                return result;
            }
        }

        /// <summary>
        /// Ascertains whether the device driver is certified for DirectX. 
        /// </summary>
        /// <returns>A value which indicates whether the device driver is certified for DirectX. On emulated devices, the method returns <see cref="DSCertification.Unsupported"/>.</returns>
        public DSCertification VerifyCertification()
        {
            DSCertification certification;
            var result = VerifyCertificationNative(out certification);
            if(result == DSResult.Unsupported)
                return DSCertification.Unsupported;
            DirectSoundException.Try(result, "IDirectSound8",
                "VerifyCertification");
            return certification;
        }
    }
}