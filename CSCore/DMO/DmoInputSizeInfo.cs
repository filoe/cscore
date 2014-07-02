namespace CSCore.DMO
{
    /// <summary>
    ///     DmoInputSizeInfo
    /// </summary>
    public class DmoInputSizeInfo : DmoSizeInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DmoInputSizeInfo(int minSize, int alignment, int maxLookahead)
            : base(minSize, alignment)
        {
            MaxLookahead = maxLookahead;
        }

        /// <summary>
        ///     The maximum amount of data that the DMO will hold for a lookahead, in bytes. If the DMO does not perform a
        ///     lookahead on the stream, the value is zero.
        /// </summary>
        public int MaxLookahead { get; private set; }
    }
}