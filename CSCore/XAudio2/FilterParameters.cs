using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Defines filter parameters for a source voice.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FilterParameters
    {
        /// <summary>
        ///     The <see cref="FilterType" />.
        /// </summary>
        public FilterType Type;

        /// <summary>
        ///     Filter radian frequency calculated as (2 * sin(pi * (desired filter cutoff frequency) / sampleRate)).
        ///     The frequency must be greater than or equal to 0 and less than or equal to 1.0f.
        ///     The maximum frequency allowable is equal to the source sound's sample rate divided by
        ///     six which corresponds to the maximum filter radian frequency of 1.
        ///     For example, if a sound's sample rate is 48000 and the desired cutoff frequency is the maximum
        ///     allowable value for that sample rate, 8000, the value for Frequency will be 1.
        /// </summary>
        public float Frequency;

        /// <summary>
        ///     Reciprocal of Q factor. Controls how quickly frequencies beyond Frequency are dampened. Larger values
        ///     result in quicker dampening while smaller values cause dampening to occur more gradually.
        ///     Must be greater than 0 and less than or equal to 1.5f.
        /// </summary>
        public float OneOverQ;
    }
}