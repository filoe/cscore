using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFTEnumFlags
    {
        None = 0,
        SyncDataProcessing = 0x1,
        AsyncDataProcessing = 0x2,
        Hardware = 0x4,

        /// <summary>
        /// Must be unlocked by the app before use.
        /// </summary>
        FieldOfUse = 0x8,

        LocalMFT = 0x10,
        TranscodeOnly = 0x20,
        SortandFilter = 0x40,
        All = 0x3F
    }
}