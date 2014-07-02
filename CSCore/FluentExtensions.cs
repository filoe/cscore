using System;
using CSCore.DSP;

namespace CSCore
{
    /// <summary>
    ///     Provides a basic fluent API for creating a source chain.
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        ///     Appends a Source to a already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <typeparam name="TResult">Output</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <param name="func">Function which appends the new source to the already existing source.</param>
        public static TResult AppendSource<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
            where TInput : IWaveStream
        {
            return func(input);
        }

        /// <summary>
        ///     Appends a Source to a already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <typeparam name="TResult">Output</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <param name="func">Function which appends the new source to the already existing source.</param>
        /// <param name="outputSource">Receives the return value.</param>
        public static TResult AppendSource<TInput, TResult>(this TInput input, out TResult outputSource,
            Func<TInput, TResult> func)
            where TInput : IWaveStream
        {
            outputSource = func(input);
            return outputSource;
        }

        /// <summary>
        ///     Changes the SampleRate of a already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <param name="input">Already existing source which sample rate has to be changed.</param>
        /// <param name="destSampleRate">Destination sample rate.</param>
        public static IWaveSource ChangeSampleRate<TInput>(this TInput input, int destSampleRate)
            where TInput : class, IWaveStream
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (destSampleRate <= 0)
                throw new ArgumentOutOfRangeException("destSampleRate");

            var source = input as IWaveSource;
            if (source == null && input is ISampleSource)
                source = ((ISampleSource) input).ToWaveSource();
            else
                throw new ArgumentException("Not supported input type.", "input");

            return new DmoResampler(source, destSampleRate);
        }
    }
}