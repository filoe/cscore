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
            get { return InputToOutput(_source.Position); }
            set { _source.Position = OutputToInput(value); }
        }

        /// <summary>
        ///     Gets the length of the stream.
        /// </summary>
        public override long Length
        {
            get { return InputToOutput(_source.Length); }
        }

        /// <summary>
        ///     Gets the <see cref="BaseStream" /> of the <see cref="DmoAggregator" />.
        /// </summary>
        public IWaveSource BaseStream
        {
            get { return _source; }
        }

        protected override int GetInputData(ref byte[] inputDataBuffer, int requested)
        {
            inputDataBuffer = inputDataBuffer.CheckBuffer(requested);
            return BaseStream.Read(inputDataBuffer, 0, requested);
        }

        protected override WaveFormat GetInputFormat()
        {
            return BaseStream.WaveFormat;
        }
    }
}