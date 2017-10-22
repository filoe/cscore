using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Default implementation of the <see cref="IXAudio2VoiceCallback" /> interface.
    /// </summary>
    [ComVisible(true)]
    public sealed class VoiceCallback : IXAudio2VoiceCallback, IDisposable
    {
        void IXAudio2VoiceCallback.OnVoiceProcessingPassStart(int bytesRequired)
        {
            EventHandler<XAudio2ProcessingPassStartEventArgs> handler = this.ProcessingPassStart;
            if (handler != null)
                handler(this, new XAudio2ProcessingPassStartEventArgs(bytesRequired));
        }

        void IXAudio2VoiceCallback.OnVoiceProcessingPassEnd()
        {
            EventHandler handler = this.ProcessingPassEnd;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void IXAudio2VoiceCallback.OnStreamEnd()
        {
            EventHandler handler = this.StreamEnd;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void IXAudio2VoiceCallback.OnBufferStart(IntPtr bufferContextPtr)
        {
            EventHandler<XAudio2BufferEventArgs> handler = this.BufferStart;
            if (handler != null)
                handler(this, new XAudio2BufferEventArgs(bufferContextPtr));
        }

        void IXAudio2VoiceCallback.OnBufferEnd(IntPtr bufferContextPtr)
        {
            EventHandler<XAudio2BufferEventArgs> handler = this.BufferEnd;
            if (handler != null)
                handler(this, new XAudio2BufferEventArgs(bufferContextPtr));
        }

        void IXAudio2VoiceCallback.OnLoopEnd(IntPtr bufferContextPtr)
        {
            EventHandler<XAudio2BufferEventArgs> handler = this.LoopEnd;
            if (handler != null)
                handler(this, new XAudio2BufferEventArgs(bufferContextPtr));
        }

        void IXAudio2VoiceCallback.OnVoiceError(IntPtr bufferContextPtr, int error)
        {
            EventHandler<XAudio2VoiceErrorEventArgs> handler = this.VoiceError;
            if (handler != null)
                handler(this, new XAudio2VoiceErrorEventArgs(bufferContextPtr, error));
        }

        /// <summary>
        ///     Called during each processing pass for each voice, just before XAudio2 reads data from the voice's buffer queue.
        ///     The only argument passed to the eventhandler is the number of required bytes:
        ///     The number of bytes that must be submitted immediately to avoid starvation. This allows the implementation of
        ///     just-in-time streaming scenarios; the client can keep the absolute minimum data queued on the voice at all times,
        ///     and pass it fresh data just before the data is required. This model provides the lowest possible latency attainable
        ///     with XAudio2. For xWMA and XMA data BytesRequired will always be zero, since the concept of a frame of xWMA or XMA
        ///     data is meaningless.
        ///     Note: In a situation where there is always plenty of data available on the source voice, BytesRequired should
        ///     always report zero, because it doesn't need any samples immediately to avoid glitching.
        /// </summary>
        public event EventHandler<XAudio2ProcessingPassStartEventArgs> ProcessingPassStart;

        /// <summary>
        ///     Called just after the processing pass for the voice ends.
        /// </summary>
        public event EventHandler ProcessingPassEnd;

        /// <summary>
        ///     Called when the voice has just finished playing a contiguous audio stream.
        /// </summary>
        public event EventHandler StreamEnd;

        /// <summary>
        ///     Called when the voice is about to start processing a new audio buffer.
        ///     The only argument passed to the eventhandler is a context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </summary>
        public event EventHandler<XAudio2BufferEventArgs> BufferStart;

        /// <summary>
        ///     Called when the voice finishes processing a buffer.
        ///     The only argument passed to the eventhandler is a context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </summary>
        public event EventHandler<XAudio2BufferEventArgs> BufferEnd;

        /// <summary>
        ///     Called when the voice reaches the end position of a loop.
        ///     The only argument passed to the eventhandler is a context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </summary>
        public event EventHandler<XAudio2BufferEventArgs> LoopEnd;

        /// <summary>
        ///     Called when a critical error occurs during voice processing.
        ///     The first argument passed to the eventhandler is a context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        ///     The second argument passed to the eventhandler is the HRESULT error code of the critical error.
        /// </summary>
        public event EventHandler<XAudio2VoiceErrorEventArgs> VoiceError;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            ProcessingPassStart = null;
            ProcessingPassEnd = null;
            StreamEnd = null;
            BufferStart = null;
            BufferEnd = null;
            LoopEnd = null;
            VoiceError = null;
        }
    }
}