using System;
using System.Diagnostics;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;

namespace CSCore
{
    /// <summary>
    ///     Provides a basic fluent API for creating a source chain.
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        ///     Appends a source to an already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <typeparam name="TResult">Output</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <param name="func">Function which appends the new source to the already existing source.</param>
        /// <returns>The return value of the <paramref name="func" /> delegate.</returns>
        public static TResult AppendSource<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
            where TInput : IAudioSource
        {
            return func(input);
        }

        /// <summary>
        ///     Appends a source to an already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <typeparam name="TResult">Output</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <param name="func">Function which appends the new source to the already existing source.</param>
        /// <param name="outputSource">Receives the return value.</param>
        /// <returns>The return value of the <paramref name="func" /> delegate.</returns>
        public static TResult AppendSource<TInput, TResult>(this TInput input, Func<TInput, TResult> func,
            out TResult outputSource)
            where TInput : IAudioSource
        {
            outputSource = func(input);
            return outputSource;
        }

        /// <summary>
        ///     Changes the SampleRate of an already existing wave source.
        /// </summary>
        /// <param name="input">Already existing wave source whose sample rate has to be changed.</param>
        /// <param name="destinationSampleRate">Destination sample rate.</param>
        /// <returns>Wave source with the specified <paramref name="destinationSampleRate" />.</returns>
        public static IWaveSource ChangeSampleRate(this IWaveSource input, int destinationSampleRate)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (destinationSampleRate <= 0)
                throw new ArgumentOutOfRangeException("destinationSampleRate");

            if (input.WaveFormat.SampleRate == destinationSampleRate)
                return input;

            return new DmoResampler(input, destinationSampleRate);
        }

        /// <summary>
        ///     Changes the SampleRate of an already existing sample source. Note: This extension has to convert the
        ///     <paramref name="input" /> to a <see cref="IWaveSource" /> and back to a <see cref="ISampleSource" />.
        /// </summary>
        /// <param name="input">Already existing sample source whose sample rate has to be changed.</param>
        /// <param name="destinationSampleRate">Destination sample rate.</param>
        /// <returns>Sample source with the specified <paramref name="destinationSampleRate" />.</returns>
        public static ISampleSource ChangeSampleRate(this ISampleSource input, int destinationSampleRate)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (destinationSampleRate <= 0)
                throw new ArgumentOutOfRangeException("destinationSampleRate");

            if (input.WaveFormat.SampleRate == destinationSampleRate)
                return input;

            return new DmoResampler(input.ToWaveSource(), destinationSampleRate).ToSampleSource();
        }


        /// <summary>
        ///     Converts the specified wave source with n channels to a wave source with two channels.
        ///     Note: If the <paramref name="input" /> has only one channel, the <see cref="ToStereo(CSCore.IWaveSource)" />
        ///     extension has to convert the <paramref name="input" /> to a <see cref="ISampleSource" /> and back to a
        ///     <see cref="IWaveSource" />.
        /// </summary>
        /// <param name="input">Already existing wave source.</param>
        /// <returns><see cref="IWaveSource" /> instance with two channels.</returns>
        public static IWaveSource ToStereo(this IWaveSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 2)
                return input;
            if (input.WaveFormat.Channels == 1)
                return new MonoToStereoSource(input.ToSampleSource()).ToWaveSource();

            var format = input.WaveFormat as WaveFormatExtensible;
            if (format != null)
            {
                ChannelMask channelMask = format.ChannelMask;
                ChannelMatrix channelMatrix = ChannelMatrix.GetMatrix(channelMask, ChannelMasks.StereoMask);
                return new DmoChannelResampler(input, channelMatrix);
            }

            //throw new ArgumentException(
            //    "The specified input can't be converted to a stereo source. The input does not provide a WaveFormatExtensible.",
            //    "input");

            Debug.WriteLine("MultiChannel stream with no ChannelMask.");

            WaveFormat waveFormat = (WaveFormat)input.WaveFormat.Clone();
            waveFormat.Channels = 2;
            return new DmoResampler(input, waveFormat);
        }

        /// <summary>
        ///     Converts the specified sample source with n channels to a wave source with two channels.
        ///     Note: If the <paramref name="input" /> has more than two channels, the
        ///     <see cref="ToStereo(CSCore.ISampleSource)" /> extension has to convert the <paramref name="input" /> to a
        ///     <see cref="IWaveSource" /> and back to a <see cref="ISampleSource" />.
        /// </summary>
        /// <param name="input">Already existing sample source.</param>
        /// <returns><see cref="ISampleSource" /> instance with two channels.</returns>
        public static ISampleSource ToStereo(this ISampleSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 2)
                return input;
            if (input.WaveFormat.Channels == 1)
                return new MonoToStereoSource(input);

            return ToStereo(input.ToWaveSource()).ToSampleSource();
        }

        /// <summary>
        ///     Converts the specified wave source with n channels to a wave source with one channel.
        ///     Note: If the <paramref name="input" /> has two channels, the <see cref="ToMono(CSCore.IWaveSource)" /> extension
        ///     has to convert the <paramref name="input" /> to a <see cref="ISampleSource" /> and back to a
        ///     <see cref="IWaveSource" />.
        /// </summary>
        /// <param name="input">Already existing wave source.</param>
        /// <returns><see cref="IWaveSource" /> instance with one channel.</returns>
        public static IWaveSource ToMono(this IWaveSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 1)
                return input;
            if (input.WaveFormat.Channels == 2)
                return new StereoToMonoSource(input.ToSampleSource()).ToWaveSource();

            WaveFormatExtensible format = input.WaveFormat as WaveFormatExtensible;
            if (format != null)
            {
                ChannelMask channelMask = format.ChannelMask;
                ChannelMatrix channelMatrix = ChannelMatrix.GetMatrix(channelMask, ChannelMasks.MonoMask);
                return new DmoChannelResampler(input, channelMatrix);
            }
            
            //throw new ArgumentException(
            //    "The specified input can't be converted to a mono source. The input does not provide a WaveFormatExtensible.",
            //    "input");

            Debug.WriteLine("MultiChannel stream with no ChannelMask.");

            WaveFormat waveFormat = (WaveFormat) input.WaveFormat.Clone();
            waveFormat.Channels = 1;
            return new DmoResampler(input, waveFormat);
        }

        /// <summary>
        ///     Converts the specified sample source with n channels to a wave source with one channel.
        ///     Note: If the <paramref name="input" /> has only one channel, the <see cref="ToMono(CSCore.ISampleSource)" />
        ///     extension has to convert the <paramref name="input" /> to a <see cref="IWaveSource" /> and back to a
        ///     <see cref="ISampleSource" />.
        /// </summary>
        /// <param name="input">Already existing sample source.</param>
        /// <returns><see cref="ISampleSource" /> instance with one channels</returns>
        public static ISampleSource ToMono(this ISampleSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 1)
                return input;
            if (input.WaveFormat.Channels == 2)
                return new StereoToMonoSource(input);

            return ToMono(input.ToWaveSource()).ToSampleSource();
        }

        /// <summary>
        ///     Appends a new instance of the <see cref="LoopStream" /> class to the audio chain.
        /// </summary>
        /// <param name="input">The underlying <see cref="IWaveSource" /> which should be looped.</param>
        /// <returns>The new instance <see cref="LoopStream" /> instance.</returns>
        public static IWaveSource Loop(this IWaveSource input)
        {
            return new LoopStream(input) {EnableLoop = true};
        }

        /// <summary>
        ///     Converts a SampleSource to either a Pcm (8, 16, or 24 bit) or IeeeFloat (32 bit) WaveSource.
        /// </summary>
        /// <param name="sampleSource">Sample source to convert to a wave source.</param>
        /// <param name="bits">Bits per sample.</param>
        /// <returns>Wave source</returns>
        public static IWaveSource ToWaveSource(this ISampleSource sampleSource, int bits)
        {
            if (sampleSource == null)
                throw new ArgumentNullException("sampleSource");

            switch (bits)
            {
                case 8:
                    return new SampleToPcm8(sampleSource);
                case 16:
                    return new SampleToPcm16(sampleSource);
                case 24:
                    return new SampleToPcm24(sampleSource);
                case 32:
                    return new SampleToIeeeFloat32(sampleSource);
                default:
                    throw new ArgumentOutOfRangeException("bits", "Must be 8, 16, 24 or 32 bits.");
            }
        }

        /// <summary>
        ///     Converts a <see cref="IWaveSource"/> to IeeeFloat (32bit) <see cref="IWaveSource"/>.
        /// </summary>
        /// <param name="sampleSource">The <see cref="ISampleSource"/> to convert to a <see cref="IWaveSource"/>.</param>
        /// <returns>The <see cref="IWaveSource"/> wrapped around the specified <paramref name="sampleSource"/>.</returns>
        public static IWaveSource ToWaveSource(this ISampleSource sampleSource)
        {
            if (sampleSource == null)
                throw new ArgumentNullException("sampleSource");

            return new SampleToIeeeFloat32(sampleSource);
        }

        /// <summary>
        ///     Converts a <see cref="IWaveSource"/> to a <see cref="ISampleSource"/>.
        /// </summary>
        /// <param name="waveSource">The <see cref="IWaveSource"/> to convert to a <see cref="ISampleSource"/>.</param>
        /// <returns>The <see cref="ISampleSource"/> wrapped around the specified <paramref name="waveSource"/>.</returns>        
        public static ISampleSource ToSampleSource(this IWaveSource waveSource)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            return WaveToSampleBase.CreateConverter(waveSource);
        }

        /// <summary>
        ///     Returns a thread-safe (synchronized) wrapper around the specified <typeparamref name="TAudioSource" /> object.
        /// </summary>
        /// <param name="audioSource">The <typeparamref name="TAudioSource" /> object to synchronize.</param>
        /// <typeparam name="TAudioSource">Type of the <paramref name="audioSource" /> argument.</typeparam>
        /// <typeparam name="T">The type of the data read by the Read method of the <paramref name="audioSource"/> method.</typeparam>
        /// <returns>A thread-safe wrapper around the specified <typeparamref name="TAudioSource" /> object.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="audioSource" /> is null.</exception>
        public static SynchronizedWaveSource<TAudioSource, T> Synchronized<TAudioSource, T>(this TAudioSource audioSource)
            where TAudioSource : class, IReadableAudioSource<T>
        {
            if (audioSource == null)
                throw new ArgumentNullException("audioSource");

            return new SynchronizedWaveSource<TAudioSource, T>(audioSource);
        }
    }
}