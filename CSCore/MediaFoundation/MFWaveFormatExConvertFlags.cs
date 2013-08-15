using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFWaveFormatExConvertFlags
    {
        Normal = 0,
        ForceExtensible = 1
    }
}