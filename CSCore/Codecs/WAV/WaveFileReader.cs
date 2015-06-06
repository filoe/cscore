using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CSCore.Codecs.WAV
{
    /// <summary>
    ///     Provides a decoder for reading wave files.
    /// </summary>
    public class WaveFileReader : IWaveSource
    {
        private readonly List<WaveFileChunk> _chunks;

        private readonly object _lockObj = new object();

        private bool _disposed;
        private Stream _stream;
        private WaveFormat _waveFormat;
        private readonly DataChunk _dataChunk;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaveFileReader" /> class.
        /// </summary>
        /// <param name="fileName">Filename which points to a wave file.</param>
        public WaveFileReader(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaveFileReader" /> class.
        /// </summary>
        /// <param name="stream">Stream which contains wave file data.</param>
        public WaveFileReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream is not readable");

            _stream = stream;

            var reader = new BinaryReader(stream);
            if (new String(reader.ReadChars(4)) == "RIFF")
            {
                reader.ReadInt32(); //FileLength
                char[] rifftype = reader.ReadChars(4); //RiffType WAVE
            }

            _chunks = ReadChunks(stream);
            _dataChunk = (DataChunk)_chunks.FirstOrDefault(x => x is DataChunk);
            if (_dataChunk == null)
                throw new ArgumentException("The specified stream does not contain any data chunks.", "stream");

            Position = 0;
        }

        /// <summary>
        ///     Gets a list of all found chunks.
        /// </summary>
        public ReadOnlyCollection<WaveFileChunk> Chunks
        {
            get { return _chunks.AsReadOnly(); }
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="WaveFileReader" /> and advances the position within the stream by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the current source.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            lock (_lockObj)
            {
                CheckForDisposed();

                count -= count % WaveFormat.BlockAlign;
                return _stream.Read(buffer, offset, count);
            }
        }

        /// <summary>
        ///     Gets the wave format of the wave file. This property gets specified by the <see cref="FmtChunk" />.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets or sets the position of the <see cref="WaveFileReader" /> in bytes.
        /// </summary>
        public long Position
        {
            get { return _stream != null ? _stream.Position - _dataChunk.DataStartPosition : 0; }
            set
            {
                lock (_lockObj)
                {
                    CheckForDisposed();

                    if (value > Length || value < 0)
                        throw new ArgumentOutOfRangeException("value", "The position must not be bigger than the length or less than zero.");
                    value -= (value % WaveFormat.BlockAlign);
                    _stream.Position = value + _dataChunk.DataStartPosition;
                }
            }
        }

        /// <summary>
        ///     Gets the length of the <see cref="WaveFileReader" /> in bytes.
        /// </summary>
        public long Length
        {
            get { return _dataChunk != null ? _dataChunk.ChunkDataSize : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="WaveFileReader"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        ///     Disposes the <see cref="WaveFileReader" /> and the underlying stream.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private List<WaveFileChunk> ReadChunks(Stream stream)
        {
            var chunks = new List<WaveFileChunk>(2);
            do
            {
                var tmp = WaveFileChunk.FromStream(stream);
                chunks.Add(tmp);

                var fmtChunk = tmp as FmtChunk;
                if (fmtChunk != null)
                {
                    _waveFormat = fmtChunk.WaveFormat;
                }
                else if (tmp is DataChunk)
                {
                    stream.Position += tmp.ChunkDataSize;
                }

            } while (stream.Length - stream.Position > 8); //8 bytes = size of chunk header

            return chunks;
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        ///     Disposes the <see cref="WaveFileReader" /> and the underlying stream.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                lock (_lockObj)
                {
                    if (_stream != null)
                    {
                        _stream.Dispose();
                        _stream = null;
                    }
                    Debug.WriteLine("WaveFile disposed.");
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Destructor which calls the <see cref="Dispose(bool)"/> method.
        /// </summary>
        ~WaveFileReader()
        {
            Dispose(false);
        }
    }
}