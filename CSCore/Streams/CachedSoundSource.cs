using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Cached wave source.
    /// </summary>
    public class CachedSoundSource : IWaveSource
    {
        private MemoryStream _cache;
        private readonly WaveFormat _waveFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSoundSource"/> class.
        /// </summary>
        /// <param name="source">Source which will be copied to a cache.</param>
        public CachedSoundSource(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source.Length > Int32.MaxValue)
                throw new ArgumentException("Length is of source is too large.");

            _waveFormat = source.WaveFormat;

            CacheSource(source);
        }

        private void CacheSource(IWaveSource source)
        {
            _cache = new MemoryStream((int)source.Length);
            int read = 0;
            int count = (int)Math.Min(source.WaveFormat.BytesPerSecond * 5, source.Length);
            byte[] buffer = new byte[count];

            long position = source.Position;

            while((read = source.Read(buffer, 0, count)) > 0)
            {
                _cache.Write(buffer, 0, read);
            }

            source.Position = position;
        }

        /// <summary>
        /// Reads data from the cache.
        /// </summary>
        /// <returns>Amount of read bytes.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            ThrowObjectDisposedEx();

            return _cache.Read(buffer, offset, count);
        }

        /// <summary>
        /// Gets the Waveformat of the data stored in the cache.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public long Position
        {
            get
            {
                return _cache.Position;
            }
            set
            {
                ThrowObjectDisposedEx();

                _cache.Position = value;
            }
        }

        /// <summary>
        /// Gets the amount of bytes stored in the cache.
        /// </summary>
        public long Length
        {
            get { return _cache.Length; }
        }

        private bool _disposed;

        /// <summary>
        /// Disposes the cache.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the internal used cache. 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_cache != null)
                    {
                        _cache.Dispose();
                        _cache = null;
                    }
                }
            }
            _disposed = true;
        }

        ~CachedSoundSource()
        {
            Dispose(false);
        }

        private void ThrowObjectDisposedEx()
        {
            if (_disposed)
                throw new ObjectDisposedException("CachedSoundSource");
        }
    }
}
