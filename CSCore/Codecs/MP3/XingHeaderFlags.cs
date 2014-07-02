using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// Defines the header flags of a xing header.
    /// </summary>
    [Flags]
    public enum XingHeaderFlags
    {
        /// <summary>
        /// Frames field is present
        /// </summary>
        Frames = 1,
        /// <summary>
        /// Bytes field is present.
        /// </summary>
        Bytes = 2,
        /// <summary>
        /// TOC field is present.
        /// </summary>
        Toc = 4,
        /// <summary>
        /// Quality indicator field is present.
        /// </summary>
        QualityIndicator = 8
    }
}
