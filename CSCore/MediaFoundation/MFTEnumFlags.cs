using System;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines flags for registering and enumeration Media Foundation transforms (MFTs).
    /// </summary>
    [Flags]
    public enum MFTEnumFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// The MFT performs synchronous data processing in software. 
        /// This flag does not apply to hardware transforms.
        /// </summary>
        SyncDataProcessing = 0x1,
        /// <summary>
        /// The MFT performs asynchronous data processing in software. See <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd317909(v=vs.85).aspx"/>.
        /// This flag does not apply to hardware transforms.
        /// </summary>
        AsyncDataProcessing = 0x2,
        /// <summary>
        /// The MFT performs hardware-based data processing, using either the AVStream driver or a GPU-based proxy MFT. MFTs in this category always process data asynchronously.
        /// See <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd940330(v=vs.85).aspx"/>.
        /// </summary>
        Hardware = 0x4,

        /// <summary>
        /// Must be unlocked by the app before use. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd389302(v=vs.85).aspx"/>.
        /// </summary>
        FieldOfUse = 0x8,

        /// <summary>
        /// For enumeration, include MFTs that were registered in the caller's process.
        /// </summary>
        LocalMFT = 0x10,
        /// <summary>
        /// The MFT is optimized for transcoding rather than playback.
        /// </summary>
        TranscodeOnly = 0x20,
        /// <summary>
        /// For enumeration, sort and filter the results. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd389302(v=vs.85).aspx"/>.
        /// </summary>
        SortAndFilter = 0x40,
        /// <summary>
        /// Bitwise OR of all the flags, excluding <see cref="SortAndFilter"/>.
        /// </summary>
        All = 0x3F
    }
}