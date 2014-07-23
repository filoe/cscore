using System;
using CSCore.DSP;
using CSCore.Streams;

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
        /// <returns>The return value of the <paramref name="func"/> delegate.</returns>
        public static TResult AppendSource<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
            where TInput : IWaveStream
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
        /// <returns>The return value of the <paramref name="func"/> delegate.</returns>
        public static TResult AppendSource<TInput, TResult>(this TInput input, Func<TInput, TResult> func, out TResult outputSource)
            where TInput : IWaveStream
        {
            outputSource = func(input);
            return outputSource;
        }

        /// <summary>
        ///     Changes the SampleRate of an already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <param name="input">Already existing source whose sample rate has to be changed.</param>
        /// <param name="destSampleRate">Destination sample rate.</param>
        /// <returns>Instance of the <see cref="DmoResampler"/> which resamples the specified <paramref name="input"/> source.</returns>
        public static IWaveSource ChangeSampleRate<TInput>(this TInput input, int destSampleRate)
            where TInput : class, IWaveStream
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (destSampleRate <= 0)
                throw new ArgumentOutOfRangeException("destSampleRate");

            var source = input as IWaveSource;
            if (source == null)
            {
                if (input is ISampleSource)
                    source = ((ISampleSource) input).ToWaveSource();
                else
                    throw new ArgumentException("Not supported input type.", "input");
            }

            if (input.WaveFormat.SampleRate == destSampleRate)
                return source;

            return new DmoResampler(source, destSampleRate);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> source to a stereo source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <returns><see cref="IWaveSource"/> instance with two channels.</returns>
        public static IWaveSource ToStereo<TInput>(this TInput input)
            where TInput : class, IWaveStream
        {
            if(input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 2)
            {
                if (input is IWaveSource)
                    return (IWaveSource) input;
                if (input is ISampleSource)
                    return ((ISampleSource) input).ToWaveSource();
                throw new ArgumentException("Unknown input type.", "input");
            }
            if (input.WaveFormat.Channels == 1)
            {
                return new MonoToStereoSource(input).ToWaveSource();
            }

            IWaveSource waveSource = input as IWaveSource;
            if (waveSource == null && input is ISampleSource)
                waveSource = ((ISampleSource) input).ToWaveSource();
            else
                throw new ArgumentException("Unknown input type.", "input");

            var dstWaveFormat = (WaveFormat)input.WaveFormat.Clone();
            dstWaveFormat.Channels = 2;
            return new DmoResampler(waveSource, dstWaveFormat);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> source to a mono source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <returns><see cref="IWaveSource"/> instance with one channel.</returns>
        public static IWaveSource ToMono<TInput>(this TInput input)
            where TInput : class, IWaveStream
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 1)
            {
                if (input is IWaveSource)
                    return (IWaveSource)input;
                if (input is ISampleSource)
                    return ((ISampleSource)input).ToWaveSource();
                throw new ArgumentException("Unknown input type.", "input");
            }
            if (input.WaveFormat.Channels == 2)
            {
                return new StereoToMonoSource(input).ToWaveSource();
            }

            IWaveSource waveSource = input as IWaveSource;
            if (waveSource == null && input is ISampleSource)
                waveSource = ((ISampleSource)input).ToWaveSource();
            else
                throw new ArgumentException("Unknown input type.", "input");

            var dstWaveFormat = (WaveFormat)input.WaveFormat.Clone();
            dstWaveFormat.Channels = 1;
            return new DmoResampler(waveSource, dstWaveFormat);
        }
    }
}