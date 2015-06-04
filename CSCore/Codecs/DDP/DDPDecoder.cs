using CSCore.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.DDP
{
    /// <summary>
    /// Mediafoundation DDP decoder.
    /// </summary>
    public class DDPDecoder : MediaFoundationDecoder
    {
        private static bool? _issupported;

        /// <summary>
        /// Gets a value which indicates whether the Mediafoundation DDP decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsSupported && MediaFoundationCore.IsTransformAvailable(MFTCategories.AudioDecoder, CommonAudioDecoderGuids.DolbyDigitalPlusDecoder);
                }
                return _issupported.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DDPDecoder"/> class.
        /// </summary>
        /// <param name="url">Url which points to a data source which provides DDP data. This is typically a filename.</param>
        public DDPDecoder(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DDPDecoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains DDP data.</param>
        public DDPDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}