using System.IO;

namespace CSCore.Codecs.AIFF
{
    /// <summary>
    ///     Provides the encoded audio data of an aiff stream.
    /// </summary>
    public class SoundDataChunk : AiffChunk
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SoundDataChunk" /> class.
        /// </summary>
        /// <param name="binaryReader">The binary reader which provides can be used to decode the chunk.</param>
        public SoundDataChunk(BinaryReader binaryReader) : base(binaryReader, "SSND")
        {
            Offset = Reader.ReadUInt32();
            BlockSize = Reader.ReadUInt32();
        }

        //use long instead of uint to guarantee clscompilance
        /// <summary>
        ///     Gets the offset. The offset determines where the first sample frame in the <see cref="SoundDataChunk" /> starts.
        /// </summary>
        /// <value>Offset in bytes.</value>
        public long Offset { get; private set; }

        /// <summary>
        ///     Gets the block size. It specifies the size in bytes of the blocks that sound data is aligned to.
        /// </summary>
        public long BlockSize { get; private set; }

        /// <summary>
        ///     Gets the zero based position in the stream, at which the encoded audio data starts.
        /// </summary>
        public long AudioDataStartPosition
        {
            get { return ChunkStartPosition + 16 + Offset; }
            //16 bytes = 2*4 bytes AiffChunk header + 2*4 bytes SoundDataChunk header
        }

        /// <summary>
        ///     Seeks to the end of the chunk.
        /// </summary>
        /// <remarks>
        ///     Can be used to make sure that the underlying <see cref="Stream" />/<see cref="System.IO.BinaryReader" /> points to
        ///     the next <see cref="AiffChunk" />.
        /// </remarks>
        public override void SkipChunk()
        {
            Reader.Skip(DataSize - 8);
        }
    }
}