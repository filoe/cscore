using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    public class MMDevice : ComObject
    {
        private const string c = "IMMDevice";

        public MMDevice(IntPtr ptr)
            : base(ptr)
        {
        }

        private PropertyStore _propertyStore;

        /// <summary>
        /// Warning: This PropertyStore is just Readable. Use the OpenPropertyStore-Method to get
        ///          writeable PropertyStore.
        /// </summary>
        public PropertyStore PropertyStore
        {
            get
            {
                return _propertyStore ?? (_propertyStore = OpenPropertyStore(StorageAccess.Read));
            }
        }

        public string DeviceID
        {
            get
            {
                string id;
                CoreAudioAPIException.Try(GetIdNative(out id), c, "GetId");
                return id;
            }
        }

        public string FriendlyName
        {
            get { return PropertyStore[PropertyStore.FriendlyName].ToString(); }
        }

        public DeviceState DeviceState
        {
            get
            {
                DeviceState state;
                CoreAudioAPIException.Try(GetStateNative(out state), c, "GetState");
                return state;
            }
        }

        public unsafe int ActivateNative(Guid iid, ExecutionContext context, IntPtr activationParams, out IntPtr pinterface)
        {
            pinterface = IntPtr.Zero;
            fixed (void* ppinterface = &pinterface)
            {
                var result = InteropCalls.CallI(_basePtr, ((void*)&iid), context, activationParams, new IntPtr(ppinterface), ((void**)(*(void**)_basePtr))[3]);
                return result;
            }
        }

        public IntPtr Activate(Guid iid, ExecutionContext context, IntPtr activationParams)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(ActivateNative(iid, context, activationParams, out ptr), c, "Activate");
            return ptr;
        }

        public PropertyStore OpenPropertyStore(StorageAccess storageAccess)
        {
            IntPtr propstorePtr;
            CoreAudioAPIException.Try(OpenPropertyStoreNative(storageAccess, out propstorePtr),
                "IMMDevice", "OpenPropertyStore");
            return new PropertyStore(propstorePtr);
        }

        public unsafe int OpenPropertyStoreNative(StorageAccess access, out IntPtr propertyStore)
        {
            propertyStore = IntPtr.Zero;
            fixed (void* pps = &propertyStore)
            {
                return InteropCalls.CallI(_basePtr, unchecked(access), pps, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        public unsafe int GetIdNative(out string deviceid)
        {
            IntPtr pdeviceid = IntPtr.Zero;
            deviceid = null;

            var err = InteropCalls.CallI(_basePtr, &pdeviceid, ((void**)(*(void**)_basePtr))[5]);
            if (err == 0 && pdeviceid != IntPtr.Zero)
            {
                deviceid = Marshal.PtrToStringAnsi(pdeviceid);
            }

            return err;
        }

        public unsafe int GetStateNative(out DeviceState state)
        {
            fixed (void* pstate = &state)
            {
                return InteropCalls.CallI(_basePtr, unchecked(pstate), ((void**)(*(void**)_basePtr))[6]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_propertyStore != null)
            {
                _propertyStore.Dispose();
                _propertyStore = null;
            }
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }

        public override string ToString()
        {
            return PropertyStore[PropertyStore.FriendlyName].ToString();
        }
    }
}