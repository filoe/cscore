
namespace CSCore.Win32
{
    public enum HResult : int
    {
        S_OK = 0x0,
        E_ABORT = unchecked((int)0x80004004),
        E_ACCESSDENIED = unchecked((int)0x80070005),
        E_NOINTERFACE = unchecked((int)0x80004002),
        E_FAIL = unchecked((int)0x80004005),
        E_INVALIDARG = unchecked((int)0x80070057),
        E_POINTER = unchecked((int)0x80004003),
        E_NOTIMPL = unchecked((int)0x80004001)
    }
}
