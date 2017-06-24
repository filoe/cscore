using System.IO;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents a flac seektable.
    /// </summary>
    public class FlacMetadataSeekTable : FlacMetadata
    {
        private FlacSeekPoint[] _seekPoints;

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

        /// <summary>
        /// Initializes the properties of the <see cref="FlacMetadata"/> by reading them from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the metadata.</param>
        protected override void InitializeByStream(Stream stream)
        {
            int entryCount = Length / 18;
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
        /// Gets the type of the <see cref="FlacMetadata"/>.
        /// </summary>
        public override FlacMetaDataType MetaDataType
        {
            get { return FlacMetaDataType.Seektable; }
        }
    }
}