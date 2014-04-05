using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Guid("44ae0fa8-ea31-4109-8d2e-4cae4997c555")]
    public class MFMediaType : MFAttributes
    {
        public static MFMediaType CreateEmpty()
        {
            MediaFoundationCore.Startup();
            return MediaFoundationCore.CreateMediaType();
        }

        public static MFMediaType FromWaveFormat(WaveFormat waveFormat)
        {
            MediaFoundationCore.Startup();
            return MediaFoundationCore.MediaTypeFromWaveFormat(waveFormat);
        }

        private const string c = "IMFMediaType";

        public MFMediaType(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Number of channels
        /// </summary>
        public int Channels
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_NUM_CHANNELS); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_NUM_CHANNELS, value); }
        }

        /// <summary>
        /// Bits per sample
        /// </summary>
        public int BitsPerSample
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_BITS_PER_SAMPLE); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_BITS_PER_SAMPLE, value); }
        }

        /// <summary>
        /// Samples per second(for one channel each)
        /// </summary>
        public int SampleRate
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_SAMPLES_PER_SECOND); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_SAMPLES_PER_SECOND, value); }
        }

        /// <summary>
        /// AVG bytes per second
        /// </summary>
        public int AverageBytesPerSecond
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_AVG_BYTES_PER_SECOND); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_AVG_BYTES_PER_SECOND, value); }
        }

        /// <summary>
        /// Audiosubtype
        /// </summary>
        public Guid SubType
        {
            get { return GetGuid(MediaFoundationAttributes.MF_MT_SUBTYPE); }
            set { SetGuid(MediaFoundationAttributes.MF_MT_SUBTYPE, value); }
        }

        /// <summary>
        /// Majortype
        /// </summary>
        public Guid MajorType
        {
            get { return GetGuid(MediaFoundationAttributes.MF_MT_MAJOR_TYPE); }
            set { SetGuid(MediaFoundationAttributes.MF_MT_MAJOR_TYPE, value); }
        }

        /// <summary>
        /// Queries whether the media type is a temporally compressed format. Temporal compression
        /// uses information from previously decoded samples when decompressing the current sample.
        /// </summary>
        public NativeBool IsCompressed
        {
            get
            {
                return IsCompressedFormat();
            }
        }

        /// <summary>
        /// Gets the major type of the format.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetMajorType(out Guid majorType)
        {
            majorType = default(Guid);
            fixed (void* ptr = &majorType)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, new IntPtr(ptr), ((void**)(*(void**)_basePtr))[33]);
            }
        }

        /// <summary>
        /// Gets the major type of the format.
        /// </summary>
        public Guid GetMajorType()
        {
            Guid result;
            MediaFoundationException.Try(GetMajorType(out result), c, "GetMajorType");
            return result;
        }

        /// <summary>
        /// Queries whether the media type is a temporally compressed format. Temporal compression
        /// uses information from previously decoded samples when decompressing the current sample.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int IsCompressedFormat(out NativeBool iscompressed)
        {
            iscompressed = default(NativeBool);
            fixed (void* ptr = &iscompressed)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, new IntPtr(ptr), ((void**)(*(void**)_basePtr))[34]);
            }
        }

        /// <summary>
        /// Queries whether the media type is a temporally compressed format. Temporal compression
        /// uses information from previously decoded samples when decompressing the current sample.
        /// </summary>
        public NativeBool IsCompressedFormat()
        {
            NativeBool result;
            MediaFoundationException.Try(IsCompressedFormat(out result), c, "IsCompressedFormat");
            return result;
        }

        /// <summary>
        /// Compares two media types and determines whether they are identical. If they are not
        /// identical, the method indicates how the two formats differ.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int IsEqual(MFMediaType mediaType, out MediaTypeEqualFlags flags)
        {
            fixed (void* ptr = &flags)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, (void*)((mediaType == null) ? IntPtr.Zero : mediaType.BasePtr), new IntPtr(ptr), ((void**)(*(void**)_basePtr))[35]);
            }
        }

        /// <summary>
        /// Compares two media types and determines whether they are identical. If they are not
        /// identical, the method indicates how the two formats differ.
        /// </summary>
        public MediaTypeEqualFlags IsEqual(MFMediaType mediaType)
        {
            MediaTypeEqualFlags flags;
            MediaFoundationException.Try(IsEqual(mediaType, out flags), c, "IsEqual");
            return flags;
        }

        /// <summary>
        /// Retrieves an alternative representation of the media type. Currently only the DirectShow
        /// AM_MEDIA_TYPE structure is supported.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetRepresentation(Guid guidRepresenation, out IntPtr representation)
        {
            fixed (void* ptr = &representation)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, guidRepresenation, new IntPtr(ptr), ((void**)(*(void**)_basePtr))[36]);
            }
        }

        /// <summary>
        /// Retrieves an alternative representation of the media type. Currently only the DirectShow
        /// AM_MEDIA_TYPE structure is supported.
        /// </summary>
        public IntPtr GetRepresentation(Guid guidRepresentation)
        {
            IntPtr result;
            MediaFoundationException.Try(GetRepresentation(guidRepresentation, out result), c, "GetRepresentation");
            return result;
        }

        /// <summary>
        /// Frees memory that was allocated by the IMFMediaType::GetRepresentation method.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int FreeRepresentationNative(Guid guidRepresentation, IntPtr representation)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, guidRepresentation, representation, ((void**)(*(void**)_basePtr))[37]);
        }

        /// <summary>
        /// Frees memory that was allocated by the IMFMediaType::GetRepresentation method.
        /// </summary>
        public void FreeRepresentation(Guid guidRepresentation, IntPtr representation)
        {
            MediaFoundationException.Try(FreeRepresentationNative(guidRepresentation, representation), c, "FreeRepresentation");
        }

        public unsafe WaveFormat ToWaveFormat(out int bufferSize, MFWaveFormatExConvertFlags flags)
        {
            IntPtr pointer = IntPtr.Zero;
            fixed (void* ptr = &bufferSize)
            {
                MediaFoundationException.Try(MFCreateWaveFormatExFromMFMediaType(BasePtr.ToPointer(), (void*)(&pointer), (void*)(ptr), (int)flags), "interop", "MFCreateWaveFormatExFromMFMediaType");
            }
            var waveformat = (WaveFormat)Marshal.PtrToStructure(pointer, typeof(WaveFormat)); //todo:
            if (waveformat.WaveFormatTag == AudioEncoding.Extensible)
                waveformat = (WaveFormatExtensible)Marshal.PtrToStructure(pointer, typeof(WaveFormatExtensible));
            return waveformat;
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport("Mfplat.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "MFCreateWaveFormatExFromMFMediaType")]
        private unsafe static extern int MFCreateWaveFormatExFromMFMediaType(void* arg0, void* arg1, void* arg2, int arg3);
    }
}