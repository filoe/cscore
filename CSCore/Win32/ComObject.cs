using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    /// <summary>
    /// Represents a native COM object.
    /// </summary>
    public unsafe class ComObject : IUnknown, IDisposable
    {
        /// <summary>
        /// Unsafe native pointer to the COM object.
        /// </summary>
        [CLSCompliant(false)]
        protected volatile void* UnsafeBasePtr;
        private readonly object _lockObj = new object();
        private bool _disposed;

        /// <summary>
        /// Gets a value which indicates whether the <see cref="ComObject"/> got already disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Native pointer to the COM object.
        /// </summary>
        public IntPtr BasePtr
        {
            get { return new IntPtr(UnsafeBasePtr); }
            protected set { UnsafeBasePtr = value.ToPointer(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComObject"/> class.
        /// </summary>
        public ComObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComObject"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public ComObject(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentException("Ptr must not be IntPtr.Zero.", "ptr");
            UnsafeBasePtr = ptr.ToPointer();
        }

        /// <summary>
        /// Queries supported interfaces/objects on a <see cref="ComObject"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="ComObject"/> being requested.</typeparam>
        /// <returns>The queried com interface/object.</returns>
        public T QueryInterface<T>() where T : ComObject
        {
            return QueryInterface1<T>();
        }

        internal T QueryInterface1<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), QueryInterface(typeof(T)));
        }

        internal static T QueryInterfaceStatic<T>(Guid guid, IntPtr ptr) where T : ComObject
        {
            IntPtr p0;
            Marshal.QueryInterface(ptr, ref guid, out p0);
            return (T) Activator.CreateInstance(typeof (T), p0);
        }

        internal IntPtr QueryInterfacePtr(Guid riid)
        {
            IntPtr ptr;
            ((IUnknown)this).QueryInterface(ref riid, out ptr);
            return ptr;
        }

        /// <summary>
        /// Retrieves a pointer to the supported interface on an object.
        /// </summary>
        /// <param name="type">Type of the requested <see cref="ComObject"/>.</param>
        /// <returns>A pointer to the requested interface.</returns>
        public IntPtr QueryInterface(Type type)
        {
            IntPtr ptr;
            Guid guid = type.GUID;
            ((IUnknown)this).QueryInterface(ref guid, out ptr);
            return ptr;
        }

        /// <summary>
        /// Retrieves pointers to the supported interfaces on an object.
        /// </summary>
        /// <param name="riid">The identifier of the interface being requested.</param>
        /// <param name="ppvObject">The address of a pointer variable that receives the interface pointer requested in the <paramref name="riid"/> parameter.</param>
        /// <returns>This method returns S_OK if the interface is supported, and E_NOINTERFACE otherwise. If ppvObject is NULL, this method returns E_POINTER.</returns>
        int IUnknown.QueryInterface(ref Guid riid, out IntPtr ppvObject)
        {
            return Marshal.QueryInterface(BasePtr, ref riid, out ppvObject);
        }

        /// <summary>
        /// Retrieves pointers to the supported interfaces on an object.
        /// </summary>
        /// <param name="riid">The identifier of the interface being requested.</param>
        /// <param name="ppvObject">The address of a pointer variable that receives the interface pointer requested in the <paramref name="riid"/> parameter.</param>
        /// <returns>This method returns S_OK if the interface is supported, and E_NOINTERFACE otherwise. If ppvObject is NULL, this method returns E_POINTER.</returns>
        protected int QueryInterface(ref Guid riid, out IntPtr ppvObject)
        {
            return ((IUnknown) this).QueryInterface(ref riid, out ppvObject);
        }

        /// <summary>
        /// Increments the reference count for an interface on an object. This method should be called for every new copy of a pointer to an interface on an object.
        /// </summary>
        /// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
        int IUnknown.AddRef()
        {
            return Marshal.AddRef(BasePtr);
        }

        /// <summary>
        /// Increments the reference count for an interface on an object. This method should be called for every new copy of a pointer to an interface on an object.
        /// </summary>
        /// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
        protected int AddRef()
        {
            return ((IUnknown) this).AddRef();
        }

        /// <summary>
        /// Decrements the reference count for an interface on an object.
        /// </summary>
        /// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
        int IUnknown.Release()
        {
            return Marshal.Release(BasePtr);
        }

        /// <summary>
        /// Decrements the reference count for an interface on an object.
        /// </summary>
        /// <returns>The method returns the new reference count. This value is intended to be used only for test purposes.</returns>
        protected int Release()
        {
            return ((IUnknown) this).Release();
        }

        /// <summary>
        /// Releases the COM object.
        /// </summary>
        public void Dispose()
        {
            lock (_lockObj)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases the COM object.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (BasePtr != IntPtr.Zero)
            {
                ((IUnknown)this).Release();
                UnsafeBasePtr = IntPtr.Zero.ToPointer();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ComObject"/> class.
        /// </summary>
        ~ComObject()
        {
            lock (_lockObj)
            {
                Dispose(false);
            }
        }
    }
}