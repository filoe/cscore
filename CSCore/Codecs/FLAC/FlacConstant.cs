namespace CSCore.Codecs.FLAC
{
    internal static class FlacConstant
    {
        public static readonly int[] SampleRateTable =
        {
            -1, 88200, 176400, 192000,
            8000, 16000, 22050, 24000,
            32000, 44100, 48000, 96000,
            -1, -1, -1, -1
        };

        public static readonly int[] BitPerSampleTable =
        {
            -1, 8, 12, -1,
            16, 20, 24, -1
        };

        public static readonly int[] FlacBlockSizes =
        {
            0, 192, 576, 1152,
            2304, 4608, 0, 0,
            256, 512, 1024, 2048,
            4096, 8192, 16384
        };

        public const int FrameHeaderSize = 16;
    }
}