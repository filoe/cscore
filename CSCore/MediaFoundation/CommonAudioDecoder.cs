using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    public static class CommonAudioDecoder
    {
        /// <summary>
        /// CLSID_CMSDDPlusDecMFT
        /// </summary>
        public static readonly Guid DolbyDigitalPlusDecoder = new Guid("177C0AFE-900B-48d4-9E4C-57ADD250B3D4");

        /// <summary>
        /// CLSID_CMSMPEGAudDecMFT
        /// </summary>
        public static readonly Guid MPEGAudioDecoder = new Guid("70707B39-B2CA-4015-ABEA-F8447D22D88B");

        /// <summary>
        /// CMSAACDecMFT
        /// </summary>
        public static readonly Guid AACDecoder = new Guid("32d186a7-218f-4c75-8876-dd77273a8999");

        /// <summary>
        /// CWMADecMediaObject
        /// </summary>
        public static readonly Guid WMAudioDecoder = new Guid("2eeb4adf-4578-4d10-bca7-bb955f56320a");

        public static readonly Guid ALawDecoder = new Guid("36CB6e0c-78c1-42b2-9943-846262f31786");

        /// <summary>
        /// ACM Wrapper
        /// </summary>
        public static readonly Guid GSMDecoder = new Guid("4a76b469-7b66-4dd4-ba2d-ddf244c766dc");

        /// <summary>
        /// CWMAudioSpdTxDMO
        /// </summary>
        public static readonly Guid WMAProDecoder = new Guid("5210f8e4-b0bb-47c3-a8d9-7b2282cc79ed");

        /// <summary>
        /// CWMSPDecMediaObject
        /// </summary>
        public static readonly Guid WMSpeechDecoder = new Guid("874131cb-4ecc-443b-8948-746b89595d20");

        /// <summary>
        /// Wrapper
        /// </summary>
        public static readonly Guid G711Decoder = new Guid("92b66080-5e2d-449e-90c4-c41f268e5514");

        /// <summary>
        /// IMA ADPCM ACM Wrapper
        /// </summary>
        public static readonly Guid ImaAdPcmDecoder = new Guid("a16e1bff-a80d-48ad-aecd-a35c005685fe");

        /// <summary>
        /// CMP3DecMediaObject
        /// </summary>
        public static readonly Guid MP3Decoder = new Guid("bbeea841-0a63-4f52-a7ab-a9b3a84ed38a");

        /// <summary>
        /// ADPCM ACM Wrapper
        /// </summary>
        public static readonly Guid AdPcmDecoder = new Guid("ca34fe0a-5722-43ad-af23-05f7650257dd");
    }
}