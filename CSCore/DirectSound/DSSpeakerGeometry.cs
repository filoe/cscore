namespace CSCore.DirectSound
{
    /// <summary>
    /// Defines values that can be combined with the <see cref="DSSpeakerConfigurations.Stereo"/> value.
    /// </summary>
    /// <remarks>To combine the a <see cref="DSSpeakerGeometry"/> value with the stereo value, use the <see cref="DirectSoundBase.CombineSpeakerConfiguration"/> method.</remarks>
    public enum DSSpeakerGeometry
    {
        /// <summary>
        /// The speakers are directed over an arc of 5 degrees.
        /// </summary>
        Min = 0x5,
        /// <summary>
        /// The speakers are directed over an arc of 10 degrees.
        /// </summary>
        Narrow = 0xA,
        /// <summary>
        /// The speakers are directed over an arc of 20 degrees.
        /// </summary>
        Wide = 0x14,
        /// <summary>
        /// The speakers are directed over an arc of 180 degrees.
        /// </summary>
        Max = 0xB4
    }
}