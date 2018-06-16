using System;
using System.IO;
using CSCore.OSXCoreAudio;
using MonoMac.AudioToolbox;
namespace CSCore.Codecs.AAC
{
    /// <summary>
    ///     Core Audio AAC decoder
    /// </summary>
    public class AACDecoderOSX : OSXAudioDecoder
    {
        private static bool? _issupported;

        /// <summary>
        ///     Gets a value which indicates whether the CoreAudio AAC decoder is supported on the current platform.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = OSXAudio.IsSupported;
                }
                return _issupported.Value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AACDecoderOSX"/> class.
        /// </summary>
        /// <param name="url">Url which points to a data source which provides AAC data. This is typically a filename.</param>
        public AACDecoderOSX(string url)
            : base(url)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AACDecoderOSX"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains AAC data.</param>
        public AACDecoderOSX(Stream stream)
            : base(stream, AudioFileType.AAC_ADTS)
        {
        }
    }
}

