using CSCore.MediaFoundation;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.AAC
{
    public class AACEncoder : MediaFoundationEncoder
    {
        private static readonly Guid MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION = new Guid("7632F0E6-9538-4d61-ACDA-EA29C8C14456");
        private MFAttributes outputTypeAttributes;

        public AACEncoder(Stream targetStream, WaveFormat sourceFormat)
            : this(sourceFormat, targetStream, 192000, TranscodeContainerTypes.MFTranscodeContainerType_MPEG4)
        {
        }

        public AACEncoder(WaveFormat sourceFormat, Stream targetStream, int defaultBitrate, Guid containerType)
        {
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceForamt");

            if (targetStream == null)
                throw new ArgumentNullException("targetStream");
            if (!targetStream.CanWrite)
                throw new ArgumentException("Stream is not writeable.");

            if (defaultBitrate <= 0)
                throw new ArgumentOutOfRangeException("defaultBitrate");

            if (containerType == Guid.Empty)
                throw new ArgumentNullException("containerType");

            var targetMediaType = FindBestMediaType(MFMediaTypes.MFAudioFormat_AAC, 
                sourceFormat.SampleRate, sourceFormat.Channels, defaultBitrate);

            if (targetMediaType == null)
                throw new NotSupportedException("No AAC-Encoder was found. Check whether your system supports AAC encoding.");

            var sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            SetTargetStream(targetStream, sourceMediaType, targetMediaType, containerType);
        }

        private MFAttributes OutputTypeAttributes
        {
            get
            {
                if (outputTypeAttributes == null && OutputMediaType != null)
                {
                    outputTypeAttributes = OutputMediaType.QueryInterface<MFAttributes>();
                }
                else if (OutputMediaType == null)
                {
                    throw new InvalidOperationException("No set outputmediatype.");
                }
                return outputTypeAttributes;
            }
        }

        public AACAudioProfileLevelIndication AudioProfileLevelIndication
        {
            get
            {
                return (AACAudioProfileLevelIndication)OutputTypeAttributes.GetUINT32(MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION);
            }
            set
            {
                OutputTypeAttributes.Set<int>(MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION, (int)value);
            }
        }
    }

    /// <summary>
    /// Specifies the audio profile and level of an Advanced Audio Coding (AAC) stream.
    /// AACProfile_L2_0x29 is the default setting.
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd319560(v=vs.85).aspx
    /// </summary>
    public enum AACAudioProfileLevelIndication
    {
        /// <summary>
        /// Default
        /// </summary>
        AACProfile_L2_0x29 = 0x29,
        AACProfile_L4_0x2A = 0x2A,
        AACProfile_L5_0x2B = 0x2B,
        HighEfficiencyAACProfile_L2_0x2C = 0x2C,
        HighEfficiencyAACProfile_L3_0x2D = 0x2D,
        HighEfficiencyAACProfile_L4_0x2E = 0x2E,
        HighEfficiencyAACProfile_L5_0x2F = 0x2F,
        ReservedForIsoUse_0x30 = 0x30,
        ReservedForIsoUse_0x31 = 0x31,
        ReservedForIsoUse_0x32 = 0x32,
        ReservedForIsoUse_0x33 = 0x33
    }
}
