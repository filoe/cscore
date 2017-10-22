using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     Represents a peak meter on an audio stream to or from an audio endpoint device.
    ///     For more information, see
    ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd368227(v=vs.85).aspx" />.
    /// </summary>
    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")]
    public class AudioMeterInformation : ComObject
    {
        private const string InterfaceName = "IAudioMeterInformation";

        /// <summary>
        ///     Initializes a new instance of <see cref="AudioMeterInformation" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer.</param>
        public AudioMeterInformation(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets the number of channels in the audio stream that are monitored by peak meters.
        ///     <seealso cref="GetMeteringChannelCount" />
        ///     <seealso cref="GetMeteringChannelCountNative" />
        /// </summary>
        public int MeteringChannelCount
        {
            get { return GetMeteringChannelCount(); }
        }

        /// <summary>
        ///     Gets the peak sample value for the given <paramref name="channelIndex" />.
        ///     <seealso cref="GetChannelsPeakValues()" />
        ///     <seealso cref="GetChannelsPeakValues(int)" />
        ///     <seealso cref="GetChannelsPeakValuesNative" />
        /// </summary>
        /// <returns>The peak sample value for the given <paramref name="channelIndex" />.</returns>
        public float this[int channelIndex]
        {
            get
            {
                if (channelIndex >= MeteringChannelCount || channelIndex < 0)
                    throw new IndexOutOfRangeException("channelIndex");
                return GetChannelsPeakValues()[channelIndex];
            }
        }

        /// <summary>
        ///     Gets the hardware-supported functions.
        ///     <seealso cref="QueryHardwareSupport" />
        ///     <seealso cref="QueryHardwareSupportNative" />
        /// </summary>
        public EndpointHardwareSupportFlags HardwareSupport
        {
            get { return QueryHardwareSupport(); }
        }

        /// <summary>
        ///     Gets the peak sample value for the channels in the audio stream.
        ///     <seealso cref="GetPeakValue" />
        ///     <seealso cref="GetPeakValueNative" />
        /// </summary>
        public float PeakValue
        {
            get { return GetPeakValue(); }
        }

        /// <summary>
        ///     Creates a new <see cref="AudioMeterInformation" /> instance for the given <paramref name="device" />.
        /// </summary>
        /// <param name="device">The underlying device to create the audio meter instance for.</param>
        /// <returns>A new <see cref="AudioMeterInformation" /> instance for the given <paramref name="device" />.</returns>
        public static AudioMeterInformation FromDevice(MMDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            IntPtr ptr = device.Activate(new Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), CLSCTX.CLSCTX_ALL,
                IntPtr.Zero);
            return new AudioMeterInformation(ptr);
        }

        /// <summary>
        ///     Gets the peak sample value for the channels in the audio stream.
        /// </summary>
        /// <param name="peak">
        ///     A variable into which the method writes the peak sample value for the audio stream. The peak value
        ///     is a number in the normalized range from 0.0 to 1.0.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetPeakValueNative(out float peak)
        {
            fixed (void* ptr = &peak)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        ///     Gets the peak sample value for the channels in the audio stream.
        /// </summary>
        /// <returns>
        ///     The peak sample value for the audio stream. The peak value is a number in the normalized range from 0.0 to
        ///     1.0.
        /// </returns>
        public float GetPeakValue()
        {
            float peak;
            CoreAudioAPIException.Try(GetPeakValueNative(out peak), InterfaceName, "GetPeakValue");
            return peak;
        }

        /// <summary>
        ///     Gets the number of channels in the audio stream that
        ///     are monitored by peak meters.
        /// </summary>
        /// <param name="channelCount">A variable into which the method writes the number of channels.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMeteringChannelCountNative(out int channelCount)
        {
            fixed (void* ptr = &channelCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        ///     Gets the number of channels in the audio stream that
        ///     are monitored by peak meters.
        /// </summary>
        /// <returns>The number of channels.</returns>
        public int GetMeteringChannelCount()
        {
            int channelCount;
            CoreAudioAPIException.Try(GetMeteringChannelCountNative(out channelCount), InterfaceName,
                "GetMeteringChannelCount");
            return channelCount;
        }

        /// <summary>
        ///     Gets the peak sample values for all the channels in the
        ///     audio stream.
        ///     <seealso cref="MeteringChannelCount" />
        /// </summary>
        /// <param name="channelCount">
        ///     The channel count. This parameter also specifies the number of elements in the
        ///     <paramref name="peakValues" /> array. If the specified count does not match the number of channels in the stream,
        ///     the method returns error code <see cref="HResult.E_INVALIDARG" />.
        /// </param>
        /// <param name="peakValues">
        ///     An array of peak sample values. The method writes the peak values for the channels into the
        ///     array. The array contains one element for each channel in the stream. The peak values are numbers in the normalized
        ///     range from 0.0 to 1.0. The array gets allocated by the <see cref="GetChannelsPeakValuesNative" /> method.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelsPeakValuesNative(int channelCount, out float[] peakValues)
        {
            peakValues = new float[channelCount];
            fixed (void* ptr = &peakValues[0])
            {
                return InteropCalls.CallI(UnsafeBasePtr, channelCount, ptr, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        ///     Gets the peak sample values for all the channels in the
        ///     audio stream.
        ///     <seealso cref="MeteringChannelCount" />
        /// </summary>
        /// <param name="channelCount">
        ///     The channel count. This parameter also specifies the number of elements in the returned
        ///     array. If the specified count does not match the number of channels in the stream, the method returns error code
        ///     <see cref="HResult.E_INVALIDARG" />.
        /// </param>
        /// <returns>
        ///     An array of peak sample values. he array contains one element for each channel in the stream. The peak values
        ///     are numbers in the normalized range from 0.0 to 1.0.
        /// </returns>
        public float[] GetChannelsPeakValues(int channelCount)
        {
            float[] val;
            CoreAudioAPIException.Try(GetChannelsPeakValuesNative(channelCount, out val), InterfaceName,
                "GetChannelsPeakValues");
            return val;
        }

        /// <summary>
        ///     Gets the peak sample values for all the channels in the
        ///     audio stream.
        /// </summary>
        /// <returns>
        ///     An array of peak sample values. he array contains one element for each channel in the stream. The peak values
        ///     are numbers in the normalized range from 0.0 to 1.0.
        /// </returns>
        public float[] GetChannelsPeakValues()
        {
            return GetChannelsPeakValues(GetMeteringChannelCount());
        }

        /// <summary>
        ///     Queries the audio endpoint device for its
        ///     hardware-supported functions.
        /// </summary>
        /// <param name="hardwareSupportMask">
        ///     A variable into which the method writes a hardware support mask that indicates the
        ///     hardware capabilities of the audio endpoint device.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int QueryHardwareSupportNative(out EndpointHardwareSupportFlags hardwareSupportMask)
        {
            fixed (void* ptr = &hardwareSupportMask)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        ///     Queries the audio endpoint device for its
        ///     hardware-supported functions.
        /// </summary>
        /// <returns>A hardware support mask that indicates the hardware capabilities of the audio endpoint device.</returns>
        public EndpointHardwareSupportFlags QueryHardwareSupport()
        {
            EndpointHardwareSupportFlags res;
            CoreAudioAPIException.Try(QueryHardwareSupportNative(out res), InterfaceName, "QueryHardwareSupport");
            return res;
        }
    }
}