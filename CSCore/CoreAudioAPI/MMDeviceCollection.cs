using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Represents a collection of multimedia device resources.
    /// </summary>
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")]
    public class MMDeviceCollection : ComObject, IEnumerable<MMDevice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MMDeviceCollection"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer.</param>
        /// <remarks>Use the <see cref="MMDeviceEnumerator.EnumAudioEndpoints"/> method to create an instance of the <see cref="MMDeviceCollection"/> class.</remarks>
        public MMDeviceCollection(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets the number of devices in the device collection.
        /// </summary>
        public int Count
        {
            get { return GetCount(); }
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public MMDevice this[int index]
        {
            get { return ItemAt(index); }
        }

        /// <summary>
        /// The <see cref="GetCount"/> method retrieves a count of the devices in the device collection.
        /// </summary>
        /// <returns>The number of devices in the device collection.</returns>
        public int GetCount()
        {
            int count = 0;
            CoreAudioAPIException.Try(GetCountNative(out count), "IMMDeviceCollection", "GetCount");
            return count;
        }

        /// <summary>
        /// The <see cref="GetCount"/> method retrieves a count of the devices in the device collection.
        /// </summary>
        /// <param name="deviceCount">Variable into which the method writes the number of devices in the device collection.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetCountNative(out int deviceCount)
        {
            fixed (void* pdeviceCount = &deviceCount)
            {
                deviceCount = 0;
                return InteropCalls.CallI(UnsafeBasePtr, pdeviceCount, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        /// The <see cref="ItemAtNative"/> method retrieves a pointer to the specified item in the device collection.
        /// </summary>
        /// <param name="deviceIndex">The device number. If the collection contains n devices, the devices are numbered 0 to n– 1.</param>
        /// <returns>The <see cref="MMDevice"/> object of the specified item in the device collection.</returns>
        public MMDevice ItemAt(int deviceIndex)
        {
            IntPtr device;
            CoreAudioAPIException.Try(ItemAtNative(deviceIndex, out device), "IMMDeviceCollection", "Item");
            return new MMDevice(device);
        }

        /// <summary>
        /// The <see cref="ItemAtNative"/> method retrieves a pointer to the specified item in the device collection.
        /// </summary>
        /// <param name="deviceIndex">The device number. If the collection contains n devices, the devices are numbered 0 to n– 1.</param>
        /// <param name="device">A pointer variable into which the method writes the address of the <see cref="MMDevice"/> object of the specified item in the device collection.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ItemAtNative(int deviceIndex, out IntPtr device)
        {
            device = IntPtr.Zero;
            fixed (void* pdevice = &device)
            {
                return InteropCalls.CallI(UnsafeBasePtr, deviceIndex, pdevice, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="MMDeviceCollection"/>.
        /// </summary>
        /// <returns>Enumerator for the <see cref="MMDeviceCollection"/>.</returns>
        public IEnumerator<MMDevice> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="MMDeviceCollection"/>.
        /// </summary>
        /// <returns>Enumerator for the <see cref="MMDeviceCollection"/>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}