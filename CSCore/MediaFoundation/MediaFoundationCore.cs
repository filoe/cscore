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

        public static MFSinkWriter CreateSinkWriterFromMFByteStream(IMFByteStream byteStream, IMFAttributes attributes)
        {
            IntPtr p;
            int result = MFInterops.ExternMFCreateSinkWriterFromURL(null, byteStream, attributes, out p);
            MediaFoundationException.Try(result, "Interops", "MFCreateSinkWriterFromURL");
            return new MFSinkWriter(p);
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
            MediaFoundationException.Try(MFInterops.MFCreateMFByteStreamOnStreamEx(stream, out result), "Interops", "MFCreateMFByteStreamOnStreamEx");
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

        public static IMFAttributes CreateEmptyAttributes(uint initialSize)
        {
            IMFAttributes attributes;
            int result = MFInterops.ExternMFCreateAttributes(out attributes, initialSize);
            if (result < 0)
            {
                MediaFoundationException.Try(result, "Interops", "MFCreateAttributes");
            }

            return attributes;
        }

        public static IntPtr CreateMemoryBuffer(int length)
        {
            IntPtr ptr;
            MediaFoundationException.Try(MFInterops.MFCreateMemoryBuffer(length, out ptr), "Interops", "MFCreateMemoryBuffer");
            return ptr;
        }

        public static IntPtr CreateEmptySample()
        {
            IntPtr ptr;
            MediaFoundationException.Try(MFInterops.MFCreateSample(out ptr), "Interops", "MFCreateSample");
            return ptr;
        }

        public static MFMediaType MediaTypeFromWaveFormat(WaveFormat waveFormat)
        {
            var mediaType = MFMediaType.CreateEmpty();
            int result = MFInterops.MFInitMediaTypeFromWaveFormatEx(mediaType.BasePtr, waveFormat, Marshal.SizeOf(waveFormat));
            MediaFoundationException.Try(result, "Interops", "MFInitMediaTypeFromWaveFormatEx");
            return mediaType;
        }

        public static MFMediaType[] GetEncoderMediaTypes(Guid audioSubType)
        {
            IMFCollection collection;
            try
            {
                MediaFoundationException.Try(MFInterops.MFTranscodeGetAudioOutputAvailableTypes(audioSubType, MFTEnumFlags.All, null, out collection),
                    "Interops",
                    "MFTranscodeGetAudioOutputAvailableTypes");

                int count;
                MediaFoundationException.Try(collection.GetElementCount(out count), "IMFCollection", "GetElementCount");
                MFMediaType[] mediaTypes = new MFMediaType[count];
                for (int i = 0; i < count; i++)
                {
                    IntPtr ptr;
                    MediaFoundationException.Try(collection.GetElement(i, out ptr), "IMFCollection", "GetElement");

                    mediaTypes[i] = new MFMediaType(ptr);
                }

                Marshal.ReleaseComObject(collection);

                return mediaTypes;
            }
            catch (MediaFoundationException ex)
            {
                if (ex.ErrorCode == unchecked((int)0xC00D36D5)) // MF_E_NOT_FOUND
                {
                    return Enumerable.Empty<MFMediaType>().ToArray();
                }

                throw;
            }
        }

        public static int[] GetEncoderBitrates(Guid audioSubType, int sampleRate, int channels)
        {
            var mediaTypes = GetEncoderMediaTypes(audioSubType);
            List<int> bitRates = new List<int>();

            foreach (var mediaType in mediaTypes)
            {
                if (mediaType.SampleRate == sampleRate && mediaType.Channels == channels)
                {
                    int t = mediaType.AverageBytesPerSecond * 8;
                    if (!bitRates.Contains(t))
                        bitRates.Add(t);
                }
            }

            return bitRates.OrderBy(x => x).ToArray();
        }
    }
}