using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    public abstract class DmoObject : IDisposable
    {
        protected object _comobj;

        private MediaObject2 _mediaObject;

        public MediaObject2 MediaObject
        {
            get { return _mediaObject; }
        }

        public DmoObject()
        {
        }

        public DmoObject(object comobj)
        {
            if (comobj == null)
                throw new ArgumentNullException("comobj");
            _comobj = comobj;

            _mediaObject = new MediaObject2(comobj as IMediaObject);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }
            Marshal.ReleaseComObject(_comobj);
        }

        ~DmoObject()
        {
            Dispose(false);
        }
    }
}