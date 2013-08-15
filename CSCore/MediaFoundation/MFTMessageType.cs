namespace CSCore.MediaFoundation
{
    /// <summary>
    /// http: //msdn.microsoft.com/de-de/library/ms697223(v=vs.85).aspx
    /// </summary>
    public enum MFTMessageType
    {
        /// <summary>
        /// Requests the MFT to flush all stored data.
        /// </summary>
        CommandFlush = 0x00000000,

        /// <summary>
        /// Requests the MFT to drain any stored data.
        /// </summary>
        CommandDrain = 0x00000001,

        /// <summary>
        /// Sets or clears the Direct3D Device Manager for DirectX Video Accereration (DXVA).
        /// </summary>
        SetD3DManager = 0x00000002,

        DropSamples = 0x00000003,
        CommandTrick = 0x00000004,

        /// <summary>
        /// Notifies the MFT that streaming is about to begin.
        /// </summary>
        NotifyBeginStreaming = 0x10000000,

        /// <summary>
        /// Notifies the MFT that streaming is about to end.
        /// </summary>
        NotifyEndStreaming = 0x10000001,

        /// <summary>
        /// Notifies the MFT that an input stream has ended.
        /// </summary>
        NotifyEndOfStream = 0x10000002,

        /// <summary>
        /// Notifies the MFT that the first sample is about to be processed.
        /// </summary>
        NotifyStartOfStream = 0x10000003,

        CommandMarker = 0x20000000
    }
}