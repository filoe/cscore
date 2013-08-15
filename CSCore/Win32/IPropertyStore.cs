using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    //propsys.h
    //http://msdn.microsoft.com/en-us/library/windows/desktop/bb761474(v=vs.85).aspx
    [ComImport]
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore : IUnknown
    {
        int GetCount(out int propertyCount);

        int GetAt(int propertyIndex, [Out] out PropertyKey propertyKey);

        int GetValue(ref PropertyKey key, [Out] out PropertyVariant value);

        int SetValue(ref PropertyKey key, ref PropertyVariant value);

        int Commit();
    }
}