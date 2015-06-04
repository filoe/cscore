using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines the CLSID values for several common mediafoundation audio decoders.
    /// </summary>
    public static class CommonAudioDecoderGuids
    {
        /// <summary>
        /// CLSID_CMSDDPlusDecMFT
        /// </summary>
        public static readonly Guid DolbyDigitalPlusDecoder = new Guid("177C0AFE-900B-48d4-9E4C-57ADD250B3D4");

        /// <summary>
        /// CLSID_CMSMPEGAudDecMFT
        /// </summary>
        public static readonly Guid MpegAudioDecoder = new Guid("70707B39-B2CA-4015-ABEA-F8447D22D88B");

        /// <summary>
        /// CMSAACDecMFT
        /// </summary>
        public static readonly Guid AacDecoder = new Guid("32D186A7-218F-4C75-8876-DD77273A8999");

        /// <summary>
        /// CWMADecMediaObject
        /// </summary>
        public static readonly Guid WmAudioDecoder = new Guid("2EEB4ADF-4578-4D10-BCA7-BB955F56320A");

        /// <summary>
        /// CALawDecMediaObject
        /// </summary>
        public static readonly Guid ALawDecoder = new Guid("36CB6E0C-78C1-42B2-9943-846262F31786");

        /// <summary>
        /// ACM Wrapper
        /// </summary>
        public static readonly Guid GsmDecoder = new Guid("4A76B469-7B66-4DD4-BA2D-DDF244C766DC");

        /// <summary>
        /// CWMAudioSpdTxDMO
        /// </summary>
        public static readonly Guid WmaProDecoder = new Guid("5210F8E4-B0BB-47C3-A8D9-7B2282CC79ED");

        /// <summary>
        /// CWMSPDecMediaObject
        /// </summary>
        public static readonly Guid WmSpeechDecoder = new Guid("874131CB-4ECC-443B-8948-746B89595D20");

        /// <summary>
        /// Wrapper
        /// </summary>
        public static readonly Guid G711Decoder = new Guid("92B66080-5E2D-449E-90C4-C41F268E5514");

        /// <summary>
        /// IMA ADPCM ACM Wrapper
        /// </summary>
        public static readonly Guid ImaAdPcmDecoder = new Guid("A16E1BFF-A80D-48AD-AECD-A35C005685FE");

        /// <summary>
        /// CMP3DecMediaObject
        /// </summary>
        public static readonly Guid Mp3Decoder = new Guid("BBEEA841-0A63-4F52-A7AB-A9B3A84ED38A");

        /// <summary>
        /// ADPCM ACM Wrapper
        /// </summary>
        public static readonly Guid AdPcmDecoder = new Guid("CA34FE0A-5722-43AD-AF23-05F7650257DD");
    }
}