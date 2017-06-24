using System.IO;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// The default implementation of the <see cref="FlacMetadata"/> class for <see cref="FlacMetaDataType"/> 
    /// with no specific implemetation.
    /// </summary>
    public sealed class DefaultFlacMetadata : FlacMetadata
    {
        private readonly FlacMetaDataType _metadataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacMetadata"/> class.
        /// </summary>
        /// <param name="metadataType">The type of the metadata.</param>
        public DefaultFlacMetadata(FlacMetaDataType metadataType)
        {
            _metadataType = metadataType;
        }

        /// <summary>
        /// Initializes the properties of the <see cref="FlacMetadata"/> by reading them from the <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the metadata.</param>
        protected override void InitializeByStream(Stream stream)
        {
        }

        /// <summary>
        /// Gets the type of the <see cref="FlacMetadata"/>.
        /// </summary>
        public override FlacMetaDataType MetaDataType
        {
            get { return _metadataType; }
        }
    }
}