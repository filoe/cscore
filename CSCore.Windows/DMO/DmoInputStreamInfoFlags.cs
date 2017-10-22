using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags that describe an input stream.
    /// </summary>
    [Flags]
    public enum DmoInputStreamInfoFlags
    {
        /// <summary>
        ///     None.
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     The stream requires whole samples. Samples must not span multiple buffers, and buffers must not contain partial
        ///     samples.
        /// </summary>
        WholeSamples = 0x1,

        /// <summary>
        ///     Each buffer must contain exactly one sample.
        /// </summary>
        SingleSamplePerBuffer = 0x2,

        /// <summary>
        ///     All the samples in this stream must be the same size.
        /// </summary>
        FixedSampleSize = 0x4,

        /// <summary>
        ///     The DMO performs lookahead on the incoming data, and may hold multiple input buffers for this stream.
        /// </summary>
        HoldBuffers = 0x8
    }
}