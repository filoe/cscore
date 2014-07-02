using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Defines an effect chain.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EffectChain
    {
        /// <summary>
        ///     Number of effects in the effect chain for the voice.
        /// </summary>
        public int EffectCount;

        /// <summary>
        ///     Pointer to an array of <see cref="EffectDescriptor" /> structures containing pointers to XAPO instances.
        /// </summary>
        public IntPtr EffectDescriptorsPtr;
    }
}