using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     <see cref="IWaveAggregator"/> implementation for Dmo based streams.
    /// </summary>
    public abstract class DmoAggregator : DmoStream, IWaveAggregator
    {
        private readonly IWaveSource _source;

        /// <summary>
        ///     Creates a new instance of the <see cref="DmoAggregator" /> class.
        /// </summary>
        /// <param name="source">Base source of the <see cref="DmoAggregator" />.</param>
        protected DmoAggregator(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
        }

        /// <summary>
        ///     Gets or sets the position of the stream in bytes.
        /// </summary>
        public override long Position
        {
            get { return CanSeek ? InputToOutput(BaseSource.Position) : 0; }
            set
            {
                if(CanSeek)
                    BaseSource.Position = OutputToInput(value);
                else
                    throw new InvalidOperationException("BaseSource is not seekable.");
            }
        }

        /// <summary>
        ///     Gets the length of the stream in bytes.
        /// </summary>
        public override long Length
        {
            get { return CanSeek ? InputToOutput(BaseSource.Length) : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return BaseSource.CanSeek; }
        }

        /// <summary>
        ///     Gets the <see cref="BaseSource" /> of the <see cref="DmoAggregator" />.
        /// </summary>
        public IWaveSource BaseSource
        {
            get { return _source; }
        }

        /// <summary>
        ///     Gets inputData to feed the Dmo MediaObject with.
        /// </summary>
        /// <param name="inputDataBuffer">
        ///     InputDataBuffer which receives the inputData.
        ///     If this parameter is null or the length is less than the amount of inputData, a new byte array will be applied.
        /// </param>
        /// <param name="requested">The requested number of bytes.</param>
        /// <returns>The number of bytes read. The number of actually read bytes does not have to be the number of requested bytes.</returns>
        protected override int GetInputData(ref byte[] inputDataBuffer, int requested)
        {
            inputDataBuffer = inputDataBuffer.CheckBuffer(requested);
            return BaseSource.Read(inputDataBuffer, 0, requested);
        }

        /// <summary>
        ///     Gets the input format to use.
        /// </summary>
        /// <returns>The input format.</returns>
        /// <remarks>Typically this is the <see cref="IAudioSource.WaveFormat"/> of the <see cref="BaseSource"/>.</remarks>
        protected override WaveFormat GetInputFormat()
        {
            return BaseSource.WaveFormat;
        }
    }
}