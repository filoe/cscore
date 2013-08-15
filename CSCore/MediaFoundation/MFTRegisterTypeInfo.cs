using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [StructLayout(LayoutKind.Sequential)]
    public class MFTRegisterTypeInfo
    {
        public Guid GuidMajorType;
        public Guid GuidSubType;
    }
}