namespace CSCore.XAudio2
{
    /// <summary>
    ///     Defines values to use with XAudio2Create to specify available processors.
    /// </summary>
    public enum XAudio2Processor
    {
        /// <summary>
        ///     Processor 1
        /// </summary>
        Processor1 = 0x00000001,

        /// <summary>
        ///     Processor 2
        /// </summary>
        Processor2 = 0x00000002,

        /// <summary>
        ///     Processor 3
        /// </summary>
        Processor3 = 0x00000004,

        /// <summary>
        ///     Processor 4
        /// </summary>
        Processor4 = 0x00000008,

        /// <summary>
        ///     Processor 5
        /// </summary>
        Processor5 = 0x00000010,

        /// <summary>
        ///     Processor 6
        /// </summary>
        Processor6 = 0x00000020,

        /// <summary>
        ///     Processor 7
        /// </summary>
        Processor7 = 0x00000040,

        /// <summary>
        ///     Processor 8
        /// </summary>
        Processor8 = 0x00000080,

        /// <summary>
        ///     Processor 9
        /// </summary>
        Processor9 = 0x00000100,

        /// <summary>
        ///     Processor 10
        /// </summary>
        Processor10 = 0x00000200,

        /// <summary>
        ///     Processor 11
        /// </summary>
        Processor11 = 0x00000400,

        /// <summary>
        ///     Processor 12
        /// </summary>
        Processor12 = 0x00000800,

        /// <summary>
        ///     Processor 13
        /// </summary>
        Processor13 = 0x00001000,

        /// <summary>
        ///     Processor 14
        /// </summary>
        Processor14 = 0x00002000,

        /// <summary>
        ///     Processor 15
        /// </summary>
        Processor15 = 0x00004000,

        /// <summary>
        ///     Processor 16
        /// </summary>
        Processor16 = 0x00008000,

        /// <summary>
        ///     Processor 17
        /// </summary>
        Processor17 = 0x00010000,

        /// <summary>
        ///     Processor 18
        /// </summary>
        Processor18 = 0x00020000,

        /// <summary>
        ///     Processor 19
        /// </summary>
        Processor19 = 0x00040000,

        /// <summary>
        ///     Processor 20
        /// </summary>
        Processor20 = 0x00080000,

        /// <summary>
        ///     Processor 21
        /// </summary>
        Processor21 = 0x00100000,

        /// <summary>
        ///     Processor 22
        /// </summary>
        Processor22 = 0x00200000,

        /// <summary>
        ///     Processor 23
        /// </summary>
        Processor23 = 0x00400000,

        /// <summary>
        ///     Processor 24
        /// </summary>
        Processor24 = 0x00800000,

        /// <summary>
        ///     Processor 25
        /// </summary>
        Processor25 = 0x01000000,

        /// <summary>
        ///     Processor 26
        /// </summary>
        Processor26 = 0x02000000,

        /// <summary>
        ///     Processor 27
        /// </summary>
        Processor27 = 0x04000000,

        /// <summary>
        ///     Processor 28
        /// </summary>
        Processor28 = 0x08000000,

        /// <summary>
        ///     Processor 29
        /// </summary>
        Processor29 = 0x10000000,

        /// <summary>
        ///     Processor 30
        /// </summary>
        Processor30 = 0x20000000,

        /// <summary>
        ///     Processor 31
        /// </summary>
        Processor31 = 0x40000000,

        /// <summary>
        ///     Processor 32
        /// </summary>
        Processor32 = unchecked((int) 0x80000000),

        /// <summary>
        ///     Any processor
        /// </summary>
        Xaudio2AnyProcessor = unchecked((int) 0xffffffff),

        /// <summary>
        ///     Default processor for XAudio2.7, which is defined as <see cref="Xaudio2AnyProcessor"/>.
        /// </summary>
        XAudio27DefaultProcessor = Xaudio2AnyProcessor,

        /// <summary>
        ///     Default processor for XAudio2.8, which is defined as <see cref="Processor1"/>.
        /// </summary>
        Xaudio28DefaultProcessor = Processor1
    }
}