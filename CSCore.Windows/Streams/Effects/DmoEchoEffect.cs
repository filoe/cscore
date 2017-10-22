using CSCore.DMO;
using CSCore.DMO.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Represents the dmo echo effect in form of an <see cref="IWaveSource"/> implementation.
    /// </summary>
    public sealed class DmoEchoEffect : DmoEffectBase<DirectSoundFXEcho, EchoParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoEchoEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoEchoEffect(IWaveSource source)
            : base(source)
        {
        }

        /// <summary>
        /// Creates and returns a new instance of the native COM object.
        /// </summary>
        /// <returns>A new instance of the native COM object.</returns>
        protected override object CreateComObject()
        {
            return new DmoEchoEffectObject();
        }

        [ComImport]
        [Guid("ef3e932c-d40b-4f51-8ccf-3f98f1b29d5d")]
        private sealed class DmoEchoEffectObject
        {
        }

        #region Properties
        /// <summary>
        /// Gets or sets the ratio of wet (processed) signal to dry (unprocessed) signal. Must be in the range from
        /// 0 through 100 (all wet). The default value is 50.
        /// </summary>
        public float WetDryMix
        {
            get
            {
                return Effect.Parameters.WetDryMix;
            }
            set
            {
                if (value < WetDryMixMin || value > WetDryMixMax)
                    throw new ArgumentOutOfRangeException("value", "See WetDryMixMin and WetDryMixMay.");
                SetValue("WetDryMix", value);
            }
        }

        /// <summary>
        /// Gets or sets the percentage of output fed back into input, in the range from 0
        /// through 100. The default value is 50.
        /// </summary>
        public float Feedback
        {
            get
            {
                return Effect.Parameters.Feedback;
            }
            set
            {
                if (value < FeedbackMin || value > FeedbackMax)
                    throw new ArgumentOutOfRangeException("value", "See FeedbackMin and FeedbackMax.");
                SetValue("Feedback", value);
            }
        }

        /// <summary>
        /// Gets or sets the delay for left channel, in milliseconds, in the range from 1
        /// through 2000. The default value is 500 ms.
        /// </summary>
        public float LeftDelay
        {
            get
            {
                return Effect.Parameters.LeftDelay;
            }
            set
            {
                if (value < LeftDelayMin || value > LeftDelayMax)
                    throw new ArgumentOutOfRangeException("value", "See LeftDelayMin and LeftDelayMax.");
                SetValue("LeftDelay", value);
            }
        }

        /// <summary>
        /// Gets or sets the delay for right channel, in milliseconds, in the range from 1
        /// through 2000. The default value is 500 ms.
        /// </summary>
        public float RightDelay
        {
            get
            {
                return Effect.Parameters.RightDelay;
            }
            set
            {
                if (value < RightDelayMin || value > RightDelayMax)
                    throw new ArgumentOutOfRangeException("value", "See RightDelayMin and RightDelayMax.");
                SetValue("RightDelay", value);
            }
        }

        /// <summary>
        /// Gets or sets the value that specifies whether to swap left and right delays with each successive echo.
        /// The default value is false, meaning no swap.
        /// </summary>
        public bool PanDelay
        {
            get
            {
                return Effect.Parameters.PanDelay == PanDelayMax;
            }
            set
            {
                var parameters = Effect.Parameters;
                parameters.PanDelay = value ? PanDelayMax : PanDelayMin;
                Effect.Parameters = parameters;
            }
        }
        #endregion

        #region constants

        /// <summary>
        /// Default value for the <see cref="Feedback"/> property.
        /// </summary>
        public const float FeedbackDefault = 50f;
        /// <summary>
        /// Maximum value for the <see cref="Feedback"/> property.
        /// </summary>
        public const float FeedbackMax = 100f;
        /// <summary>
        /// Minimum value for the <see cref="Feedback"/> property.
        /// </summary>
        public const float FeedbackMin = 0f;

        /// <summary>
        /// Default value for the <see cref="LeftDelay"/> property.
        /// </summary>
        public const float LeftDelayDefault = 500f;
        /// <summary>
        /// Maximum value for the <see cref="LeftDelay"/> property.
        /// </summary>
        public const float LeftDelayMax = 2000f;
        /// <summary>
        /// Minimum value for the <see cref="LeftDelay"/> property.
        /// </summary>
        public const float LeftDelayMin = 1f;

        /// <summary>
        /// Default value for the <see cref="PanDelay"/> property.
        /// </summary>
        public const bool PanDelayDefault = false;
        /// <summary>
        /// Maximum value for the <see cref="PanDelay"/> property.
        /// </summary>
        public const int PanDelayMax = 1;
        /// <summary>
        /// Minimum value for the <see cref="PanDelay"/> property.
        /// </summary>
        public const int PanDelayMin = 0;

        /// <summary>
        /// Default value for the <see cref="RightDelay"/> property.
        /// </summary>
        public const float RightDelayDefault = 500f;
        /// <summary>
        /// Maximum value for the <see cref="RightDelay"/> property.
        /// </summary>
        public const float RightDelayMax = 2000f;
        /// <summary>
        /// Minimum value for the <see cref="RightDelay"/> property.
        /// </summary>
        public const float RightDelayMin = 1f;

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

        #endregion constants
    }
}
