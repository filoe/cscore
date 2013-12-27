using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
    public class AudioEndpointVolume : ComObject
    {
        public static AudioEndpointVolume FromDevice(MMDevice device)
        {
            var ptr = device.Activate(new Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), ExecutionContext.CLSCTX_ALL, IntPtr.Zero);
            return new AudioEndpointVolume(ptr);
        }

        private const string c = "IAudioEndpointVolume";
        private readonly List<IAudioEndpointVolumeCallback> _notifies;

        public IEnumerable<IAudioEndpointVolumeCallback> RegisteredCallbacks
        {
            get { return _notifies; }
        }

        /// <summary>
        /// Gets the number of available channels. <seealso cref="GetChannelCount()"/>
        /// </summary>
        public uint ChannelCount
        {
            get { return GetChannelCount(); }
        }

        /// <summary>
        /// Gets or sets the MasterVolumeLevel in decibel.
        /// <seealso cref="AudioEndpointVolume.GetMasterVolumeLevel"/>
        /// <seealso cref="AudioEndpointVolume.SetMasterVolumeLevel"/>
        /// </summary>
        public float MasterVolumeLevel
        {
            get { return GetMasterVolumeLevel(); }
            set { SetMasterVolumeLevel(value, Guid.Empty); }
        }

        /// <summary>
        /// Gets or sets the MasterVolumeLevel as a normalized value in the range from 0.0 to 1.0.
        /// </summary>
        public float MasterVolumeLevelScalar
        {
            get { return GetMasterVolumeLevelScalar(); }
            set { SetMasterVolumeLevelScalar(value, Guid.Empty); }
        }

        private readonly List<AudioEndpointVolumeChannel> _channels;

        public List<AudioEndpointVolumeChannel> Channels
        {
            get { return _channels; }
        }

        public AudioEndpointVolume(IntPtr ptr)
            : base(ptr)
        {
            _notifies = new List<IAudioEndpointVolumeCallback>();
            _channels = new List<AudioEndpointVolumeChannel>();

            for (int i = 0; i < ChannelCount; i++)
            {
                _channels.Add(new AudioEndpointVolumeChannel(this, i));
            }
        }

        /// <summary>
        /// The RegisterControlChangeNotify method registers a client's notification callback
        /// interface.
        /// </summary>
        /// <param name="notify">Notificationprovider</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// When notifications are no longer needed, the client can call the
        /// IAudioEndpointVolume::UnregisterControlChangeNotify method to terminate the
        /// notifications.
        /// </remarks>
        public unsafe int RegisterControlChangeNotifyNative(IAudioEndpointVolumeCallback notify)
        {
            int result = 0;
            if (!_notifies.Contains(notify))
            {
                result = InteropCalls.CallI(_basePtr, Marshal.GetComInterfaceForObject(notify, typeof(IAudioEndpointVolumeCallback)), ((void**)(*(void**)_basePtr))[3]);
                _notifies.Add(notify);
            }
            return result;
        }

        /// <summary>
        /// The RegisterControlChangeNotify method registers a client's notification callback
        /// interface.
        /// </summary>
        /// <param name="notify">Notificationprovider</param>
        /// <remarks>
        /// When notifications are no longer needed, the client can call the
        /// IAudioEndpointVolume::UnregisterControlChangeNotify method to terminate the
        /// notifications.
        /// </remarks>
        public void RegisterControlChangeNotify(IAudioEndpointVolumeCallback notify)
        {
            CoreAudioAPIException.Try(RegisterControlChangeNotifyNative(notify), c, "RegisterControlChangeNotify");
        }

        /// <summary>
        /// The UnregisterControlChangeNotify method deletes the registration of a client's
        /// notification callback interface that the client registered in a previous call to the
        /// IAudioEndpointVolume::RegisterControlChangeNotify method.
        /// </summary>
        /// <param name="notify">Notificationprovider</param>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterControlChangeNotifyNative(IAudioEndpointVolumeCallback notify)
        {
            int result = 0;
            if (_notifies.Contains(notify))
            {
                result = InteropCalls.CallI(_basePtr, Marshal.GetComInterfaceForObject(notify, typeof(IAudioEndpointVolumeCallback)), ((void**)(*(void**)_basePtr))[4]);
                _notifies.Remove(notify);
            }
            return result;
        }

        /// <summary>
        /// The UnregisterControlChangeNotify method deletes the registration of a client's
        /// notification callback interface that the client registered in a previous call to the
        /// IAudioEndpointVolume::RegisterControlChangeNotify method.
        /// </summary>
        /// <param name="notify">Notificationprovider</param>
        public void UnregisterControlChangeNotify(IAudioEndpointVolumeCallback notify)
        {
            CoreAudioAPIException.Try(UnregisterControlChangeNotifyNative(notify), c, "UnregisterControlChangeNotify");
        }

        /// <summary>
        /// The GetChannelCount method gets a count of the channels in the audio stream that enters
        /// or leaves the audio endpoint device.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelCountNative(out uint channelCount)
        {
            fixed (void* ptr = &channelCount)
            {
                return InteropCalls.CallI(_basePtr, ptr, ((void**)(*(void**)_basePtr))[5]);
            }
        }

        /// <summary>
        /// The GetChannelCount method gets a count of the channels in the audio stream that enters
        /// or leaves the audio endpoint device.
        /// </summary>
        public uint GetChannelCount()
        {
            uint result;
            CoreAudioAPIException.Try(GetChannelCountNative(out result), c, "GetChannelCount");
            return result;
        }

        /// <summary>
        /// The SetMasterVolumeLevel method sets the master volume level, in decibels, of the audio
        /// stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">The new master volume level in decibels. To obtain the range and
        /// granularity of the volume levels that can be set by this method, call the
        /// IAudioEndpointVolume::GetVolumeRange method.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMasterVolumeLevelNative(float levelDB, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, levelDB, &eventContext, ((void**)(*(void**)_basePtr))[6]);
        }

        /// <summary>
        /// The SetMasterVolumeLevel method sets the master volume level, in decibels, of the audio
        /// stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">The new master volume level in decibels. To obtain the range and
        /// granularity of the volume levels that can be set by this method, call the
        /// IAudioEndpointVolume::GetVolumeRange method.</param>
        public void SetMasterVolumeLevel(float levelDB, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetMasterVolumeLevelNative(levelDB, eventContext), c, "SetMasterVolumeLevel");
        }

        /// <summary>
        /// The SetMasterVolumeLevelScalar method sets the master volume level of the audio stream
        /// that enters or leaves the audio endpoint device. The volume level is expressed as a
        /// normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <param name="level">The new master volume level. The level is expressed as a normalized
        /// value in the range from 0.0 to 1.0.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMasterVolumeLevelScalarNative(float level, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, level, &eventContext, ((void**)(*(void**)_basePtr))[7]);
        }

        /// <summary>
        /// The SetMasterVolumeLevelScalar method sets the master volume level of the audio stream
        /// that enters or leaves the audio endpoint device. The volume level is expressed as a
        /// normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <param name="level">The new master volume level. The level is expressed as a normalized
        /// value in the range from 0.0 to 1.0.</param>
        public void SetMasterVolumeLevelScalar(float level, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetMasterVolumeLevelScalarNative(level, eventContext), c, "SetMasterVolumeLevelScalar");
        }

        /// <summary>
        /// The GetMasterVolumeLevel method gets the master volume level, in decibels, of the audio
        /// stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">Pointer to the master volume level. This parameter points to a
        /// float variable into which the method writes the volume level in decibels. To get the
        /// range of volume levels obtained from this method, call the
        /// IAudioEndpointVolume::GetVolumeRange method.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMasterVolumeLevelNative(out float levelDB)
        {
            fixed (void* ptr = &levelDB)
            {
                return InteropCalls.CallI(_basePtr, ptr, ((void**)(*(void**)_basePtr))[8]);
            }
        }

        /// <summary>
        /// The GetMasterVolumeLevel method gets the master volume level, in decibels, of the audio
        /// stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <returns>Volume level in decibels. To get the range of volume levels obtained from this
        /// method, call the IAudioEndpointVolume::GetVolumeRange method.</returns>
        public float GetMasterVolumeLevel()
        {
            float result;
            CoreAudioAPIException.Try(GetMasterVolumeLevelNative(out result), c, "GetMasterVolumeLevel");
            return result;
        }

        /// <summary>
        /// The GetMasterVolumeLevelScalar method gets the master volume level of the audio stream
        /// that enters or leaves the audio endpoint device. The volume level is expressed as a
        /// normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <param name="level">Pointer to the master volume level. This parameter points to a float
        /// variable into which the method writes the volume level. The level is expressed as a
        /// normalized value in the range from 0.0 to 1.0.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMasterVolumeLevelScalarNative(out float level)
        {
            fixed (void* ptr = &level)
            {
                return InteropCalls.CallI(_basePtr, ptr, ((void**)(*(void**)_basePtr))[9]);
            }
        }

        /// <summary>
        /// The GetMasterVolumeLevelScalar method gets the master volume level of the audio stream
        /// that enters or leaves the audio endpoint device. The volume level is expressed as a
        /// normalized, audio-tapered value in the range from 0.0 to 1.0.
        /// </summary>
        /// <returns>Volume level. The level is expressed as a normalized value in the range from
        /// 0.0 to 1.0.</returns>
        public float GetMasterVolumeLevelScalar()
        {
            float result;
            CoreAudioAPIException.Try(GetMasterVolumeLevelScalarNative(out result), c, "GetMasterVolumeLevelScalar");
            return result;
        }

        /// <summary>
        /// The SetChannelVolumeLevel method sets the volume level, in decibels, of the specified
        /// channel of the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">The new volume level in decibels. To obtain the range and
        /// granularity of the volume levels that can be set by this method, call the
        /// IAudioEndpointVolume::GetVolumeRange method.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetChannelVolumeLevelNative(uint channel, float levelDB, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, channel, levelDB, &eventContext, ((void**)(*(void**)_basePtr))[10]);
        }

        /// <summary>
        /// The SetChannelVolumeLevel method sets the volume level, in decibels, of the specified
        /// channel of the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">The new volume level in decibels. To obtain the range and
        /// granularity of the volume levels that can be set by this method, call the
        /// IAudioEndpointVolume::GetVolumeRange method.</param>
        public void SetChannelVolumeLevel(uint channel, float levelDB, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetChannelVolumeLevelNative(channel, levelDB, eventContext), c, "SetChannelVolumeLevel");
        }

        /// <summary>
        /// The SetChannelVolumeLevelScalar method sets the normalized, audio-tapered volume level
        /// of the specified channel in the audio stream that enters or leaves the audio endpoint
        /// device.
        /// </summary>
        /// <param name="level">The volume level. The volume level is expressed as a normalized
        /// value in the range from 0.0 to 1.0.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetChannelVolumeLevelScalarNative(uint channel, float level, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, channel, level, &eventContext, ((void**)(*(void**)_basePtr))[11]);
        }

        /// <summary>
        /// The SetChannelVolumeLevelScalar method sets the normalized, audio-tapered volume level
        /// of the specified channel in the audio stream that enters or leaves the audio endpoint
        /// device.
        /// </summary>
        /// <param name="level">The volume level. The volume level is expressed as a normalized
        /// value in the range from 0.0 to 1.0.</param>
        public void SetChannelVolumeLevelScalar(uint channel, float level, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetChannelVolumeLevelScalarNative(channel, level, eventContext), c, "SetChannelVolumeLevelScalar");
        }

        /// <summary>
        /// The GetChannelVolumeLevel method gets the volume level, in decibels, of the specified
        /// channel in the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="levelDB">Pointer to a float variable into which the method writes the
        /// volume level in decibels. To get the range of volume levels obtained from this method,
        /// call the IAudioEndpointVolume::GetVolumeRange method.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelVolumeLevelNative(uint channel, out float levelDB)
        {
            fixed (void* ptr = &levelDB)
            {
                return InteropCalls.CallI(_basePtr, channel, ptr, ((void**)(*(void**)_basePtr))[12]);
            }
        }

        /// <summary>
        /// The GetChannelVolumeLevel method gets the volume level, in decibels, of the specified
        /// channel in the audio stream that enters or leaves the audio endpoint device.
        /// </summary>
        /// <returns>Volume level in decibels. To get the range of volume levels obtained from this
        /// method, call the IAudioEndpointVolume::GetVolumeRange method.</returns>
        public float GetChannelVolumeLevel(uint channel)
        {
            float result;
            CoreAudioAPIException.Try(GetChannelVolumeLevelNative(channel, out result), c, "GetChannelVolumeLevel");
            return result;
        }

        /// <summary>
        /// The GetChannelVolumeLevelScalar method gets the normalized, audio-tapered volume level
        /// of the specified channel of the audio stream that enters or leaves the audio endpoint
        /// device.
        /// </summary>
        /// <param name="level">Pointer to a float variable into which the method writes the volume
        /// level. The level is expressed as a normalized value in the range from 0.0 to
        /// 1.0.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetChannelVolumeLevelScalarNative(uint channel, out float level)
        {
            fixed (void* ptr = &level)
            {
                return InteropCalls.CallI(_basePtr, channel, ptr, ((void**)(*(void**)_basePtr))[13]);
            }
        }

        /// <summary>
        /// The GetChannelVolumeLevelScalar method gets the normalized, audio-tapered volume level
        /// of the specified channel of the audio stream that enters or leaves the audio endpoint
        /// device.
        /// </summary>
        /// <returns>Volume level of a specific channel. The level is expressed as a normalized
        /// value in the range from 0.0 to 1.0.</returns>
        public float GetChannelVolumeLevelScalar(uint channel)
        {
            float result;
            CoreAudioAPIException.Try(GetChannelVolumeLevelScalarNative(channel, out result), c, "GetChannelVolumeLevelScalar");
            return result;
        }

        /// <summary>
        /// The SetMute method sets the muting state of the audio stream that enters or leaves the
        /// audio endpoint device.
        /// </summary>
        /// <param name="mute">True mutes the stream. False turns off muting.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMuteNative(NativeBool mute, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, mute, &eventContext, ((void**)(*(void**)_basePtr))[14]);
        }

        /// <summary>
        /// The SetMute method sets the muting state of the audio stream that enters or leaves the
        /// audio endpoint device.
        /// </summary>
        /// <param name="mute">True mutes the stream. False turns off muting.</param>
        public unsafe void SetMute(bool mute, Guid eventContext)
        {
            CoreAudioAPIException.Try(SetMuteNative(mute, eventContext), c, "SetMute");
        }

        /// <summary>
        /// The GetMute method gets the muting state of the audio stream that enters or leaves the
        /// audio endpoint device.
        /// </summary>
        /// <param name="mute">True = Stream is muted. False = Stream is not muted.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMuteNative(out NativeBool mute)
        {
            fixed (void* ptr = &mute)
            {
                return InteropCalls.CallI(_basePtr, ptr, ((void**)(*(void**)_basePtr))[15]);
            }
        }

        /// <summary>
        /// The GetMute method gets the muting state of the audio stream that enters or leaves the
        /// audio endpoint device.
        /// </summary>
        /// <returns>True = Stream is muted. False = Stream is not muted.</returns>
        public bool GetMute()
        {
            NativeBool result;
            CoreAudioAPIException.Try(GetMuteNative(out result), c, "GetMute");
            return result;
        }

        /// <summary>
        /// The GetVolumeStepInfo method gets information about the current step in the volume
        /// range.
        /// </summary>
        /// <param name="currentStep">Current stepindex. This value is a value in the range from 0
        /// to stepCount.</param>
        /// <param name="stepCount">Number of steps in the volume range.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetVolumeStepInfoNative(out uint currentStep, out uint stepCount)
        {
            fixed (void* ptr1 = &currentStep, ptr2 = &stepCount)
            {
                return InteropCalls.CallI(_basePtr, ptr1, ptr2, ((void**)(*(void**)_basePtr))[16]);
            }
        }

        /// <summary>
        /// The GetVolumeStepInfo method gets information about the current step in the volume
        /// range.
        /// </summary>
        /// <param name="currentStep">Current stepindex. This value is a value in the range from 0
        /// to stepCount.</param>
        /// <param name="stepCount">Number of steps in the volume range.</param>
        public void GetVolumeStepInfo(out uint currentStep, out uint stepCount)
        {
            CoreAudioAPIException.Try(GetVolumeStepInfoNative(out currentStep, out stepCount), c, "GetVolumeStepInfo");
        }

        /// <summary>
        /// The VolumeStepUp method increments, by one step, the volume level of the audio stream
        /// that enters or leaves the audio endpoint device.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int VolumeStepUpNative(Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, &eventContext, ((void**)(*(void**)_basePtr))[17]);
        }

        /// <summary>
        /// The VolumeStepUp method increments, by one step, the volume level of the audio stream
        /// that enters or leaves the audio endpoint device.
        /// </summary>
        public void VolumeStepUp(Guid eventContext)
        {
            CoreAudioAPIException.Try(VolumeStepUpNative(eventContext), c, "VolumeStepUp");
        }

        /// <summary>
        /// The VolumeStepDown method decrements, by one step, the volume level of the audio stream
        /// that enters or leaves the audio endpoint device.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int VolumeStepDownNative(Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, &eventContext, ((void**)(*(void**)_basePtr))[18]);
        }

        /// <summary>
        /// The VolumeStepDown method decrements, by one step, the volume level of the audio stream
        /// that enters or leaves the audio endpoint device.
        /// </summary>
        public void VolumeStepDown(Guid eventContext)
        {
            CoreAudioAPIException.Try(VolumeStepDownNative(eventContext), c, "VolumeStepDown");
        }

        /// <summary>
        /// The QueryHardwareSupport method queries the audio endpoint device for its
        /// hardware-supported functions.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int QueryHardwareSupportNative(out EndpointHardwareSupport hardwareSupportMask)
        {
            fixed (void* ptr = &hardwareSupportMask)
            {
                return InteropCalls.CallI(_basePtr, ptr, ((void**)(*(void**)_basePtr))[19]);
            }
        }

        /// <summary>
        /// The QueryHardwareSupport method queries the audio endpoint device for its
        /// hardware-supported functions.
        /// </summary>
        public EndpointHardwareSupport QueryHardwareSupport()
        {
            EndpointHardwareSupport result;
            CoreAudioAPIException.Try(QueryHardwareSupportNative(out result), c, "QueryHardWareSupport");
            return result;
        }

        /// <summary>
        /// The GetVolumeRange method gets the volume range, in decibels, of the audio stream that
        /// enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="volumeMinDB">Minimum volume level in decibels. This value remains constant
        /// for the lifetime of the IAudioEndpointVolume interface instance.</param>
        /// <param name="volumeMaxDB">Maximum volume level in decibels. This value remains constant
        /// for the lifetime of the IAudioEndpointVolume interface instance.</param>
        /// <param name="volumeIncrementDB">Volume increment in decibels. This increment remains
        /// constant for the lifetime of the IAudioEndpointVolume interface instance.</param>
        /// <returns>HREUSLT</returns>
        public unsafe int GetVolumeRangeNative(out float volumeMinDB, out float volumeMaxDB, out float volumeIncrementDB)
        {
            fixed (void* ptr1 = &volumeMinDB, ptr2 = &volumeMaxDB, ptr3 = &volumeIncrementDB)
            {
                return InteropCalls.CallI(_basePtr, ptr1, ptr2, ptr3, ((void**)(*(void**)_basePtr))[20]);
            }
        }

        /// <summary>
        /// The GetVolumeRange method gets the volume range, in decibels, of the audio stream that
        /// enters or leaves the audio endpoint device.
        /// </summary>
        /// <param name="volumeMinDB">Minimum volume level in decibels. This value remains constant
        /// for the lifetime of the IAudioEndpointVolume interface instance.</param>
        /// <param name="volumeMaxDB">Maximum volume level in decibels. This value remains constant
        /// for the lifetime of the IAudioEndpointVolume interface instance.</param>
        /// <param name="volumeIncrementDB">Volume increment in decibels. This increment remains
        /// constant for the lifetime of the IAudioEndpointVolume interface instance.</param>
        public void GetVolumeRange(out float volumeMinDB, out float volumeMaxDB, out float volumeIncrementDB)
        {
            CoreAudioAPIException.Try(GetVolumeRangeNative(out volumeMinDB, out volumeMaxDB, out volumeIncrementDB), c, "GetVolumeRange");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}