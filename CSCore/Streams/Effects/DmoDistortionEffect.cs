// ReSharper disable InconsistentNaming -> disable "EQ"-warnings
using CSCore.DMO.Effects;
using System;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Represents the dmo distortion effect in form of an <see cref="IWaveSource"/> implementation.
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

        /// <summary>
        /// Creates and returns a new instance of the native COM object.
        /// </summary>
        /// <returns>A new instance of the native COM object.</returns>
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
        /// Gets or sets the amount of signal change after distortion, in the range from -60 dB through 0 dB. The default value is -18 dB.
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
        /// Gets or sets the percentage of distortion intensity, in the range in the range from 0 % through 100 %. The default value is 15 percent.
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
        /// Gets or sets the center frequency of harmonic content addition, in the range from 100 Hz through 8000 Hz. The default value is 2400 Hz.
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
        /// Gets or sets the width of frequency band that determines range of harmonic content addition, in the range from 100 Hz through 8000 Hz. The default value is 2400 Hz.
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
        /// Gets or sets the filter cutoff for high-frequency harmonics attenuation, in the range from 100 Hz through 8000 Hz. The default value is 8000 Hz.
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
        /// <summary>
        /// Default value for the <see cref="Edge"/> property.
        /// </summary>
        public const float EdgeDefault = 15f;
        /// <summary>
        /// Maximum value for the <see cref="Edge"/> property.
        /// </summary>
        public const float EdgeMax = 100f;
        /// <summary>
        /// Minimum value for the <see cref="Edge"/> property.
        /// </summary>
        public const float EdgeMin = 0f;

        /// <summary>
        /// Default value for the <see cref="Gain"/> property.
        /// </summary>
        public const float GainDefault = -18f;
        /// <summary>
        /// Maximum value for the <see cref="Gain"/> property.
        /// </summary>
        public const float GainMax = 0f;
        /// <summary>
        /// Minimum value for the <see cref="Gain"/> property.
        /// </summary>
        public const float GainMin = -60f;

        /// <summary>
        /// Default value for the <see cref="PostEQBandwidth"/> property.
        /// </summary>
        public const float PostEQBandwidthDefault = 2400f;
        /// <summary>
        /// Maximum value for the <see cref="PostEQBandwidth"/> property.
        /// </summary>
        public const float PostEQBandwidthMax = 8000f;
        /// <summary>
        /// Minimum value for the <see cref="PostEQBandwidth"/> property.
        /// </summary>
        public const float PostEQBandwidthMin = 100f;

        /// <summary>
        /// Default value for the <see cref="PostEQCenterFrequency"/> property.
        /// </summary>
        public const float PostEQCenterFrequencyDefault = 2400f;
        /// <summary>
        /// Maximum value for the <see cref="PostEQCenterFrequency"/> property.
        /// </summary>
        public const float PostEQCenterFrequencyMax = 8000f;
        /// <summary>
        /// Minimum value for the <see cref="PostEQCenterFrequency"/> property.
        /// </summary>
        public const float PostEQCenterFrequencyMin = 100f;

        /// <summary>
        /// Default value for the <see cref="PreLowpassCutoff"/> property.
        /// </summary>
        public const float PreLowPassCutoffDefault = 8000f;
        /// <summary>
        /// Maximum value for the <see cref="PreLowpassCutoff"/> property.
        /// </summary>
        public const float PreLowPassCutoffMax = 8000f;
        /// <summary>
        /// Minimum value for the <see cref="PreLowpassCutoff"/> property.
        /// </summary>
        public const float PreLowPassCutoffMin = 100f;
        #endregion
    }
}
// ReSharper restore InconsistentNaming
