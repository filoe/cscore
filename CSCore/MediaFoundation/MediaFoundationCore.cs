using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    public static class MediaFoundationCore
    {
        public const int MF_SDK_VERSION = 0x2;
        public const int MF_API_VERSION = 0x70;
        public const int MF_VERSION = (MF_SDK_VERSION << 16) | MF_API_VERSION;

        public const int MF_SOURCE_READER_FIRST_AUDIO_STREAM = unchecked((int)0xFFFFFFFD);
        public const int MF_SOURCE_READER_ALL_STREAMS = unchecked((int)0xFFFFFFFE);

        public const int MF_SOURCE_READER_MEDIASOURCE = unchecked((int)0xFFFFFFFF); //pass this to mfattributes streamindex arguments

        /// <summary>
        /// </summary>
        /// <param name="category">See CSCore.MediaFoundation.MFTCategories</param>
        public static IEnumerable<MFActivate> EnumerateTransforms(Guid category, MFTEnumFlags flags)
        {
            IntPtr ptr;
            int count;
            int res = MFInterops.MFTEnumEx(category, flags, null, null, out ptr, out count);
            MediaFoundationException.Try(res, "Interops", "MFTEnumEx");
            for (int i = 0; i < count; i++)
            {
                var ptr0 = ptr;
                var ptr1 = Marshal.ReadIntPtr(new IntPtr(ptr0.ToInt64() + i * Marshal.SizeOf(ptr0)));
                yield return new MFActivate(ptr1);
            }

            Marshal.FreeCoTaskMem(ptr);
        }

        public static bool IsTransformAvailable(IEnumerable<MFActivate> transforms, Guid transformGuid)
        {
            foreach (var t in transforms)
            {
                var value = (Guid)t[MediaFoundationAttributes.MFT_TRANSFORM_CLSID_Attribute];
                if (value == transformGuid)
                    return true;
            }
            return false;
        }

        public static IMFByteStream IStreamToByteStream(IStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            IMFByteStream result;
            MediaFoundationException.Try(MFInterops.MFCreateMFByteStreamOnStream(stream, out result), "Interops", "MFCreateMFByteStreamOnStreamEx");
            return result;
        }

        public static MFSourceReader CreateSourceReaderFromByteStream(IntPtr byteStream, IntPtr attributes)
        {
            if (byteStream == IntPtr.Zero)
                throw new ArgumentNullException("byteStream");

            IntPtr result = IntPtr.Zero;
            MediaFoundationException.Try(MFInterops.MFCreateSourceReaderFromByteStream(byteStream, attributes, out result), "Interops", "MFCreateSourceReaderFromByteStream");
            return new MFSourceReader(result);
        }

        public static MFSourceReader CreateSourceReaderFromByteStream(IMFByteStream byteStream, IntPtr attributes)
        {
            return CreateSourceReaderFromByteStream(Marshal.GetComInterfaceForObject(byteStream, typeof(IMFByteStream)), attributes);
        }

        public static MFSourceReader CreateSourceReaderFromUrl(string url)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = MFInterops.MFCreateSourceReaderFromURL(url, IntPtr.Zero, out ptr);
            MediaFoundationException.Try(result, "Interops", "MFCreateSourceReaderFromURL");
            return new MFSourceReader(ptr);
        }

        private static bool _isstarted = false;

        public static void Startup()
        {
            if (!_isstarted)
            {
                MediaFoundationException.Try(MFInterops.MFStartup(MFInterops.MF_VERSION, 0), "Interops", "MFStartup");
                _isstarted = true;
            }
        }

        public static void Shutdown()
        {
            if (_isstarted)
            {
                MediaFoundationException.Try(MFInterops.MFShutdown(), "Interops", "MFShutdown");
                _isstarted = false;
            }
        }

        public static MFMediaType CreateMediaType()
        {
            IntPtr mediaType;
            MediaFoundationException.Try(MFInterops.MFCreateMediaType(out mediaType), "Interops", "MFCreateMediaType");
            return new MFMediaType(mediaType);
        }
    }
}