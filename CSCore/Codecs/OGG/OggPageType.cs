using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.OGG
{
    /// <summary>
    /// see http://xiph.org/vorbis/doc/framing.html header_type_flag
    /// </summary>
    [Flags]
    public enum OggPageHeaderType
    {
        None = 0,
        ContinuedPacket = 1,
        FirstPageOfLBS = 2,
        LastPageOfLBS = 3
    }
}