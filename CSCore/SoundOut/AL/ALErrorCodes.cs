namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// Defines OpenAL Error Codes.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public enum ALErrorCode
    {
        /// <summary>
        /// No Error
        /// </summary>
        NoError = 0x0,

        /// <summary>
        /// Invalid Name
        /// </summary>
        InvalidName = 0xA001,

        /// <summary>
        /// Invalid Enum
        /// </summary>
        InvalidEnum = 0xA002,

        /// <summary>
        /// Invalid Value
        /// </summary>
        InvalidValue = 0xA003,

        /// <summary>
        /// Invalid Operation
        /// </summary>
        InvalidOperation = 0xA004,

        /// <summary>
        /// Out of Memory
        /// </summary>
        OutOfMemory = 0xA005
    }
}
