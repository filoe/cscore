using CSCore.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.WMA
{
    public class WMADecoder : MediaFoundationDecoder
    {
        private static bool? _isspeechsupported;
        private static bool? _iswmasupported;
        private static bool? _iswmaprosupported;

        public static bool IsSupported
        {
            get
            {
                return IsSpeechSupported || IsWMAProfessionalSupported || IsWMASupported;
            }
        }

        public static bool IsSpeechSupported
        {
            get
            {
                if (_isspeechsupported == null)
                {
                    _isspeechsupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.WMSpeechDecoder);
                }
                return _isspeechsupported.Value;
            }
        }

        public static bool IsWMAProfessionalSupported
        {
            get
            {
                if (_iswmaprosupported == null)
                {
                    _iswmaprosupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.WMAProDecoder);
                }
                return _iswmaprosupported.Value;
            }
        }

        public static bool IsWMASupported
        {
            get
            {
                if (_iswmasupported == null)
                {
                    _iswmasupported = MediaFoundationCore.IsTransformAvailable(MediaFoundationCore.EnumerateTransforms(MFTCategories.AudioDecoder, MFTEnumFlags.All),
                        CommonAudioDecoderGuids.WMAudioDecoder);
                }
                return _iswmasupported.Value;
            }
        }

        public WMADecoder(string url)
            : base(url)
        {
        }

        public WMADecoder(Stream stream)
            : base(stream)
        {
        }
    }
}