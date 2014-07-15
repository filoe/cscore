using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Defines a destination voice that is the target of a send from another voice and specifies whether a filter should
    ///     be used.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VoiceSendDescriptor
    {
        /// <summary>
        ///     Either 0 or XAUDIO2_SEND_USEFILTER.
        /// </summary>
        public VoiceSendFlags Flags;

        /// <summary>
        ///     This send's destination voice.
        /// </summary>
        public IntPtr OutputVoicePtr;

        /// <summary>
        ///     Creates a new instance of the <see cref="VoiceSendDescriptor" /> structure.
        /// </summary>
        public VoiceSendDescriptor(VoiceSendFlags flags, XAudio2Voice outputVoice)
        {
            Flags = flags;
            OutputVoicePtr = outputVoice.BasePtr;
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="VoiceSendDescriptor" /> structure.
        /// </summary>
        public VoiceSendDescriptor(VoiceSendFlags flags, IntPtr outputVoicePtr)
        {
            Flags = flags;
            OutputVoicePtr = outputVoicePtr;
        }
    }
}