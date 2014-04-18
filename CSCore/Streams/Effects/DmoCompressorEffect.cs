using CSCore.DMO.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Compressor Effect.
    /// </summary>
    public class DmoCompressorEffect : DmoEffectBase<DirectSoundFXCompressor, CompressorParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoCompressorEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoCompressorEffect(IWaveSource source)
            : base(source)
        {
        }

        protected override object CreateComObject()
        {
            return new DmoCompressorEffectObject();
        }

        [ComImport]
        [Guid("ef011f79-4000-406d-87af-bffb3fc39d57")]
        private sealed class DmoCompressorEffectObject
        {
        }

        #region properties
        /// <summary>
        /// Time before compression reaches its full value, in the range from 0.01 ms to 500 ms. The default value is 10 ms.
        /// </summary>
        public float Attack
        {
            get { return Effect.Parameters.Attack; }
            set
            {
                if (value < AttackMin || value > AttackMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Attack", value);
            }
        }

        /// <summary>
        /// Output gain of signal after compression, in the range from -60 dB to 60 dB. The default value is 0 dB.
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
        /// Time after <see cref="Threshold"/> is reached before attack phase is started, in milliseconds, in the range from 0 ms to 4 ms. The default value is 4 ms.
        /// </summary>
        public float Predelay
        {
            get { return Effect.Parameters.Predelay; }
            set
            {
                if (value < PredelayMin || value > PredelayMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Predelay", value);
            }
        }

        /// <summary>
        /// Compression ratio, in the range from 1 to 100. The default value is 3, which means 3:1 compression.
        /// </summary>
        public float Ratio
        {
            get { return Effect.Parameters.Ratio; }
            set
            {
                if (value < RatioMin || value > RatioMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Ratio", value);
            }
        }

        /// <summary>
        /// Speed at which compression is stopped after input drops below fThreshold, in the range from 50 ms to 3000 ms. The default value is 200 ms.
        /// </summary>
        public float Release
        {
            get { return Effect.Parameters.Release; }
            set
            {
                if (value < ReleaseMin || value > ReleaseMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Release", value);
            }
        }

        /// <summary>
        /// Point at which compression begins, in decibels, in the range from -60 dB to 0 dB. The default value is -20 dB.
        /// </summary>
        public float Threshold
        {
            get { return Effect.Parameters.Threshold; }
            set
            {
                if (value < ThresholdMin || value > ThresholdMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("Threshold", value);
            }
        }
        #endregion
        #region constants
        public const float AttackDefault = 10f;
        public const float AttackMax = 500f;
        public const float AttackMin = 0.01f;
        public const float GainDefault = 0f;
        public const float GainMax = 60f;
        public const float GainMin = -60f;
        public const float PredelayDefault = 4f;
        public const float PredelayMax = 4f;
        public const float PredelayMin = 0f;
        public const float RatioDefault = 3f;
        public const float RatioMax = 100f;
        public const float RatioMin = 1f;
        public const float ReleaseDefault = 200f;
        public const float ReleaseMax = 3000f;
        public const float ReleaseMin = 50f;
        public const float ThresholdDefault = -20f;
        public const float ThresholdMax = 0f;
        public const float ThresholdMin = -60f;
        #endregion
    }
}
