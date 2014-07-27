using System;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFByteStreamCapsFlags
    {
        None = 0x0,
        IsReadable = 0x00000001,
        IsWriteable = 0x00000002,
        IsSeekable = 0x00000004,
        IsRemote = 0x00000008,
        IsDirectory = 0x00000080,
        HasSlowSeek = 0x00000100,
        IsPartiallyDownloaded = 0x00000200,
        ShareWrite = 0x00000400
    }
}