using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Codecs.WAV
{
    /// <summary>
    ///     Represents the <see cref="FmtChunk" /> of a wave file.
    /// </summary>
    public class FmtChunk : WaveFileChunk
    {
        /// <summary>
        ///     Chunk ID of the <see cref="FmtChunk" />.
        /// </summary>
        public const int FmtChunkID = 0x20746D66;

        private readonly WaveFormat _waveFormat;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FmtChunk" /> class.
        /// </summary>
        /// <param name="stream"><see cref="Stream" /> which contains the fmt chunk.</param>
        public FmtChunk(Stream stream)
            : this(new BinaryReader(stream))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FmtChunk" /> class.
        /// </summary>
        /// <param name="reader"><see cref="BinaryReader" /> which should be used to read the fmt chunk.</param>
        public FmtChunk(BinaryReader reader)
            : base(reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (ChunkID == FmtChunkID) //"fmt "
            {
                var encoding = (AudioEncoding) reader.ReadInt16();
                int channels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                int avgBps = reader.ReadInt32();
                int blockAlign = reader.ReadInt16();
                int bitsPerSample = reader.ReadInt16();

                int extraSize = 0;
                if (ChunkDataSize > 16)
                {
                    extraSize = reader.ReadInt16();
                    if (extraSize != ChunkDataSize - 18)
                        //TODO: Check whether this is the correct way of reading a fmt chunk
                        extraSize = ChunkDataSize - 18;

                    for (int i = ChunkDataSize - 16; i > 0; i--)
                    {
                        reader.ReadByte();
                    }

                    reader.BaseStream.Position -= 2;
                }

                _waveFormat = new WaveFormat(sampleRate, (short) bitsPerSample, (short) channels, encoding, extraSize);
                Debug.Assert(blockAlign == _waveFormat.BlockAlign);
                Debug.Assert(avgBps == _waveFormat.BytesPerSecond);
            }
        }

        /// <summary>
        ///     Gets the <see cref="WaveFormat" /> specified by the <see cref="FmtChunk" />.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }
    }
}