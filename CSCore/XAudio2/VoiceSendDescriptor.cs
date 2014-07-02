using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Defines a destination voice that is the target of a send from another voice and specifies whether a filter should
    ///     be used.
    /// </summary>
    public struct VoiceSendDescriptor
    {
        /// <summary>
        ///     Either 0 or XAUDIO2_SEND_USEFILTER.
        /// </summary>
        public VoiceSendFlags Flags;

        /// <summary>
        ///     This send's destination voice.
        /// </summary>
        private IntPtr _outputVoicePtr;

        /// <summary>
        ///     Creates a new instance of the <see cref="VoiceSendDescriptor" /> structure.
        /// </summary>
        public VoiceSendDescriptor(VoiceSendFlags flags, XAudio2Voice outputVoice)
        {
            Flags = flags;
            _outputVoicePtr = IntPtr.Zero;
            OutputVoice = outputVoice;
        }

        /// <summary>
        ///     This send's destination voice.
        /// </summary>
        public XAudio2Voice OutputVoice
        {
            get { return _outputVoicePtr == IntPtr.Zero ? null : new XAudio2Voice(_outputVoicePtr); }
            set { _outputVoicePtr = value == null ? IntPtr.Zero : value.BasePtr; }
        }
    }
}