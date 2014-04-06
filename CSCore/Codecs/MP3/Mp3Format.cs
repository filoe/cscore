using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Codecs.MP3
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class MP3Format : WaveFormat
    {
        public Mp3FormatId _id;
        public Mp3PaddingFlags _flags;
        public ushort _blockSize;
        public ushort _framesPerBlock;
        public ushort _codecDelay;

        private const int Mp3WaveFormatExtraBytes = 12;

        public MP3Format(int sampleRate, int channels, int blockSize, int bitRate)
            : base(sampleRate, 0, channels, AudioEncoding.MpegLayer3, Mp3WaveFormatExtraBytes)
        {
            if (bitRate < 0)
                throw new ArgumentOutOfRangeException("bitRate");

            _bytesPerSecond = bitRate / 8;
            blockAlign = 1; // must be 1

            extraSize = Mp3WaveFormatExtraBytes;
            _id = Mp3FormatId.Mpeg;
            _flags = Mp3PaddingFlags.PaddingIso;
            blockSize = (ushort)blockSize;
            _framesPerBlock = 1;
            _codecDelay = 0;
        }
    }

    /// <summary>
    /// Padding Flags
    /// </summary>
    [Flags]
    public enum Mp3PaddingFlags
    {
        PaddingIso = 0,
        PaddingOn = 1,
        PaddingOff = 2,
    }

    public enum Mp3FormatId : ushort
    {
        Unknown = 0,
        Mpeg = 1,
        ConstFrameSize = 2
    }
}