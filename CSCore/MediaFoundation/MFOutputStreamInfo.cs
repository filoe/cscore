using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Contains information about an output stream on a Media Foundation transform (MFT). To get these values, call IMFTransform::GetOutputStreamInfo.
    /// </summary>
    /// <remarks>
    /// Before the media types are set, the only values that should be considered valid is the MFT_OUTPUT_STREAM_OPTIONAL flag in the dwFlags member. This flag indicates that the stream is optional and does not require a media type.
    /// After you set a media type on all of the input and output streams (not including optional streams), all of the values returned by the GetOutputStreamInfo method are valid. They might change if you set different media types.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MFOutputStreamInfo
    {
        /// <summary>
        /// Bitwise OR of zero or more flags from the _MFT_OUTPUT_STREAM_INFO_FLAGS enumeration.
        /// </summary>
        public MFOutputStreamInfoFlags Flags;

        /// <summary>
        /// Minimum size of each output buffer, in bytes. If the MFT does not require a specific
        /// size, the value is zero. For uncompressed audio, the value should be the audio frame
        /// size, which you can get from the MF_MT_AUDIO_BLOCK_ALIGNMENT attribute in the media
        /// type. If the dwFlags member contains the MFT_OUTPUT_STREAM_PROVIDES_SAMPLES flag, the
        /// value is zero, because the MFT allocates the output buffers.
        /// </summary>
        public int Size;

        /// <summary>
        /// The memory alignment required for output buffers. If the MFT does not require a specific
        /// alignment, the value is zero. If the dwFlags member contains the
        /// MFT_OUTPUT_STREAM_PROVIDES_SAMPLES flag, this value is the alignment that the MFT uses
        /// internally when it allocates samples. It is recommended, but not required, that MFTs
        /// allocate buffers with at least a 16-byte memory alignment.
        /// </summary>
        public int cbAlignment;

        /*public OutputStreamInfo()
        {
            Flags = (OutputStreamInfoFlags)0;
            cbAlignment = 0;
            Size = Marshal.SizeOf(typeof(OutputStreamInfo));
        }*/
    }
}