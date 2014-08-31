using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     IWaveAggreator base class for Dmo based streams.
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
        ///     Gets or sets the position of the stream.
        /// </summary>
        public override long Position
        {
            get { return CanSeek ? InputToOutput(_source.Position) : 0; }
            set
            {
                if(CanSeek) 
                    _source.Position = OutputToInput(value);
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Gets the length of the stream.
        /// </summary>
        public override long Length
        {
            get { return CanSeek ? InputToOutput(_source.Length) : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IWaveStream"/> supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return _source.CanSeek; }
        }

        /// <summary>
        ///     Gets the <see cref="BaseStream" /> of the <see cref="DmoAggregator" />.
        /// </summary>
        public IWaveSource BaseStream
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
            return BaseStream.Read(inputDataBuffer, 0, requested);
        }

        /// <summary>
        ///     Gets the input format to use.
        /// </summary>
        /// <returns></returns>
        protected override WaveFormat GetInputFormat()
        {
            return BaseStream.WaveFormat;
        }
    }
}