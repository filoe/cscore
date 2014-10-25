using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// see http://msdn.microsoft.com/en-us/library/windows/desktop/ms696989%28v=vs.85%29.aspx
    /// </summary>
    internal static class MediaFoundationAttributes
    {
        public static readonly Guid MF_MT_AUDIO_BITS_PER_SAMPLE = new Guid("f2deb57f-40fa-4764-aa33-ed4f2d1ff669");
        public static readonly Guid MF_MT_AUDIO_NUM_CHANNELS = new Guid("37e48bf5-645e-4c5b-89de-ada9e29b696a");
        public static readonly Guid MF_MT_AUDIO_SAMPLES_PER_SECOND = new Guid("5faeeae7-0290-4c31-9e8a-c534f68d9dba");
        public static readonly Guid MF_MT_AUDIO_AVG_BYTES_PER_SECOND = new Guid("1aab75c8-cfef-451c-ab95-ac034b8e1731");
        public static readonly Guid MF_MT_SUBTYPE = new Guid("f7e34c9a-42e8-4714-b74b-cb29d72c35e5");
        public static readonly Guid MF_MT_MAJOR_TYPE = new Guid("48eba18e-f8c9-4687-bf11-0a74c9f96a8f");
        public static readonly Guid MF_PD_DURATION = new Guid("6c990d33-bb8e-477a-8598-0d5d96fcd88a");
        public static readonly Guid MF_SOURCE_READER_MEDIASOURCE_CHARACTERISTICS = new Guid("6d23f5c8-c5d7-4a9b-9971-5d11f8bca880");
        public static readonly Guid MFT_TRANSFORM_CLSID_Attribute = new Guid("6821c42b-65a4-4e82-99bc-9a88205ecd0c");

        public static readonly Guid MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS = new Guid("a634a91c-822b-41b9-a494-4de4643612b0");
        public static readonly Guid MF_TRANSCODE_CONTAINERTYPE = new Guid("150ff23f-4abc-478b-ac4f-e1916fba1cca");
        public static readonly Guid MF_MT_AUDIO_CHANNEL_MASK = new Guid("55fb5765-644a-4caf-8479-938983bb1588");
    }
}