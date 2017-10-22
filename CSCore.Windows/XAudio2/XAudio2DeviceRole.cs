namespace CSCore.XAudio2
{
    /// <summary>
    ///     Describes device roles of an XAudio2 Device. Used in <see cref="DeviceDetails" />.
    /// </summary>
    public enum XAudio2DeviceRole
    {
        /// <summary>
        ///     Device is not used as the default device for any applications.
        /// </summary>
        NotDefaultDevice = 0x0,

        /// <summary>
        ///     Device is used in audio console applications.
        /// </summary>
        DefaultConsoleDevice = 0x1,

        /// <summary>
        ///     Device is used to play multimedia.
        /// </summary>
        DefaultMultimediaDevice = 0x2,

        /// <summary>
        ///     Device is used for voice communication.
        /// </summary>
        DefaultCommunicationsDevice = 0x4,

        /// <summary>
        ///     Device is used in for games.
        /// </summary>
        DefaultGameDevice = 0x8,

        /// <summary>
        ///     Devices is the default device for all applications.
        /// </summary>
        GlobalDefaultDevice = 0xf,

        /// <summary>
        ///     The role of the device is not valid.
        /// </summary>
        InvalidDeviceRole = ~GlobalDefaultDevice,
    }
}