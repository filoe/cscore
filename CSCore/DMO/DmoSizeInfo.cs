namespace CSCore.DMO
{
    /// <summary>
    ///     Encapsulates the values retrieved by the <see cref="MediaObject.GetInputSizeInfo"/>- and the <see cref="MediaObject.GetOutputSizeInfo"/>- method.
    /// </summary>
    public class DmoSizeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DmoInputSizeInfo"/> class.
        /// </summary>
        /// <param name="minSize">The minimum size of an input buffer for the stream, in bytes.</param>
        /// <param name="alignment">The required buffer alignment, in bytes. If the stream has no alignment requirement, the value is 1.</param>
        public DmoSizeInfo(int minSize, int alignment)
        {
            MinSize = minSize;
            Alignment = alignment;
        }

        /// <summary>
        ///     Gets the minimum size of an input buffer for this stream, in bytes.
        /// </summary>
        public int MinSize { get; private set; }

        /// <summary>
        ///     Gets the required buffer alignment, in bytes. If the input stream has no alignment requirement, the value is 1.
        /// </summary>
        public int Alignment { get; private set; }
    }
}