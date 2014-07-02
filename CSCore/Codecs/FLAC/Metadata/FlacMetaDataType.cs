// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Defines flac metadata types.
    /// </summary>
    public enum FlacMetaDataType
    {
        /// <summary>
        /// Streaminfo metadata.
        /// </summary>
        StreamInfo = 0,
        /// <summary>
        /// Padding metadata.
        /// </summary>
        Padding = 1,
        /// <summary>
        /// Application metadata.
        /// </summary>
        Application = 2,
        /// <summary>
        /// Seektable metadata.
        /// </summary>
        Seektable = 3,
        /// <summary>
        /// Vorbis comment metadata.
        /// </summary>
        VorbisComment = 4,
        /// <summary>
        /// Cue sheet metadata.
        /// </summary>
        CueSheet = 5,
        /// <summary>
        /// Picture metadata.
        /// </summary>
        Picture = 6,
        /// <summary>
        /// Undefined metadata. Used for custom metadata fields.
        /// </summary>
        Undef = 7
    }
}