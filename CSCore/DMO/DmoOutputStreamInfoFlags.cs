using System;

namespace CSCore.DMO
{
    /// <summary>
    /// The <see cref="DmoOutputStreamInfoFlags"/> enumeration defines flags that describe an output stream.
    /// </summary>
    [Flags]
    public enum DmoOutputStreamInfoFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     The stream contains whole samples. Samples do not span multiple buffers, and buffers do not contain partial
        ///     samples.
        /// </summary>
        WholeSamples = 0x1,

        /// <summary>
        ///     Each buffer contains exactly one sample.
        /// </summary>
        SingleSamplePerBuffer = 0x2,

        /// <summary>
        ///     All the samples in this stream are the same size.
        /// </summary>
        FixedSampleSize = 0x4,

        /// <summary>
        ///     The stream is discardable. Within calls to IMediaObject::ProcessOutput, the DMO can discard data for this stream
        ///     without copying it to an output buffer.
        /// </summary>
        Discardable = 0x8,

        /// <summary>
        ///     The stream is optional. An optional stream is discardable. Also, the application can ignore this stream entirely;
        ///     it does not have to set the media type for the stream. Optional streams generally contain additional information,
        ///     or data not needed by all applications.
        /// </summary>
        Optional = 0x10
    }
}