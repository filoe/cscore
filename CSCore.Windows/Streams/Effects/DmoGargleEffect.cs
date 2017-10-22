using CSCore.DMO.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Represents the dmo gargle effect in form of an <see cref="IWaveSource"/> implementation.
    /// </summary>
    public class DmoGargleEffect : DmoEffectBase<DirectSoundFXGargle, GargleParameters>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DmoGargleEffect"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoGargleEffect(IWaveSource source)
            : base(source)
        {
        }

        /// <summary>
        /// Creates and returns a new instance of the native COM object.
        /// </summary>
        /// <returns>A new instance of the native COM object.</returns>
        protected override object CreateComObject()
        {
            return new DmoGargleEffectObject();
        }

        [ComImport]
        [Guid("dafd8210-5711-4b91-9fe3-f75b7ae279bf")]
        private sealed class DmoGargleEffectObject
        {
        }

        #region properties

        /// <summary>
        /// Gets or sets the rate of modulation, in Hertz. Must be in the range from 20Hz through 1000Hz. The default value is 20Hz.
        /// </summary>
        public int RateHz
        {
            get { return Effect.Parameters.RateHz; }
            set 
            {
                if (value < RateMin || value > RateMax)
                    throw new ArgumentOutOfRangeException("value");
                SetValue("RateHz", value); 
            }
        }

        /// <summary>
        /// Gets or sets the shape of the modulation waveform.
        /// </summary>
        public GargleWaveShape WaveShape
        {
            get { return (GargleWaveShape)Effect.Parameters.WaveShape; }
            set { SetValue("WaveShape", (int)value); }
        }

        #endregion
        #region constants
        /// <summary>
        /// Default value for the <see cref="RateHz"/> property.
        /// </summary>
        public const int RateDefault = 20;
        /// <summary>
        /// Maximum value for the <see cref="RateHz"/> property.
        /// </summary>
        public const int RateMax = 1000;
        /// <summary>
        /// Minimum value for the <see cref="RateHz"/> property.
        /// </summary>
        public const int RateMin = 1;

        /// <summary>
        /// Default value for the <see cref="WaveShape"/> property.
        /// </summary>
        /// <remarks>Use the <see cref="GargleWaveShape"/> enumeration instead.</remarks>
        public const int WaveShapeDefault = 0;
        /// <summary>
        /// Square Waveform.
        /// </summary>
        /// <remarks>Use the <see cref="GargleWaveShape"/> enumeration instead.</remarks>        
        public const int WaveShapeSquare = 1;
        /// <summary>
        /// Triangle Waveform
        /// </summary>
        /// <remarks>Use the <see cref="GargleWaveShape"/> enumeration instead.</remarks>        
        public const int WaveShapeTriangle = 0;
        #endregion
    }
}
