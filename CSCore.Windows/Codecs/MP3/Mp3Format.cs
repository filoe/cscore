using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// The <see cref="Mp3Format"/> class describes an MPEG Audio Layer-3 (MP3) audio format. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public sealed class Mp3Format : WaveFormat
    {
        /// <summary>
        /// Set this member to <see cref="Mp3FormatId.Mpeg"/>.
        /// </summary>
        public Mp3FormatId Id;

        /// <summary>
        /// Indicates whether padding is used to adjust the average bitrate to the sampling rate. 
        /// </summary>
        public Mp3PaddingFlags Flags;

        /// <summary>
        /// Block size in bytes. This value equals the frame length in bytes x <see cref="FramesPerBlock"/>. For MP3 audio, the frame length is calculated as follows: 144 x (bitrate / sample rate) + padding.
        /// </summary>
        public short BlockSize;

        /// <summary>
        /// Number of audio frames per block.
        /// </summary>
        public short FramesPerBlock;

        /// <summary>
        /// Encoder delay in samples. If you do not know this value, set this structure member to zero.
        /// </summary>
        public short CodecDelay;

        /// <summary>
        /// MPEGLAYER3_WFX_EXTRA_BYTES
        /// </summary>
        private const int Mp3WaveFormatExtraBytes = 12;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3Format"/> class.
        /// </summary>
        /// <param name="sampleRate">Sample rate in Hz.</param>
        /// <param name="channels">Number of channels.</param>
        /// <param name="blockSize">Block size in bytes. This value equals the frame length in bytes x <see cref="FramesPerBlock"/>. For MP3 audio, the frame length is calculated as follows: 144 x (bitrate / sample rate) + padding.</param>
        /// <param name="bitRate">Bitrate.</param>
        public Mp3Format(int sampleRate, int channels, int blockSize, int bitRate)
            : base(sampleRate, 0, channels, AudioEncoding.MpegLayer3, Mp3WaveFormatExtraBytes)
        {
            if (bitRate < 0)
                throw new ArgumentOutOfRangeException("bitRate");

            BytesPerSecond = bitRate / 8;
            BlockAlign = 1; // must be 1

            ExtraSize = Mp3WaveFormatExtraBytes;
            Id = Mp3FormatId.Mpeg;
            Flags = Mp3PaddingFlags.PaddingIso;
            BlockSize = (short) blockSize;
            FramesPerBlock = 1;
            CodecDelay = 0;
        }

        /// <summary>
        /// Updates the <see cref="WaveFormat.BlockAlign"/>- and the <see cref="WaveFormat.BytesPerSecond"/>-property.
        /// </summary>
        internal protected override void UpdateProperties()
        {
            //do nothing
        }
    }
}