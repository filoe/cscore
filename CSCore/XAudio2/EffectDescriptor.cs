using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Contains information about an XAPO for use in an effect chain.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EffectDescriptor
    {
        /// <summary>
        ///     Pointer to the IUnknown interface of the XAPO object.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IntPtr IUnknownEffect;

        /// <summary>
        ///     TRUE if the effect should begin in the enabled state. Otherwise, FALSE.
        /// </summary>
        public NativeBool InitialState;

        /// <summary>
        ///     Number of output channels the effect should produce.
        /// </summary>
        public int OutputChannels;
    }
}