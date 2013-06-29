
namespace CSCore.Utils
{
    public enum HResult : int
    {
        S_OK = 0x0,
        E_INVALIDARG = unchecked((int)0x80070057),
        E_POINTER = unchecked((int)0x80004003),
    }
}
