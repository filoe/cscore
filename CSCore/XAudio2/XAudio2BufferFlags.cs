namespace CSCore.XAudio2
{
    /// <summary>
    ///     Flags that provide additional information about the audio buffer.
    /// </summary>
    public enum XAudio2BufferFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     Indicates that there cannot be any buffers in the queue after this buffer. The only effect of this flag is to
        ///     suppress debug output warnings caused by starvation of the buffer queue.
        /// </summary>
        EndOfStream = 0x0040
    }
}