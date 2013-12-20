using CSCore.DMO;
using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Resampler based on the DmoResampler. Supportet since Windows XP
    /// </summary>
    public class DmoChannelResampler : DmoResampler
    {
        private ChannelMatrix _channelMatrix = ChannelMatrix.TwoToSixChannels;

        /// <summary>
        /// Resampler based on wavesource and new samplerate
        /// </summary>
        /// <param name="source">Source which has to get resampled</param>
        /// <param name="channelMatrix">Channelmatrix which contains information about the channel
        /// mapping.</param>
        public DmoChannelResampler(IWaveSource source, ChannelMatrix channelMatrix)
            : this(source, channelMatrix, source.WaveFormat.SampleRate)
        {
        }

        public DmoChannelResampler(IWaveSource source, ChannelMatrix channelMatrix, int destSampleRate)
            : base(source, destSampleRate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (channelMatrix == null)
                throw new ArgumentNullException("channelMatrix");

            if (source.WaveFormat.Channels != channelMatrix.InputChannelCount)
                throw new ArgumentException("source.WaveFormat.Channels != channelMatrix.InputChannelCount");

            WaveFormatExtensible inputformat = new WaveFormatExtensible(source.WaveFormat.SampleRate, source.WaveFormat.BitsPerSample,
                source.WaveFormat.Channels, WaveFormatExtensible.SubTypeFromWaveFormat(source.WaveFormat), _channelMatrix.InputMask);

            _outputformat = new WaveFormat(destSampleRate, source.WaveFormat.BitsPerSample, 6, source.WaveFormat.WaveFormatTag, source.WaveFormat.ExtraSize);
            WaveFormatExtensible outputformat = new WaveFormatExtensible(_outputformat.SampleRate, _outputformat.BitsPerSample,
            _outputformat.Channels, WaveFormatExtensible.SubTypeFromWaveFormat(_outputformat), _channelMatrix.OutputMask);

            Init(inputformat, outputformat);
            _resampler.ResamplerProps.SetUserChannelMtx(_channelMatrix.GetMatrix());
        }
    }
}