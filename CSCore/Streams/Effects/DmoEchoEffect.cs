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
    /// Wrapper of the DirectX Echo Effect.
    /// </summary>
    public sealed class DmoEchoEffect : DmoAggregator
    {
        private DmoEchoEffectObject _comObj;
        private DirectSoundFXEcho _effect;

        public DmoEchoEffect(IWaveSource source)
            : base(source)
        {
            Initialize();
        }

        protected override MediaObject CreateMediaObject(WaveFormat inputFormat, WaveFormat outputFormat)
        {
            _comObj = new DmoEchoEffectObject();
            var mediaObject = new MediaObject(Marshal.GetComInterfaceForObject(_comObj, typeof(IMediaObject)));
            _effect = mediaObject.QueryInterface<DirectSoundFXEcho>();

            return mediaObject;
        }

        protected override WaveFormat GetOutputFormat()
        {
            return GetInputFormat();
        }

        private DirectSoundFXEcho Effect
        {
            get { return _effect; }
        }

        [ComImport]
        [Guid("ef3e932c-d40b-4f51-8ccf-3f98f1b29d5d")]
        private sealed class DmoEchoEffectObject
        {
        }
        private void SetValue<T>(string fieldname, T value) where T : struct
        {
            var p = Effect.Parameters;
            p.GetType().GetField(fieldname).SetValueForValueType(ref p, value);
            Effect.Parameters = p;
        }

        #region Properties
        /// <summary>
        /// Ratio of wet (processed) signal to dry (unprocessed) signal. Must be in the range from
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
        /// Percentage of output fed back into input, in the range from 0
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
        /// Delay for left channel, in milliseconds, in the range from 1
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
        /// Delay for right channel, in milliseconds, in the range from 1
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
        /// Value that specifies whether to swap left and right delays with each successive echo.
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

        public const float FeedbackDefault = 50f;
        public const float FeedbackMax = 100f;
        public const float FeedbackMin = 0f;
        public const float LeftDelayDefault = 500f;
        public const float LeftDelayMax = 2000f;
        public const float LeftDelayMin = 1f;
        public const bool PanDelayDefault = false;
        public const int PanDelayMax = 1;
        public const int PanDelayMin = 0;
        public const float RightDelayDefault = 500f;
        public const float RightDelayMax = 2000f;
        public const float RightDelayMin = 1f;
        public const float WetDryMixDefault = 50f;
        public const float WetDryMixMax = 100f;
        public const float WetDryMixMin = 0f;

        #endregion constants
    }
}
