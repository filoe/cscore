using System;
using System.IO;

namespace CSCore.Codecs.WAV
{
    /// <summary>
    /// See http://www.sonicspot.com/guide/wavefiles.html#wavefilechunks
    /// </summary>
    public class WaveFileChunk
    {
        public static WaveFileChunk FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (stream.CanRead == false)
                throw new ArgumentException("stream is not readable");

            BinaryReader reader = new BinaryReader(stream);
            int id = reader.ReadInt32();
            stream.Position -= 4;

            if (id == FMTChunk.chunkID)
                return new FMTChunk(reader);
            else if (id == DataChunk.chunkID)
                return new DataChunk(reader);
            else
                return new WaveFileChunk(reader);
        }

        public int ChunkID { get; private set; }

        public int ChunkDataSize { get; private set; }

        public WaveFileChunk(Stream stream)
            : this(new BinaryReader(stream))
        {
        }

        public WaveFileChunk(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            ChunkID = reader.ReadInt32();
            ChunkDataSize = reader.ReadInt32();
        }
    }
}