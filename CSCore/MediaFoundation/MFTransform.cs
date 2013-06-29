using System;

namespace CSCore.MediaFoundation
{
    public class MFTransform : IDisposable
    {
        IMFTransform _transform;

        public IMFTransform NativeTransform
        {
            get { return _transform; }
        }

        public MFTransform(IMFTransform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");
            _transform = transform;
        }

        public void Dispose()
        {
            Dispose(true);
			GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
			if(disposing)
			{
				//dispose managed
			}
            if (_transform != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_transform);

            _transform = null;
        }

        ~MFTransform()
        {
            Dispose(false);
        }
    }
}
