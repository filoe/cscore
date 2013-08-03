using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.AAC
{
    public class AACDecoder : MediaFoundationDecoder
    {
        static bool? _issupported;
        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoder.AACDecoder);
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
