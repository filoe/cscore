using CSCore.DMO;
using System;
using System.Runtime.InteropServices;

namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Base class for all DMO effects.
    /// </summary>
    /// <typeparam name="TDXEffect">DMO effect itself.</typeparam>
    /// <typeparam name="TDXEffectStruct">Parameter struct of the DMO effect.</typeparam>
    public abstract class DmoEffectBase<TDXEffect, TDXEffectStruct> : DmoAggregator 
        where TDXEffect : DMO.Effects.DirectSoundFxBase<TDXEffectStruct>
        where TDXEffectStruct : struct
    {
        private Object _comObj;
        private TDXEffect _effect;
        private bool _isEnabled = true;

        /// <summary>
        /// Creates a new instance of <see cref="DmoEffectBase{TDXEffect,TDXEffectStruct}"/> class.
        /// </summary>
        /// <param name="source">The base source, which feeds the effect with data.</param>
        protected DmoEffectBase(IWaveSource source)
            : base(source)
        {
            Initialize();
        }

        /// <summary>
        /// Creates and returns a new instance of the native COM object.
        /// </summary>
        /// <returns>A new instance of the native COM object.</returns>
        protected abstract Object CreateComObject();

        /// <summary>
        /// Creates an MediaObject from the effect DMO.
        /// </summary>
        /// <param name="inputFormat">The input format of the <see cref="MediaObject" /> to create.</param>
        /// <param name="outputFormat">The output format of the <see cref="MediaObject" /> to create.</param>
        /// <returns>
        /// The created <see cref="MediaObject" /> to use for processing audio data.
        /// </returns>
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
        /// <returns>The output format of the effect.</returns>
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
        /// <typeparam name="T">Type of the <paramref name="value"/>.</typeparam>
        /// <param name="fieldname">Name of the field to set the value for.</param>
        /// <param name="value">Value to set.</param>
        protected void SetValue<T>(string fieldname, T value) where T : struct
        {
            var p = Effect.Parameters;
            p.GetType().GetField(fieldname).SetValueForValueType(ref p, value);
            Effect.Parameters = p;
        }

        /// <summary>
        ///     Reads a sequence of bytes from the stream and applies the Dmo effect to them (only if the <see cref="IsEnabled"/> property is set to true).
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the read bytes.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the stream</param>
        /// <returns>The actual number of read bytes.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (IsEnabled)
                return base.Read(buffer, offset, count);
            return BaseSource.Read(buffer, offset, count);
        }
    }
}
