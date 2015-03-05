namespace CSCore.DMO
{
    /// <summary>
    ///     Encapsulates the values retrieved by the <see cref="MediaObject.GetInputSizeInfo"/> method.
    /// </summary>
    public class DmoInputSizeInfo : DmoSizeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DmoInputSizeInfo"/> class.
        /// </summary>
        /// <param name="minSize">The minimum size of an input buffer for the stream, in bytes.</param>
        /// <param name="alignment">The required buffer alignment, in bytes. If the stream has no alignment requirement, the value is 1</param>
        /// <param name="maxLookahead">The maximum amount of data that the DMO will hold for a lookahead, in bytes. If the DMO does not perform a lookahead on the stream, the value is zero.</param>
        public DmoInputSizeInfo(int minSize, int alignment, int maxLookahead)
            : base(minSize, alignment)
        {
            MaxLookahead = maxLookahead;
        }

        /// <summary>
        ///     Gets the maximum amount of data that the DMO will hold for a lookahead, in bytes. If the DMO does not perform a
        ///     lookahead on the stream, the value is zero.
        /// </summary>
        public int MaxLookahead { get; private set; }
    }
}