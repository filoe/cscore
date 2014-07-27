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
        ///     Changes the SampleRate of an already existing wave source.
        /// </summary>
        /// <param name="input">Already existing wave source whose sample rate has to be changed.</param>
        /// <param name="destinationSampleRate">Destination sample rate.</param>
        /// <returns>Wave source with the specified <paramref name="destinationSampleRate"/>.</returns>
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
        ///     Changes the SampleRate of an already existing sample source. Note: This extension has to convert the <paramref name="input"/> to a <see cref="IWaveSource"/> and back to a <see cref="ISampleSource"/>.
        /// </summary>
        /// <param name="input">Already existing sample source whose sample rate has to be changed.</param>
        /// <param name="destinationSampleRate">Destination sample rate.</param>
        /// <returns>Sample source with the specified <paramref name="destinationSampleRate"/>.</returns>
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
        /// Converts the specified wave source with n channels to a wave source with two channels.
        /// Note: If the <see cref="input"/> has only one channel, the <see cref="ToStereo(CSCore.IWaveSource)"/> extension has to convert the <paramref name="input"/> to a <see cref="ISampleSource"/> and back to a <see cref="IWaveSource"/>.        
        /// </summary>
        /// <param name="input">Already existing wave source.</param>
        /// <returns><see cref="IWaveSource"/> instance with two channels.</returns>
        public static IWaveSource ToStereo(this IWaveSource input)
        {
            if(input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 2)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1)
            {
                return new MonoToStereoSource(input).ToWaveSource();
            }

            var dstWaveFormat = (WaveFormat)input.WaveFormat.Clone();
            dstWaveFormat.Channels = 2;
            return new DmoResampler(input, dstWaveFormat);
        }

        /// <summary>
        /// Converts the specified sample source with n channels to a wave source with two channels.
        /// Note: If the <see cref="input"/> has more than two channels, the <see cref="ToStereo(CSCore.ISampleSource)"/> extension has to convert the <paramref name="input"/> to a <see cref="IWaveSource"/> and back to a <see cref="ISampleSource"/>.
        /// </summary>
        /// <param name="input">Already existing sample source.</param>
        /// <returns><see cref="ISampleSource"/> instance with two channels.</returns>
        public static ISampleSource ToStereo(this ISampleSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 2)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1)
            {
                return new MonoToStereoSource(input);
            }

            var dstWaveFormat = (WaveFormat)input.WaveFormat.Clone();
            dstWaveFormat.Channels = 2;
            return new DmoResampler(input.ToWaveSource(), dstWaveFormat).ToSampleSource();
        }

        /// <summary>
        /// Converts the specified wave source with n channels to a wave source with one channel.
        /// Note: If the <see cref="input"/> has two channels, the <see cref="ToMono(CSCore.IWaveSource)"/> extension has to convert the <paramref name="input"/> to a <see cref="ISampleSource"/> and back to a <see cref="IWaveSource"/>.        
        /// </summary>
        /// <param name="input">Already existing wave source.</param>
        /// <returns><see cref="IWaveSource"/> instance with one channel.</returns>
        public static IWaveSource ToMono(this IWaveSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 1)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 2)
            {
                return new StereoToMonoSource(input).ToWaveSource();
            }

            var dstWaveFormat = (WaveFormat)input.WaveFormat.Clone();
            dstWaveFormat.Channels = 1;
            return new DmoResampler(input, dstWaveFormat);
        }

        /// <summary>
        /// Converts the specified sample source with n channels to a wave source with one channel.
        /// Note: If the <see cref="input"/> has only one channel, the <see cref="ToMono(CSCore.ISampleSource)"/> extension has to convert the <paramref name="input"/> to a <see cref="IWaveSource"/> and back to a <see cref="ISampleSource"/>.
        /// </summary>
        /// <param name="input">Already existing sample source.</param>
        /// <returns><see cref="ISampleSource"/> instance with one channels</returns>
        public static ISampleSource ToMono(this ISampleSource input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.WaveFormat.Channels == 1)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 2)
            {
                return new StereoToMonoSource(input);
            }

            var dstWaveFormat = (WaveFormat)input.WaveFormat.Clone();
            dstWaveFormat.Channels = 1;
            return new DmoResampler(input.ToWaveSource(), dstWaveFormat).ToSampleSource();
        }
    }
}