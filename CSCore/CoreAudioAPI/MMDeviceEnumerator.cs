using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    public class MMDeviceEnumerator : ComObject
    {
        private const string c = "IMMDeviceEnumerator";

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

        public MMDevice GetDefaultAudioEndpoint(DataFlow dataFlow, Role role)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetDefaultAudioEndpointNative(dataFlow, role, out ptr), c, "GetDefaultAudioEndpoint");
            return new MMDevice(ptr);
        }

        public unsafe int GetDefaultAudioEndpointNative(DataFlow dataFlow, Role role, out IntPtr device)
        {
            IntPtr pdevice;
            int result = InteropCalls.CallI(_basePtr, unchecked(dataFlow), unchecked(role), &pdevice, ((void**)(*(void**)_basePtr))[4]);
            device = pdevice;
            return result;
        }

        public MMDeviceCollection EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask)
        {
            IntPtr pcollection;
            CoreAudioAPIException.Try(EnumAudioEndpointsNative(dataFlow, stateMask, out pcollection), c, "EnumAudioEndpoints");
            return new MMDeviceCollection(pcollection);
        }

        public unsafe int EnumAudioEndpointsNative(DataFlow dataFlow, DeviceState stateMask, out IntPtr collection)
        {
            IntPtr pcollection;
            int result = InteropCalls.CallI(_basePtr, unchecked(dataFlow), unchecked(stateMask), &pcollection, ((void**)(*(void**)_basePtr))[3]);
            collection = pcollection;
            return result;
        }

        public MMDevice GetDevice(string id)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetDeviceNative(id, out ptr), c, "GetDevice");
            return new MMDevice(ptr);
        }

        public unsafe int GetDeviceNative(string id, out IntPtr device)
        {
            var pid = Marshal.StringToHGlobalAnsi(id);
            IntPtr pdevice;
            int result = InteropCalls.CallI(_basePtr, pid.ToPointer(), &pdevice, ((void**)(*(void**)_basePtr))[5]);
            device = pdevice;
            return result;
        }

        public void RegisterEndpointNotificationCallback(IMMNotificationClient client)
        {
            CoreAudioAPIException.Try(RegisterEndpointNotificationCallbackNative(client), c, "RegisterEndpointNotificationCallback");
        }

        public unsafe int RegisterEndpointNotificationCallbackNative(IMMNotificationClient client)
        {
            int result = InteropCalls.CallI(_basePtr, client, ((void**)(*(void**)_basePtr))[6]);
            return result;
        }

        public void UnregisterEndpointNotificationCallback(IMMNotificationClient client)
        {
            CoreAudioAPIException.Try(UnregisterEndpointNotificationCallbackNative(client), c, "UnregisterEndpointNotificationCallback");
        }

        public unsafe int UnregisterEndpointNotificationCallbackNative(IMMNotificationClient client)
        {
            return InteropCalls.CallI(_basePtr, client, ((void**)(*(void**)_basePtr))[7]);
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }

        [ComImport]
        [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumeratorObject
        {
        }
    }
}