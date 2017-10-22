namespace CSCore.DirectSound
{
    /*
     #define DS_CERTIFIED                0x00000000
     #define DS_UNCERTIFIED              0x00000001
     */

    /// <summary>
    /// Defines possible return values for the <see cref="DirectSound8.VerifyCertification"/> method.
    /// </summary>
    /// <remarks>For more information, see <see cref="DirectSound8.VerifyCertification"/> or <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.verifycertification(v=vs.85).aspx"/>.</remarks>
    public enum DSCertification
    {
        /// <summary>
        /// Driver is certified for DirectSound.
        /// </summary>
        Certified = 0,
        /// <summary>
        /// Driver is not certified for DirectSound.
        /// </summary>
        Uncertified = 1,
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <remarks>The <see cref="DirectSound8.VerifyCertificationNative"/> method returned DSERR_UNSUPPORTED.</remarks>
        Unsupported = 2
    }
}