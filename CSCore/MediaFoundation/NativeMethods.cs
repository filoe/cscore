using System.Security;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
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
        public static extern int MFCreateMFByteStreamOnStream(IStream stream, [Out] out IntPtr byteStream);

        [DllImport("mfplat.dll", EntryPoint = "MFCreateMFByteStreamOnStream")]
        public static extern int MFCreateMFByteStreamOnStreamPtr(IStream stream, out IntPtr byteStream);

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

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFTEnum([In] Guid category, [In] int flags,
            [In] MFTRegisterTypeInfo inputtype, [In] MFTRegisterTypeInfo outputType, [In] IntPtr pAttributes,
            [Out] out IntPtr ppclsid, [Out] out int mftCount);

        [DllImport("Mfreadwrite.dll", EntryPoint="MFCreateSinkWriterFromURL")]
        public static extern int ExternMFCreateSinkWriterFromURL(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszOutputURL,
            [In] IntPtr pByteStream,
            [In] IntPtr pAttributes,
            [Out] out IntPtr ppSinkWriter);

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFCreateMemoryBuffer(
            int cbMaxLength, [Out] out IntPtr ppBuffer);

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFCreateSample([Out] out IntPtr ppIMFSample);

        [DllImport("mfplat.dll", ExactSpelling = true)]
        public static extern int MFInitMediaTypeFromWaveFormatEx([In] IntPtr pMFType, [In] WaveFormat pWaveFormat, [In] int cbBufSize);

        [DllImport("mf.dll", ExactSpelling = true)]
        public static extern int MFTranscodeGetAudioOutputAvailableTypes(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidSubType,
            [In] MFTEnumFlags dwMFTFlags,
            [In] IntPtr pCodecConfig /*IMFAttributes*/,
            [Out, MarshalAs(UnmanagedType.Interface)] out IMFCollection ppAvailableTypes);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("Mfplat.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MFCreateAttributes")]
        internal static extern int MFCreateAttributes_(IntPtr ptr, int initialSize);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("Mfplat.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MFCreateWaveFormatExFromMFMediaType")]
        public unsafe static extern int MFCreateWaveFormatExFromMFMediaType(void* arg0, void* arg1, void* arg2, int arg3);
    }
}