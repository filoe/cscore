using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    internal static class MFInterops
    {
        public const int MF_SDK_VERSION = 0x2;
        public const int MF_API_VERSION = 0x70;
        public const int MF_VERSION = (MF_SDK_VERSION << 16) | MF_API_VERSION;

        public const int MF_SOURCE_READER_FIRST_AUDIO_STREAM = unchecked((int)0xFFFFFFFD);
        public const int MF_SOURCE_READER_ALL_STREAMS = unchecked((int)0xFFFFFFFE);

        public const int MF_SOURCE_READER_MEDIASOURCE = unchecked((int)0xFFFFFFFF); //pass this to mfattributes streamindex arguments

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFStartup(int version, int dwFlags = 0);

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFShutdown();

        [DllImport("mfplat.dll")]
        public static extern int MFCreateMFByteStreamOnStream(IStream stream, out IMFByteStream byteStream);

        [DllImport("mfreadwrite.dll")]
        public static extern int MFCreateSourceReaderFromByteStream(IntPtr byteStream, IntPtr attributes, out IntPtr sourceReader);

        [DllImport("mfreadwrite.dll", ExactSpelling = true)]
        public static extern int MFCreateSourceReaderFromURL([In, MarshalAs(UnmanagedType.LPWStr)] string pwszURL, [In] IntPtr pAttributes,
                                                                [Out] out IntPtr ppSourceReader);

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFCreateMediaType(out IntPtr ppMFType);

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFTEnumEx([In] Guid category, [In] MFTEnumFlags enumflags,
            [In] MFTRegisterTypeInfo inputtype, [In] MFTRegisterTypeInfo outputType,
            [Out] out IntPtr pppMftActivate, [Out] out int mftCount);
    }
}