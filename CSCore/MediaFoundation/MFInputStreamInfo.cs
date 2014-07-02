using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Contains information about an input stream on a Media Foundation transform (MFT). To get these values, call IMFTransform::GetInputStreamInfo.
    /// </summary>
    /// <remarks>
    /// Before the media types are set, the only values that should be considered valid are the MFT_INPUT_STREAM_REMOVABLE and MFT_INPUT_STREAM_OPTIONAL flags in the dwFlags member.
    /// -The MFT_INPUT_STREAM_REMOVABLE flag indicates that the stream can be deleted.
    /// -The MFT_INPUT_STREAM_OPTIONAL flag indicates that the stream is optional and does not require a media type.
    /// After you set a media type on all of the input and output streams (not including optional streams), all of the values returned by the GetInputStreamInfo method are valid. They might change if you set different media types.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MFInputStreamInfo
    {
        /// <summary>
        /// Maximum amount of time between an input sample and the corresponding output sample, in
        /// 100-nanosecond units. For example, an MFT that buffers two samples, each with a duration
        /// of 1 second, has a maximum latency of two seconds. If the MFT always turns input samples
        /// directly into output samples, with no buffering, the latency is zero.
        /// </summary>
        public long HnsMaxLatency;

        /// <summary>
        /// Bitwise OR of zero or more flags from the InputStreamInfoFlags enumeration.
        /// </summary>
        public MFInputStreamInfoFlags Flags;

        /// <summary>
        /// The minimum size of each input buffer, in bytes. If the size is variable or the MFT does
        /// not require a specific size, the value is zero. For uncompressed audio, the value should
        /// be the audio frame size, which you can get from the MF_MT_AUDIO_BLOCK_ALIGNMENT
        /// attribute in the media type.
        /// </summary>
        public int Size;

        /// <summary>
        /// Maximum amount of input data, in bytes, that the MFT holds to perform lookahead.
        /// Lookahead is the action of looking forward in the data before processing it. This value
        /// should be the worst-case value. If the MFT does not keep a lookahead buffer, the value
        /// is zero.
        /// </summary>
        public int cbMaxLookahead;

        /// <summary>
        /// The memory alignment required for input buffers. If the MFT does not require a specific
        /// alignment, the value is zero.
        /// </summary>
        public int cbAlignment;

        /*public InputStreamInfo()
        {
            HnsMaxLatency = 0;
            Flags = (InputStreamInfoFlags)0;
            cbMaxLookahead = 0;
            cbAlignment = 0;
            Size = Marshal.SizeOf(typeof(InputStreamInfo));
        }*/
    }
}