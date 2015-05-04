using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Returns the voice's current state and cursor position data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VoiceState
    {
        /// <summary>
        ///     Pointer to a buffer context provided in the <see cref="XAudio2Buffer" /> that is processed currently, or,
        ///     if the voice is stopped currently, to the next buffer due to be processed.
        ///     <see cref="CurrentBufferContextPtr" /> is NULL if there are no buffers in the queue.
        /// </summary>
        public IntPtr CurrentBufferContextPtr;

        /// <summary>
        ///     Number of audio buffers currently queued on the voice, including the one that is processed currently.
        /// </summary>
        public int BuffersQueued;

        /// <summary>
        ///     Total number of samples processed by this voice since it last started, or since the last audio stream ended (as
        ///     marked with the <see cref="XAudio2BufferFlags.EndOfStream" /> flag).
        ///     This total includes samples played multiple times due to looping.
        ///     Theoretically, if all audio emitted by the voice up to this time is captured, this parameter would be the length of
        ///     the audio stream in samples.
        ///     If you specify <see cref="GetVoiceStateFlags.NoSamplesPlayed" /> when you call
        ///     <see cref="XAudio2SourceVoice.GetState(CSCore.XAudio2.GetVoiceStateFlags)" />,
        ///     this member won't be calculated, and its value is unspecified on return from
        ///     <see cref="XAudio2SourceVoice.GetState(CSCore.XAudio2.GetVoiceStateFlags)" />.
        ///     <see cref="XAudio2SourceVoice.GetState(CSCore.XAudio2.GetVoiceStateFlags)" /> takes about one-third as much time to
        ///     complete when you specify <see cref="GetVoiceStateFlags.NoSamplesPlayed" />.
        /// </summary>
        public long SamplesPlayed;
    }
}