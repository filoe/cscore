using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides methods for enumerating multimedia device resources.
    /// </summary>
    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceEnumerator : IUnknown
    {
        /// <summary>
        /// Generates a collection of audio endpoint devices that meet the specified criteria.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction for the endpoint device.</param>
        /// <param name="stateMask">The state or states of the endpoints that are to be included in the collection.</param>
        /// <param name="deviceCollection">Pointer to a pointer variable into which the method writes the address of the <see cref="MMDeviceCollection"/> COM object of the device-collection object.</param>
        /// <returns>HRESULT</returns>
        int EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask, [Out] out IMMDeviceCollection deviceCollection);

        /// <summary>
        /// The <see cref="GetDefaultAudioEndpoint"/> method retrieves the default audio endpoint for the specified data-flow direction and role.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction for the endpoint device.</param>
        /// <param name="role">The role of the endpoint device.</param>
        /// <param name="device">Pointer to a pointer variable into which the method writes the address of the <see cref="MMDevice"/> COM object of the endpoint object for the default audio endpoint device. </param>
        /// <returns>HRESULT</returns>
        int GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, [Out] out IMMDevice device);

        /// <summary>
        /// Retrieves an audio endpoint device that is identified by an endpoint ID string.
        /// </summary>
        /// <param name="id">Endpoint ID. The caller typically obtains this string from the <see cref="MMDevice.DeviceID"/> property or any method of the <see cref="IMMNotificationClient"/>.</param>
        /// <param name="device">Pointer to a pointer variable into which the method writes the address of the IMMDevice interface for the specified device. Through this method, the caller obtains a counted reference to the interface.</param>
        /// <returns>HREUSLT</returns>
        int GetDevice([In, MarshalAs(UnmanagedType.LPWStr)] string id, [Out] out IMMDevice device);

        /// <summary>
        /// Registers a client's notification callback interface.
        /// </summary>
        /// <param name="notificationClient">Implementation of the <see cref="IMMNotificationClient"/> which is should receive the notificaitons.</param>
        /// <returns>HRESULT</returns>
        int RegisterEndpointNotificationCallback(IMMNotificationClient notificationClient);

        /// <summary>
        /// Deletes the registration of a notification interface that the client registered in a previous call to the <see cref="RegisterEndpointNotificationCallback"/> method.
        /// </summary>
        /// <param name="notificationClient">Implementation of the <see cref="IMMNotificationClient"/> which should be unregistered from any notifications.</param>
        /// <returns>HRESULT</returns>
        int UnregisterEndpointNotificationCallback(IMMNotificationClient notificationClient);
    }
}