using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Provides data for the <see cref="VoiceCallback.BufferStart" />, the <see cref="VoiceCallback.BufferEnd" /> and the
    ///     <see cref="VoiceCallback.LoopEnd" /> event.
    /// </summary>
    public class XAudio2BufferEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2BufferEventArgs" /> class.
        /// </summary>
        /// <param name="bufferContext">
        ///     The context pointer that was assigned to the <see cref="XAudio2Buffer.ContextPtr" /> member
        ///     of the <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </param>
        public XAudio2BufferEventArgs(IntPtr bufferContext)
        {
            BufferContext = bufferContext;
        }

        /// <summary>
        ///     Gets the context pointer that was assigned to the <see cref="XAudio2Buffer.ContextPtr" /> member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </summary>
        public IntPtr BufferContext { get; private set; }
    }
}