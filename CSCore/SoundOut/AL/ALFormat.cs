namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// Defines different OpenAL formats.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal enum ALFormat
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Mono, 8Bit.
        /// </summary>
        Mono8Bit = 0x1100,

        /// <summary>
        /// Mono, 16Bit.
        /// </summary>
        Mono16Bit = 0x1101,

        /// <summary>
        /// Stereo, 8Bit.
        /// </summary>
        Stereo8Bit = 0x1102,

        /// <summary>
        /// Stereo, 16Bit.
        /// </summary>
        Stereo16Bit = 0x1103,

        /// <summary>
        /// Mono, float 32Bit.
        /// This is not required to be supported on all implementations
        /// </summary>
        MonoFloat32Bit = 0x10010,

        /// <summary>
        /// Stereo, float 32bit
        /// This is not required to be supported on all implementations
        /// </summary>
        StereoFloat32Bit = 0x10011
    }
}
