using CSCore.DMO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Base class for all DMO effects.
    /// </summary>
    /// <typeparam name="TDXEffect">DMO effect itself.</typeparam>
    /// <typeparam name="TDXEffectStruct">Parameter struct of the DMO effect.</typeparam>
    public abstract class DmoEffectBase<TDXEffect, TDXEffectStruct> : DmoAggregator 
        where TDXEffect : DMO.Effects.DirectSoundFXBase<TDXEffectStruct>
        where TDXEffectStruct : struct
    {
        private Object _comObj;
        private TDXEffect _effect;
        private bool _isEnabled;

        /// <summary>
        /// Creates a new instance of <see cref="DmoEffectBase{TDXEffect,TDXEffectStruct}"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        public DmoEffectBase(IWaveSource source)
            : base(source)
        {
            Initialize();
        }

        /// <summary>
        /// Returns a new instance of the DMO effect.
        /// </summary>
        /// <returns>DMO effect.</returns>
        protected abstract Object CreateComObject();

        /// <summary>
        /// Creates an MediaObject from the effect DMO.
        /// </summary>
        protected override MediaObject CreateMediaObject(WaveFormat inputFormat, WaveFormat outputFormat)
        {
            _comObj = CreateComObject();
            var mediaObject = new MediaObject(Marshal.GetComInterfaceForObject(_comObj, typeof(IMediaObject)));
            _effect = mediaObject.QueryInterface<TDXEffect>();
            return mediaObject;
        }

        /// <summary>
        /// Gets the output format of the effect.
        /// </summary>
        /// <returns></returns>
        protected override WaveFormat GetOutputFormat()
        {
            return GetInputFormat();
        }

        /// <summary>
        /// Gets the underlying effect.
        /// </summary>
        protected TDXEffect Effect
        {
            get
            {
                return _effect;
            }
        }

        /// <summary>
        /// Gets or sets whether the effect is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get 
            { 
                return _isEnabled; 
            }
            set 
            { 
                _isEnabled = value; 
            }
        }

        /// <summary>
        /// Sets the value for one of the effects parameter and updates the effect.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldname"></param>
        /// <param name="value"></param>
        protected void SetValue<T>(string fieldname, T value) where T : struct
        {
            var p = Effect.Parameters;
            p.GetType().GetField(fieldname).SetValueForValueType(ref p, value);
            Effect.Parameters = p;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (IsEnabled)
                return base.Read(buffer, offset, count);
            else
                return BaseStream.Read(buffer, offset, count);
        }
    }
}
