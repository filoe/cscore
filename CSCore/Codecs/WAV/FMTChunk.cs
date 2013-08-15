using System;
using System.IO;

namespace CSCore.Codecs.WAV
{
    public class FMTChunk : WaveFileChunk
    {
        public const int chunkID = 0x20746D66;

        private WaveFormat _waveFormat;

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public FMTChunk(Stream stream)
            : this(new BinaryReader(stream))
        {
        }

        public FMTChunk(BinaryReader reader)
            : base(reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            if (ChunkID == chunkID) //"fmt "
            {
                AudioEncoding encoding = (AudioEncoding)reader.ReadInt16();
                int channels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                int avgBPS = reader.ReadInt32();
                int blockAlign = reader.ReadInt16();
                int bitsPerSample = reader.ReadInt16();

                int extraSize = 0;
                if (ChunkDataSize > 16)
                {
                    extraSize = reader.ReadInt16();
                    if (extraSize != ChunkDataSize - 18)
                        extraSize = ChunkDataSize - 18;

                    for (int i = ChunkDataSize - 16; i > 0; i--)
                    {
                        reader.ReadByte();
                    }

                    reader.BaseStream.Position -= 2;
                }

                _waveFormat = new WaveFormat(sampleRate, (short)bitsPerSample, (short)channels, encoding, extraSize);
            }
        }
    }
}