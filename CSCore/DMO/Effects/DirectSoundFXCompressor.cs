using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// The DirectSoundFXCompressor interface is used to set and retrieve effect parameters.
    /// </summary>
    [Guid("4bbd1154-62f6-4e2c-a15c-d3b6c417f7a0")]
    public class DirectSoundFXCompressor : DirectSoundFXBase<CompressorParameters>
    {
        /// <summary>
        /// Creates a DirectSoundFXCompressor wrapper based on a pointer to a IDirectSoundFXCompressor cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXCompressor interface.</param>
        public DirectSoundFXCompressor(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXCompressor"; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CompressorParameters
    {
        public float Gain;
        public float Attack;
        public float Release;
        public float Threshold;
        public float Ratio;
        public float Predelay;
    }
}
