using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.MP3
{
    [Flags]
    public enum XingHeaderFlags
    {
        Frames = 1,
        Bytes = 2,
        Toc = 4,
        VbrScale = 8
    }
}
