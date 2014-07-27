using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    ///     The IDirectSoundFXEcho interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("8bd28edf-50db-4e92-a2bd-445488d1ed42")]
    public class DirectSoundFXEcho : DirectSoundFxBase<EchoParameters>
    {
        /// <summary>
        ///     Creates a DirectSoundFXEcho wrapper based on a pointer to a IDirectSoundFXEcho cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXEcho interface.</param>
        public DirectSoundFXEcho(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXEcho"; }
        }
    }
}