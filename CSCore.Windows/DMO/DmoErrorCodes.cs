namespace CSCore.DMO
{
    /// <summary>
    ///     Error codes that are specific to Microsoft DirectX Media Objects.
    /// </summary>
    public enum DmoErrorCodes
    {
        /// <summary>
        ///     Invalid stream index.
        /// </summary>
        DMO_E_INVALIDSTREAMINDEX = unchecked((int) 0x80040201),

        /// <summary>
        ///     Invalid media type.
        /// </summary>
        DMO_E_INVALIDTYPE = unchecked((int) 0x80040202),

        /// <summary>
        ///     Media type was not set. One or more streams require a media type before this operation can be performed.
        /// </summary>
        DMO_E_TYPE_NOT_SET = unchecked((int) 0x80040203),

        /// <summary>
        ///     Data cannot be accepted on this stream. You might need to process more output data; see MediaObject::ProcessInput
        ///     (-> http://msdn.microsoft.com/en-us/library/windows/desktop/dd406959(v=vs.85).aspx).
        /// </summary>
        DMO_E_NOTACCEPTING = unchecked((int) 0x80040204),

        /// <summary>
        ///     Media type was not accepted.
        /// </summary>
        DMO_E_TYPE_NOT_ACCEPTED = unchecked((int) 0x80040205),

        /// <summary>
        ///     Media-type index is out of range.
        /// </summary>
        DMO_E_NO_MORE_ITEMS = unchecked((int) 0x80040206)
    }
}