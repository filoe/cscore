namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Defines possible values for the <see cref="DmoChorusEffect.Waveform"/> property.
    /// Default value is WaveformSin (used for <see cref="DmoChorusEffect.Waveform"/>).
    /// </summary>
    public enum ChorusWaveform
    {
        /// <summary>
        /// Sine 
        /// Default value for <see cref="DmoChorusEffect.Waveform"/>.
        /// </summary>
        WaveformSin = 1,
        /// <summary>
        /// Trinagle
        /// </summary>
        WaveformTriangle = 0
    }
}