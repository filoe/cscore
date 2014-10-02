using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     The <see cref="AudioClock" /> class enables a client to monitor a stream's data rate and the current position in
    ///     the stream.
    /// </summary>
    [Guid("CD63314F-3FBA-4a1b-812C-EF96358728E7")]
    public class AudioClock : ComObject
    {
        private const string InterfaceName = "IAudioClock";

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioClock" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the IAudioClock COM Object.</param>
        public AudioClock(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Gets the device frequency. For more information, see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370889(v=vs.85).aspx" />.
        /// </summary>
        public long Pu64Frequency
        {
            get
            {
                long value;
                CoreAudioAPIException.Try(GetFrequencyNative(out value), InterfaceName, "GetFrequency");
                return value;
            }
        }

        /// <summary>
        ///     Gets the device position.
        /// </summary>
        public long Pu64Position
        {
            get
            {
                long value0, value1;
                CoreAudioAPIException.Try(GetPositionNative(out value0, out value1), InterfaceName, "GetPosition");
                return value0;
            }
        }

        /// <summary>
        ///     Creates a new <see cref="AudioCaptureClient" /> by calling the <see cref="AudioClient.GetService" /> method of the
        ///     specified <paramref name="audioClient" />.
        /// </summary>
        /// <param name="audioClient">
        ///     <see cref="AudioClient" /> which should be used to create the <see cref="AudioCaptureClient" />-instance
        ///     with.
        /// </param>
        /// <returns>A new <see cref="AudioCaptureClient" />.</returns>
        public static AudioClock FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new AudioClock(audioClient.GetService(typeof (AudioClock).GUID));
        }

        /// <summary>
        ///     The GetFrequency method gets the device frequency.
        /// </summary>
        /// <param name="pu64Frequency">
        ///     The device frequency. For more information, see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370889(v=vs.85).aspx" />.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetFrequencyNative(out long pu64Frequency)
        {
            fixed (void* p = &pu64Frequency)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        ///     The GetPosition method gets the current device position.
        /// </summary>
        /// <param name="pu64Position">
        ///     The device position is the offset from the start of the stream to the current position in the stream. However, the
        ///     units in which this offset is expressed are undefined—the device position value has meaning only in relation to the
        ///     <see cref="Pu64Frequency" />. For more information, see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370889(v=vs.85).aspx" />.
        /// </param>
        /// <param name="pu64QPCPosition">
        ///     The value of the performance counter at the time that the audio endpoint device read the device position
        ///     (<paramref name="pu64Position" />) in response to the <see cref="GetPositionNative" /> call. The method converts
        ///     the counter value to 100-nanosecond time
        ///     units before writing it to <paramref name="pu64QPCPosition" />.
        /// </param>
        /// <returns>HRESULT</returns>
// ReSharper disable once InconsistentNaming
        public unsafe int GetPositionNative(out long pu64Position, out long pu64QPCPosition)
        {
            fixed (void* p0 = &pu64Position, p1 = &pu64QPCPosition)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p0, p1, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        ///     The GetCharacteristics method is reserved for future use.
        /// </summary>
        /// <param name="characteristics">Value that indicates the characteristics of the audio clock.</param>
        /// <returns>HREUSLT</returns>
        public unsafe int GetCharacteristicsNative(out int characteristics)
        {
            fixed (void* p = &characteristics)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }
    }
}