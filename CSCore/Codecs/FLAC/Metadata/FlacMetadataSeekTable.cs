using System.IO;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents a flac seektable.
    /// </summary>
    public class FlacMetadataSeekTable : FlacMetadata
    {
        private readonly FlacSeekPoint[] _seekPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacMetadataSeekTable"/> class.
        /// </summary>
        /// <param name="stream">The stream which contains the seektable.</param>
        /// <param name="length">The length of the seektable inside of the stream in bytes. Does not include the metadata header.</param>
        /// <param name="lastBlock">A value which indicates whether this is the last <see cref="FlacMetadata"/> block inside of the stream. <c>true</c> means that this is the last <see cref="FlacMetadata"/> block inside of the stream.</param>
        public FlacMetadataSeekTable(Stream stream, int length, bool lastBlock)
            : base(FlacMetaDataType.Seektable, lastBlock, length)
        {
            int entryCount = length / 18;
            EntryCount = entryCount;
            _seekPoints = new FlacSeekPoint[entryCount];
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                for (int i = 0; i < entryCount; i++)
                {
                    _seekPoints[i] = new FlacSeekPoint(reader.ReadInt64(), reader.ReadInt64(), reader.ReadInt16());
                }
            }
            catch (IOException e)
            {
                throw new FlacException(e, FlacLayer.Metadata);
            }
        }

        /// <summary>
        /// Gets the number of entries, the seektable offers.
        /// </summary>
        public int EntryCount { get; private set; }

        /// <summary>
        /// Gets the seek points.
        /// </summary>
        public FlacSeekPoint[] SeekPoints
        {
            get { return _seekPoints; }
        }

        /// <summary>
        /// Gets the <see cref="FlacSeekPoint"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <value>
        /// The <see cref="FlacSeekPoint"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="FlacSeekPoint"/> at the specified <paramref name="index"/>.</returns>
        public FlacSeekPoint this[int index]
        {
            get
            {
                return _seekPoints[index];
            }
        }
    }
}