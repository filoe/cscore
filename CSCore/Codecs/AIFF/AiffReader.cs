using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CSCore.Codecs.AIFF
{
    /// <summary>
    ///     Decodes a aiff stream/file.
    /// </summary>
    public class AiffReader : IWaveSource
    {
        private readonly AiffChunkContainer _chunkContainer;
        private readonly SoundDataChunk _soundDataChunk;
        private readonly Stream _stream;
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffReader" /> class for the specified <paramref name="filename" />.
        /// </summary>
        /// <param name="filename">The complete file path to be decoded.</param>
        /// <exception cref="CSCore.Codecs.AIFF.AiffException">
        ///     No COMM Chunk found.
        ///     or
        ///     No SSND Chunk found.
        ///     or
        ///     Format not supported.
        /// </exception>
        public AiffReader(string filename)
            : this(File.OpenRead(filename))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffReader" /> class for the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="stream">The stream to be decoded.</param>
        /// <exception cref="System.ArgumentNullException">stream</exception>
        /// <exception cref="System.ArgumentException">
        ///     Stream is not readable.;stream
        ///     or
        ///     Stream is not seekable.;stream
        /// </exception>
        /// <exception cref="CSCore.Codecs.AIFF.AiffException">
        ///     No COMM Chunk found.
        ///     or
        ///     No SSND Chunk found.
        ///     or
        ///     Format not supported.
        /// </exception>
        public AiffReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            if (!stream.CanSeek)
                throw new ArgumentException("Stream is not seekable.", "stream");

            _stream = stream;

            _chunkContainer = new AiffChunkContainer(new BinaryReader(_stream));
            var commonChunk = (CommonChunk) _chunkContainer.Chunks.FirstOrDefault(x => x is CommonChunk);
            if (commonChunk == null)
                throw new AiffException("No COMM Chunk found.");
            _soundDataChunk =
                (SoundDataChunk) _chunkContainer.Chunks.FirstOrDefault(x => x is SoundDataChunk);
            if (_soundDataChunk == null)
                throw new AiffException("No SSND Chunk found.");

            WaveFormat = commonChunk.GetWaveFormat();
            if (WaveFormat.BitsPerSample != 8 &&
                WaveFormat.BitsPerSample != 16 &&
                WaveFormat.BitsPerSample != 24 &&
                WaveFormat.BitsPerSample != 32)
            {
                throw new AiffException("Format not supported.",
                    new NotSupportedException(
                        string.Format("BitsPerSample of {0} is not supported. Must be 8, 16, 24 or 32 bits.",
                            WaveFormat.BitsPerSample)));
            }

            //Length = commonChunk.NumberOfSampleFrames * WaveFormat.BlockAlign;
            //some encoders won't set the NumberOfSampleFrames field ... better prefer:
            Length = _soundDataChunk.DataSize - 8; //sub the 2 SSND fields

            Position = 0;
        }

        /// <summary>
        ///     Gets the found <see cref="AiffChunk" />s of the aiff stream/file.
        /// </summary>
        public ReadOnlyCollection<AiffChunk> Chunks
        {
            get { return _chunkContainer.Chunks; }
        }

        /// <summary>
        ///     Reads a sequence of elements from the <see cref="AiffReader" /> and advances the position within the     stream by
        ///     the     number of elements read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of elements. When this method returns, the <paramref name="buffer" /> contains the
        ///     specified     array of elements with the values between <paramref name="offset" /> and (<paramref name="offset" />
        ///     +     <paramref name="count" /> - 1) replaced by the elements read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of elements to read from the current source.</param>
        /// <returns>
        ///     The total number of elements read into the buffer.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">buffer</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     offset
        ///     or
        ///     count
        /// </exception>
        /// <exception cref="System.ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="CSCore.Codecs.AIFF.AiffException">Unexpected error. Not supported bps.</exception>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            CheckDisposed();

            count -= count % WaveFormat.BlockAlign;
            if (count <= 0)
                return 0;

            count = (int) Math.Min(count, Length - Position);
            if (buffer.Length < offset + count)
                throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

            var b = new byte[count];

            var read = _stream.Read(b, 0, count);

            var bps = WaveFormat.BitsPerSample;
            if (bps != 8)
            {
                for (var i = 0; i < read; i += WaveFormat.BytesPerSample)
                {
                    if (bps == 16)
                    {
                        buffer[offset + i + 0] = b[i + 1];
                        buffer[offset + i + 1] = b[i + 0];
                    }
                    else if (bps == 24)
                    {
                        buffer[offset + i + 0] = b[i + 2];
                        buffer[offset + i + 1] = b[i + 1];
                        buffer[offset + i + 2] = b[i + 0];
                    }
                    else if (bps == 32)
                    {
                        buffer[offset + i + 0] = b[i + 3];
                        buffer[offset + i + 1] = b[i + 2];
                        buffer[offset + i + 2] = b[i + 1];
                        buffer[offset + i + 3] = b[i + 0];
                    }
                    else
                    {
                        //should get handled in ctor
                        throw new AiffException("Unexpected error. Not supported bps.");
                    }
                }
            }
            else
                Buffer.BlockCopy(b, 0, buffer, offset, read);

            return read;
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="AiffReader" /> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the <see cref="WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        ///     Gets or sets the current position in bytes.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">The value is less than zero or greater than <see cref="Length" />.</exception>
        public long Position
        {
            get
            {
                if (_disposed)
                    return 0;
                return _stream.Position - _soundDataChunk.AudioDataStartPosition;
            }
            set
            {
                CheckDisposed();
                if (value > Length || value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _stream.Position = value + _soundDataChunk.AudioDataStartPosition;
            }
        }

        /// <summary>
        ///     Gets the length of the audio data in bytes.
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("AiffReader");
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _stream.Dispose();
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="AiffReader" /> class.
        /// </summary>
        ~AiffReader()
        {
            Dispose(false);
        }
    }
}