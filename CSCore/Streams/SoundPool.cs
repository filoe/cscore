using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class SoundPool : ISampleSource
    {
        private WaveFormat _mixFormat;
        private List<SoundPoolItem> _sources;

        public SoundPool(WaveFormat mixFormat)
        {
            if (mixFormat == null)
                throw new ArgumentNullException("mixFormat");

            _mixFormat = mixFormat;
            Sources = new List<SoundPoolItem>();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            

            return 0;
        }

        public void QueueSound(SoundPoolItem item)
        {
            if(item == null)
                throw new ArgumentNullException("item");
            _sources.Add(item);
        }

        public WaveFormat WaveFormat
        {
            get { return _mixFormat; }
        }

        protected List<SoundPoolItem> Sources
        {
            get { return _sources; }
            set { _sources = value; }
        }

        public long Position
        {
            get
            {
                return 0;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public long Length
        {
            get { return 0; }
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //dispose managed
                }
            }
            _disposed = true;
        }

        ~SoundPool()
        {
            Dispose(false);
        }
    }
}
