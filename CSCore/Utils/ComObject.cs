using System.Diagnostics;

namespace System.Runtime.InteropServices
{
    public unsafe class ComObject : IUnknown, IDisposable
    {
        protected void* _basePtr;
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
            if (ptr == IntPtr.Zero) throw new ArgumentException("ptr is IntPtr.Zero");
            _basePtr = ptr.ToPointer();
        }

        public T QueryInterface<T>() where T : ComObject
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

        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
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
            Debug.Assert(!AssertOnNoDispose(), "ComObject.Dispose not called. Type: " + this.GetType().FullName);
            Dispose(false);
        }
    }
}
