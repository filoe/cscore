using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.ACM
{
    //see http://msdn.microsoft.com/en-us/library/windows/desktop/dd742890(v=vs.85).aspx
    [Flags]
    public enum AcmDriverEnumFlags
    {
        None = 0x0,
        Disabled = unchecked((int)0x80000000),
        NoLocal = 0x40000000
    }
}
