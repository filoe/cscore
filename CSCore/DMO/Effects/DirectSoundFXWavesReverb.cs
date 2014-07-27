using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    ///     The DirectSoundFXReverb interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("46858c3a-0dc6-45e3-b760-d4eef16cb325")]
    public class DirectSoundFXWavesReverb : DirectSoundFxBase<WavesReverbParameters>
    {
        /// <summary>
        ///     Creates a DirectSoundFXWavesReverb wrapper based on a pointer to a IDirectSoundFXWavesReverb cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXWavesReverb interface.</param>
        public DirectSoundFXWavesReverb(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXWavesReverb"; }
        }
    }
}