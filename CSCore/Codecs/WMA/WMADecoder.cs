using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.MediaFoundation;
using System.IO;

namespace CSCore.Codecs.WMA
{
    public class WMADecoder : MediaFoundationDecoder
    {
        static bool? _isspeechsupported;
        static bool? _iswmasupported;
        static bool? _iswmaprosupported;

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
                        CommonAudioDecoder.WMSpeechDecoder);
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
                        CommonAudioDecoder.WMAProDecoder);
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
                        CommonAudioDecoder.WMAudioDecoder);
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
