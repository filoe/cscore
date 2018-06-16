using System;
using System.IO;
using CSCore.OSXCoreAudio;
using MonoMac.AudioToolbox;
namespace CSCore.Codecs.MP3
{
    /// <summary>
    ///     Core Audio MP3 Decoder
    /// </summary>
    public class MP3DecoderOSX : OSXAudioDecoder
    {
        private static bool? _issupported;

        /// <summary>
        ///     Gets a value which indicates whether the CoreAudio MP3 decoder is supported on the current platform.
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
        ///     Initializes a new instance of the <see cref="MP3DecoderOSX"/> class.
        /// </summary>
        /// <param name="url">Url which points to a data source which provides MP3 data. This is typically a filename.</param>
        public MP3DecoderOSX(string url)
            : base(url)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MP3DecoderOSX"/> class.
        /// </summary>
        /// <param name="stream">Stream which contains MP3 data.</param>
        public MP3DecoderOSX(Stream stream)
            : base(stream, AudioFileType.MP3)
        {
        }
    }
}

