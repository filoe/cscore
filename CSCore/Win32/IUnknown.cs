using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    /// <summary>
    /// Enables clients to get pointers to other interfaces on a given object through the <see cref="QueryInterface"/> method, and manage the existence of the object through the <see cref="AddRef"/> and <see cref="Release"/> methods.
    /// </summary>
    [Guid("00000000-0000-0000-C000-000000000046")]
    public interface IUnknown
    {
        /// <summary>
        /// Retrieves pointers to the supported interfaces on an object.
        /// </summary>
        /// <param name="riid">The identifier of the interface being requested.</param>
        /// <param name="ppvObject">The address of a pointer variable that receives the interface pointer requested in the <paramref name="riid"/> parameter.</param>
        /// <returns>This method returns S_OK if the interface is supported, and E_NOINTERFACE otherwise. If ppvObject is NULL, this method returns E_POINTER.</returns>
        int QueryInterface(ref Guid riid, out IntPtr ppvObject);

        /// <summary>
        /// Increments the reference count for an interface on an object. This method should be called for every new copy of a pointer to an interface on an object.
        /// </summary>
        /// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
        int AddRef();

        /// <summary>
        /// Decrements the reference count for an interface on an object.
        /// </summary>
        /// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
        int Release();
    }
}