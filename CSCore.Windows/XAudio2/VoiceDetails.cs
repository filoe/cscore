using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Contains information about the creation flags, input channels, and sample rate of a voice.
    /// </summary>
    public class VoiceDetails
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct VoiceDetails27
        {
            public VoiceFlags CreationFlags;
            public int InputChannels;
            public int InputSampleRate;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct VoiceDetails28
        {
            public VoiceFlags CreationFlags;
            public int ActiveFlags;
            public int InputChannels;
            public int InputSampleRate;
        }

        internal static VoiceDetails FromNativeVoiceDetailsObject(object nativeVoiceDetailsObj)
        {
            if (nativeVoiceDetailsObj is VoiceDetails27)
            {
                var nativeObj = (VoiceDetails27) nativeVoiceDetailsObj;
                return new VoiceDetails()
                {
                    ActiveFlags = -1,
                    CreationFlags = nativeObj.CreationFlags,
                    InputChannels = nativeObj.InputChannels,
                    InputSampleRate = nativeObj.InputSampleRate
                };
            }
            if (nativeVoiceDetailsObj is VoiceDetails28)
            {
                var nativeObj = (VoiceDetails28) nativeVoiceDetailsObj;
                return new VoiceDetails()
                {
                    ActiveFlags = nativeObj.ActiveFlags,
                    CreationFlags = nativeObj.CreationFlags,
                    InputChannels = nativeObj.InputChannels,
                    InputSampleRate = nativeObj.InputSampleRate
                };
            }
            throw new ArgumentException("Invalid type.");
        }

        /// <summary>
        ///     Flags used to create the voice; see the individual voice interfaces for more information.
        /// </summary>
        public VoiceFlags CreationFlags;

        /// <summary>
        ///     Flags that are currently set on the voice.
        /// </summary>
        public int ActiveFlags;

        /// <summary>
        ///     The number of input channels the voice expects.
        /// </summary>
        public int InputChannels;

        /// <summary>
        ///     The input sample rate the voice expects.
        /// </summary>
        public int InputSampleRate;
    }
}