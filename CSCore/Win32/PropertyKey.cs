using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PropertyKey
    {
        public Guid ID;
        public int PropertyID;

        public PropertyKey(Guid id, int propertyid)
        {
            ID = id;
            PropertyID = propertyid;
        }
    }
}