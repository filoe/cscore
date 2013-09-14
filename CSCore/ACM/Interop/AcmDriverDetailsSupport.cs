using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.ACM
{
    //see http://msdn.microsoft.com/en-us/library/windows/desktop/dd742891(v=vs.85).aspx
    [Flags]
    public enum AcmDriverDetailsSupport
    {
        None = 0x0,
        Codec = 0x1,
        Converter = 0x2,
        Filter = 0x4,
        Hardware = 0x8,
        Async = 0x10,
        Local = 0x40000000,
        Disabled = unchecked((int)0x80000000)
    }
}
