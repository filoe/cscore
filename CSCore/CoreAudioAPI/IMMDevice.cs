using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [ComImport]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDevice : IUnknown
    {
        //http://msdn.microsoft.com/en-us/library/windows/desktop/dd371405(v=vs.85).aspx
        int Activate(Guid iid, ExecutionContext clsctx, IntPtr activationParams/*zero*/, [Out]out IntPtr pinterface);

        int OpenPropertyStore(StorageAccess access, [Out] out IPropertyStore propertystore);

        int GetId([Out, MarshalAs(UnmanagedType.LPWStr)]out string deviceID);

        int GetState([Out] out DeviceState state);
    }
}