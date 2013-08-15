using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceEnumerator : IUnknown
    {
        int EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask, [Out] out IMMDeviceCollection deviceCollection);

        int GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, [Out] out IMMDevice device);

        int GetDevice([In, MarshalAs(UnmanagedType.LPWStr)] string id, [Out] out IMMDevice device);

        int RegisterEndpointNotificationCallback(IMMNotificationClient notificationClient);

        int UnregisterEndpointNotificationCallback(IMMNotificationClient notificationClient);
    }
}