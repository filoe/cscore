using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides methods for enumerating multimedia device resources.
    /// </summary>
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    public class MMDeviceEnumerator : ComObject
    {
        private const string InterfaceName = "IMMDeviceEnumerator";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFlow"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static MMDevice DefaultAudioEndpoint(DataFlow dataFlow, Role role)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(dataFlow, role);
            }
        }


        public MMDeviceEnumerator()
        {
            var mmde = new MMDeviceEnumeratorObject() as IMMDeviceEnumerator;
            BasePtr = Marshal.GetComInterfaceForObject(mmde, typeof(IMMDeviceEnumerator));
        }

        public MMDevice this[string deviceID]
        {
            get { return GetDevice(deviceID); }
        }

        /// <summary>
        /// The <see cref="GetDefaultAudioEndpoint"/> method retrieves the default audio endpoint for the specified data-flow direction and role.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction for the endpoint device.</param>
        /// <param name="role">The role of the endpoint device.</param>
        /// <returns><see cref="MMDevice"/> instance of the endpoint object for the default audio endpoint device.</returns>
        public MMDevice GetDefaultAudioEndpoint(DataFlow dataFlow, Role role)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetDefaultAudioEndpointNative(dataFlow, role, out ptr), InterfaceName, "GetDefaultAudioEndpoint");
            return new MMDevice(ptr);
        }

        /// <summary>
        /// The <see cref="GetDefaultAudioEndpoint"/> method retrieves the default audio endpoint for the specified data-flow direction and role.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction for the endpoint device.</param>
        /// <param name="role">The role of the endpoint device.</param>
        /// <param name="device">Pointer to a pointer variable into which the method writes the address of the <see cref="MMDevice"/> COM object of the endpoint object for the default audio endpoint device. </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetDefaultAudioEndpointNative(DataFlow dataFlow, Role role, out IntPtr device)
        {
            IntPtr pdevice;
            int result = InteropCalls.CallI(UnsafeBasePtr, unchecked(dataFlow), unchecked(role), &pdevice, ((void**)(*(void**)UnsafeBasePtr))[4]);
            device = pdevice;
            return result;
        }


        /// <summary>
        /// Generates a collection of audio endpoint devices that meet the specified criteria.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction for the endpoint device.</param>
        /// <param name="stateMask">The state or states of the endpoints that are to be included in the collection.</param>
        /// <returns><see cref="MMDeviceCollection"/> which contains the enumerated devices.</returns>
        public MMDeviceCollection EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask)
        {
            IntPtr pcollection;
            CoreAudioAPIException.Try(EnumAudioEndpointsNative(dataFlow, stateMask, out pcollection), InterfaceName, "EnumAudioEndpoints");
            return new MMDeviceCollection(pcollection);
        }

        /// <summary>
        /// Generates a collection of audio endpoint devices that meet the specified criteria.
        /// </summary>
        /// <param name="dataFlow">The data-flow direction for the endpoint device.</param>
        /// <param name="stateMask">The state or states of the endpoints that are to be included in the collection.</param>
        /// <param name="collection">Pointer to a pointer variable into which the method writes the address of the <see cref="MMDeviceCollection"/> COM object of the device-collection object.</param>
        /// <returns>HRESULT</returns>
        public unsafe int EnumAudioEndpointsNative(DataFlow dataFlow, DeviceState stateMask, out IntPtr collection)
        {
            IntPtr pcollection;
            int result = InteropCalls.CallI(UnsafeBasePtr, unchecked(dataFlow), unchecked(stateMask), &pcollection, ((void**)(*(void**)UnsafeBasePtr))[3]);
            collection = pcollection;
            return result;
        }

        /// <summary>
        /// Retrieves an audio endpoint device that is identified by an endpoint ID string.
        /// </summary>
        /// <param name="id">Endpoint ID. The caller typically obtains this string from the <see cref="MMDevice.DeviceID"/> property or any method of the <see cref="IMMNotificationClient"/>.</param>
        /// <returns><see cref="MMDevice"/> instance for specified device.</returns>
        public MMDevice GetDevice(string id)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetDeviceNative(id, out ptr), InterfaceName, "GetDevice");
            return new MMDevice(ptr);
        }

        /// <summary>
        /// Retrieves an audio endpoint device that is identified by an endpoint ID string.
        /// </summary>
        /// <param name="id">Endpoint ID. The caller typically obtains this string from the <see cref="MMDevice.DeviceID"/> property or any method of the <see cref="IMMNotificationClient"/>.</param>
        /// <param name="device">Pointer to a pointer variable into which the method writes the address of the IMMDevice interface for the specified device. Through this method, the caller obtains a counted reference to the interface.</param>
        /// <returns>HREUSLT</returns>
        public unsafe int GetDeviceNative(string id, out IntPtr device)
        {
            var pid = Marshal.StringToHGlobalAnsi(id);
            IntPtr pdevice;
            int result = InteropCalls.CallI(UnsafeBasePtr, pid.ToPointer(), &pdevice, ((void**)(*(void**)UnsafeBasePtr))[5]);
            device = pdevice;
            return result;
        }

        /// <summary>
        /// Registers a client's notification callback interface.
        /// </summary>
        /// <param name="client">Implementation of the <see cref="IMMNotificationClient"/> which is should receive the notificaitons.</param>
        public void RegisterEndpointNotificationCallback(IMMNotificationClient client)
        {
            CoreAudioAPIException.Try(RegisterEndpointNotificationCallbackNative(client), InterfaceName, "RegisterEndpointNotificationCallback");
        }

        /// <summary>
        /// Registers a client's notification callback interface.
        /// </summary>
        /// <param name="client">Implementation of the <see cref="IMMNotificationClient"/> which is should receive the notificaitons.</param>
        /// <returns>HRESULT</returns>
        public unsafe int RegisterEndpointNotificationCallbackNative(IMMNotificationClient client)
        {
            int result = InteropCalls.CallI(UnsafeBasePtr, client, ((void**)(*(void**)UnsafeBasePtr))[6]);
            return result;
        }

        /// <summary>
        /// Deletes the registration of a notification interface that the client registered in a previous call to the <see cref="RegisterEndpointNotificationCallback"/> method.
        /// </summary>
        /// <param name="client">Implementation of the <see cref="IMMNotificationClient"/> which should be unregistered from any notifications.</param>
        public void UnregisterEndpointNotificationCallback(IMMNotificationClient client)
        {
            CoreAudioAPIException.Try(UnregisterEndpointNotificationCallbackNative(client), InterfaceName, "UnregisterEndpointNotificationCallback");
        }

        /// <summary>
        /// Deletes the registration of a notification interface that the client registered in a previous call to the <see cref="RegisterEndpointNotificationCallback"/> method.
        /// </summary>
        /// <param name="client">Implementation of the <see cref="IMMNotificationClient"/> which should be unregistered from any notifications.</param>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterEndpointNotificationCallbackNative(IMMNotificationClient client)
        {
            return InteropCalls.CallI(UnsafeBasePtr, client, ((void**)(*(void**)UnsafeBasePtr))[7]);
        }

        [ComImport]
        [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumeratorObject
        {
        }
    }
}