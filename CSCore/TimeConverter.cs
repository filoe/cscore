using System;

namespace CSCore
{
    /// <summary>
    /// Defines a base class for all time converts. A time converter can be used to convert raw positions (depending on the implementation i.e. bytes or samples) to a human
    /// readable <see cref="TimeSpan"/>.
    /// </summary>
    public abstract class TimeConverter
    {
        /// <summary>
        /// A <see cref="TimeConverter"/> for <see cref="ISampleSource"/> objects.
        /// </summary>
        public static readonly TimeConverter SampleSourceTimeConverter = new _SampleSourceTimeConverter();
        /// <summary>
        /// A <see cref="TimeConverter"/> for <see cref="IWaveSource"/> objects.
        /// </summary>
        public static readonly TimeConverter WaveSourceTimeConverter = new _WaveSourceTimeConverter();

        /// <summary>
        /// Converts a <see cref="TimeSpan"/> back to raw elements, a source works with. The unit of these raw elements depends on the implementation. For more information, see <see cref="TimeConverter"/>.
        /// </summary>
        /// <param name="waveFormat">The <see cref="WaveFormat"/> of the source which gets used to convert the <paramref name="timeSpan"/>.</param>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> to convert to raw elements.</param>
        /// <returns>The converted <see cref="TimeSpan"/> in raw elements.</returns>
        public abstract long ToRawElements(WaveFormat waveFormat, TimeSpan timeSpan);

        /// <summary>
        /// Converts raw elements to a <see cref="TimeSpan"/> value. The unit of these raw elements depends on the implementation. For more information, see <see cref="TimeConverter"/>.
        /// </summary>
        /// <param name="waveFormat">The <see cref="WaveFormat"/> of the source which gets used to convert the <paramref name="rawElements"/>. </param>
        /// <param name="rawElements">The raw elements to convert to a <see cref="TimeSpan"/>.</param>
        /// <returns>The <see cref="TimeSpan"/>.</returns>
        public abstract TimeSpan ToTimeSpan(WaveFormat waveFormat, long rawElements);

        internal class _WaveSourceTimeConverter : TimeConverter
        {
            public override long ToRawElements(WaveFormat waveFormat, TimeSpan timeSpan)
            {
                return waveFormat.MillisecondsToBytes(timeSpan.TotalMilliseconds);
            }

            public override TimeSpan ToTimeSpan(WaveFormat waveFormat, long rawElements)
            {
                return TimeSpan.FromMilliseconds(waveFormat.BytesToMilliseconds(rawElements));
            }
        }

        internal class _SampleSourceTimeConverter : TimeConverter
        {
            public override long ToRawElements(WaveFormat waveFormat, TimeSpan timeSpan)
            {
                return waveFormat.MillisecondsToBytes(timeSpan.TotalMilliseconds) / waveFormat.BytesPerSample;
            }

            public override TimeSpan ToTimeSpan(WaveFormat waveFormat, long rawElements)
            {
                return TimeSpan.FromMilliseconds(waveFormat.BytesToMilliseconds(rawElements * waveFormat.BytesPerSample));
            }
        }
    }
}