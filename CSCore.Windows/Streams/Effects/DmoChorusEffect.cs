using CSCore.DMO.Effects;
using System;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Represents the dmo chorus effect in form of an <see cref="IWaveSource"/> implementation.
    /// </summary>
    public sealed class DmoChorusEffect : DmoEffectBase<DirectSoundFXChorus, ChorusParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoChorusEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoChorusEffect(IWaveSource source)
            : base(source)
        {
        }

        /// <summary>
        /// Creates and returns a new instance of the native COM object.
        /// </summary>
        /// <returns>A new instance of the native COM object.</returns>
        protected override object CreateComObject()
        {
            return new DmoChorusEffectObject();
        }

        [ComImport]
        [Guid("efe6629c-81f7-4281-bd91-c9d604a95af6")]
        private sealed class DmoChorusEffectObject
        {
        }

        #region properties
        /// <summary>
        /// Gets or sets the number of milliseconds the input is delayed before it is played back, in the range from 0 to 20. The default value is 16 ms.
        /// </summary>
        public float Delay
        {
            get { return Effect.Parameters.Delay; }
            set
            {
                if (value < DelayMin || value > DelayMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Delay", value);
            }
        }

        /// <summary>
        /// Gets or sets the percentage by which the delay time is modulated by the low-frequency oscillator, in hundredths of a percentage point. Must be in the range from 0 through 100. The default value is 10.
        /// </summary>
        public float Depth
        {
            get { return Effect.Parameters.Depth; }
            set
            {
                if (value < DepthMin || value > DepthMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Depth", value);
            }
        }

        /// <summary>
        /// Gets or sets the percentage of output signal to feed back into the effect's input, in the range from -99 to 99. The default value is 25.
        /// </summary>
        public float Feedback
        {
            get { return Effect.Parameters.Feedback; }
            set
            {
                if (value < FeedbackMin || value > FeedbackMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Feedback", value);
            }
        }

        /// <summary>
        /// Gets or sets the frequency of the LFO, in the range from 0 to 10. The default value is 1.1.
        /// </summary>
        public float Frequency
        {
            get { return Effect.Parameters.Frequency; }
            set
            {
                if (value < FrequencyMin || value > FrequencyMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Frequency", value);
            }
        }

        /// <summary>
        /// Gets or sets the waveform shape of the LFO. By default, the waveform is a sine.
        /// </summary>
        public ChorusWaveform Waveform
        {
            get { return (ChorusWaveform)Effect.Parameters.Waveform; }
            set
            {
                SetValue("Waveform", (int)value);
            }
        }

        /// <summary>
        /// Gets or sets the phase differential between left and right LFOs. The default value is Phase90.
        /// </summary>
        public ChorusPhase Phase
        {
            get { return (ChorusPhase)Effect.Parameters.Phase; }
            set
            {
                SetValue("Phase", (int)value);
            }
        }

        /// <summary>
        /// Gets or sets the ratio of wet (processed) signal to dry (unprocessed) signal. Must be in the range from 0 through 100 (all wet). The default value is 50.
        /// </summary>
        public float WetDryMix
        {
            get { return Effect.Parameters.WetDryMix; }
            set
            {
                if (value < WetDryMixMin || value > WetDryMixMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("WetDryMix", value);
            }
        }

        #endregion

        #region contants
        /// <summary>
        /// Default value for the <see cref="Delay"/> property.
        /// </summary>
        public const float DelayDefault = 16f;
        /// <summary>
        /// Maximum value for the <see cref="Delay"/> property.
        /// </summary>
        public const float DelayMax = 20f;
        /// <summary>
        /// Minimum value for the <see cref="Delay"/> property.
        /// </summary>
        public const float DelayMin = 0f;

        /// <summary>
        /// Default value for the <see cref="Depth"/> property.
        /// </summary>
        public const float DepthDefault = 10f;
        /// <summary>
        /// Maximum value for the <see cref="Depth"/> property.
        /// </summary>
        public const float DepthMax = 100f;
        /// <summary>
        /// Minimum value for the <see cref="Depth"/> property.
        /// </summary>
        public const float DepthMin = 0f;

        /// <summary>
        /// Default value for the <see cref="Feedback"/> property.
        /// </summary>
        public const float FeedbackDefault = 25f;
        /// <summary>
        /// Maximum value for the <see cref="Feedback"/> property.
        /// </summary>
        public const float FeedbackMax = 99f;
        /// <summary>
        /// Minimum value for the <see cref="Feedback"/> property.
        /// </summary>
        public const float FeedbackMin = -99f;

        /// <summary>
        /// Default value for the <see cref="Frequency"/> property.
        /// </summary>
        public const float FrequencyDefault = 1.1f;
        /// <summary>
        /// Maximum value for the <see cref="Frequency"/> property.
        /// </summary>
        public const float FrequencyMax = 10f;
        /// <summary>
        /// Minimum value for the <see cref="Frequency"/> property.
        /// </summary>
        public const float FrequencyMin = 0f;

        /// <summary>
        /// 180° Phase
        /// </summary>
        public const int Phase180 = 4;
        /// <summary>
        /// 90° Phase
        /// </summary>
        public const int Phase90 = 3;
        /// <summary>
        /// Default value for the <see cref="Phase"/> property.
        /// </summary>
        public const int PhaseDefault = 3;
        /// <summary>
        /// Maximum value for the <see cref="Phase"/> property.
        /// </summary>
        public const int PhaseMax = 4;
        /// <summary>
        /// Minimum value for the <see cref="Phase"/> property.
        /// </summary>
        public const int PhaseMin = 0;
        /// <summary>
        /// -180° Phase
        /// </summary>
        public const int PhaseNegative180 = 0;
        /// <summary>
        /// -90° Phase
        /// </summary>
        public const int PhaseNegative90 = 1;
        /// <summary>
        /// 0° Phase
        /// </summary>
        public const int PhaseZero = 2;

        /// <summary>
        /// Default value for the <see cref="Waveform"/> property.
        /// </summary>
        public const int WaveformDefault = 1;
        /// <summary>
        /// Sine waveform
        /// </summary>
        public const int WaveformSin = 1;
        /// <summary>
        /// Triangle waveform
        /// </summary>
        public const int WaveformTriangle = 0;

        /// <summary>
        /// Default value for the <see cref="WetDryMix"/> property.
        /// </summary>
        public const float WetDryMixDefault = 50f;
        /// <summary>
        /// Maximum value for the <see cref="WetDryMix"/> property.
        /// </summary>
        public const float WetDryMixMax = 100f;
        /// <summary>
        /// Minimum value for the <see cref="WetDryMix"/> property.
        /// </summary>
        public const float WetDryMixMin = 0f;
        #endregion
    }
}
