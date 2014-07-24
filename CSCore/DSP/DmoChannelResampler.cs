using System;

namespace CSCore.DSP
{
    /// <summary>
    ///     Resampler based on the <see cref="DmoResampler" /> which can change the number of channels based on a
    ///     <see cref="ChannelMatrix" />. Supported since Windows XP.
    /// </summary>
    public class DmoChannelResampler : DmoResampler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DmoChannelResampler" /> class.
        /// </summary>
        /// <param name="source">Underlying source which has to get resampled.</param>
        /// <param name="channelMatrix"><see cref="ChannelMatrix" /> which defines how to map each channel.</param>
        public DmoChannelResampler(IWaveSource source, ChannelMatrix channelMatrix)
            : this(source, channelMatrix, source.WaveFormat.SampleRate)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DmoChannelResampler" /> class.
        /// </summary>
        /// <param name="source">Underlying source which has to get resampled.</param>
        /// <param name="channelMatrix"><see cref="ChannelMatrix" /> which defines how to map each channel.</param>
        /// <param name="destSampleRate">The destination sample rate.</param>
        public DmoChannelResampler(IWaveSource source, ChannelMatrix channelMatrix, int destSampleRate)
            : base(source, destSampleRate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (channelMatrix == null)
                throw new ArgumentNullException("channelMatrix");

            if (source.WaveFormat.Channels != channelMatrix.InputChannelCount)
            {
                throw new ArgumentException(
                    "The number of channels of the source has to be equal to the number of input channels specified by the channelMatrix.");
            }

            var inputformat = new WaveFormatExtensible(source.WaveFormat.SampleRate, source.WaveFormat.BitsPerSample,
                source.WaveFormat.Channels, WaveFormatExtensible.SubTypeFromWaveFormat(source.WaveFormat),
                channelMatrix.InputMask);

            Outputformat = new WaveFormatExtensible(
                destSampleRate,
                source.WaveFormat.BitsPerSample,
                channelMatrix.OutputChannelCount,
                WaveFormatExtensible.SubTypeFromWaveFormat(Outputformat),
                channelMatrix.OutputMask);

            Initialize(inputformat, Outputformat);
            Resampler.ResamplerProps.SetUserChannelMtx(channelMatrix.GetOneDimensionalMatrix());
        }
    }
}