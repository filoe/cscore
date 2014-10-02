using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     Used to get the device position.
    /// </summary>
    [Guid("6f49ff73-6727-49ac-a008-d98cf5e70048")]
    public class AudioClock2 : ComObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioClock2" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the IAudioClock2 COM object.</param>
        public AudioClock2(IntPtr ptr) : base(ptr)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioClock2" /> class.
        /// </summary>
        /// <param name="audioClock">
        ///     An <see cref="AudioClock" /> instance which should be used to query the
        ///     <see cref="AudioClock2" /> object.
        /// </param>
        /// <exception cref="ArgumentNullException">The <paramref name="audioClock" /> argument is null.</exception>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="AudioClock2" /> COM object is not supported on the current platform. Only supported on Windows
        ///     7/Windows Server 2008 R2 and above.
        ///     For more information, see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370883(v=vs.85).aspx" />.
        /// </exception>
        public AudioClock2(AudioClock audioClock)
        {
            if (audioClock == null)
                throw new ArgumentNullException("audioClock");

            IntPtr ptr = audioClock.QueryInterface(typeof (AudioClock2));
            if (ptr == null)
                throw new NotSupportedException("The IAudioClock2 COM object is not supported on current platform.");
            BasePtr = ptr;
        }

        /// <summary>
        ///     The <see cref="GetDevicePositionNative" /> method gets the current device position, in frames, directly from the
        ///     hardware.
        /// </summary>
        /// <param name="devicePosition">
        ///     Receives the device position, in frames. The received position is an unprocessed value
        ///     that the method obtains directly from the hardware. For more information, see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370883(v=vs.85).aspx" />.
        /// </param>
        /// <param name="qpcPosition">
        ///     Receives the value of the performance counter at the time that the audio endpoint device read
        ///     the device position retrieved in the <paramref name="devicePosition" /> parameter in response to the
        ///     <see cref="GetDevicePositionNative" /> call.
        ///     <see cref="GetDevicePositionNative" /> converts the counter value to 100-nanosecond time units before writing it to
        ///     QPCPosition.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetDevicePositionNative(out long devicePosition, out long qpcPosition)
        {
            fixed (void* p0 = &devicePosition, p1 = &qpcPosition)
            {
                return InteropCalls.CallI(UnsafeBasePtr, p0, p1, ((void**) (*(void**) UnsafeBasePtr))[3]);
            }
        }

        /// <summary>
        ///     The <see cref="GetDevicePosition" /> method gets the current device position, in frames, directly from the
        ///     hardware.
        /// </summary>
        /// <param name="devicePosition">
        ///     Receives the device position, in frames. The received position is an unprocessed value
        ///     that the method obtains directly from the hardware. For more information, see
        ///     <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd370883(v=vs.85).aspx" />.
        /// </param>
        /// <param name="qpcPosition">
        ///     Receives the value of the performance counter at the time that the audio endpoint device read
        ///     the device position retrieved in the <paramref name="devicePosition" /> parameter in response to the
        ///     <see cref="GetDevicePosition" /> call.
        ///     <see cref="GetDevicePosition" /> converts the counter value to 100-nanosecond time units before writing it to
        ///     QPCPosition.
        /// </param>
        public void GetDevicePosition(out long devicePosition, out long qpcPosition)
        {
            CoreAudioAPIException.Try(GetDevicePositionNative(out devicePosition, out qpcPosition), "GetDevicePosition",
                "IAudioClock2");
        }
    }
}