using System;

namespace CSCore.Streams
{
    /// <summary>
    ///     Provides a mechanism for fading in/out audio.
    /// </summary>
    /// <remarks>
    ///     The <see cref="SampleRate" />- and the <see cref="Channels" />-property must be set before the
    ///     <see cref="IFadeStrategy" /> can be used.
    /// </remarks>
    public interface IFadeStrategy
    {
        /// <summary>
        ///     Gets a value which indicates whether the current volume equals the target volume. If not, the
        ///     <see cref="IsFading" /> property returns false.
        /// </summary>
        bool IsFading { get; }

        /// <summary>
        ///     Gets or sets the sample rate to use.
        /// </summary>
        int SampleRate { get; set; }

        /// <summary>
        ///     Gets or sets the number of channels.
        /// </summary>
        int Channels { get; set; }

        /// <summary>
        ///     Occurs when the fading process has reached its target volume.
        /// </summary>
        event EventHandler FadingFinished;

        /// <summary>
        ///     Applies the fading algorithm to the waveform-audio data.
        /// </summary>
        /// <param name="buffer">Float-array which contains IEEE-Float samples.</param>
        /// <param name="offset">Zero-based offset of the <paramref name="buffer" />.</param>
        /// <param name="count">The number of samples, the fading algorithm has to be applied on.</param>
        void ApplyFading(float[] buffer, int offset, int count);

        /// <summary>
        ///     Starts fading <paramref name="from" /> a specified volume <paramref name="to" /> another volume.
        /// </summary>
        /// <param name="from">
        ///     The start volume in the range from 0.0 to 1.0. If no value gets specified, the default volume will be used.
        ///     The default volume is typically 100% or the current volume.
        /// </param>
        /// <param name="to">The target volume in the range from 0.0 to 1.0.</param>
        /// <param name="duration">The duration.</param>
        void StartFading(float? from, float to, TimeSpan duration);

        /// <summary>
        ///     Starts fading <paramref name="from" /> a specified volume <paramref name="to" /> another volume.
        /// </summary>
        /// <param name="from">
        ///     The start volume in the range from 0.0 to 1.0. If no value gets specified, the default volume will be used.
        ///     The default volume is typically 100% or the current volume.
        /// </param>
        /// <param name="to">The target volume in the range from 0.0 to 1.0.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        void StartFading(float? @from, float to, double duration);

        /// <summary>
        ///     Stops the fading.
        /// </summary>
        void StopFading();
    }
}