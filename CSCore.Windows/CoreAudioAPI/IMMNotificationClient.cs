using System.Runtime.CompilerServices;
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
    /// The <see cref="IMMNotificationClient"/> interface provides notifications when an audio endpoint device is added or removed, when the state or properties of an endpoint device change, or when there is a change in the default role assigned to an endpoint device.
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
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="deviceState">Specifies the new state of the endpoint device.</param>
        /// <returns>HRESULT</returns>
        void OnDeviceStateChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId, DeviceState deviceState);

        /// <summary>
        /// The OnDeviceAdded method indicates that a new audio endpoint device has been added.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <returns>HRESULT</returns>
        void OnDeviceAdded([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// The OnDeviceRemoved method indicates that an audio endpoint device has been removed.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <returns>HRESULT</returns>
        void OnDeviceRemoved([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// The OnDefaultDeviceChanged method notifies the client that the default audio endpoint
        /// device for a particular device role has changed.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction of the endpoint device.</param>
        /// <param name="role">The device role of the audio endpoint device.</param>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <returns>HRESULT</returns>
        void OnDefaultDeviceChanged([In] DataFlow dataFlow, [In] Role role,
            [In, MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// The OnPropertyValueChanged method indicates that the value of a property belonging to an
        /// audio endpoint device has changed.
        /// </summary>
        /// <param name="deviceId">The device id that identifies the audio endpoint device.</param>
        /// <param name="key">The <see cref="Win32.PropertyKey"/> that specifies the changed property.</param>        
        /// <returns>HRESULT</returns>
        void OnPropertyValueChanged([In, MarshalAs(UnmanagedType.LPWStr)] string deviceId, PropertyKey key);
    }
}