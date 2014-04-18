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
    [Guid("880842e3-145f-43e6-a934-a71806e50547")]
    public class DirectSoundFXChorus : DirectSoundFXBase<ChorusParameters>
    {
        /// <summary>
        /// Creates a DirectSoundFXChorus wrapper based on a pointer to a IDirectSoundFXChorus cominterface.
        /// </summary>
        /// <param name="ptr">Pointer of a DirectSoundFXChorus interface.</param>
        public DirectSoundFXChorus(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Interface name used for generating DmoExceptions.
        /// </summary>
        protected override string InterfaceName
        {
            get { return "IDirectSoundFXChorus"; }
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct ChorusParameters
    {
        public float WetDryMix;
        public float Depth;
        public float Feedback;
        public float Frequency;
        public int Waveform;
        public float Delay;
        public int Phase;
    }
}
