using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Represents a description of a media format. 
    /// </summary>
    [Guid("44ae0fa8-ea31-4109-8d2e-4cae4997c555")]
// ReSharper disable once InconsistentNaming
    public class MFMediaType : MFAttributes
    {
        /// <summary>
        /// Creates an empty <see cref="MFMediaType"/>.
        /// </summary>
        /// <returns>Returns an empty <see cref="MFMediaType"/>.</returns>
        public static MFMediaType CreateEmpty()
        {
            MediaFoundationCore.Startup();
            return MediaFoundationCore.CreateMediaType();
        }

        /// <summary>
        /// Creates a new <see cref="MFMediaType"/> based on a specified <paramref name="waveFormat"/>.
        /// </summary>
        /// <param name="waveFormat"><see cref="WaveFormat"/> which should be "converted" to a <see cref="MFMediaType"/>.</param>
        /// <returns>Returns a new <see cref="MFMediaType"/>.</returns>
        public static MFMediaType FromWaveFormat(WaveFormat waveFormat)
        {
            MediaFoundationCore.Startup();
            return MediaFoundationCore.MediaTypeFromWaveFormat(waveFormat);
        }

        private const string InterfaceName = "IMFMediaType";

        /// <summary>
        /// Initializes a new instance of the <see cref="MFMediaType"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public MFMediaType(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets or sets the number of channels.
        /// </summary>
        public int Channels
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_NUM_CHANNELS); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_NUM_CHANNELS, value); }
        }

        /// <summary>
        /// Gets or sets the number of bits per sample.
        /// </summary>
        public int BitsPerSample
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_BITS_PER_SAMPLE); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_BITS_PER_SAMPLE, value); }
        }

        /// <summary>
        /// Gets or sets the number of samples per second (for one channel each).
        /// </summary>
        public int SampleRate
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_SAMPLES_PER_SECOND); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_SAMPLES_PER_SECOND, value); }
        }

        /// <summary>
        /// Gets or sets the channelmask.
        /// </summary>
        public ChannelMask ChannelMask
        {
            get { return (ChannelMask) GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_CHANNEL_MASK); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_CHANNEL_MASK, (int) value); }
        }

        /// <summary>
        /// Gets or sets the average number of bytes per second.
        /// </summary>
        public int AverageBytesPerSecond
        {
            get { return GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_AVG_BYTES_PER_SECOND); }
            set { SetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_AVG_BYTES_PER_SECOND, value); }
        }

        /// <summary>
        /// Gets or sets the audio subtype.
        /// </summary>
        public Guid SubType
        {
            get { return GetGuid(MediaFoundationAttributes.MF_MT_SUBTYPE); }
            set { SetGuid(MediaFoundationAttributes.MF_MT_SUBTYPE, value); }
        }

        /// <summary>
        /// Gets or sets the major type.
        /// </summary>
        public Guid MajorType
        {
            get { return GetGuid(MediaFoundationAttributes.MF_MT_MAJOR_TYPE); }
            set { SetGuid(MediaFoundationAttributes.MF_MT_MAJOR_TYPE, value); }
        }

        /// <summary>
        /// Gets a value, indicating whether the media type is a temporally compressed format.
        /// Temporal compression uses information from previously decoded samples when 
        /// decompressing the current sample.
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
        /// <param name="majorType">Receives the major type <see cref="Guid"/>. 
        /// The major type describes the broad category of the format, such as audio or video. For a list of possible values, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa367377(v=vs.85).aspx"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMajorTypeNative(out Guid majorType)
        {
            majorType = default(Guid);
            fixed (void* ptr = &majorType)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, new IntPtr(ptr), ((void**)(*(void**)UnsafeBasePtr))[33]);
            }
        }

        /// <summary>
        /// Gets the major type of the format.
        /// </summary>
        /// <returns>The major type <see cref="Guid"/>. The major type describes the broad category of the format, such as audio or video. For a list of possible values, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa367377(v=vs.85).aspx"/>.</returns>
        public Guid GetMajorType()
        {
            Guid result;
            MediaFoundationException.Try(GetMajorTypeNative(out result), InterfaceName, "GetMajorType");
            return result;
        }

        /// <summary>
        /// Queries whether the media type is a temporally compressed format. Temporal compression
        /// uses information from previously decoded samples when decompressing the current sample.
        /// </summary>
        /// <param name="iscompressed">Receives a Boolean value. The value is <c>TRUE</c> if the format uses temporal compression, or <c>FALSE</c> if the format does not use temporal compression.</param>
        /// <returns>HRESULT</returns>
        public unsafe int IsCompressedFormatNative(out NativeBool iscompressed)
        {
            iscompressed = default(NativeBool);
            fixed (void* ptr = &iscompressed)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, new IntPtr(ptr), ((void**)(*(void**)UnsafeBasePtr))[34]);
            }
        }

        /// <summary>
        /// Queries whether the media type is a temporally compressed format. Temporal compression
        /// uses information from previously decoded samples when decompressing the current sample.
        /// </summary>
        /// <returns><see cref="NativeBool.True"/> if the format uses temporal compression. <see cref="NativeBool.False"/> if the format does not use temporal compression.</returns>
        public NativeBool IsCompressedFormat()
        {
            NativeBool result;
            MediaFoundationException.Try(IsCompressedFormatNative(out result), InterfaceName, "IsCompressedFormat");
            return result;
        }

        /// <summary>
        /// Compares two media types and determines whether they are identical. If they are not
        /// identical, the method indicates how the two formats differ.
        /// </summary>
        /// <param name="mediaType">The <see cref="MFMediaType"/> to compare.</param>
        /// <param name="flags">Receives a bitwise OR of zero or more flags, indicating the degree of similarity between the two media types.</param>
        /// <returns>HRESULT</returns>
        public unsafe int IsEqualNative(MFMediaType mediaType, out MediaTypeEqualFlags flags)
        {
            fixed (void* ptr = &flags)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, (void*)((mediaType == null) ? IntPtr.Zero : mediaType.BasePtr), new IntPtr(ptr), ((void**)(*(void**)UnsafeBasePtr))[35]);
            }
        }

        /// <summary>
        /// Compares two media types and determines whether they are identical. If they are not
        /// identical, the method indicates how the two formats differ.
        /// </summary>
        /// <param name="mediaType">The <see cref="MFMediaType"/> to compare.</param>
        /// <returns>A bitwise OR of zero or more flags, indicating the degree of similarity between the two media types.</returns>
        public MediaTypeEqualFlags IsEqual(MFMediaType mediaType)
        {
            MediaTypeEqualFlags flags;
            MediaFoundationException.Try(IsEqualNative(mediaType, out flags), InterfaceName, "IsEqual");
            return flags;
        }

        /// <summary>
        /// Retrieves an alternative representation of the media type. Currently only the DirectShow
        /// AM_MEDIA_TYPE structure is supported.
        /// </summary>
        /// <param name="guidRepresentation"><see cref="Guid"/> that specifies the representation to retrieve. The following values are defined.</param>
        /// <param name="representation">Receives a pointer to a structure that contains the representation. The method allocates the memory for the structure. The caller must release the memory by calling <see cref="FreeRepresentation"/>.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms695248(v=vs.85).aspx"/>.</remarks>
        public unsafe int GetRepresentationNative(Guid guidRepresentation, out IntPtr representation)
        {
            fixed (void* ptr = &representation)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, guidRepresentation, new IntPtr(ptr), ((void**)(*(void**)UnsafeBasePtr))[36]);
            }
        }

        /// <summary>
        /// Retrieves an alternative representation of the media type. Currently only the DirectShow
        /// AM_MEDIA_TYPE structure is supported.
        /// </summary>
        /// <param name="guidRepresentation"><see cref="Guid"/> that specifies the representation to retrieve. The following values are defined.</param>
        /// <returns>A pointer to a structure that contains the representation. The method allocates the memory for the structure. The caller must release the memory by calling <see cref="FreeRepresentation"/>.</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms695248(v=vs.85).aspx"/>.</remarks>        
        public IntPtr GetRepresentation(Guid guidRepresentation)
        {
            IntPtr result;
            MediaFoundationException.Try(GetRepresentationNative(guidRepresentation, out result), InterfaceName, "GetRepresentation");
            return result;
        }

        /// <summary>
        /// Frees memory that was allocated by the <see cref="GetRepresentation"/> method.
        /// </summary>
        /// <param name="guidRepresentation"><see cref="Guid"/> that was passed to the <see cref="GetRepresentation"/> method.</param>
        /// <param name="representation">Pointer to the buffer that was returned by the <see cref="GetRepresentation"/> method.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms703846(v=vs.85).aspx"/>.</remarks>        
        public unsafe int FreeRepresentationNative(Guid guidRepresentation, IntPtr representation)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, guidRepresentation, representation, ((void**)(*(void**)UnsafeBasePtr))[37]);
        }

        /// <summary>
        /// Frees memory that was allocated by the <see cref="GetRepresentation"/> method.
        /// </summary>
        /// <param name="guidRepresentation"><see cref="Guid"/> that was passed to the <see cref="GetRepresentation"/> method.</param>
        /// <param name="representation">Pointer to the buffer that was returned by the <see cref="GetRepresentation"/> method.</param>
        /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms703846(v=vs.85).aspx"/>.</remarks>
        public void FreeRepresentation(Guid guidRepresentation, IntPtr representation)
        {
            MediaFoundationException.Try(FreeRepresentationNative(guidRepresentation, representation), InterfaceName, "FreeRepresentation");
        }

        /// <summary>
        /// Converts the <see cref="MFMediaType"/> to a <see cref="WaveFormat"/>.
        /// </summary>
        /// <param name="flags">Contains a flag from the <see cref="MFWaveFormatExConvertFlags"/> enumeration.</param>
        /// <returns>The <see cref="WaveFormat"/> which got created based on the <see cref="MFMediaType"/>.</returns>
        public unsafe WaveFormat ToWaveFormat(MFWaveFormatExConvertFlags flags)
        {
            IntPtr pointer = IntPtr.Zero;
            try
            {
                int cbSize;
                MediaFoundationException.Try(
                    NativeMethods.MFCreateWaveFormatExFromMFMediaType(BasePtr.ToPointer(), &pointer, &cbSize,
                        (int) flags), "Interop", "MFCreateWaveFormatExFromMFMediaType");
            }
            finally
            {
                Marshal.FreeCoTaskMem(pointer);
            }
            //var waveformat = (WaveFormat)Marshal.PtrToStructure(pointer, typeof(WaveFormat));
            //if (waveformat.WaveFormatTag == AudioEncoding.Extensible)
            //    waveformat = (WaveFormatExtensible)Marshal.PtrToStructure(pointer, typeof(WaveFormatExtensible));
            //return waveformat;
            return WaveFormatMarshaler.PointerToWaveFormat(pointer);
        }
    }
}