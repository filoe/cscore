namespace CSCore.DirectSound
{
    /// <summary>
    /// Defines possible DirectSound return values.
    /// </summary>
    /// <remarks>For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ee416775%28v=vs.85%29.aspx"/>.</remarks>
    public enum DSResult
    {
        /// <summary>
        /// The method succeeded.
        /// </summary>
        Ok = 0x00000000,
        /// <summary>
        /// The DirectSound subsystem could not allocate sufficient memory to complete the caller's request.
        /// </summary>
        OutOfMemory = 0x00000007,
        /// <summary>
        /// The requested COM interface is not available.
        /// </summary>
        NoInterface = 0x000001AE,
        /// <summary>
        /// The buffer was created, but another 3D algorithm was substituted.
        /// </summary>
        NoVirtualization = 0x0878000A,
        /// <summary>
        /// The method succeeded, but not all the optional effects were obtained.
        /// </summary>
        Incomplete = 0x08780014,
        /// <summary>
        /// The function called is not supported at this time.
        /// </summary>
        Unsupported = unchecked((int)0x80004001),
        /// <summary>
        /// An undetermined error occurred inside the DirectSound subsystem.
        /// </summary>
        Generic = unchecked((int)0x80004005),
        /// <summary>
        /// The request failed because access was denied.
        /// </summary>
        AccessDenied = unchecked((int)0x80070005),
        /// <summary>
        /// An invalid parameter was passed to the returning function.
        /// </summary>
        InvalidParam = unchecked((int)0x80070057),
        /// <summary>
        /// The request failed because resources, such as a priority level, were already in use by another caller.
        /// </summary>
        Allocated = unchecked((int)0x8878000A),
        /// <summary>
        /// The buffer control (volume, pan, and so on) requested by the caller is not available. Controls must be specified when the buffer is created, using the <see cref="DSBufferDescription.Flags"/> member of <see cref="DSBufferDescription"/>.
        /// </summary>
        ControlUnavail = unchecked((int)0x8878001E),
        /// <summary>
        /// This function is not valid for the current state of this object.
        /// </summary>
        InvalidCall = unchecked((int)0x88780032),
        /// <summary>
        /// A cooperative level of <see cref="DSCooperativeLevelType.Priority"/> or higher is required.
        /// </summary>
        PrioLevelNeeded = unchecked((int)0x88780046),
        /// <summary>
        /// The specified wave format is not supported.
        /// </summary>
        BadFormat = unchecked((int)0x88780064),
        /// <summary>
        /// No sound driver is available for use, or the given GUID is not a valid DirectSound device ID.
        /// </summary>
        NoDriver = unchecked((int)0x88780078),
        /// <summary>
        /// The object is already initialized.
        /// </summary>
        AlreadyInitialized = unchecked((int)0x88780082),
        /// <summary>
        /// The buffer memory has been lost and must be restored.
        /// </summary>
        BufferLost = unchecked((int)0x88780096),
        /// <summary>
        /// Another application has a higher priority level, preventing this call from succeeding.
        /// </summary>
        OtherAppHasPrio = unchecked((int)0x887800A0),
        /// <summary>
        /// The <see cref="DirectSoundBase.Initialize"/> method has not been called or has not been called successfully before other methods were called.
        /// </summary>
        Uninitialized = unchecked((int)0x887800AA),
        /// <summary>
        /// The buffer size is not great enough to enable effects processing.
        /// </summary>
        BufferTooSmall = unchecked((int)0x887800b4),
        /// <summary>
        /// A DirectSound object of class CLSID_DirectSound8 or later is required for the requested functionality.
        /// </summary>
        DirectSound8Required = unchecked((int)0x887810BE),
        /// <summary>
        /// A circular loop of send effects was detected.
        /// </summary>
        SendLoop = unchecked((int)0x887810C8),
        /// <summary>
        /// The GUID specified in an audiopath file does not match a valid mix-in buffer.
        /// </summary>
        BadSendBufferGuid = unchecked((int)0x887810D2),
        /// <summary>
        /// The effects requested could not be found on the system, or they are in the wrong order or in the wrong location; for example, an effect expected in hardware was found in software.
        /// </summary>
        FxUnavailable = unchecked((int)0x887810DC),
        /// <summary>
        /// The requested object was not found.
        /// </summary>
        ObjectNotFound = unchecked((int)0x88781161),
    }
}