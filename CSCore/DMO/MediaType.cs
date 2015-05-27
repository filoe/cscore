using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    /// <summary>
    ///     The <see cref="MediaType"/> structure describes the format of the data used by a stream in a Microsoft DirectX Media Object (DMO).
    ///     For more information, <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd375504(v=vs.85).aspx"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaType : IDisposable
    {
        private static readonly Guid FORMAT_WaveFormatEx = new Guid("05589f81-c356-11ce-bf01-00aa0055595a");

        /// <summary>
        ///     Creates a MediaType based on a given WaveFormat. Don't forget to call Free() for the returend MediaType.
        /// </summary>
        /// <param name="waveFormat">WaveFormat to create a MediaType from.</param>
        /// <returns>Dmo MediaType</returns>
        public static MediaType FromWaveFormat(WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");

            var mediaType = new MediaType();
            NativeMethods.MoInitMediaType(ref mediaType, Marshal.SizeOf(waveFormat));

            mediaType.MajorType = AudioSubTypes.MediaTypeAudio;
            mediaType.SubType = WaveFormatExtensible.SubTypeFromWaveFormat(waveFormat);
            mediaType.FixedSizeSamples = (mediaType.SubType == AudioSubTypes.IeeeFloat ||
                                          mediaType.SubType == AudioSubTypes.Pcm)
                ? 1
                : 0;
            mediaType.FormatType = FORMAT_WaveFormatEx;

            IntPtr hWaveFormat = Marshal.AllocHGlobal(Marshal.SizeOf(waveFormat));

            Marshal.StructureToPtr(waveFormat, hWaveFormat, false);

            if (hWaveFormat == IntPtr.Zero)
                throw new InvalidOperationException("hWaveFormat == IntPtr.Zero");
            if (mediaType.CbFormat < Marshal.SizeOf(waveFormat))
                throw new InvalidOperationException("No memory for Format reserved");
            mediaType.PtrFormat = hWaveFormat;

            return mediaType;
        }

        /// <summary>
        ///     A GUID identifying the stream's major media type. This must be one of the DMO Media
        ///     Types(see <see href="http://msdn.microsoft.com/en-us/library/aa924843.aspx"/>).
        /// </summary>
        public Guid MajorType;

        /// <summary>
        ///     Subtype GUID of the stream.
        /// </summary>
        public Guid SubType;

        /// <summary>
        ///     If TRUE, samples are of a fixed size. This field is informational only. For audio, it is
        ///     generally set to TRUE. For video, it is usually TRUE for uncompressed video and FALSE
        ///     for compressed video.
        /// </summary>
        public int FixedSizeSamples;

        /// <summary>
        ///     If TRUE, samples are compressed using temporal (interframe) compression. A value of TRUE
        ///     indicates that not all frames are key frames. This field is informational only.
        /// </summary>
        public int TemporalCompression;

        /// <summary>
        ///     Size of the sample, in bytes. For compressed data, the value can be zero.
        /// </summary>
        public int SampleSize;

        /// <summary>
        ///     GUID specifying the format type. The pbFormat member points to the corresponding format
        ///     structure. (see <see href="http://msdn.microsoft.com/en-us/library/aa929922.aspx"/>)
        /// </summary>
        public Guid FormatType;

        //not used
        private readonly IntPtr pUnk;

        /// <summary>
        ///     Size of the format block of the media type.
        /// </summary>
        public int CbFormat;

        /// <summary>
        ///     Pointer to the format structure. The structure type is specified by the formattype
        ///     member. The format structure must be present, unless formattype is GUID_NULL or
        ///     FORMAT_None.
        /// </summary>
        public IntPtr PtrFormat;

        /// <summary>
        /// Frees the allocated members of a media type structure by calling the <c>MoFreeMediaType</c> function.
        /// </summary>
        public void Dispose()
        {
            NativeMethods.MoFreeMediaType(ref this);
        }
    }
}