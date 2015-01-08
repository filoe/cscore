using System;
using System.IO;

namespace CSCore.Codecs.WAV
{
    /// <summary>
    ///     Represents the <see cref="DataChunk" /> of a wave file.
    /// </summary>
    public class DataChunk : WaveFileChunk
    {
        /// <summary>
        ///     Chunk ID of the <see cref="DataChunk" />.
        /// </summary>
        public const int DataChunkID = 0x61746164;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataChunk" /> class.
        /// </summary>
        /// <param name="stream"><see cref="Stream" /> which contains the data chunk.</param>
        public DataChunk(Stream stream)
            : this(new BinaryReader(stream))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataChunk" /> class.
        /// </summary>
        /// <param name="reader"><see cref="BinaryReader" /> which should be used to read the data chunk.</param>
        public DataChunk(BinaryReader reader)
            : base(reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (ChunkID != DataChunkID)
                throw new FormatException("Chunk is no datachunk: " + DataChunkID.ToString("x") + " != \"0x61746164\"");

            DataStartPosition = reader.BaseStream.Position;
        }

        /// <summary>
        /// Gets the zero-based position inside of the stream at which the audio data starts.
        /// </summary>
        public long DataStartPosition { get; private set; }
    }
}