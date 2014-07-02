using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Defines a set of voices to receive data from a single output voice.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VoiceSends
    {
        /// <summary>
        ///     Number of voices to receive the output of the voice. An OutputCount value of 0 indicates the voice should not send
        ///     output to any voices.
        /// </summary>
        public int SendCount;

        /// <summary>
        ///     Array of <see cref="VoiceSendDescriptor" />s.
        /// </summary>
        public IntPtr SendsPtr;
    }
}