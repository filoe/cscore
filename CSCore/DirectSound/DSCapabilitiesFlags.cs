using System;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Flags describing device capabilities.
    /// </summary>
    [Flags]
    public enum DSCapabilitiesFlags
    {
        /// <summary>
        /// The driver has been tested and certified by Microsoft. This flag is always set for WDM drivers. To test for certification, use <see cref="DirectSound8.VerifyCertification"/>.
        /// </summary>
        Certified = 0x40,
        /// <summary>
        /// The device supports all sample rates between the <see cref="DirectSoundCapabilities.MinSecondarySampleRate"/> and <see cref="DirectSoundCapabilities.MaxSecondarySampleRate"/> member values. Typically, this means that the actual output rate will be within +/- 10 hertz (Hz) of the requested frequency.
        /// </summary>
        ContinousRate = 0x10,
        /// <summary>
        /// The device does not have a DirectSound driver installed, so it is being emulated through the waveform-audio functions. Performance degradation should be expected.
        /// </summary>
        EmulatedDriver = 0x20,
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// The device supports a primary buffer with 16-bit samples.
        /// </summary>
        PrimaryBuffer16Bit = 8,
        /// <summary>
        /// The device supports primary buffers with 8-bit samples.
        /// </summary>
        PrimaryBuffer8Bit = 4,
        /// <summary>
        /// The device supports monophonic primary buffers.
        /// </summary>
        PrimaryBufferMono = 1,
        /// <summary>
        /// The device supports stereo primary buffers.
        /// </summary>
        PrimaryBufferStereo = 2,
        /// <summary>
        /// The device supports hardware-mixed secondary sound buffers with 16-bit samples.
        /// </summary>
        SecondaryBuffer16Bit = 0x800,
        /// <summary>
        /// The device supports hardware-mixed secondary buffers with 8-bit samples.
        /// </summary>
        SecondaryBuffer8Bit = 0x400,
        /// <summary>
        /// The device supports hardware-mixed monophonic secondary buffers.
        /// </summary>
        SecondaryBufferMono = 0x100,
        /// <summary>
        /// The device supports hardware-mixed stereo secondary buffers.
        /// </summary>
        SecondaryBufferStereo = 0x200
    }
}