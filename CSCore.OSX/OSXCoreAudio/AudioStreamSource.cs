using System;
using System.IO;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;

namespace CSCore.OSXCoreAudio
{
    /// <summary>
    ///     An class derived from <see cref="MonoMac.AudioToolbox.AudioSource"/> that supports reading from streams.
    ///     Can be wrapped by an ExtFileAudio object
    /// </summary>
    public class AudioStreamSource : AudioSource
    {
        private Stream _audioStream;
        private bool _disposed = false;

        /// <summary>
        /// The underlying audio stream
        /// </summary>
        /// <value>The audio stream.</value>
        public Stream AudioStream {get {return _audioStream;}}

        //begin with 8kb buffer
        private byte[] _byteBuffer = new byte[8 * 1024];

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:CSCore.OSXCoreAudio.AudioStreamSource"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream. Note this cannot be a network stream - must be either Memory or File</param>
        /// <param name="fileType">The codec of the audio stream.</param>
        public AudioStreamSource(Stream stream, AudioFileType fileType) : base()
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", nameof(stream));
            if (!(stream is FileStream) && !(stream is MemoryStream))
                throw new ArgumentException("Stream must be either filestream or memorystream", nameof(stream));

            _audioStream = stream;

            Open(fileType);
        }

        /// <summary>
        ///     Gets or sets the size (length) of the stream in bytes
        /// </summary>
        /// <value>The size (length) of the stream in bytes</value>
        public override long Size
        {
            get
            {
                return _disposed ? 0 : _audioStream.Length;
            }

            set
            {
                if (!_disposed)
                    _audioStream.SetLength(value);
            }
        }

        /// <summary>
        ///     Callback used by <see cref="MonoMac.AudioToolbox.AudioFile"/> to read data from the stream
        /// </summary>
        /// <param name="position">The position to read from in the stream</param>
        /// <param name="requestCount">The requested number of bytes to read from the stream</param>
        /// <param name="buffer">The buffer to read the data into</param>
        /// <param name="actualCount">The actual number of bytes read into the buffer</param>
        public override bool Read(long position, int requestCount, IntPtr buffer, out int actualCount)
        {
            try
            {
                //casting is okay here since value is always ≤ requestCount (an int)
                actualCount = (int)Math.Min(_audioStream.Length - position, requestCount);

                //seek to the correct position in the stream
                _audioStream.Position = position;

                if (_audioStream.Position != position)
                    throw new Exception("Seeking to new position in AudioStreamSource failed!");

                //automatically grow byte buffer if it's not large enough
                //hopefully don't have to worry about it getting too large
                if (_byteBuffer.Length < actualCount) _byteBuffer = new byte[actualCount];

                //read data from the stream into the temporary byte array
                actualCount = _audioStream.Read(_byteBuffer, 0, actualCount);

                //copy data from the temporary byte array into the native buffer
                Marshal.Copy(_byteBuffer, 0, buffer, actualCount);

                return true;
            }
            catch
            {
                actualCount = 0;
                return false;
            }
        }

        /// <summary>
        /// NOT SUPPORTED
        /// </summary>
        public override bool Write(long position, int requestCount, IntPtr buffer, out int actualCount)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Disposes the audio stream and parent AudioSource
        /// </summary>
        /// <param name="disposing">Indicates that we are manually diposing underlying AudioSource</param>
        protected override void Dispose(bool disposing)
        {
            if (_audioStream != null)
            {
                _audioStream.Dispose();
                _audioStream = null;
            }
            _disposed = true;
            try
            {
                base.Dispose(disposing);
            }
            catch {}
        }
    }
}

