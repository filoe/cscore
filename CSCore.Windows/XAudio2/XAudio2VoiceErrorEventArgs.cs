using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Provides data for the <see cref="VoiceCallback.VoiceError" /> event.
    /// </summary>
    public sealed class XAudio2VoiceErrorEventArgs : XAudio2BufferEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2VoiceErrorEventArgs" /> class.
        /// </summary>
        /// <param name="bufferContext">
        ///     The context pointer that was assigned to the <see cref="XAudio2Buffer.ContextPtr" /> member
        ///     of the <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </param>
        /// <param name="error">The HRESULT code of the error encountered</param>
        public XAudio2VoiceErrorEventArgs(IntPtr bufferContext, int error) : base(bufferContext)
        {
            Error = error;
        }

        /// <summary>
        ///     Gets the HRESULT code of the error encountered.
        /// </summary>
        public int Error { get; private set; }
    }
}