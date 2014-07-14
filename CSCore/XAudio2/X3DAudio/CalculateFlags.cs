using System;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    /// Flags which define calculate flags for calculating the 3D audio parameters.
    /// </summary>
    [Flags]
    public enum CalculateFlags
    {
        /// <summary>
        /// Enables matrix coefficient table calculation. 
        /// </summary>
        Matrix = 0x1,
        /// <summary>
        /// 	Enables delay time array calculation (stereo only). 
        /// </summary>
        Delay = 0x2,
        /// <summary>
        /// Enables low pass filter (LPF) direct-path coefficient calculation. 
        /// </summary>
        LpfDirect = 0x4,
        /// <summary>
        /// Enables LPF reverb-path coefficient calculation. 
        /// </summary>
        LpfReverb = 0x8,
        /// <summary>
        /// Enables reverb send level calculation. 
        /// </summary>
        Reverb = 0x10,
        /// <summary>
        /// Enables Doppler shift factor calculation. 
        /// </summary>
        Doppler = 0x20,
        /// <summary>
        /// Enables emitter-to-listener interior angle calculation. 
        /// </summary>
        EmitterAngle = 0x40,
        /// <summary>
        /// Fills the center channel with silence. This flag allows you to keep a 6-channel matrix so you do not have to remap the channels, but the center channel will be silent. This flag is only valid if you also set <see cref="Matrix"/>. 
        /// </summary>
        ZeroCenter = 0x10000,
        /// <summary>
        /// Applies an equal mix of all source channels to a low frequency effect (LFE) destination channel. It only applies to matrix calculations with a source that does not have an LFE channel and a destination that does have an LFE channel. This flag is only valid if you also set <see cref="Matrix"/>.
        /// </summary>
        RedirectToLfe = 0x20000
    }
}