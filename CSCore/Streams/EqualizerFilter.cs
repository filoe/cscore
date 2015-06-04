using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSCore.Streams
{
    /// <summary>
    ///     Represents an EqualizerFilter which holds an <see cref="EqualizerChannelFilter" /> for each channel.
    /// </summary>
    [DebuggerDisplay("{AverageFrequency}Hz")]
    public sealed class EqualizerFilter : IEnumerable<KeyValuePair<int, EqualizerChannelFilter>>, IComparable<EqualizerFilter>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EqualizerFilter" /> class.
        /// </summary>
        public EqualizerFilter()
        {
            Filters = new Dictionary<int, EqualizerChannelFilter>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EqualizerFilter" /> class.
        /// </summary>
        /// <param name="channels">The number of channels to use.</param>
        /// <param name="filter">The channel filter which should be used for all channels.</param>
        public EqualizerFilter(int channels, EqualizerChannelFilter filter)
            : this()
        {
            Filters.Add(0, filter);
            for (int c = 1; c < channels; c++)
            {
                Filters.Add(c, (EqualizerChannelFilter) filter.Clone());
            }
        }

        /// <summary>
        ///     Gets all underlying <see cref="EqualizerChannelFilter" /> as a dictionary where the key represents the channel
        ///     index and the value to <see cref="EqualizerChannelFilter" /> itself.
        /// </summary>
        public Dictionary<int, EqualizerChannelFilter> Filters { get; private set; }

        /// <summary>
        ///     Gets the average frequency of all <see cref="Filters" />.
        /// </summary>
        public double AverageFrequency
        {
            get { return (Filters != null && Filters.Count > 0) ? Filters.Average(x => x.Value.Frequency) : 0; }
        }

        /// <summary>
        ///     Gets or sets the average gain value of all <see cref="Filters" />.
        /// </summary>
        /// <remarks>
        ///     When using the setter of the <see cref="AverageGainDB" /> property, the new gain value will be applied to all
        ///     <see cref="Filters" />.
        /// </remarks>
        public double AverageGainDB
        {
            get { return Filters.Average(x => x.Value.GainDB); }
            set { SetGain(value); }
        }

        int IComparable<EqualizerFilter>.CompareTo(EqualizerFilter other)
        {
            if (other == null)
                return 1;

            return AverageFrequency.CompareTo(other.AverageFrequency);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Filters"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the <see cref="Filters"/>.
        /// </returns>
        public IEnumerator<KeyValuePair<int, EqualizerChannelFilter>> GetEnumerator()
        {
            return Filters.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a <see cref="Filters"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the <see cref="Filters"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="EqualizerFilter" /> class.
        /// </summary>
        /// <param name="channels">The number of channels to use.</param>
        /// <param name="sampleRate">The samplerate of the data to process.</param>
        /// <param name="frequency">The frequency of the filter.</param>
        /// <param name="bandWidth">The bandwidth.</param>
        /// <param name="gain">The gain value.</param>
        /// <returns>A new instance of the <see cref="EqualizerFilter" /> class.</returns>
        public static EqualizerFilter CreateFilter(int channels, int sampleRate, double frequency, int bandWidth,
            float gain)
        {
            var result = new EqualizerFilter();
            for (int c = 0; c < channels; c++)
            {
                result.Filters.Add(c, new EqualizerChannelFilter(sampleRate, frequency, bandWidth, gain));
            }

            return result;
        }

        private void SetGain(double gainDB)
        {
            foreach (var c in Filters)
            {
                c.Value.GainDB = gainDB;
            }
        }
    }
}