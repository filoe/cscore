using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// The IDirectSoundFXChorus interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("d616f352-d622-11ce-aac5-0020af0b99a3")]
    public class DirectSoundFXGargle : DirectSoundFXBase<GargleParameters>
    {   
        /// <summary>
        /// Creates a DirectSoundFXGargle wrapper based on a pointer to a IDirectSoundFXGargle cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXGargle interface.</param>
        public DirectSoundFXGargle(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXGargle"; }
        }
    }

    public struct GargleParameters
    {
        public int RateHz;
        public int WaveShape;
    }
}
