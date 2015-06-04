using System;
using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// MP3 Mediafoundation Decoder.
    /// </summary>
    public class Mp3MediafoundationDecoder : MediaFoundationDecoder
    {
        private static bool? _issupported;

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation MP3 decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsSupported && MediaFoundationCore.IsTransformAvailable(MFTCategories.AudioDecoder, CommonAudioDecoderGuids.Mp3Decoder);
                }
                return _issupported.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3MediafoundationDecoder"/> class.
        /// </summary>
        /// <param name="url">Url which points to a data source which provides MP3 data. This is typically a filename.</param>
        public Mp3MediafoundationDecoder(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3MediafoundationDecoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains MP3 data.</param>
        public Mp3MediafoundationDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}
