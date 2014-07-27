using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    ///     The DirectSoundFXFlanger interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("903e9878-2c92-4072-9b2c-ea68f5396783")]
    public class DirectSoundFXFlanger : DirectSoundFxBase<FlangerParameters>
    {
        /// <summary>
        ///     Creates a DirectSoundFXFlanger wrapper based on a pointer to a IDirectSoundFXFlanger cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXFlanger interface.</param>
        public DirectSoundFXFlanger(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "DirectSoundFXFlanger"; }
        }
    }
}