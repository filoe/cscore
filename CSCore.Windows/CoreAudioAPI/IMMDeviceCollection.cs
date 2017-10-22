using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Represents a collection of multimedia device resources.
    /// </summary>
    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceCollection : IUnknown
    {
        /// <summary>
        /// The <see cref="GetCount"/> method retrieves a count of the devices in the device collection.
        /// </summary>
        /// <param name="deviceCount">Variable into which the method writes the number of devices in the device collection.</param>
        /// <returns>HRESULT</returns>
        int GetCount(ref int deviceCount);

        /// <summary>
        /// The <see cref="Item"/> method retrieves a pointer to the specified item in the device collection.
        /// </summary>
        /// <param name="deviceIndex">The device number. If the collection contains n devices, the devices are numbered 0 to n– 1.</param>
        /// <param name="device">The <see cref="IMMDevice"/> object of the specified item in the device collection.</param>
        /// <returns>HRESULT</returns>
        int Item(int deviceIndex, [Out] out IMMDevice device);
    }
}