using CSCore.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.MP3
{
    public class MP3MediafoundationDecoder : MediaFoundationDecoder
    {
        private static bool? _issupported;
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.MP3Decoder);
                }
                return _issupported.Value;
            }
        }

        public MP3MediafoundationDecoder(string url)
            : base(url)
        {
        }

        public MP3MediafoundationDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}
