using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags that describe an input stream.
    ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375502(v=vs.85).aspx.
    /// </summary>
    [Flags]
    public enum InputStreamInfoFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None,

        /// <summary>
        ///     The stream contains whole samples. Samples do not span multiple buffers, and buffers do
        ///     not contain partial samples.
        /// </summary>
        WholeSamples = 0x1,

        /// <summary>
        ///     Each buffer contains exactly one sample.
        /// </summary>
        SingleSamplePerBuffer = 0x2,

        /// <summary>
        ///     The stream is discardable. Within calls to IMediaObject::ProcessOutput, the DMO can
        ///     discard data for this stream without copying it to an output buffer.
        /// </summary>
        FixedSampleSize = 0x4,

        /// <summary>
        ///     The DMO performs lookahead on the incoming data, and may hold multiple input buffers for
        ///     this stream.
        /// </summary>
        HoldsBuffers = 0x8
    }
}