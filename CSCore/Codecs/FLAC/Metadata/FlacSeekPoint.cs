// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents a single flac seek point.
    /// </summary>
    public class FlacSeekPoint
    {
        /// <summary>
        /// The sample number for a placeholder point.
        /// </summary>
        public long PlaceHolderPointSampleNumber = unchecked ((long) 0xFFFFFFFFFFFFFFFF);

        /// <summary>
        /// Gets the sample number of the first sample in the target frame, or <see cref="PlaceHolderPointSampleNumber"/> for a placeholder point.
        /// </summary>
        /// <value>
        /// The sample number of the first sample in the target frame.
        /// </value>
        /// <remarks>According to https://xiph.org/flac/format.html#metadata_block_seektable.</remarks>
        public long SampleNumber { get; private set; }

        /// <summary>
        /// Gets the offset (in bytes) from the first byte of the first frame header to the first byte of the target frame's header.
        /// </summary>
        /// <value>
        /// The offset (in bytes) from the first byte of the first frame header to the first byte of the target frame's header.
        /// </value>"/>
        /// <remarks>According to https://xiph.org/flac/format.html#metadata_block_seektable.</remarks>
        public long Offset { get; private set; }

        /// <summary>
        /// Gets the number of samples in the target frame.
        /// </summary>
        /// <value>
        /// The number of samples in the target frame.
        /// </value>
        /// <remarks>According to https://xiph.org/flac/format.html#metadata_block_seektable.</remarks>
        public int FrameSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacSeekPoint"/> class.
        /// </summary>
        public FlacSeekPoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacSeekPoint"/> class.
        /// </summary>
        /// <param name="sampleNumber">The <see cref="SampleNumber"/> of the target frame.</param>
        /// <param name="offset">The <see cref="Offset"/> of the target frame.</param>
        /// <param name="frameSize">The <see cref="FrameSize"/> of the target frame.</param>
        public FlacSeekPoint(long sampleNumber, long offset, int frameSize)
        {
            SampleNumber = sampleNumber;
            Offset = offset;
            FrameSize = frameSize;
        }
    }
}