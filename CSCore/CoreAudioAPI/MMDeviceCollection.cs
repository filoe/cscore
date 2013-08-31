using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")]
    public class MMDeviceCollection : ComObject, IEnumerable<MMDevice>
    {
        public MMDeviceCollection(IntPtr ptr)
            : base(ptr)
        {
        }

        public int Count { get { return GetCount(); } }

        public MMDevice this[int index]
        {
            get { return ItemAt(index); }
        }

        public int GetCount()
        {
            int count = 0;
            CoreAudioAPIException.Try(GetCountNative(out count), "IMMDeviceCollection", "GetCount");
            return count;
        }

        public unsafe int GetCountNative(out int deviceCount)
        {
            fixed (void* pdeviceCount = &deviceCount)
            {
                deviceCount = 0;
                return InteropCalls.CallI(_basePtr, pdeviceCount, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        public MMDevice ItemAt(int deviceIndex)
        {
            IntPtr device;
            CoreAudioAPIException.Try(ItemAtNative(deviceIndex, out device), "IMMDeviceCollection", "Item");
            return new MMDevice(device);
        }

        public unsafe int ItemAtNative(int deviceIndex, out IntPtr device)
        {
            device = IntPtr.Zero;
            fixed (void* pdevice = &device)
            {
                return InteropCalls.CallI(_basePtr, deviceIndex, pdevice, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        public IEnumerator<MMDevice> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }
    }
}