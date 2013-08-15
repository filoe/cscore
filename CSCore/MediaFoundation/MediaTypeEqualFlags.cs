using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MediaTypeEqualFlags
    {
        None = 0x0,
        MajorTypes = 0x1,
        FormatTypes = 0x2,
        Data = 0x4,
        UserData = 0x8
    }
}