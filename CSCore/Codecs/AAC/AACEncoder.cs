using CSCore.MediaFoundation;
using System;
using System.IO;

namespace CSCore.Codecs.AAC
{
    /// <summary>
    /// Provides an encoder for encoding raw waveform-audio data to the AAC (Advanced Audio Codec) format.
    /// </summary>
    public class AacEncoder : MediaFoundationEncoder
    {
// ReSharper disable once InconsistentNaming
        private static readonly Guid MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION = new Guid("7632F0E6-9538-4d61-ACDA-EA29C8C14456");
        private MFAttributes _outputTypeAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AacEncoder"/> class.
        /// </summary>
        /// <param name="sourceFormat"><see cref="WaveFormat"/> of the audio data which gets encoded.</param>
        /// <param name="targetStream"><see cref="Stream"/> which should be used to save the encoded data in.</param>
        public AacEncoder(WaveFormat sourceFormat, Stream targetStream)
            : this(sourceFormat, targetStream, 192000, TranscodeContainerTypes.MFTranscodeContainerType_MPEG4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AacEncoder"/> class.
        /// </summary>
        /// <param name="sourceFormat"><see cref="WaveFormat"/> of the audio data which gets encoded.</param>
        /// <param name="targetStream"><see cref="Stream"/> which should be used to save the encoded data in.</param>
        /// <param name="defaultBitrate">Default samplerate. Use 192000 as the default value.</param>
        /// <param name="containerType">Guid of the container type. Use <see cref="TranscodeContainerTypes.MFTranscodeContainerType_MPEG4"/> as the default container.</param>
        public AacEncoder(WaveFormat sourceFormat, Stream targetStream, int defaultBitrate, Guid containerType)
        {
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceFormat");

            if (targetStream == null)
                throw new ArgumentNullException("targetStream");
            if (!targetStream.CanWrite)
                throw new ArgumentException("Stream is not writeable.");

            if (defaultBitrate <= 0)
                throw new ArgumentOutOfRangeException("defaultBitrate");

            if (containerType == Guid.Empty)
                throw new ArgumentNullException("containerType");

            var targetMediaType = FindBestMediaType(AudioSubTypes.MPEG_HEAAC, 
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
                if (_outputTypeAttributes == null && OutputMediaType != null)
                {
                    _outputTypeAttributes = OutputMediaType.QueryInterface<MFAttributes>();
                }
                else if (OutputMediaType == null)
                {
                    throw new InvalidOperationException("No set outputmediatype.");
                }
                return _outputTypeAttributes;
            }
        }

        /// <summary>
        /// Gets or sets the audio profile and level of an Advanced Audio Coding (AAC) stream.
        /// </summary>
        /// <remarks>
        /// This attribute contains the value of the audioProfileLevelIndication field, as defined by ISO/IEC 14496-3.
        /// </remarks>
        public AacAudioProfileLevelIndication AudioProfileLevelIndication
        {
            get
            {
                return (AacAudioProfileLevelIndication)OutputTypeAttributes.GetUINT32(MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION);
            }
            set
            {
                OutputTypeAttributes.Set(MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION, (int)value);
            }
        }
    }
}
