using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFSourceReaderFlag
    {
        None = 0x0,
        Error = 0x00000001,
        EndOfStream = 0x00000002,
        NewStream = 0x00000004,
        NativeMediaTypeChanged = 0x00000010,
        CurrentMediaTypeChanged = 0x00000020,
        StreamTick = 0x00000100,
        AllEffectsRemoved = 0x00000200
    }
}