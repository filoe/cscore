// ReSharper disable InconsistentNaming

using CSCore.DMO.Effects;
using System;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Represents the dmo waves reverb effect in form of an <see cref="IWaveSource"/> implementation.
    /// </summary>
    public class DmoWavesReverbEffect : DmoEffectBase<DirectSoundFXWavesReverb, WavesReverbParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoWavesReverbEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoWavesReverbEffect(IWaveSource source)
            : base(source)
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Creates and returns a new instance of the native COM object.
        /// </summary>
        /// <returns>A new instance of the native COM object.</returns>
        protected override object CreateComObject()
        {
            return new DmoWavesReverbEffectObject();
        }

        [ComImport]
        [Guid("87fc0268-9a55-4360-95aa-004a1d9de26c")]
        private sealed class DmoWavesReverbEffectObject
        {
        }

        #region properties
        /// <summary>
        /// Gets or sets the input gain of signal, in decibels (dB), in the range from -96 dB through 0 dB. The default value is 0 dB.
        /// </summary>
        public float InGain
        {
            get { return Effect.Parameters.InGain; }
            set
            {
                if (value < InGainMin || value > InGainMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("InGain", value);
            }
        }

        /// <summary>
        /// Gets or sets the reverb mix, in dB, in the range from -96 dB through 0 dB. The default value is 0 dB.
        /// </summary>
        public float ReverbMix
        {
            get { return Effect.Parameters.ReverbMix; }
            set
            {
                //if (value < ReverbMixMax || value > ReverbMixMin) -> for some reason these values are incorrect...
                //    throw new ArgumentOutOfRangeException("value");
                SetValue("ReverbMix", value);
            }
        }

        /// <summary>
        /// Gets or sets the reverb time, in milliseconds, in the range from 0.001 through 3000. The default value is 1000.
        /// </summary>
        public float ReverbTime
        {
            get { return Effect.Parameters.ReverbTime; }
            set
            {
                //if (value < ReverbTimeMin || value > ReverbTimeMax)
                //    throw new ArgumentOutOfRangeException("value");
                SetValue("ReverbTime", value);
            }
        }

        /// <summary>
        /// Gets or sets the high-frequency reverb time ratio, in the range from 0.001 through 0.999. The default value is 0.001.
        /// </summary>
        public float HighFrequencyRTRatio
        {
            get { return Effect.Parameters.HighFreqRTRatio; }
            set
            {
                //if (value < HighFrequencyRTRatioMin || value > HighFrequencyRTRatioMax) -> for some reason these values are incorrect...
                //    throw new ArgumentOutOfRangeException("value");
                SetValue("HighFreqRTRatio", value);
            }
        }
        #endregion
        #region constants
        /// <summary>
        /// Default value for the <see cref="HighFrequencyRTRatio"/> property.
        /// </summary>
        public const float HighFrequencyRTRatioDefault = 0.001f;
        /// <summary>
        /// Maximum value for the <see cref="HighFrequencyRTRatio"/> property.
        /// </summary>
        public const float HighFrequencyRTRatioMax = 0.999f;
        /// <summary>
        /// Minimum value for the <see cref="HighFrequencyRTRatio"/> property.
        /// </summary>
        public const float HighFrequencyRTRatioMin = 0.001f;

        /// <summary>
        /// Default value for the <see cref="InGain"/> property.
        /// </summary>
        public const float InGainDefault = 0f;
        /// <summary>
        /// Maximum value for the <see cref="InGain"/> property.
        /// </summary>
        public const float InGainMax = 0f;
        /// <summary>
        /// Minimum value for the <see cref="InGain"/> property.
        /// </summary>
        public const float InGainMin = -96f;

        /// <summary>
        /// Default value for the <see cref="ReverbMix"/> property.
        /// </summary>
        public const float ReverbMixDefault = 0f;
        /// <summary>
        /// Maximum value for the <see cref="ReverbMix"/> property.
        /// </summary>
        public const float ReverbMixMax = 0f;
        /// <summary>
        /// Minimum value for the <see cref="ReverbMix"/> property.
        /// </summary>
        public const float ReverbMixMin = -96f;

        /// <summary>
        /// Default value for the <see cref="ReverbTime"/> property.
        /// </summary>
        public const float ReverbTimeDefault = 1000f;
        /// <summary>
        /// Maximum value for the <see cref="ReverbTime"/> property.
        /// </summary>
        public const float ReverbTimeMax = 3000f;
        /// <summary>
        /// Minimum value for the <see cref="ReverbTime"/> property.
        /// </summary>
        public const float ReverbTimeMin = 0.001f;
        #endregion
    }
}
// ReSharper restore InconsistentNaming
