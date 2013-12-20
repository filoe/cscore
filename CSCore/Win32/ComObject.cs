using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    //((void**)(*(void**)_basePtr))[3]
    public unsafe class ComObject : IUnknown, IDisposable
    {
        protected volatile void* _basePtr;

        public IntPtr BasePtr
        {
            get { return new IntPtr(_basePtr); }
            protected set { _basePtr = value.ToPointer(); }
        }

        public ComObject()
        {
        }

        public ComObject(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentException("ptr is IntPtr.Zero");
            _basePtr = ptr.ToPointer();
        }

        public T QueryInterface<T>() where T : ComObject
        {
            return (T)Activator.CreateInstance(typeof(T), QueryInterface(typeof(T)));
        }

        internal T QueryInterface1<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), QueryInterface(typeof(T)));
        }

        public IntPtr QueryInterface(Type type)
        {
            IntPtr ptr;
            Guid guid = type.GUID;
            ((IUnknown)this).QueryInterface(ref guid, out ptr);
            return ptr;
        }

        int IUnknown.QueryInterface(ref Guid giid, out IntPtr ppvObject)
        {
            return Marshal.QueryInterface(BasePtr, ref giid, out ppvObject);
        }

        int IUnknown.AddRef()
        {
            return Marshal.AddRef(BasePtr);
        }

        int IUnknown.Release()
        {
            return Marshal.Release(BasePtr);
        }

        private object _lockObj = new object();

        private bool disposed = false;

        public void Dispose()
        {
            lock (_lockObj)
            {
                if (!disposed)
                {
                    disposed = true;
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (BasePtr != IntPtr.Zero)
            {
                ((IUnknown)this).Release();
                _basePtr = IntPtr.Zero.ToPointer();
            }
        }

        protected virtual bool AssertOnNoDispose()
        {
            return true;
        }

        ~ComObject()
        {
            lock (_lockObj)
            {
                //if (!disposed)
                    //Debug.Assert(!AssertOnNoDispose(), "ComObject.Dispose not called. Type: " + this.GetType().FullName);
                Dispose(false);
            }
        }
    }
}