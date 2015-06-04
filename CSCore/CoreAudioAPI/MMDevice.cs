using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Encapsulates the generic features of a multimedia device resource. 
    /// </summary>
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
// ReSharper disable once InconsistentNaming
    public class MMDevice : ComObject
    {
        private const string InterfaceName = "IMMDevice";

        private PropertyStore _propertyStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MMDevice"/> class. 
        /// </summary>
        /// <param name="ptr">Native pointer.</param>
        /// <remarks>Use the <see cref="MMDeviceEnumerator"/> class to create a new <see cref="MMDevice"/> instance.</remarks>
        public MMDevice(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the propertystore associated with the <see cref="MMDevice"/>.
        /// </summary>
        /// <remarks>Warning: This PropertyStore is only <c>readable</c>. Use the OpenPropertyStore-Method to get
        /// writeable PropertyStore.</remarks>
        public PropertyStore PropertyStore
        {
            get { return _propertyStore ?? (_propertyStore = OpenPropertyStore(StorageAccess.Read)); }
        }

        /// <summary>
        /// Gets the device id. For information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370837(v=vs.85).aspx"/>.
        /// </summary>
        public string DeviceID
        {
            get
            {
                string id;
                CoreAudioAPIException.Try(GetIdNative(out id), InterfaceName, "GetId");
                return id;
            }
        }

        /// <summary>
        /// Gets the friendly name of the device.
        /// </summary>
        /// <remarks>This value is stored in the <see cref="PropertyStore"/>.</remarks>
        public string FriendlyName
        {
            get { return PropertyStore[PropertyStore.FriendlyName].ToString(); }
        }

        /// <summary>
        /// Gets the device state of the device.
        /// </summary>
        public DeviceState DeviceState
        {
            get
            {
                DeviceState state;
                CoreAudioAPIException.Try(GetStateNative(out state), InterfaceName, "GetState");
                return state;
            }
        }

        /// <summary>
        /// Creates a COM object with the specified interface.
        /// </summary>
        /// <param name="iid">The interface identifier. This parameter is a reference to a GUID that identifies the interface that the caller requests be activated. The caller will use this interface to communicate with the COM object.</param>
        /// <param name="context">The execution context in which the code that manages the newly created object will run. </param>
        /// <param name="activationParams">Use <see cref="IntPtr.Zero"/> as the default value. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd371405%28v=vs.85%29.aspx for more details.</param>
        /// <param name="pinterface">A pointer variable into which the method writes the address of the interface specified by parameter <paramref name="iid"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ActivateNative(Guid iid, CLSCTX context, IntPtr activationParams, out IntPtr pinterface)
        {
            pinterface = IntPtr.Zero;
            fixed (void* ppinterface = &pinterface)
            {
                var result = InteropCalls.CallI(UnsafeBasePtr, &iid, (uint) context, activationParams,
                    new IntPtr(ppinterface), ((void**) (*(void**) UnsafeBasePtr))[3]);
                return result;
            }
        }

        /// <summary>
        /// Creates a COM object with the specified interface.
        /// </summary>
        /// <param name="iid">The interface identifier. This parameter is a reference to a GUID that identifies the interface that the caller requests be activated. The caller will use this interface to communicate with the COM object.</param>
        /// <param name="context">The execution context in which the code that manages the newly created object will run. </param>
        /// <param name="activationParams">Use <see cref="IntPtr.Zero"/> as the default value. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd371405%28v=vs.85%29.aspx for more details.</param>
        /// <returns>A pointer variable into which the method writes the address of the interface specified by parameter <paramref name="iid"/>.</returns>
        public IntPtr Activate(Guid iid, CLSCTX context, IntPtr activationParams)
        {
            IntPtr ptr;
            CoreAudioAPIException.Try(ActivateNative(iid, context, activationParams, out ptr), InterfaceName, "Activate");
            return ptr;
        }

        /// <summary>
        /// Retrieves an interface to the device's property store.
        /// </summary>
        /// <param name="storageAccess">The storage-access mode. This parameter specifies whether to open the property store in read mode, write mode, or read/write mode.</param>
        /// <returns><see cref="PropertyStore"/> for the <see cref="MMDevice"/>.</returns>
        public PropertyStore OpenPropertyStore(StorageAccess storageAccess)
        {
            IntPtr propstorePtr;
            CoreAudioAPIException.Try(OpenPropertyStoreNative(storageAccess, out propstorePtr),
                "IMMDevice", "OpenPropertyStore");
            return new PropertyStore(propstorePtr);
        }

        /// <summary>
        /// Retrieves an interface to the device's property store.
        /// </summary>
        /// <param name="storageAccess">The storage-access mode. This parameter specifies whether to open the property store in read mode, write mode, or read/write mode.</param>
        /// <param name="propertyStore">A pointer variable into which the method writes the address of the IPropertyStore interface of the device's property store.</param>
        /// <returns>HRESULT</returns>
        public unsafe int OpenPropertyStoreNative(StorageAccess storageAccess, out IntPtr propertyStore)
        {
            propertyStore = IntPtr.Zero;
            fixed (void* pps = &propertyStore)
            {
                return InteropCalls.CallI(UnsafeBasePtr, unchecked(storageAccess), pps,
                    ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// Retrieves an endpoint ID string that identifies the audio endpoint device.
        /// </summary>
        /// <param name="deviceid">The variable which will receive the id of the device.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetIdNative(out string deviceid)
        {
            IntPtr pdeviceid = IntPtr.Zero;
            deviceid = null;

            var err = InteropCalls.CallI(UnsafeBasePtr, &pdeviceid, ((void**) (*(void**) UnsafeBasePtr))[5]);
            if (err == 0 && pdeviceid != IntPtr.Zero)
            {
                deviceid = Marshal.PtrToStringUni(pdeviceid);
                Marshal.FreeCoTaskMem(pdeviceid);
            }

            return err;
        }

        /// <summary>
        /// Retrieves the current device state.
        /// </summary>
        /// <param name="state">The variable which will receive the <see cref="CoreAudioAPI.DeviceState"/> of the device.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetStateNative(out DeviceState state)
        {
            fixed (void* pstate = &state)
            {
                return InteropCalls.CallI(UnsafeBasePtr, unchecked(pstate), ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        /// Disposes the <see cref="MMDevice"/> and its default property store (see <see cref="PropertyStore"/> property).
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (_propertyStore != null)
            {
                _propertyStore.Dispose();
                _propertyStore = null;
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        private bool _disposed;

        /// <summary>
        /// Returns the <see cref="FriendlyName"/> of the <see cref="MMDevice"/>.
        /// </summary>
        /// <returns>The <see cref="FriendlyName"/>.</returns>
        public override string ToString()
        {
            return PropertyStore[PropertyStore.FriendlyName].ToString();
        }
    }
}