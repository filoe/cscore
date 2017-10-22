using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Provides data for the <see cref="VoiceCallback.ProcessingPassStart" /> event.
    /// </summary>
    public sealed class XAudio2ProcessingPassStartEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2ProcessingPassStartEventArgs" /> class.
        /// </summary>
        /// <param name="bytesRequired">The number of bytes that must be submitted immediately to avoid starvation.</param>
        public XAudio2ProcessingPassStartEventArgs(int bytesRequired)
        {
            BytesRequired = bytesRequired;
        }

        /// <summary>
        ///     Gets the number of bytes that must be submitted immediately to avoid starvation.
        /// </summary>
        public int BytesRequired { get; private set; }
    }
}