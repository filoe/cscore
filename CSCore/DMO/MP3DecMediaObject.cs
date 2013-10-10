using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.DMO
{
    public sealed class MP3DecMediaObject : IDisposable
    {
        private MediaObject2 _mediaObject;
        private CMP3DecMediaObject _obj;

        public MediaObject2 MediaObject
        {
            get { return _mediaObject; }
        }

        public MP3DecMediaObject()
        {
            var obj = new CMP3DecMediaObject();
            _mediaObject = new MediaObject2((IMediaObject)obj);
            _obj = obj;
        }

        [ComImport, Guid("bbeea841-0a63-4f52-a7ab-a9b3a84ed38a")]
        private class CMP3DecMediaObject
        {
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
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
            _disposed = true;
        }

        ~MP3DecMediaObject()
        {
            Dispose(false);
        }
    }
}
