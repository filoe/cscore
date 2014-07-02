namespace CSCore.DSP
{
    /// <summary>
    ///     Defines FFT data size constants that can be used for FFT calculations.
    ///     Note that only the half of the specified size can be used for visualizations.
    /// </summary>
    public enum FftSize
    {
        /// <summary>
        ///     64 bands.
        /// </summary>
        Fft64 = 64,

        /// <summary>
        ///     128 bands.
        /// </summary>
        Fft128 = 128,

        /// <summary>
        ///     256 bands.
        /// </summary>
        Fft256 = 256,

        /// <summary>
        ///     512 bands.
        /// </summary>
        Fft512 = 512,

        /// <summary>
        ///     1024 bands.
        /// </summary>
        Fft1024 = 1024,

        /// <summary>
        ///     2014 bands.
        /// </summary>
        Fft2048 = 2048,

        /// <summary>
        ///     4096 bands.
        /// </summary>
        Fft4096 = 4096,

        /// <summary>
        ///     8192 bands.
        /// </summary>
        Fft8192 = 8192,

        /// <summary>
        ///     16384 bands.
        /// </summary>
        Fft16384 = 16384
    }
}