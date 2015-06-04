namespace CSCore.Win32
{
    /// <summary>
    /// Defines common HRESULT error codes.
    /// </summary>
    public enum HResult
    {
        /// <summary>
        /// S_OK
        /// </summary>
        S_OK = 0x0,
        /// <summary>
        /// S_FALSE
        /// </summary>
        S_FALSE = 0x1,
        /// <summary>
        /// E_ABORT
        /// </summary>
        E_ABORT = unchecked((int)0x80004004),
        /// <summary>
        /// E_ACCESSDENIED
        /// </summary>
        E_ACCESSDENIED = unchecked((int)0x80070005),
        /// <summary>
        /// E_NOINTERFACE
        /// </summary>
        E_NOINTERFACE = unchecked((int)0x80004002),
        /// <summary>
        /// E_FAIL
        /// </summary>
        E_FAIL = unchecked((int)0x80004005),
        /// <summary>
        /// E_INVALIDARG
        /// </summary>
        E_INVALIDARG = unchecked((int)0x80070057),
        /// <summary>
        /// E_POINTER
        /// </summary>
        E_POINTER = unchecked((int)0x80004003),
        /// <summary>
        /// E_NOTIMPL
        /// </summary>
        E_NOTIMPL = unchecked((int)0x80004001),
        /// <summary>
        /// MF_E_ATTRIBUTENOTFOUND
        /// </summary>
        MF_E_ATTRIBUTENOTFOUND = unchecked((int)0xC00D36E6),
        /// <summary>
        /// MF_E_SHUTDOWN
        /// </summary>
        MF_E_SHUTDOWN = unchecked((int)0xc00d3e85),

        /// <summary>
        /// AUDCLNT_E_UNSUPPORTED_FORMAT 
        /// </summary>
        AUDCLNT_E_UNSUPPORTED_FORMAT = unchecked ((int)0x88890008)
    }
}