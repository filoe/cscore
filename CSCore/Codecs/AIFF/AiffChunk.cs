using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Codecs.AIFF
{
    /// <summary>
    ///     Decodes an aiff-chunk and provides its stored data.
    /// </summary>
    [DebuggerDisplay("{ChunkId}")]
    public class AiffChunk
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AiffChunk" /> class.
        /// </summary>
        /// <param name="binaryReader">The binary reader which provides can be used to decode the chunk.</param>
        /// <param name="chunkId">The chunk identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     binaryReader
        ///     or
        ///     chunkId
        /// </exception>
        public AiffChunk(BinaryReader binaryReader, string chunkId)
        {
            if (binaryReader == null)
                throw new ArgumentNullException("binaryReader");
            if (string.IsNullOrEmpty(chunkId))
                throw new ArgumentNullException("chunkId");

            BinaryReader = binaryReader;
            ChunkStartPosition = BinaryReader.BaseStream.Position - 4; //sub the chunkid

            ChunkId = chunkId;
            Reader = new AiffBinaryReader(binaryReader);

            //ChunkId = binaryReader.ReadChars(4);
            DataSize = Reader.ReadInt32();

            //if odd -> add a zero pad byte at the end
            if (DataSize % 2 != 0)
                DataSize++;
        }

        internal AiffBinaryReader Reader { get; private set; }
        internal long ChunkStartPosition { get; private set; }

        /// <summary>
        ///     Gets the underlying binary reader.
        /// </summary>
        /// <remarks>Care endianness.</remarks>
        protected BinaryReader BinaryReader { get; private set; }

        /// <summary>
        ///     Gets the ChunkId of the <see cref="AiffChunk" />. The is used to determine the type of the <see cref="AiffChunk" />
        ///     .
        /// </summary>
        public string ChunkId { get; private set; }

        /// <summary>
        ///     Gets the size of the <see cref="AiffChunk" /> in bytes. The <see cref="ChunkId" /> and the <see cref="DataSize" />
        ///     (4 bytes each) are not included.
        /// </summary>
        public long DataSize { get; private set; }

        /// <summary>
        ///     Seeks to the end of the chunk.
        /// </summary>
        /// <remarks>
        ///     Can be used to make sure that the underlying <see cref="Stream" />/<see cref="System.IO.BinaryReader" /> points to
        ///     the next <see cref="AiffChunk" />.
        /// </remarks>
        public virtual void SkipChunk()
        {
            Reader.Skip((int) DataSize);
        }
    }
}