using CSCore.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.AAC
{
    public class AACDecoder : MediaFoundationDecoder
    {
        private static bool? _issupported;

        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.AACDecoder);
                }
                return _issupported.Value;
            }
        }

        public AACDecoder(string url)
            : base(url)
        {
        }

        public AACDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}