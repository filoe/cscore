using CSCore.CoreAudioAPI;
using CSCore.MediaFoundation;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    public class WMResampler : IDisposable
    {
        WMResamplerProps _resamplerprops;
        IWMResamplerProps _nativeResamplerProps;
        PropertyStore _propertyStore;
        MFTransform _transform;
        MediaObject2 _mediaObject;
        WMResamplerObject _obj;

        public WMResamplerProps ResamplerProps
        {
            get { return _resamplerprops; }
        }

        public PropertyStore PropertyStore
        {
            get { return _propertyStore; }
        }

        public MFTransform Transform
        {
            get { return _transform; }
        }

        public MediaObject2 MediaObject
        {
            get { return _mediaObject; }
        }

        public WMResampler()
        {
            var obj = new WMResamplerObject();
            _transform = new MFTransform((IMFTransform)obj);
            _mediaObject = new MediaObject2((IMediaObject)obj);
            _propertyStore = new PropertyStore(Marshal.GetComInterfaceForObject((IPropertyStore)obj, typeof(IPropertyStore)));
            _nativeResamplerProps = obj as IWMResamplerProps;
            _resamplerprops = new WMResamplerProps(Marshal.GetComInterfaceForObject(_nativeResamplerProps, typeof(IWMResamplerProps)));
            _obj = obj;
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

            _transform.Dispose();
            _propertyStore.Dispose();
            _mediaObject.Dispose();
            
            if (_nativeResamplerProps != null)
            {
                Marshal.ReleaseComObject(_nativeResamplerProps);
                _nativeResamplerProps = null;
            }
            if (_resamplerprops != null)
            {
                _resamplerprops.Dispose();
                _resamplerprops = null;
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

        [ComImport]
        [Guid("f447b69e-1884-4a7e-8055-346f74d6edb3")]
        private class WMResamplerObject
        {
        }
    }
}
