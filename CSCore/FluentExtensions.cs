using System;

namespace CSCore
{
    /// <summary>
    /// Provides a basic fluent API for creating a source chain.
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        /// Appends a Source to a already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <typeparam name="TResult">Output</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <param name="func">Function which appends the new source to the already existing source.</param>
        public static TResult AppendSource<TInput, TResult>(this TInput input, Func<TInput, TResult> func)
        {
            return func(input);
        }

        /// <summary>
        /// Appends a Source to a already existing source.
        /// </summary>
        /// <typeparam name="TInput">Input</typeparam>
        /// <typeparam name="TResult">Output</typeparam>
        /// <param name="input">Already existing source.</param>
        /// <param name="func">Function which appends the new source to the already existing source.</param>
        /// <param name="outputSource">Receives the return value.</param>
        public static TResult AppendSource<TInput, TResult>(this TInput input, out TResult outputSource, Func<TInput, TResult> func)
        {
            outputSource = func(input);
            return outputSource;
        }
    }
}
