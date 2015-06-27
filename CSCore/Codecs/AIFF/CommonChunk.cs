using System;
using System.IO;

namespace CSCore.Codecs.AIFF
{
    /// <summary>
    ///     Provides the format of the encoded audio data of a AIFF-file.
    /// </summary>
    public class CommonChunk : AiffChunk
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommonChunk" /> class.
        /// </summary>
        /// <param name="binaryReader">The binary reader which provides can be used to decode the chunk.</param>
        /// <exception cref="CSCore.Codecs.AIFF.AiffException">Compression type not supported.</exception>
        public CommonChunk(BinaryReader binaryReader) : base(binaryReader, "COMM")
        {
            NumberOfChannels = Reader.ReadInt16();
            NumberOfSampleFrames = Reader.ReadUInt32();
            BitsPerSample = Reader.ReadInt16();
            SampleRate = Reader.ReadIeeeExtended();

            if (DataSize > 18)
            {
                CompressionType = new string(binaryReader.ReadChars(4));
                if (!string.Equals(CompressionType, "none", StringComparison.OrdinalIgnoreCase))
                {
                    throw new AiffException("Compression type not supported.",
                        new NotSupportedException("The compression type of the Aiff stream is not supported."));
                }
            }
        }

        /// <summary>
        ///     Gets the number of channels.
        /// </summary>
        public short NumberOfChannels { get; private set; }

        /// <summary>
        ///     Gets the total number of sample frames.
        /// </summary>
        /// <remarks>
        ///     To get the total number of samples multiply <see cref="NumberOfSampleFrames" /> by
        ///     <see cref="NumberOfChannels" />.
        /// </remarks>
        public long NumberOfSampleFrames { get; private set; }

        /// <summary>
        ///     Gets the number of bits per sample.
        /// </summary>
        public short BitsPerSample { get; private set; }

        /// <summary>
        ///     Gets the sample rate in Hz.
        /// </summary>
        public double SampleRate { get; private set; }

        /// <summary>
        ///     Gets the compression type.
        /// </summary>
        /// <remarks>All compression types except PCM are currently <b>not</b> supported.</remarks>
        public string CompressionType { get; private set; }

        /// <summary>
        ///     Gets the wave format.
        /// </summary>
        /// <returns>The wave format.</returns>
        /// <remarks>
        ///     This method does not take care about multi channel formats. It won't setup a channel mask.
        /// </remarks>
        public WaveFormat GetWaveFormat()
        {
            //todo: take care about multi channel formats
            return new WaveFormat((int) SampleRate, BitsPerSample, NumberOfChannels, AudioEncoding.Pcm);
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
            if (CompressionType == null)
                Reader.Skip(DataSize - 18);
            else
                Reader.Skip(DataSize - 22);
        }
    }
}