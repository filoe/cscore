using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="AudioVolumeNotificationData"/> structure describes a change in the volume level or muting state of an audio endpoint device.
    /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/dd370799(v=vs.85).aspx"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioVolumeNotificationData
    {
        /// <summary>
        ///     The event context value.
        /// </summary>
        /// <remarks>
        ///     Context value for the <see cref="IAudioEndpointVolumeCallback.OnNotify" /> method. This member is the value of the
        ///     event-context GUID that was provided as an input parameter to the <see cref="AudioEndpointVolume" /> method call
        ///     that changed the endpoint volume level or muting state. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370799(v=vs.85).aspx" />.
        /// </remarks>
        public Guid EventContext;

        /// <summary>
        ///     A value indicating whether the audio stream is currently muted. <c>true</c> if the audio stream is currently muted;
        ///     otherwise, <c>false</c>.
        /// </summary>
        public NativeBool Muted;

        /// <summary>
        ///     Specifies the current master volume level of the audio stream. The volume level is
        ///     normalized to the range from 0.0 to 1.0, where 0.0 is the minimum volume level and 1.0
        ///     is the maximum level. Within this range, the relationship of the normalized volume level
        ///     to the attenuation of signal amplitude is described by a nonlinear, audio-tapered curve.
        /// </summary>
        public float MasterVolume;

        /// <summary>
        ///     The number of channels.
        /// </summary>
        public int Channels;

        /// <summary>
        ///     The first element of an array which specifies the volume level of each channel. Use the
        ///     <see cref="GetAllChannelVolumes" /> method to get all channel volumes.
        /// </summary>
        public float ChannelVolumes; //array?

        /// <summary>
        ///     Gets all channel volumes.
        /// </summary>
        /// <returns>
        ///     The volume level for each channel is normalized to the range from 0.0 to 1.0, where 0.0
        ///     is the minimum volume level and 1.0 is the maximum level. Within this range, the
        ///     relationship of the normalized volume level to the attenuation of signal amplitude is
        ///     described by a nonlinear, audio-tapered curve.
        /// </returns>
        public unsafe float[] GetAllChannelVolumes()
        {
            fixed (void* p = &this)
            {
                var ptr = (IntPtr) p;
                var result = new float[Channels];
                var pchannels =
                    (IntPtr)
                        ((long) ptr + (long) Marshal.OffsetOf(typeof (AudioVolumeNotificationData), "ChannelVolumes"));
                for (int i = 0; i < Channels; i++)
                {
                    result[i] = (float) Marshal.PtrToStructure(pchannels, typeof (float));
                    int size = Marshal.SizeOf(typeof (float));
                    pchannels = new IntPtr((byte*) pchannels.ToPointer() + size);
                }
                return result;
            }
        }
    }
}