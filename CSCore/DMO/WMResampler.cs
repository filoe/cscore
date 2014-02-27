using CSCore.CoreAudioAPI;
using CSCore.MediaFoundation;
using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    public sealed class WMResampler : IDisposable
    {
        private WMResamplerProps _resamplerprops;
        private IWMResamplerProps _nativeResamplerProps;
        private MediaObject2 _mediaObject;
        private WMResamplerObject _obj;

        public WMResamplerProps ResamplerProps
        {
            get { return _resamplerprops; }
        }

        public MediaObject2 MediaObject
        {
            get { return _mediaObject; }
        }

        public WMResampler()
        {
            var obj = new WMResamplerObject();
            _mediaObject = new MediaObject2((IMediaObject)obj);
            _nativeResamplerProps = obj as IWMResamplerProps;
            _resamplerprops = new WMResamplerProps(Marshal.GetComInterfaceForObject(_nativeResamplerProps, typeof(IWMResamplerProps)));

            _obj = obj;
        }

        private bool _disposed;

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
            if (_nativeResamplerProps != null)
            {
                Marshal.ReleaseComObject(_nativeResamplerProps);
                _nativeResamplerProps = null;
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