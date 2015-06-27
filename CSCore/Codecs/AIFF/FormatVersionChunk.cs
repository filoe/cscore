using System.IO;

namespace CSCore.Codecs.AIFF
{
    /// <summary>
    ///     Provides the format version of the aifc file.
    /// </summary>
    public class FormatVersionChunk : AiffChunk
    {
        /// <summary>
        ///     Defines Aiff-Versions.
        /// </summary>
        public enum AifcVersion
        {
            /// <summary>
            ///     Version 1.
            /// </summary>
            Version1 = unchecked((int) 0xA2805140)
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormatVersionChunk" /> class.
        /// </summary>
        /// <param name="binaryReader">The binary reader which provides can be used to decode the chunk.</param>
        /// <exception cref="CSCore.Codecs.AIFF.AiffException">Invalid AIFF-C Version.</exception>
        public FormatVersionChunk(BinaryReader binaryReader) : base(binaryReader, "FVER")
        {
            var version = Reader.ReadInt32();
            Version = (AifcVersion) version;

            if (Version != AifcVersion.Version1)
            {
                throw new AiffException(
                    string.Format(
                        "Invalid AIFF-C Version in FormatVersionChunk with ChunkId {0}. The Version was: 0x{1:x}.",
                        ChunkId, version));
            }
        }

        /// <summary>
        ///     Gets the version of the aifc file.
        /// </summary>
        public AifcVersion Version { get; private set; }

        /// <summary>
        ///     Seeks to the end of the chunk.
        /// </summary>
        /// <remarks>
        ///     Can be used to make sure that the underlying <see cref="Stream" />/<see cref="System.IO.BinaryReader" /> points to
        ///     the next <see cref="AiffChunk" />.
        /// </remarks>
        public override void SkipChunk()
        {
            Reader.Skip(DataSize - 4);
        }
    }
}