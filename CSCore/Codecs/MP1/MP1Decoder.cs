using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.MP1
{
    /// <summary>
    /// Mediafoundation MP1 decoder.
    /// </summary>
    public class Mp1Decoder : MediaFoundationDecoder
    {
        private static bool? _issupported;

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation MP1 decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsSupported && MediaFoundationCore.IsTransformAvailable(MFTCategories.AudioDecoder, CommonAudioDecoderGuids.MpegAudioDecoder);
                }
                return _issupported.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp1Decoder"/> class.
        /// </summary>
        /// <param name="url">Url which points to a data source which provides MP1 data. This is typically a filename.</param>
        public Mp1Decoder(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp1Decoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains MP1 data.</param>
        public Mp1Decoder(Stream stream)
            : base(stream)
        {
        }
    }
}