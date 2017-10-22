using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    ///     The DirectSoundFXDistortion interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("8ecf4326-455f-4d8b-bda9-8d5d3e9e3e0b")]
    public class DirectSoundFXDistortion : DirectSoundFxBase<DistortionParameters>
    {
        /// <summary>
        ///     Creates a DirectSoundFXDistortion wrapper based on a pointer to a IDirectSoundFXDistortion cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXDistortion interface.</param>
        public DirectSoundFXDistortion(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXDistortion"; }
        }
    }
}