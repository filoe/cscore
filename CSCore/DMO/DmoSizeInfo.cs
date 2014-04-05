using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.DMO
{
    /// <summary>
    /// DmoSizeInfo
    /// </summary>
    public class DmoSizeInfo
    {
        /// <summary>
        /// Minimum size of an input buffer for this stream, in bytes.
        /// </summary>
        public int MinSize { get; private set; }
        /// <summary>
        /// The required buffer alignment, in bytes. If the input stream has no alignment requirement, the value is 1.
        /// </summary>
        public int Alignment { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DmoSizeInfo(int minSize, int alignment)
        {
            MinSize = minSize;
            Alignment = alignment;
        }
    }

    /// <summary>
    /// DmoInputSizeInfo
    /// </summary>
    public class DmoInputSizeInfo : DmoSizeInfo
    {
        /// <summary>
        /// The maximum amount of data that the DMO will hold for a lookahead, in bytes. If the DMO does not perform a lookahead on the stream, the value is zero.
        /// </summary>
        public int MaxLookahead { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DmoInputSizeInfo(int minSize, int alignment, int maxLookahead)
            : base(minSize, alignment)
        {
            MaxLookahead = maxLookahead;
        }
    }
}
