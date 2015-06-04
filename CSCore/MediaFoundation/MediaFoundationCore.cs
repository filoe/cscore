using System.IO;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    internal static class MediaFoundationCore
    {
        public const int MF_SDK_VERSION = 0x2;
        public const int MF_API_VERSION = 0x70;
        public const int MF_VERSION = (MF_SDK_VERSION << 16) | MF_API_VERSION;

        public const int MF_SOURCE_READER_FIRST_AUDIO_STREAM = unchecked((int)0xFFFFFFFD);
        public const int MF_SOURCE_READER_ALL_STREAMS = unchecked((int)0xFFFFFFFE);

        public const int MF_SOURCE_READER_MEDIASOURCE = unchecked((int)0xFFFFFFFF); //pass this to mfattributes streamindex arguments

        public static bool IsSupported { get; private set; }

        static MediaFoundationCore()
        {
            try
            {
                Startup();
                IsSupported = true;

                AppDomain.CurrentDomain.ProcessExit += (s, e) => Shutdown();
            }
            catch (Exception)
            {
                IsSupported = false;
            }
        }

        public static IntPtr CreateSinkWriterFromMFByteStreamNative(MFByteStream byteStream, MFAttributes attributes)
        {
            IntPtr p;
            int result = NativeMethods.ExternMFCreateSinkWriterFromURL(null, byteStream.BasePtr, attributes.BasePtr, out p);
            MediaFoundationException.Try(result, "Interops", "MFCreateSinkWriterFromURL");
            return p;
        }

        public static bool IsTransformAvailable(IEnumerable<MFActivate> transforms, Guid transformGuid)
        {
            try
            {
                return
                    transforms.Select(t => (Guid) t[MediaFoundationAttributes.MFT_TRANSFORM_CLSID_Attribute])
                        .Any(value => value == transformGuid);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsTransformAvailable(Guid category, Guid transformClsid)
        {
            var clsids = MFTEnumerator.EnumerateTransforms(category);
            return clsids.Any(x => x == transformClsid);
        }

        public static MFByteStream IStreamToByteStream(IStream stream)
        {
            return new MFByteStream(IStreamToByteStreamNative(stream));
        }
        public static IntPtr IStreamToByteStreamNative(IStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            IntPtr result;
            MediaFoundationException.Try(NativeMethods.MFCreateMFByteStreamOnStream(stream, out result), "Interops", "MFCreateMFByteStreamOnStream");
            return result;
        }


        public static IntPtr StreamToByteStreamNative(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return IStreamToByteStreamNative(new ComStream(stream));
        }

        public static MFSourceReader CreateSourceReaderFromByteStream(IntPtr byteStream, IntPtr attributes)
        {
            return new MFSourceReader(CreateSourceReaderFromByteStreamNative(byteStream, attributes));
        }

        public static IntPtr CreateSourceReaderFromByteStreamNative(IntPtr byteStream, IntPtr attributes)
        {
            if (byteStream == IntPtr.Zero)
                throw new ArgumentNullException("byteStream");

            IntPtr result = IntPtr.Zero;
            MediaFoundationException.Try(NativeMethods.MFCreateSourceReaderFromByteStream(byteStream, attributes, out result), "Interops", "MFCreateSourceReaderFromByteStream");
            return result;
        }

        public static MFSourceReader CreateSourceReaderFromUrl(string url)
        {
            return new MFSourceReader(CreateSourceReaderFromUrlNative(url));
        }

        public static IntPtr CreateSourceReaderFromUrlNative(string url)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = NativeMethods.MFCreateSourceReaderFromURL(url, IntPtr.Zero, out ptr);
            MediaFoundationException.Try(result, "Interops", "MFCreateSourceReaderFromURL");
            return ptr;
        }

        private static bool _isstarted;

        public static void Startup()
        {
            if (!_isstarted)
            {
                MediaFoundationException.Try(NativeMethods.MFStartup(NativeMethods.MF_VERSION, 0), "Interops", "MFStartup");
                _isstarted = true;
            }
        }

        public static void Shutdown()
        {
            if (_isstarted)
            {
                MediaFoundationException.Try(NativeMethods.MFShutdown(), "Interops", "MFShutdown");
                _isstarted = false;
            }
        }

        public static MFMediaType CreateMediaType()
        {
            IntPtr mediaType;
            MediaFoundationException.Try(NativeMethods.MFCreateMediaType(out mediaType), "Interops", "MFCreateMediaType");
            return new MFMediaType(mediaType);
        }

        public static IntPtr CreateMemoryBuffer(int size)
        {
            if(size <= 0)
                throw new ArgumentOutOfRangeException("size");
            IntPtr ptr;
            MediaFoundationException.Try(NativeMethods.MFCreateMemoryBuffer(size, out ptr), "Interops", "MFCreateMemoryBuffer");
            return ptr;
        }

        public static IntPtr CreateEmptySample()
        {
            IntPtr ptr;
            MediaFoundationException.Try(NativeMethods.MFCreateSample(out ptr), "Interops", "MFCreateSample");
            return ptr;
        }

        public static MFMediaType MediaTypeFromWaveFormat(WaveFormat waveFormat)
        {
            var mediaType = MFMediaType.CreateEmpty();
            int result = NativeMethods.MFInitMediaTypeFromWaveFormatEx(mediaType.BasePtr, waveFormat, Marshal.SizeOf(waveFormat));
            MediaFoundationException.Try(result, "Interops", "MFInitMediaTypeFromWaveFormatEx");
            return mediaType;
        }
    }
}