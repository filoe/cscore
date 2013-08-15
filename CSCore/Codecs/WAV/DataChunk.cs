using System;
using System.IO;

namespace CSCore.Codecs.WAV
{
    public class DataChunk : WaveFileChunk
    {
        public const int chunkID = 0x61746164;

        public DataChunk(Stream stream)
            : this(new BinaryReader(stream))
        {
        }

        public DataChunk(BinaryReader reader)
            : base(reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            if (ChunkID != chunkID)
            {
                throw new FormatException("Chunk is no datachunk: " + ChunkID.ToString("x") + " != \"0x61746164\"");
            }
        }
    }
}