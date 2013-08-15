using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    public unsafe class MFActivate : MFAttributes
    {
        private const string c = "IMFActivate";

        public MFActivate(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Creates the object associated with this activation object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ActivateObjectNative(Guid riid, out IntPtr ptr)
        {
            fixed (void* pptr = &ptr)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, &riid, pptr, ((void**)(*(void**)_basePtr))[33]);
            }
        }

        /// <summary>
        /// Creates the object associated with this activation object.
        /// </summary>
        public T ActivateObject<T>(Guid riid) where T : ComObject
        {
            IntPtr ptr = ActivateObject(riid);
            using (var tmp = new ComObject(ptr))
            {
                return tmp.QueryInterface<T>();
            }
        }

        /// <summary>
        /// Creates the object associated with this activation object.
        /// </summary>
        public IntPtr ActivateObject(Guid riid)
        {
            IntPtr ptr;
            MediaFoundationException.Try(ActivateObjectNative(riid, out ptr), c, "ActivateObject");
            return ptr;
        }

        /// <summary>
        /// Shuts down the created object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ShutdownObjectNative()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[34]);
        }

        /// <summary>
        /// Shuts down the created object.
        /// </summary>
        public void ShutdownObject()
        {
            MediaFoundationException.Try(ShutdownObjectNative(), c, "ShutdownObject");
        }

        /// <summary>
        /// Detaches the created object from the activation object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DetachObjectNative()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[35]);
        }

        /// <summary>
        /// Detaches the created object from the activation object.
        /// </summary>
        public void DetachObject()
        {
            MediaFoundationException.Try(DetachObjectNative(), c, "DetachObject");
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }
    }
}