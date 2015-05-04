namespace CSCore.DirectSound
{
    /// <summary>
    /// Defines possible speaker configurations.
    /// </summary>
    public enum DSSpeakerConfigurations
    {
        /// <summary>
        /// The audio is passed through directly, without being configured for speakers.
        /// </summary>
        DirectOut = 0x0,
        /// <summary>
        /// The audio is played through headphones.
        /// </summary>
        HeadPhone = 0x1,
        /// <summary>
        /// The audio is played through a single speaker.
        /// </summary>
        Mono = 0x2,
        /// <summary>
        /// The audio is played through quadraphonic speakers.
        /// </summary>
        Quad = 0x3,
        /// <summary>
        /// The audio is played through stereo speakers (default value).
        /// </summary>
        Stereo = 0x4,
        /// <summary>
        /// The audio is played through surround speakers.
        /// </summary>
        Surround = 0x5,
        /// <summary>
        /// The audio is played through a home theater speaker arrangement of five surround speakers with a subwoofer.
        /// </summary>
        /// <remarks>Obsolete 5.1 setting. Use <see cref="FivePointOneSurround"/> instead.</remarks>
        FivePointOne = 0x6,
        /// <summary>
        /// The audio is played through a home theater speaker arrangement of seven surround speakers with a subwoofer.
        /// </summary>
        /// <remarks>Obsolete 7.1 setting. Use <see cref="SevenPointOneSurround"/> instead.</remarks>
        SevenPointOne = 0x7,
        /// <summary>
        /// The audio is played through a home theater speaker arrangement of seven surround speakers with a subwoofer. This value applies to Windows XP SP2 or later.
        /// </summary>
        SevenPointOneSurround = 0x8,
        /// <summary>
        /// The audio is played through a home theater speaker arrangement of five surround speakers with a subwoofer. This value applies to Windows Vista or later.
        /// </summary>
        FivePointOneSurround = 0x9,
        /// <summary>
        /// The audio is played through a wide speaker arrangement of seven surround speakers with a subwoofer. (<see cref="SevenPointOne"/> is still defined, but is obsolete as of Windows XP SP 2. Use <see cref="SevenPointOneWide"/> instead.)
        /// </summary>
        SevenPointOneWide = SevenPointOne,
        /// <summary>
        /// The audio is played through a speaker arrangement of five surround speakers with a subwoofer. (<see cref="FivePointOne"/> is still defined, but is obsolete as of Windows Vista. Use <see cref="FivePointOneBack"/> instead.)
        /// </summary>
        FivePointOneBack = FivePointOne
    }
}