using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Defines window functions.
    /// </summary>
    public static class WindowFunctions
    {
        /// <summary>
        /// Hamming Window
        /// </summary>
        public static readonly WindowFunction Hamming = (index, width) 
            => (float) (0.54 - 0.46 * Math.Cos((2 * Math.PI * index) / (width - 1)));

        /// <summary>
        /// Hamming Window (periodic version)
        /// </summary>
        public static readonly WindowFunction HammingPeriodic = (index, width)
            => (float)(0.54 - 0.46 * Math.Cos((2 * Math.PI * index) / (width)));

        /// <summary>
        /// Hanning Window 
        /// </summary>
        public static readonly WindowFunction Hanning = (index, width) 
            => (float) (0.5 - 0.5 * Math.Cos(index * ((2.0 * Math.PI) / width)));

        /// <summary>
        /// Hanning Window (periodic version)
        /// </summary>
        public static readonly WindowFunction HanningPeriodic = (index, width) 
            => (float)(0.5 - 0.5 * Math.Cos(index * ((2.0 * Math.PI) / width)));

        public static readonly WindowFunction None = (index, width) => 1.0f;
    }
}