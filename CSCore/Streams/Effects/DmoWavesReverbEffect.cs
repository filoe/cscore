using CSCore.DMO.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// WavesReverb Effect.
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
        /// Input gain of signal, in decibels (dB), in the range from -96 dB through 0 dB. The default value is 0 dB.
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
        /// Reverb mix, in dB, in the range from -96 dB through 0 dB. The default value is 0 dB.
        /// </summary>
        public float ReverbMix
        {
            get { return Effect.Parameters.ReverbMix; }
            set
            {
                if (value < ReverbMixMin || value > ReverbMixMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("ReverbMix", value);
            }
        }

        /// <summary>
        /// Reverb time, in milliseconds, in the range from 0.001 through 3000. The default value is 1000.
        /// </summary>
        public float ReverbTime
        {
            get { return Effect.Parameters.ReverbTime; }
            set
            {
                if (value < ReverbTimeMin || value > ReverbTimeMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("ReverbTime", value);
            }
        }

        /// <summary>
        /// High-frequency reverb time ratio, in the range from 0.001 through 0.999. The default value is 0.001.
        /// </summary>
        public float HighFrequencyRTRatio
        {
            get { return Effect.Parameters.HighFreqRTRatio; }
            set
            {
                if (value < HighFrequencyRTRatioMin || value > HighFrequencyRTRatioMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("HighFreqRTRatio", value);
            }
        }
        #endregion
        #region constants
        public const float HighFrequencyRTRatioDefault = 0.001f;
        public const float HighFrequencyRTRatioMax = 0.999f;
        public const float HighFrequencyRTRatioMin = 0.001f;
        public const float InGainDefault = 0f;
        public const float InGainMax = 0f;
        public const float InGainMin = -96f;
        public const float ReverbMixDefault = 0f;
        public const float ReverbMixMax = 0f;
        public const float ReverbMixMin = -96f;
        public const float ReverbTimeDefault = 1000f;
        public const float ReverbTimeMax = 3000f;
        public const float ReverbTimeMin = 0.001f;
        #endregion
    }
}
