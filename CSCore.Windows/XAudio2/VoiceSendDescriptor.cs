using System;
using System.ComponentModel;
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
        ///     Either <see cref="VoiceSendFlags.None"/> or <see cref="VoiceSendFlags.UseFilter"/>.
        /// </summary>
        public readonly VoiceSendFlags Flags;

        /// <summary>
        ///     The destination voice.
        /// </summary>
        public readonly IntPtr OutputVoicePtr;

        /// <summary>
        ///     Creates a new instance of the <see cref="VoiceSendDescriptor" /> structure.
        /// </summary>
        /// <param name="flags">The <see cref="VoiceSendFlags"/>. Must be either <see cref="VoiceSendFlags.None"/> or <see cref="VoiceSendFlags.UseFilter"/>.</param>
        /// <param name="outputVoice">The destination voice. Must not be null.</param>
        public VoiceSendDescriptor(VoiceSendFlags flags, XAudio2Voice outputVoice)
        {
            if(flags != VoiceSendFlags.None && flags != VoiceSendFlags.UseFilter)
                throw new InvalidEnumArgumentException("flags", (int)flags, typeof(VoiceSendFlags));
            if (outputVoice == null)
                throw new ArgumentNullException("outputVoice");
            Flags = flags;
            OutputVoicePtr = outputVoice.BasePtr;
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="VoiceSendDescriptor" /> structure.
        /// </summary>
        /// <param name="flags">The <see cref="VoiceSendFlags"/>. Must be either <see cref="VoiceSendFlags.None"/> or <see cref="VoiceSendFlags.UseFilter"/>.</param>
        /// <param name="outputVoicePtr">Pointer to the destination voice. Must not be <see cref="IntPtr.Zero"/>.</param>
        public VoiceSendDescriptor(VoiceSendFlags flags, IntPtr outputVoicePtr)
        {
            if (flags != VoiceSendFlags.None && flags != VoiceSendFlags.UseFilter)
                throw new InvalidEnumArgumentException("flags", (int)flags, typeof(VoiceSendFlags));
            if(outputVoicePtr == IntPtr.Zero)
                throw new ArgumentException("Must not be Zero.", "outputVoicePtr");
            Flags = flags;
            OutputVoicePtr = outputVoicePtr;
        }
    }
}