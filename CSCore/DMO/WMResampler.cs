using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    internal sealed class WMResampler : IDisposable
    {
        private bool _disposed;
        private MediaObject _mediaObject;
        private WMResamplerObject _obj;
        private WMResamplerProps _resamplerprops;

        public WMResampler()
        {
            //create a resampler instance
            var obj = new WMResamplerObject();

            _mediaObject = new MediaObject(Marshal.GetComInterfaceForObject((IMediaObject) obj, typeof (IMediaObject)));
            _resamplerprops =
                new WMResamplerProps(Marshal.GetComInterfaceForObject(obj as IWMResamplerProps,
                    typeof (IWMResamplerProps)));

            _obj = obj;
        }

        public WMResamplerProps ResamplerProps
        {
            get { return _resamplerprops; }
        }

        public MediaObject MediaObject
        {
            get { return _mediaObject; }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                GC.SuppressFinalize(this);
                Dispose(true);
            }
        }

        private void Dispose(bool disposing)
        {
            if (_resamplerprops != null)
            {
                _resamplerprops.Dispose();
                _resamplerprops = null;
            }
            if (_mediaObject != null)
            {
                _mediaObject.Dispose();
                _mediaObject = null;
            }
            if (_obj != null)
            {
                Marshal.ReleaseComObject(_obj);
                _obj = null;
            }
        }

        ~WMResampler()
        {
            Dispose(false);
        }

        //object to create an instance of wmresampler
        [ComImport]
        [Guid("f447b69e-1884-4a7e-8055-346f74d6edb3")]
        private class WMResamplerObject
        {
        }
    }
}