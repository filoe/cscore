using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.ACM
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AcmDriverDetails
    {
        public int cbStruct;
        /// <summary>
        /// Must be ACMDRIVERDETAILS_FCCTYPE_AUDIOCODEC for acm drivers
        /// </summary>
        public uint FourCCType;
        /// <summary>
        /// Currently not used. Default value = ACMDRIVERDETAILS_FCCCOMP_UNDEFINED 
        /// </summary>
        public uint FourCCComp;
        public ushort ManufacturerID;
        public ushort ProductID;
        public uint AcmVersion;
        public AcmDriverDetailsSupport SupportFlags;
        public int FormatTags;
        public int FilterTags;
        public IntPtr HIcon;
        /// <summary>
        /// Length = ACMDRIVERDETAILS_SHORTNAME_CHARS = 32 chars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string ShortName;
        /// <summary>
        /// Length = ACMDRIVERDETAILS_LONGNAME_CHARS = 128 chars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
        public string LongName;
        /// <summary>
        /// Length = ACMDRIVERDETAILS_COPYRIGHT_CHARS = 80 chars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string Copyright;
        /// <summary>
        /// Length = ACMDRIVERDETAILS_LICENSING_CHARS = 128 chars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Licensing;
        /// <summary>
        /// Length = ACMDRIVERDETAILS_FEATURES_CHARS = 512 chars
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Features;
    }
}
