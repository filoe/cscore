using CSCore.Win32;
using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Enables the application to defer the creation of an object. This interface is exposed by activation objects.
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms703039(v=vs.85).aspx"/>.</remarks>
    public class MFActivate : MFAttributes
    {
        private const string InterfaceName = "IMFActivate";

        /// <summary>
        /// Initializes a new instance of the <see cref="MFActivate"/> class.
        /// </summary>
        /// <param name="ptr">The underlying native pointer.</param>
        public MFActivate(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Creates the object associated with this activation object.
        /// </summary>
        /// <param name="riid">Interface identifier (IID) of the requested interface.</param>
        /// <param name="ptr">Receives a pointer to the requested interface. The caller must release the interface.</param>
        /// <returns>HRESULT</returns>
        public unsafe int ActivateObjectNative(Guid riid, out IntPtr ptr)
        {
            fixed (void* pptr = &ptr)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, &riid, pptr, ((void**)(*(void**)UnsafeBasePtr))[33]);
            }
        }

        /// <summary>
        /// Creates the object associated with this activation object.
        /// <seealso cref="ActivateObject"/>
        /// <seealso cref="ActivateObject{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of the com object to create.</typeparam>
        /// <param name="riid">Interface identifier (IID) of the requested interface.</param>
        /// <returns>An instance of the requested interface.</returns>
        public T ActivateObject<T>(Guid riid) where T : ComObject
        {
            IntPtr ptr = ActivateObject(riid);
            return QueryInterfaceStatic<T>(riid, ptr);
        }

        /// <summary>
        /// Creates the object associated with this activation object.
        /// </summary>
        /// <param name="riid">Interface identifier (IID) of the requested interface.</param>
        /// <returns>A pointer to the requested interface. The caller must release the interface.</returns>
        public IntPtr ActivateObject(Guid riid)
        {
            IntPtr ptr;
            MediaFoundationException.Try(ActivateObjectNative(riid, out ptr), InterfaceName, "ActivateObject");
            return ptr;
        }

        /// <summary>
        /// Shuts down the created object.
        /// <see cref="ShutdownObject"/>
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ShutdownObjectNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[34]);
        }

        /// <summary>
        /// Shuts down the created object.
        /// </summary>
        public void ShutdownObject()
        {
            MediaFoundationException.Try(ShutdownObjectNative(), InterfaceName, "ShutdownObject");
        }

        /// <summary>
        /// Detaches the created object from the activation object.
        /// <see cref="DetachObject"/>
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DetachObjectNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[35]);
        }

        /// <summary>
        /// Detaches the created object from the activation object.
        /// </summary>
        public void DetachObject()
        {
            MediaFoundationException.Try(DetachObjectNative(), InterfaceName, "DetachObject");
        }
    }
}