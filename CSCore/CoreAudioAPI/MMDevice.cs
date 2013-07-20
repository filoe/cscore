using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    public class MMDevice : ComObject
    {
        const string c = "IMMDevice";

        public MMDevice(IntPtr ptr)
            : base(ptr)
        {
        }

        PropertyStore _propertyStore;
        /// <summary>
        /// Warning: This PropertyStore is just Readable. Use the OpenPropertyStore-Method to get writeable PropertyStore.
        /// </summary>
        public PropertyStore PropertyStore
        {
            get
            {
                if (_propertyStore == null)
                {
                    IntPtr propstorePtr;
                    CoreAudioAPIException.Try(OpenPropertyStore(StorageAccess.Read, out propstorePtr), 
                        "IMMDevice", "OpenPropertyStore");
                    _propertyStore = new PropertyStore(propstorePtr);
                }
                return _propertyStore;
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

        public unsafe int OpenPropertyStore(StorageAccess access, out IntPtr propertyStore)
        {
            propertyStore = IntPtr.Zero;
            fixed (void* pps = &propertyStore)
            {
                return InteropCalls.CallI(_basePtr, unchecked(access), pps, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        public unsafe int GetId(out string deviceid)
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

        public unsafe int GetState(out DeviceState state)
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
