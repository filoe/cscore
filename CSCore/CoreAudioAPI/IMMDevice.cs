using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Encapsulates the generic features of a multimedia device resource. 
    /// </summary>
    [ComImport]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDevice : IUnknown
    {
        /// <summary>
        /// Creates a COM object with the specified interface.
        /// </summary>
        /// <param name="iid">The interface identifier. This parameter is a reference to a GUID that identifies the interface that the caller requests be activated. The caller will use this interface to communicate with the COM object.</param>
        /// <param name="clsctx">The execution context in which the code that manages the newly created object will run. </param>
        /// <param name="activationParams">Use <see cref="IntPtr.Zero"/> as the default value. See http://msdn.microsoft.com/en-us/library/windows/desktop/dd371405%28v=vs.85%29.aspx for more details.</param>
        /// <param name="pinterface">Pointer to a pointer variable into which the method writes the address of the interface specified by parameter <paramref name="iid"/>.</param>
        /// <returns>HRESULT</returns>
        int Activate(Guid iid, CLSCTX clsctx, IntPtr activationParams /*zero*/, [Out] out IntPtr pinterface);

        /// <summary>
        /// Retrieves an interface to the device's property store.
        /// </summary>
        /// <param name="access">The storage-access mode. This parameter specifies whether to open the property store in read mode, write mode, or read/write mode.</param>
        /// <param name="propertystore">Pointer to a pointer variable into which the method writes the address of the IPropertyStore interface of the device's property store.</param>
        /// <returns>HRESULT</returns>
        int OpenPropertyStore(StorageAccess access, [Out] out IntPtr propertystore);

        /// <summary>
        /// Retrieves an endpoint ID string that identifies the audio endpoint device.
        /// </summary>
        /// <param name="deviceId">The variable which will receive the id of the device.</param>
        /// <returns>HRESULT</returns>
        int GetId([Out, MarshalAs(UnmanagedType.LPWStr)] out string deviceId);

        /// <summary>
        /// Retrieves the current device state.
        /// </summary>
        /// <param name="state">The variable which will receive the <see cref="CoreAudioAPI.DeviceState"/> of the device.</param>
        /// <returns>HRESULT</returns>
        int GetState([Out] out DeviceState state);
    }
}