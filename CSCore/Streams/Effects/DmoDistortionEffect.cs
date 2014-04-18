using CSCore.DMO.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// DistortionEffect.
    /// </summary>
    public class DmoDistortionEffect : DmoEffectBase<DirectSoundFXDistortion, DistortionParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoDistortionEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoDistortionEffect(IWaveSource source)
            : base(source)
        {
        }

        protected override object CreateComObject()
        {
            return new DmoDistortionEffectObject();
        }

        [ComImport]
        [Guid("ef114c90-cd1d-484e-96e5-09cfaf912a21")]
        private sealed class DmoDistortionEffectObject
        {
        }

        #region properties
        /// <summary>
        /// Amount of signal change after distortion, in the range from -60 dB through 0 dB. The default value is -18 dB.
        /// </summary>
        public float Gain
        {
            get { return Effect.Parameters.Gain; }
            set
            {
                if (value < GainMin || value > GainMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Gain", value);
            }
        }

        /// <summary>
        /// Percentage of distortion intensity, in the range in the range from 0 % through 100 %. The default value is 15 percent.
        /// </summary>
        public float Edge
        {
            get { return Effect.Parameters.Edge; }
            set
            {
                if (value < EdgeMin || value > EdgeMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Edge", value);
            }
        }

        /// <summary>
        /// Center frequency of harmonic content addition, in the range from 100 Hz through 8000 Hz. The default value is 2400 Hz.
        /// </summary>
        public float PostEQCenterFrequency
        {
            get { return Effect.Parameters.PostEQCenterFrequency; }
            set
            {
                if (value < PostEQBandwidthMin || value > PostEQBandwidthMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("PostEQCenterFrequency", value);
            }
        }

        /// <summary>
        /// Width of frequency band that determines range of harmonic content addition, in the range from 100 Hz through 8000 Hz. The default value is 2400 Hz.
        /// </summary>
        public float PostEQBandwidth
        {
            get { return Effect.Parameters.PostEQBandwidth; }
            set
            {
                if (value < PostEQBandwidthMin || value > PostEQBandwidthMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("PostEQBandwidth", value);
            }
        }

        /// <summary>
        /// Filter cutoff for high-frequency harmonics attenuation, in the range from 100 Hz through 8000 Hz. The default value is 8000 Hz.
        /// </summary>
        public float PreLowpassCutoff
        {
            get { return Effect.Parameters.PreLowpassCutoff; }
            set 
            {
                if (value < PreLowPassCutoffMin || value > PreLowPassCutoffMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("PreLowpassCutoff", value);
            }
        }
        #endregion
        #region constants
        public const float EdgeDefault = 15f;
        public const float EdgeMax = 100f;
        public const float EdgeMin = 0f;
        public const float GainDefault = -18f;
        public const float GainMax = 0f;
        public const float GainMin = -60f;
        public const float PostEQBandwidthDefault = 2400f;
        public const float PostEQBandwidthMax = 8000f;
        public const float PostEQBandwidthMin = 100f;
        public const float PostEQCenterFrequencyDefault = 2400f;
        public const float PostEQCenterFrequencyMax = 8000f;
        public const float PostEQCenterFrequencyMin = 100f;
        public const float PreLowPassCutoffDefault = 8000f;
        public const float PreLowPassCutoffMax = 8000f;
        public const float PreLowPassCutoffMin = 100f;
        #endregion
    }
}
