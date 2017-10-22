using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    ///     The DirectSoundFXCompressor interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("4bbd1154-62f6-4e2c-a15c-d3b6c417f7a0")]
    public class DirectSoundFXCompressor : DirectSoundFxBase<CompressorParameters>
    {
        /// <summary>
        ///     Creates a DirectSoundFXCompressor wrapper based on a pointer to a IDirectSoundFXCompressor cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXCompressor interface.</param>
        public DirectSoundFXCompressor(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXCompressor"; }
        }
    }
}