using CSCore.DMO.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Flanger Effect.
    /// </summary>
    public class DmoFlangerEffect : DmoEffectBase<DirectSoundFXFlanger, FlangerParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoFlangerEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoFlangerEffect(IWaveSource source)
            : base(source)
        {
        }

        protected override object CreateComObject()
        {
            return new DmoFlangerEffectObject();
        }

        [ComImport]
        [Guid("efca3d92-dfd8-4672-a603-7420894bad98")]
        private sealed class DmoFlangerEffectObject
        {
        }

        #region properties
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

        /// <summary>
        /// Gets or sets the percentage by which the delay time is modulated by the low-frequency oscillator (LFO), in hundredths of a percentage point. Must be in the range from 0 through 100. The default value is 100.
        /// </summary>
        public float Depth
        {
            get { return Effect.Parameters.Delay; }
            set
            {
                if (value < DepthMin || value > DepthMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Depth", value);
            }
        }

        /// <summary>
        /// Gets or sets the percentage of output signal to feed back into the effect's input, in the range from -99 to 99. The default value is -50.
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
        /// Gets or sets the frequency of the LFO, in the range from 0 to 10. The default value is 0.25.
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
        public FlangerWaveform Waveform
        {
            get { return (FlangerWaveform)Effect.Parameters.Waveform; }
            set 
            {
                SetValue("Waveform", (int)value);
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds the input is delayed before it is played back, in the range from 0ms to 4ms. The default value is 2 ms.
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
        /// Phase differential between left and right LFOs. The default value is <see cref="FlangerPhase.PhaseZero"/>.
        /// </summary>
        public FlangerPhase Phase
        {
            get { return (FlangerPhase)Effect.Parameters.Phase; }
            set
            {
                SetValue("Phase", (int)value);
            }
        }
        #endregion

        #region constants
        public const float DelayDefault = 2f;
        public const float DelayMax = 4f;
        public const float DelayMin = 0f;
        public const float DepthDefault = 100f;
        public const float DepthMax = 100f;
        public const float DepthMin = 0f;
        public const float FeedbackDefault = -50f;
        public const float FeedbackMax = 99f;
        public const float FeedbackMin = -99f;
        public const float FrequencyDefault = 0.25f;
        public const float FrequencyMax = 10f;
        public const float FrequencyMin = 0f;
        public const float WetDryMixDefault = 50f;
        public const float WetDryMixMax = 100f;
        public const float WetDryMixMin = 0f;
        #endregion
    }

    /// <summary>
    /// FlangerWaveform. See <see cref="DmoFlangerEffect.Waveform"/>.
    /// </summary>
    public enum FlangerWaveform
    {
        /// <summary>
        /// Triangle.
        /// </summary>
        Triangle = 0,
        /// <summary>
        /// Sine. Default value.
        /// </summary>
        Sin = 1
    }

    /// <summary>
    /// FlangerPhase. See <see cref="DmoFlangerEffect.Phase"/>.
    /// </summary>
    public enum FlangerPhase
    {
        /// <summary>
        /// 180° Phase.
        /// </summary>
        Phase180 = 4,
        /// <summary>
        /// 90° Phase. 
        /// Default value for <see cref="DmoFlangerEffect.Phase"/>. 
        /// </summary>
        Phase90 = 3,
        /// <summary>
        /// 0° Phase.
        /// </summary>
        PhaseZero = 2,
        /// <summary>
        /// -90° Phase.
        /// </summary>
        PhaseNegative90 = 1,
        /// <summary>
        /// -180° Phase.
        /// </summary>
        PhaseNegative180 = 0,
    }
}
