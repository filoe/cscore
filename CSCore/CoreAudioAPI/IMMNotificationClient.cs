using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// mmdeviceapi.h line 221
    /// </summary>
    [ComImport]
    [Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IMMNotificationClient
    {
        /// <summary>
        /// The OnDeviceStateChanged method indicates that the state of an audio endpoint device has
        /// changed.
        /// </summary>
        /// <returns>HRESULT</returns>
        int OnDeviceStateChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID, DeviceState deviceState);

        /// <summary>
        /// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
        /// </summary>
        /// <returns>HRESULT</returns>
        int OnDeviceAdded([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID);

        /// <summary>
        /// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
        /// </summary>
        /// <returns>HRESULT</returns>
        int OnDeviceRemoved([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID);

        /// <summary>
        /// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint
        /// device for a particular device role has changed.
        /// </summary>
        /// <returns>HRESULT</returns>
        int OnDefaultDeviceChanged([In] DataFlow flow, [In] Role role, [In, MarshalAs(UnmanagedType.LPWStr)] string deviceID);

        /// <summary>
        /// The OnPropertyValueChanged method indicates that the value of a property belonging to an
        /// audio endpoint device has changed.
        /// </summary>
        /// <returns>HRESULT</returns>
        int OnPropertyValueChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceID, PropertyKey key);
    }
}
