namespace CSCore.DMO
{
    /// <summary>
    ///     DmoSizeInfo
    /// </summary>
    public class DmoSizeInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DmoSizeInfo(int minSize, int alignment)
        {
            MinSize = minSize;
            Alignment = alignment;
        }

        /// <summary>
        ///     Minimum size of an input buffer for this stream, in bytes.
        /// </summary>
        public int MinSize { get; private set; }

        /// <summary>
        ///     The required buffer alignment, in bytes. If the input stream has no alignment requirement, the value is 1.
        /// </summary>
        public int Alignment { get; private set; }
    }
}