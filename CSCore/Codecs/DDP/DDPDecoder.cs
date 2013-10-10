using CSCore.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.DDP
{
    public class DDPDecoder : MediaFoundationDecoder
    {
        private static bool? _issupported;

        public static bool IsSupported
        {
            get
            {
                if (_issupported == null)
                {
                    _issupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.DolbyDigitalPlusDecoder);
                }
                return _issupported.Value;
            }
        }

        public DDPDecoder(string url)
            : base(url)
        {
        }

        public DDPDecoder(Stream stream)
            : base(stream)
        {
        }
    }
}