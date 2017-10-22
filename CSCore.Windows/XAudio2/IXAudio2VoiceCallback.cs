using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     The IXAudio2VoiceCallback interface contains methods that notify the client when certain events happen in a given
    ///     <see cref="XAudio2SourceVoice" />.
    /// </summary>
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IXAudio2VoiceCallback
    {
        /// <summary>
        ///     Called during each processing pass for each voice, just before XAudio2 reads data from the voice's buffer queue.
        /// </summary>
        /// <param name="bytesRequired">
        ///     The number of bytes that must be submitted immediately to avoid starvation. This allows the implementation of
        ///     just-in-time streaming scenarios; the client can keep the absolute minimum data queued on the voice at all times,
        ///     and pass it fresh data just before the data is required. This model provides the lowest possible latency attainable
        ///     with XAudio2. For xWMA and XMA data BytesRequired will always be zero, since the concept of a frame of xWMA or XMA
        ///     data is meaningless.
        ///     Note: In a situation where there is always plenty of data available on the source voice, BytesRequired should
        ///     always report zero, because it doesn't need any samples immediately to avoid glitching.
        /// </param>
        void OnVoiceProcessingPassStart([In] Int32 bytesRequired);

        /// <summary>
        ///     Called just after the processing pass for the voice ends.
        /// </summary>
        void OnVoiceProcessingPassEnd();

        /// <summary>
        ///     Called when the voice has just finished playing a contiguous audio stream.
        /// </summary>
        void OnStreamEnd();

        /// <summary>
        ///     Called when the voice is about to start processing a new audio buffer.
        /// </summary>
        /// <param name="bufferContextPtr">
        ///     Context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </param>
        void OnBufferStart([In] IntPtr bufferContextPtr);

        /// <summary>
        ///     Called when the voice finishes processing a buffer.
        /// </summary>
        /// <param name="bufferContextPtr">
        ///     Context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </param>
        void OnBufferEnd([In] IntPtr bufferContextPtr);

        /// <summary>
        ///     Called when the voice reaches the end position of a loop.
        /// </summary>
        /// <param name="bufferContextPtr">
        ///     Context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </param>
        void OnLoopEnd([In] IntPtr bufferContextPtr);

        /// <summary>
        ///     Called when a critical error occurs during voice processing.
        /// </summary>
        /// <param name="bufferContextPtr">
        ///     Context pointer that was assigned to the pContext member of the
        ///     <see cref="XAudio2Buffer" /> structure when the buffer was submitted.
        /// </param>
        /// <param name="error">The HRESULT code of the error encountered.</param>
        void OnVoiceError([In] IntPtr bufferContextPtr, [In] int error);
    }
}