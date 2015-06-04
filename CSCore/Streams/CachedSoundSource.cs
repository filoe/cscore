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
            _cache = new MemoryStream {Position = 0};
            int read = 0;
            int count = (int)Math.Min(source.WaveFormat.BytesPerSecond * 5, source.Length);
            byte[] buffer = new byte[count];

            long position = 0;
            if(source.CanSeek)
                position = source.Position;

            while((read = source.Read(buffer, 0, count)) > 0)
            {
                _cache.Write(buffer, 0, read);
            }

            if (source.CanSeek)
            {
                source.Position = position;
                _cache.Position = source.Position;
            }
            else
            {
                _cache.Position = 0;
            }
        }

        /// <summary>
        ///     Reads a sequence of bytes from the cache and advances the position within the cache by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the cache.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the cache.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the cache.</param>
        /// <returns>The total number of bytes read into the <paramref name="buffer"/>.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            CheckForDisposed();

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
                CheckForDisposed();

                _cache.Position = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return true; }
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

        /// <summary>
        /// Finalizes an instance of the <see cref="CachedSoundSource"/> class.
        /// </summary>
        ~CachedSoundSource()
        {
            Dispose(false);
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("CachedSoundSource");
        }
    }
}
