using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    public class MMDeviceEnumerator : ComObject
    {
        const string c = "IMMDeviceEnumerator";

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

        public MMDevice GetDefaultAudioEndpoint(DataFlow dataFlow, Role role)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(GetDefaultAudioEndpoint(dataFlow, role, out ptr), c, "GetDefaultAudioEndpoint");
            return new MMDevice(ptr);
        }

        public MMDeviceCollection EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask)
        {
            IntPtr pcollection;
            CoreAudioAPIException.Try(EnumAudioEndpoints(dataFlow, stateMask, out pcollection), c, "EnumAudioEndpoints");
            return new MMDeviceCollection(pcollection);
        }

        public unsafe int EnumAudioEndpoints(DataFlow dataFlow, DeviceState stateMask, out IntPtr collection)
        {
            IntPtr pcollection;
            int result =  InteropCalls.CallI(_basePtr, unchecked(dataFlow), unchecked(stateMask), &pcollection, ((void**)(*(void**)_basePtr))[3]);
            collection = pcollection;
            return result;
        }

        public unsafe int GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, out IntPtr device)
        {
            IntPtr pdevice;
            int result = InteropCalls.CallI(_basePtr, unchecked(dataFlow), unchecked(role), &pdevice, ((void**)(*(void**)_basePtr))[4]);
            device = pdevice;
            return result;
        }

        public unsafe int GetDevice(string id, out IntPtr device)
        {
            var pid = Marshal.StringToHGlobalAnsi(id);
            IntPtr pdevice;
            int result = InteropCalls.CallI(_basePtr, pid.ToPointer(), &pdevice, ((void**)(*(void**)_basePtr))[5]);
            device = pdevice;
            return result;
        }

        public unsafe int RegisterEndpointNotificationCallback(IMMNotificationClient client)
        {
            //if (client == IntPtr.Zero)
            //    throw new ArgumentNullException("client");

            int result = InteropCalls.CallI(_basePtr, client, ((void**)(*(void**)_basePtr))[6]);
            return result;
        }

        public unsafe int UnregisterEndpointNotificationCallback(IMMNotificationClient client)
        {
            //if (client == IntPtr.Zero)
            //    throw new ArgumentNullException("client");

            return InteropCalls.CallI(_basePtr, client, ((void**)(*(void**)_basePtr))[7]);
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }

        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumeratorObject
        {
        }
    }
}
