namespace CSCore.DirectSound
{
    /// <summary>
    ///     Defines cooperative levels which can be set by calling the <see cref="DirectSoundBase.SetCooperativeLevel" />
    ///     method.
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.idirectsound8.idirectsound8.setcooperativelevel(v=vs.85).aspx"/>.</remarks>
    public enum DSCooperativeLevelType
    {
        /// <summary>
        ///     Sets the normal level. This level has the smoothest multitasking and resource-sharing behavior, but because it does
        ///     not allow the primary buffer format to change, output is restricted to the default 8-bit format.
        /// </summary>
        Normal = 0x00000001,

        /// <summary>
        ///     Sets the priority level. Applications with this cooperative level can call the SetFormat and Compact methods.
        /// </summary>
        Priority = 0x00000002,

        /// <summary>
        ///     For DirectX 8.0 and later, has the same effect as <see cref="Priority" />. For previous versions, sets the
        ///     application to the exclusive level. This means that when it has the input focus, the application will be the only
        ///     one audible; sounds from applications with the GlobalFocus flag set will be muted. With this level, it also
        ///     has all the privileges of the DSSCL_PRIORITY level. DirectSound will restore the hardware format, as specified by
        ///     the most recent call to the SetFormat method, after the application gains the input focus.
        /// </summary>
        Exclusive = 0x00000003,

        /// <summary>
        ///     Sets the write-primary level. The application has write access to the primary buffer. No secondary buffers can be
        ///     played. This level cannot be set if the DirectSound driver is being emulated for the device; that is, if the
        ///     GetCaps method returns the DSCAPS_EMULDRIVER flag in the DSCAPS structure.
        /// </summary>
        WritePrimary = 0x00000004
    }
}