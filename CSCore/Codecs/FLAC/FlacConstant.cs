namespace CSCore.Codecs.FLAC
{
    public static class FlacConstant
    {
        public static readonly int[] SampleRateTable = new int[]
        {
            -1, 88200, 176400, 192000,
            8000, 16000, 22050, 24000,
            32000, 44100, 48000, 96000,
            -1, -1, -1, -1
        };

        public static readonly int[] BitPerSampleTable = new int[]
        {
            -1, 8, 12, -1,
            16, 20, 24, -1
        };

        public static readonly int[] FlacBlockSizes = new int[]
        {
            0, 192, 576, 1152,
            2304, 4608, 0, 0,
            256, 512, 1024, 2048,
            4096, 8192, 16384
        };

        public const int FrameHeaderSize = 16;
        public const int MAX_FIXED_ORDER = 4;
        public const int MAX_LPC_ORDER = 32;

        //FLAC__SUBFRAME_LPC_QLP_COEFF_PRECISION_LEN
        public const int SUBFRAME_LPC_QLP_COEFF_PRECISION_LEN = 4;

        //FLAC__SUBFRAME_LPC_QLP_SHIFT_LEN
        public const int SUBFRAME_LPC_QLP_SHIFT_LEN = 5;

        //FLAC__ENTROPY_CODING_METHOD_TYPE_LEN
        public const int ENTROPY_CODING_METHOD_TYPE_LEN = 2;

        //FLAC__ENTROPY_CODING_METHOD_PARTITIONED_RICE_ORDER_LEN
        public const int ENTROPY_CODING_METHOD_PARTITIONED_RICE_ORDER_LEN = 4;

        /// <summary>
        /// 4 bit
        /// </summary>
        public const int ENTROPY_CODING_METHOD_PARTITIONED_RICE_PARAMETER_LEN = 4;

        /// <summary>
        /// 5 bit
        /// </summary>
        public const int ENTROPY_CODING_METHOD_PARTITIONED_RICE2_PARAMETER_LEN = 5;

        /// <summary>
        /// 15 bit
        /// </summary>
        public const int ENTROPY_CODING_METHOD_PARTITIONED_RICE_ESCAPE_PARAMETER = 15;  //1111

        /// <summary>
        /// 31 bit
        /// </summary>
        public const int ENTROPY_CODING_METHOD_PARTITIONED_RICE2_ESCAPE_PARAMETER = 31; //11111

        /// <summary>
        /// 5 bit
        /// </summary>
        public const int ENTROPY_CODING_METHOD_PARTITIONED_RICE_RAW_LEN = 5;
    }
}