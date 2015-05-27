using CSCore.Utils.Buffer;
using System;
using System.Diagnostics;

namespace CSCore.Streams
{
    /// <summary>
    /// Converts a stereo source to a mono source.
    /// </summary>
    public class StereoToMonoSource : SampleAggregatorBase
    {
        private readonly WaveFormat _waveFormat;
        private float[] _buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StereoToMonoSource"/> class.
        /// </summary>
        /// <param name="source">The underlying stereo source.</param>
        /// <exception cref="ArgumentException">The <paramref name="source"/> has more or less than two channels.</exception>
        public StereoToMonoSource(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source.WaveFormat.Channels != 2)
                throw new ArgumentException("The WaveFormat of the source has be a stereo format (two channels).", "source");

            _waveFormat = new WaveFormat(source.WaveFormat.SampleRate, 32, 1, AudioEncoding.IeeeFloat);
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="StereoToMonoSource" /> and advances the position within the stream by
        ///     the number of samples read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of floats. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     float array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the floats read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of samples to read from the current source.</param>
        /// <returns>The total number of samples read into the buffer.</returns>
        public unsafe override int Read(float[] buffer, int offset, int count)
        {
            _buffer = _buffer.CheckBuffer(count * 2);
            int read = BaseSource.Read(_buffer, 0, count * 2);
            fixed (float* pbuffer = buffer)
            {
                float* ppbuffer = pbuffer + offset;
                for (int i = 0; i < read - 1; i += 2)
                {
                    *(ppbuffer++) = (_buffer[i] + _buffer[i + 1]) / 2;
                }
            }

            return read / 2;
        }

        /// <summary>
        ///     Gets or sets the position in samples.
        /// </summary>
        public override long Position
        {
            get
            {
                return BaseSource.Position / 2;
            }
            set
            {
                BaseSource.Position = value * 2;
            }
        }

        /// <summary>
        ///     Gets the length in samples.
        /// </summary>
        public override long Length
        {
            get { return BaseSource.Length / 2; }
        }

        /// <summary>
        ///     Gets the <see cref="IAudioSource.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Disposes the <see cref="StereoToMonoSource" /> and the underlying <see cref="SampleAggregatorBase.BaseSource" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}