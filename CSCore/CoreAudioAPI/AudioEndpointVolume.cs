using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     The <see cref="AudioEndpointVolume" /> interface represents the volume controls on the audio stream to or from an
    ///     audio endpoint device.
    ///     For more information, see
    ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370892(v=vs.85).aspx" />.
    /// </summary>
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
    public class AudioEndpointVolume : ComObject
    {
        private const string C = "IAudioEndpointVolume";
        private readonly List<AudioEndpointVolumeChannel> _channels;
        private readonly List<IAudioEndpointVolumeCallback> _notifies;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioEndpointVolume" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer of the <see cref="AudioEndpointVolume" /> object.</param>
        public AudioEndpointVolume(IntPtr ptr)
            : base(ptr)
        {
            int channelCount = ChannelCount;
            _notifies = new List<IAudioEndpointVolumeCallback>(channelCount);
            _channels = new List<AudioEndpointVolumeChannel>(channelCount);

            for (int i = 0; i < channelCount; i++)
            {
                _channels.Add(new AudioEndpointVolumeChannel(this, i));
            }
        }

        /// <summary>
        ///     Gets all registered <see cref="IAudioEndpointVolumeCallback" />.
        /// </summary>
        public ReadOnlyCollection<IAudioEndpointVolumeCallback> RegisteredCallbacks
        {
            get { return _notifies.AsReadOnly(); }
        }

        /// <summary>
        ///     Gets the number of available channels.
        ///     <seealso cref="GetChannelCount()" />
        /// </summary>
        public int ChannelCount
        {
            get { return GetChannelCount(); }
        }

        /// <summary>
        ///     Gets or sets the MasterVolumeLevel in decibel.
        ///     <seealso cref="AudioEndpointVolume.GetMasterVolumeLevel" />
        ///     <seealso cref="AudioEndpointVolume.SetMasterVolumeLevel" />
        /// </summary>
        public float MasterVolumeLevel
        {
            get { return GetMasterVolumeLevel(); }
            set { SetMasterVolumeLevel(value, Guid.Empty); }
        }

        /// <summary>
        ///     Gets or sets the MasterVolumeLevel as a normalized value in the range from 0.0 to 1.0.
        ///     <seealso cref="GetMasterVolumeLevelScalar" />
        ///     <seealso cref="SetMasterVolumeLevelScalar" />
        /// </summary>
        public float MasterVolumeLevelScalar
        {
            get { return GetMasterVolumeLevelScalar(); }
            set { SetMasterVolumeLevelScalar(value, Guid.Empty); }
        }

        /// <summary>
        ///     Gets all available channels.
        /// </summary>
        public ReadOnlyCollection<AudioEndpointVolumeChannel> Channels
        {
            get { return _channels.AsReadOnly(); }
        }

        /// <summary>
        ///     Returns a new <see cref="AudioEndpointVolume" /> instance based on a <see cref="MMDevice" /> instance.
        /// </summary>
        /// <param name="device"><see cref="MMDevice" /> instance to create the <see cref="AudioEndpointVolume" /> for.</param>
        /// <returns>A new <see cref="AudioEndpointVolume" /> instance based on the specified <paramref name="device" />.</returns>
        public static AudioEndpointVolume FromDevice(MMDevice device)
        {
            IntPtr ptr = device.Activate(new Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), CLSCTX.CLSCTX_ALL,
                IntPtr.Zero);
            return new AudioEndpointVolume(ptr);
        }

        /// <summary>
        ///     Registers a client's notification callback
        ///     interface.
        /// </summary>
        /// <param name="notify">The callback instance that the client is registering for notification callbacks.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     When notifications are no longer needed, the client can call the
        ///     <see cref="UnregisterControlChangeNotify" /> method to terminate the
        ///     notifications.
        /// </remarks>
        public unsafe int RegisterControlChangeNotifyNative(IAudioEndpointVolumeCallback notify)
        {
            int result = 0;
            if (!_notifies.Contains(notify))
            {
                result = InteropCalls.CallI(UnsafeBasePtr,
                    Marshal.GetComInterfaceForObject(notify, typeof (IAudioEndpointVolumeCallback)),
                    ((void**) (*(void**) UnsafeBasePtr))[3]);
                _notifies.Add(notify);
            }
            return result;
        }

        /// <summary>
        ///     Registers a client's notification callback
        ///     interface.
        /// </summary>
        /// <param name="notify">The callback instance that the client is registering for notification callbacks.</param>
        /// <remarks>
        ///     When notifications are no longer needed, the client can call the
        ///     <see cref="UnregisterControlChangeNotify" /> method to terminate the
        ///     notifications.
        /// </remarks>
        public void RegisterControlChangeNotify(IAudioEndpointVolumeCallback notify)
        {
            CoreAudioAPIException.Try(RegisterControlChangeNotifyNative(notify), C, "RegisterControlChangeNotify");
        }

        /// <summary>
        ///     Deletes the registration of a client's
        ///     notification callback interface that the client registered in a previous call to the
        ///     <see cref="RegisterControlChangeNotify" /> method.
        /// </summary>
        /// <param name="notify">
        ///     The callback instance to unregister. The client passed this same object to the endpoint volume
        ///     object in the previous call to the <see cref="RegisterControlChangeNotify" /> method.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterControlChangeNotifyNative(IAudioEndpointVolumeCallback notify)
        {
            int result = 0;
            if (_notifies.Contains(notify))
            {
                result = InteropCalls.CallI(UnsafeBasePtr,
                    Marshal.GetComInterfaceForObject(notify, typeof (IAudioEndpointVolumeCallback)),
                    ((void**) (*(void**) UnsafeBasePtr))[4]);
                _notifies.Remove(notify);
            }
            return result;
        }

        /// <summary>
        ///     Deletes the registration of a client's
        ///     notification callback interface that the client registered in a previous call to the
        ///     <see cref="RegisterControlChangeNotify" /> method.
        /// </summary>
        /// <param name="notify">
        ///     The callback instance to unregister. The client passed this same object to the endpoint volume
        ///     object in the previous call to the <see cref="RegisterControlChangeNotify" /> method.
        /// </param>
        public void UnregisterControlChangeNotify(IAudioEndpointVolumeCallback notify)
        {
            CoreAudioAPIException.Try(UnregisterControlChangeNotifyNative(notify), C, "UnregisterControlChangeNotify");
        }

        /// <summary>
        ///     Gets the number of channels in the audio stream that enters
        ///     or leaves the audio endpoint device.
        /// </summary>
        /// <param name="channelCount">Retrieves the number of channels in the audio stream.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelCountNative(out int channelCount)
        {
            fixed (void* ptr = &channelCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
        }

        /// <summary>
        ///     Gets the number of channels in the audio stream that enters
        ///     or leaves the audio endpoint device.
        /// </summary>
        /// <returns>The number of channels in the audio stream.</returns>
        public int GetChannelCount()
        {
            int result;
            CoreAudioAPIException.Try(GetChannelCountNative(out result), C, "GetChannelCount");
            return result;
        }

        /// <summary>
        ///     Sets the master volume level, in decibels, of the audio
        ///     stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">
        ///     The new master volume level in decibels. To obtain the range and
        ///     granularity of the volume levels that can be set by this method, call the
        ///     <see cref="GetVolumeRange" /> method.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMasterVolumeLevelNative(float levelDB, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, levelDB, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[6]);
        }

        /// <summary>
        ///     Sets the master volume level, in decibels, of the audio
        ///     stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">
        ///     The new master volume level in decibels. To obtain the range and
        ///     granularity of the volume levels that can be set by this method, call the
        ///     <see cref="GetVolumeRange" /> method.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        public void SetMasterVolumeLevel(float levelDB, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetMasterVolumeLevelNative(levelDB, eventContext), C, "SetMasterVolumeLevel");
        }

        /// <summary>
        ///     Sets the master volume level of the audio stream
        ///     that enters or leaves the audio endpoint device. The volume level is expressed as a
        ///     normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <param name="level">
        ///     The new master volume level. The level is expressed as a normalized
        ///     value in the range from 0.0 to 1.0.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMasterVolumeLevelScalarNative(float level, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, level, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[7]);
        }

        /// <summary>
        ///     Sets the master volume level of the audio stream
        ///     that enters or leaves the audio endpoint device. The volume level is expressed as a
        ///     normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <param name="level">
        ///     The new master volume level. The level is expressed as a normalized
        ///     value in the range from 0.0 to 1.0.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        public void SetMasterVolumeLevelScalar(float level, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetMasterVolumeLevelScalarNative(level, eventContext), C,
                "SetMasterVolumeLevelScalar");
        }

        /// <summary>
        ///     Gets the master volume level, in decibels, of the audio
        ///     stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">
        ///     A
        ///     float variable into which the method writes the volume level in decibels. To get the
        ///     range of volume levels obtained from this method, call the
        ///     <see cref="GetVolumeRange" /> method.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMasterVolumeLevelNative(out float levelDB)
        {
            fixed (void* ptr = &levelDB)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[8]);
            }
        }

        /// <summary>
        ///     Gets the master volume level, in decibels, of the audio
        ///     stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <returns>
        ///     Volume level in decibels. To get the range of volume levels obtained from this
        ///     method, call the <see cref="GetVolumeRange" /> method.
        /// </returns>
        public float GetMasterVolumeLevel()
        {
            float result;
            CoreAudioAPIException.Try(GetMasterVolumeLevelNative(out result), C, "GetMasterVolumeLevel");
            return result;
        }

        /// <summary>
        ///     Gets the master volume level of the audio stream
        ///     that enters or leaves the audio endpoint device. The volume level is expressed as a
        ///     normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <param name="level">
        ///     A float
        ///     variable into which the method writes the volume level. The level is expressed as a
        ///     normalized value in the range from 0.0 to 1.0.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMasterVolumeLevelScalarNative(out float level)
        {
            fixed (void* ptr = &level)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[9]);
            }
        }

        /// <summary>
        ///     Gets the master volume level of the audio stream
        ///     that enters or leaves the audio endpoint device. The volume level is expressed as a
        ///     normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <returns>
        ///     Volume level. The level is expressed as a normalized value in the range from
        ///     0.0 to 1.0.
        /// </returns>
        public float GetMasterVolumeLevelScalar()
        {
            float result;
            CoreAudioAPIException.Try(GetMasterVolumeLevelScalarNative(out result), C, "GetMasterVolumeLevelScalar");
            return result;
        }

        /// <summary>
        ///     Sets the volume level, in decibels, of the specified
        ///     channel of the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">
        ///     The new volume level in decibels. To obtain the range and
        ///     granularity of the volume levels that can be set by this method, call the
        ///     <see cref="GetVolumeRange" /> method.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int SetChannelVolumeLevelNative(int channel, float levelDB, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, channel, levelDB, &eventContext,
                ((void**) (*(void**) UnsafeBasePtr))[10]);
        }

        /// <summary>
        ///     Sets the volume level, in decibels, of the specified
        ///     channel of the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">
        ///     The new volume level in decibels. To obtain the range and
        ///     granularity of the volume levels that can be set by this method, call the
        ///     <see cref="GetVolumeRange" /> method.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        public void SetChannelVolumeLevel(int channel, float levelDB, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetChannelVolumeLevelNative(channel, levelDB, eventContext), C,
                "SetChannelVolumeLevel");
        }

        /// <summary>
        ///     Sets the normalized, audio-tapered volume level
        ///     of the specified channel in the audio stream that enters or leaves the audio endpoint
        ///     device.
        /// </summary>
        /// <param name="level">
        ///     The volume level. The volume level is expressed as a normalized
        ///     value in the range from 0.0 to 1.0.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int SetChannelVolumeLevelScalarNative(int channel, float level, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, channel, level, &eventContext,
                ((void**) (*(void**) UnsafeBasePtr))[11]);
        }

        /// <summary>
        ///     Sets the normalized, audio-tapered volume level
        ///     of the specified channel in the audio stream that enters or leaves the audio endpoint
        ///     device.
        /// </summary>
        /// <param name="level">
        ///     The volume level. The volume level is expressed as a normalized
        ///     value in the range from 0.0 to 1.0.
        /// </param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        public void SetChannelVolumeLevelScalar(int channel, float level, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetChannelVolumeLevelScalarNative(channel, level, eventContext), C,
                "SetChannelVolumeLevelScalar");
        }

        /// <summary>
        ///     Gets the volume level, in decibels, of the specified
        ///     channel in the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">
        ///     A float variable into which the method writes the
        ///     volume level in decibels. To get the range of volume levels obtained from this method,
        ///     call the <see cref="GetVolumeRange" /> method.
        /// </param>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelVolumeLevelNative(int channel, out float levelDB)
        {
            fixed (void* ptr = &levelDB)
            {
                return InteropCalls.CallI(UnsafeBasePtr, channel, ptr, ((void**) (*(void**) UnsafeBasePtr))[12]);
            }
        }

        /// <summary>
        ///     Gets the volume level, in decibels, of the specified
        ///     channel in the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        /// <returns>
        ///     Volume level in decibels. To get the range of volume levels obtained from this
        ///     method, call the <see cref="GetVolumeRange" /> method.
        /// </returns>
        public float GetChannelVolumeLevel(int channel)
        {
            float result;
            CoreAudioAPIException.Try(GetChannelVolumeLevelNative(channel, out result), C, "GetChannelVolumeLevel");
            return result;
        }

        /// <summary>
        ///     Gets the normalized, audio-tapered volume level
        ///     of the specified channel of the audio stream that enters or leaves the audio endpoint
        ///     device.
        /// </summary>
        /// <param name="level">
        ///     A float variable into which the method writes the volume
        ///     level. The level is expressed as a normalized value in the range from 0.0 to
        ///     1.0.
        /// </param>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelVolumeLevelScalarNative(int channel, out float level)
        {
            fixed (void* ptr = &level)
            {
                return InteropCalls.CallI(UnsafeBasePtr, channel, ptr, ((void**) (*(void**) UnsafeBasePtr))[13]);
            }
        }

        /// <summary>
        ///     Gets the normalized, audio-tapered volume level
        ///     of the specified channel of the audio stream that enters or leaves the audio endpoint
        ///     device.
        /// </summary>
        /// <param name="channel">
        ///     The channel number. If the audio stream contains n channels, the channels are numbered from 0 to
        ///     n–1.
        /// </param>
        /// <returns>
        ///     Volume level of a specific channel. The level is expressed as a normalized
        ///     value in the range from 0.0 to 1.0.
        /// </returns>
        public float GetChannelVolumeLevelScalar(int channel)
        {
            float result;
            CoreAudioAPIException.Try(GetChannelVolumeLevelScalarNative(channel, out result), C,
                "GetChannelVolumeLevelScalar");
            return result;
        }

        /// <summary>
        ///     Sets the muting state of the audio stream that enters or leaves the
        ///     audio endpoint device.
        /// </summary>
        /// <param name="mute"><c>True</c> mutes the stream. <c>False</c> turns off muting.</param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMuteNative(NativeBool mute, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, mute, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[14]);
        }

        /// <summary>
        ///     Sets the muting state of the audio stream that enters or leaves the
        ///     audio endpoint device.
        /// </summary>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <param name="mute"><c>True</c> mutes the stream. <c>False</c> turns off muting.</param>
        public void SetMute(bool mute, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetMuteNative(mute, eventContext), C, "SetMute");
        }

        /// <summary>
        ///     Gets the muting state of the audio stream that enters or leaves the
        ///     audio endpoint device.
        /// </summary>
        /// <param name="mute">
        ///     A Variable into which the method writes the muting state.
        ///     If <paramref name="mute" /> is <c>true</c>, the stream is muted. If <c>false</c>, the stream is not muted.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMuteNative(out NativeBool mute)
        {
            fixed (void* ptr = &mute)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[15]);
            }
        }

        /// <summary>
        ///     Gets the muting state of the audio stream that enters or leaves the
        ///     audio endpoint device.
        /// </summary>
        /// <returns>If the method returns <c>true, the stream is muted. If <c>false</c>, the stream is not muted.</c></returns>
        public bool GetMute()
        {
            NativeBool result;
            CoreAudioAPIException.Try(GetMuteNative(out result), C, "GetMute");
            return result;
        }

        /// <summary>
        ///     Gets information about the current step in the volume
        ///     range.
        /// </summary>
        /// <param name="currentStep">
        ///     A variable into which the method writes the current step index. This index is a value in the
        ///     range from 0 to <paramref name="stepCount" />– 1, where 0 represents the minimum volume level and
        ///     <paramref name="stepCount" />– 1 represents the maximum level.
        /// </param>
        /// <param name="stepCount">
        ///     A variable into which the method writes the number of steps in the volume range. This number
        ///     remains constant for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetVolumeStepInfoNative(out int currentStep, out int stepCount)
        {
            fixed (void* ptr1 = &currentStep, ptr2 = &stepCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr1, ptr2, ((void**) (*(void**) UnsafeBasePtr))[16]);
            }
        }

        /// <summary>
        ///     Gets information about the current step in the volume
        ///     range.
        /// </summary>
        /// <param name="currentStep">
        ///     A variable into which the method writes the current step index. This index is a value in the
        ///     range from 0 to <paramref name="stepCount" />– 1, where 0 represents the minimum volume level and
        ///     <paramref name="stepCount" />– 1 represents the maximum level.
        /// </param>
        /// <param name="stepCount">
        ///     A variable into which the method writes the number of steps in the volume range. This number
        ///     remains constant for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        public void GetVolumeStepInfo(out int currentStep, out int stepCount)
        {
            CoreAudioAPIException.Try(GetVolumeStepInfoNative(out currentStep, out stepCount), C, "GetVolumeStepInfo");
        }

        /// <summary>
        ///     Increments, by one step, the volume level of the audio stream
        ///     that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <returns>HRESULT</returns>
        public unsafe int VolumeStepUpNative(Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[17]);
        }

        /// <summary>
        ///     Increments, by one step, the volume level of the audio stream
        ///     that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        public void VolumeStepUp(Guid eventContext)
        {
            CoreAudioAPIException.Try(VolumeStepUpNative(eventContext), C, "VolumeStepUp");
        }

        /// <summary>
        ///     Decrements, by one step, the volume level of the audio stream
        ///     that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <returns>HRESULT</returns>
        public unsafe int VolumeStepDownNative(Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[18]);
        }

        /// <summary>
        ///     Decrements, by one step, the volume level of the audio stream
        ///     that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        public void VolumeStepDown(Guid eventContext)
        {
            CoreAudioAPIException.Try(VolumeStepDownNative(eventContext), C, "VolumeStepDown");
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
                return InteropCalls.CallI(UnsafeBasePtr, ptr, ((void**) (*(void**) UnsafeBasePtr))[19]);
            }
        }

        /// <summary>
        ///     Queries the audio endpoint device for its
        ///     hardware-supported functions.
        /// </summary>
        /// <returns>A hardware support mask that indicates the hardware capabilities of the audio endpoint device.</returns>
        public EndpointHardwareSupportFlags QueryHardwareSupport()
        {
            EndpointHardwareSupportFlags result;
            CoreAudioAPIException.Try(QueryHardwareSupportNative(out result), C, "QueryHardWareSupport");
            return result;
        }

        /// <summary>
        ///     Gets the volume range, in decibels, of the audio stream that
        ///     enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="volumeMinDB">
        ///     Minimum volume level in decibels. This value remains constant
        ///     for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        /// <param name="volumeMaxDB">
        ///     Maximum volume level in decibels. This value remains constant
        ///     for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        /// <param name="volumeIncrementDB">
        ///     Volume increment in decibels. This increment remains
        ///     constant for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        /// <returns>HREUSLT</returns>
        public unsafe int GetVolumeRangeNative(out float volumeMinDB, out float volumeMaxDB, out float volumeIncrementDB)
        {
            fixed (void* ptr1 = &volumeMinDB, ptr2 = &volumeMaxDB, ptr3 = &volumeIncrementDB)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ptr1, ptr2, ptr3, ((void**) (*(void**) UnsafeBasePtr))[20]);
            }
        }

        /// <summary>
        ///     Gets the volume range, in decibels, of the audio stream that
        ///     enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="volumeMinDB">
        ///     Minimum volume level in decibels. This value remains constant
        ///     for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        /// <param name="volumeMaxDB">
        ///     Maximum volume level in decibels. This value remains constant
        ///     for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        /// <param name="volumeIncrementDB">
        ///     Volume increment in decibels. This increment remains
        ///     constant for the lifetime of the <see cref="AudioEndpointVolume" /> object instance.
        /// </param>
        public void GetVolumeRange(out float volumeMinDB, out float volumeMaxDB, out float volumeIncrementDB)
        {
            CoreAudioAPIException.Try(GetVolumeRangeNative(out volumeMinDB, out volumeMaxDB, out volumeIncrementDB), C,
                "GetVolumeRange");
        }
    }
}