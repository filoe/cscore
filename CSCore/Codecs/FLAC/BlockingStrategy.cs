namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Defines the blocking strategy of the a flac frame.
    /// </summary>
    public enum BlockingStrategy
    {
        /// <summary>
        /// The <see cref="FlacFrameHeader.BlockSize"/> of flac frames is variable.
        /// </summary>
        VariableBlockSize,
        /// <summary>
        /// Each flac frame uses the same <see cref="FlacFrameHeader.BlockSize"/>.
        /// </summary>
        FixedBlockSize
    }
}