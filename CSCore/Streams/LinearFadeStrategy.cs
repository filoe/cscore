using System;
using System.Diagnostics;

namespace CSCore.Streams
{
    /// <summary>
    ///     Provides a linear fading algorithm.
    /// </summary>
    public class LinearFadeStrategy : IFadeStrategy
    {
        private volatile float _currentVolume = 1;
        private float _startVolume;
        private volatile float _step;
        private float _targetVolume = 1;

        /// <summary>
        ///     Gets the current volume.
        /// </summary>
        public float CurrentVolume
        {
            get { return _currentVolume; }
        }

        /// <summary>
        ///     Gets the target volume.
        /// </summary>
        public float TargetVolume
        {
            get { return _targetVolume; }
        }

        /// <summary>
        ///     Occurs when the fading process has reached its target volume.
        /// </summary>
        public event EventHandler FadingFinished;

        /// <summary>
        ///     Gets a value which indicates whether the <see cref="LinearFadeStrategy" /> class is fading.
        ///     True means that the <see cref="LinearFadeStrategy" /> class is fading audio data.
        ///     False means that the <see cref="CurrentVolume" /> equals the <see cref="TargetVolume" />.
        /// </summary>
        public bool IsFading { get; private set; }

        /// <summary>
        ///     Gets or sets the sample rate to use.
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        ///     Gets or sets the number of channels.
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        ///     Starts fading <paramref name="from" /> a specified volume <paramref name="to" /> another volume.
        /// </summary>
        /// <param name="from">
        ///     The start volume in the range from 0.0 to 1.0. If no value gets specified, the default volume will be used.
        ///     The default volume is typically 100% or the current volume.
        /// </param>
        /// <param name="to">The target volume in the range from 0.0 to 1.0.</param>
        /// <param name="duration">The duration.</param>
        public void StartFading(float? from, float to, TimeSpan duration)
        {
            StartFading(@from, to, (int) duration.TotalMilliseconds);
        }

        /// <summary>
        ///     Starts fading <paramref name="from" /> a specified volume <paramref name="to" /> another volume.
        /// </summary>
        /// <param name="from">
        ///     The start volume in the range from 0.0 to 1.0. If no value gets specified, the default volume will be used.
        ///     The default volume is typically 100% or the current volume.
        /// </param>
        /// <param name="to">The target volume in the range from 0.0 to 1.0.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        public void StartFading(float? from, float to, double duration)
        {
            if (SampleRate <= 0)
                throw new InvalidOperationException("SampleRate property is not set to a valid value.");
            if (Channels <= 0)
                throw new InvalidOperationException("Channels property it not set to a valid value.");
            if (to < 0 || to > 1)
                throw new ArgumentOutOfRangeException("to");

            if (IsFading)
                StopFadingInternal();

            if (!from.HasValue)
                _startVolume = CurrentVolume;
            else
            {
                if (from.Value < 0 || from.Value > 1)
                    throw new ArgumentOutOfRangeException("from");
                _startVolume = from.Value;
            }

            _targetVolume = to;
            _currentVolume = _startVolume;

            //calculate the step
            var durationInBlocks = (int) (duration / 1000 * SampleRate);
            float delta = _targetVolume - _startVolume;
            _step = delta / durationInBlocks;

            IsFading = true;
        }

        /// <summary>
        ///     Stops the fading.
        /// </summary>
        public void StopFading()
        {
            StopFadingInternal();
            OnFadingFinished();
        }

        /// <summary>
        ///     Applies the fading algorithm to the <paramref name="buffer" />.
        /// </summary>
        /// <param name="buffer">Float-array which contains IEEE-Float samples.</param>
        /// <param name="offset">Zero-based offset of the <paramref name="buffer"/>.</param>
        /// <param name="count">The number of samples, the fading algorithm has to be applied on.</param>
        public void ApplyFading(float[] buffer, int offset, int count)
        {
            if (!IsFading)
            {
                for (int i = offset; i < count; i++)
                {
                    buffer[i] *= _currentVolume;
                }

                return;
            }
            if (IsFading && IsFadingFinished())
            {
                FinalizeFading();
                ApplyFading(buffer, offset, count);

                return;
            }

            int channels = Channels;
            count -= (count % channels);

            int sampleIndex = offset;

            while ((sampleIndex - offset) < count)
            {
                if (channels == 1)
                    buffer[sampleIndex++] *= _currentVolume;
                else if (channels == 2)
                {
                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;
                }
                else if (channels == 3)
                {
                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;

                    buffer[sampleIndex++] *= _currentVolume;
                }
                else if (channels == 4)
                {
                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;

                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;
                }
                else if (channels == 5)
                {
                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;

                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;

                    buffer[sampleIndex++] *= _currentVolume;
                }
                else if (channels == 6)
                {
                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;

                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;

                    buffer[sampleIndex++] *= _currentVolume;
                    buffer[sampleIndex++] *= _currentVolume;
                }
                else
                {
                    for (int i = 0; i < channels; i++)
                    {
                        buffer[sampleIndex++] *= _currentVolume;
                    }
                }

                _currentVolume += _step;
                if (IsFading && IsFadingFinished())
                {
                    FinalizeFading();
                    int c = count - (sampleIndex - offset);
                    if (c > 0)
                        ApplyFading(buffer, sampleIndex, c); //apply the rest

                    sampleIndex += c;
                }
            }
        }

        private bool IsFadingFinished()
        {
            return Math.Abs(_currentVolume - _targetVolume) < 0.00001f;
        }

        private void StopFadingInternal()
        {
            _targetVolume = _currentVolume;
            IsFading = false;
        }

        private void FinalizeFading()
        {
            _currentVolume = _targetVolume;
            IsFading = false;
            OnFadingFinished();
        }

        private void OnFadingFinished()
        {
            EventHandler handler = FadingFinished;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}