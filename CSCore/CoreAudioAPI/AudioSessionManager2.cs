using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// IAudioSessionManager2
    /// </summary>
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F")]
    public class AudioSessionManager2 : AudioSessionManager
    {
        private const string c = "IAudioSessionManager2";
        private static readonly Guid IID_IAudioSessionManager2 = new Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F");
        private List<IAudioSessionNotification> _sessionNotifications;
        private List<IAudioVolumeDuckNotification> _volumeDuckNotifications;

        /// <summary>
        /// Creates a new instance of AudioSessionManager2 based on a MMDevice.
        /// </summary>
        public static AudioSessionManager2 FromMMDevice(MMDevice device)
        {
            return new AudioSessionManager2(device.Activate(IID_IAudioSessionManager2, ExecutionContext.CLSCTX_ALL, IntPtr.Zero));
        }

        public AudioSessionManager2(IntPtr ptr)
            : base(ptr)
        {
            _sessionNotifications = new List<IAudioSessionNotification>();
            _volumeDuckNotifications = new List<IAudioVolumeDuckNotification>();
        }

        /// <summary>
        /// The GetSessionEnumerator method gets a pointer to the audio session enumerator object.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetSessionEnumeratorNative(out AudioSessionEnumerator sessionEnumerator)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &ptr, ((void**)(*(void**)_basePtr))[5]);
            if (ptr != IntPtr.Zero)
                sessionEnumerator = new AudioSessionEnumerator(ptr);
            else
                sessionEnumerator = null;
            return result;
        }

        /// <summary>
        /// The GetSessionEnumerator method gets a pointer to the audio session enumerator object.
        /// </summary>
        public AudioSessionEnumerator GetSessionEnumerator()
        {
            AudioSessionEnumerator sessionEnumerator;
            CoreAudioAPIException.Try(GetSessionEnumeratorNative(out sessionEnumerator), c, "GetSessionEnumerator");
            return sessionEnumerator;
        }

        /// <summary>
        /// The RegisterSessionNotification method registers the application to receive a notification when a session is created.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int RegisterSessionNotificationNative(IAudioSessionNotification sessionNotification)
        {
            int result = 0;
            if (!_sessionNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioSessionNotification)) : IntPtr.Zero;
                result = InteropCalls.CallI(_basePtr, (void*)ptr, ((void**)(*(void**)_basePtr))[6]);
                _sessionNotifications.Add(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// The RegisterSessionNotification method registers the application to receive a notification when a session is created.
        /// </summary>
        public void RegisterSessionNotification(IAudioSessionNotification sessionNotification)
        {
            CoreAudioAPIException.Try(RegisterSessionNotificationNative(sessionNotification), c, "RegisterSessionNotification");
        }

        /// <summary>
        /// The UnregisterSessionNotification method deletes the registration to receive a notification when a session is created.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterSessionNotificationNative(IAudioSessionNotification sessionNotification)
        {
            int result = 0;
            if (_sessionNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioSessionNotification)) : IntPtr.Zero;
                result = InteropCalls.CallI(_basePtr, (void*)ptr, ((void**)(*(void**)_basePtr))[7]);
                _sessionNotifications.Remove(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// The UnregisterSessionNotification method deletes the registration to receive a notification when a session is created.
        /// </summary>
        public void UnregisterSessionNotification(IAudioSessionNotification sessionNotification)
        {
            CoreAudioAPIException.Try(UnregisterSessionNotificationNative(sessionNotification), c, "UnregisterSessionNotification");
        }

        //--

        /// <summary>
        /// Registers the application to receive ducking notifications.
        /// </summary>
        /// <param name="sessionID"> Applications that are playing a media stream and want to provide custom stream attenuation or ducking behavior, pass their own session instance identifier.
        /// Other applications that do not want to alter their streams but want to get all the ducking notifications must pass NULL.</param>
        /// <returns>HRESULT</returns>
        public unsafe int RegisterDuckNotificationNative(string sessionID, IAudioVolumeDuckNotification sessionNotification)
        {
            int result = 0;
            if (!_volumeDuckNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioVolumeDuckNotification)) : IntPtr.Zero;
                IntPtr ptr0 = sessionID != null ? Marshal.StringToHGlobalUni(sessionID) : IntPtr.Zero;
                result = InteropCalls.CallI(_basePtr, (void*)ptr0, (void*)ptr, ((void**)(*(void**)_basePtr))[8]);
                if (ptr0 != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr0);

                _volumeDuckNotifications.Add(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Registers the application to receive ducking notifications.
        /// </summary>
        /// <param name="sessionID"> Applications that are playing a media stream and want to provide custom stream attenuation or ducking behavior, pass their own session instance identifier.
        /// Other applications that do not want to alter their streams but want to get all the ducking notifications must pass NULL.</param>
        public void RegisterDuckNotification(string sessionID, IAudioVolumeDuckNotification sessionNotification)
        {
            CoreAudioAPIException.Try(RegisterDuckNotificationNative(sessionID, sessionNotification), c, "RegisterDuckNotification");
        }

        /// <summary>
        /// Deletes the registration to receive ducking notifications.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterDuckNotificationNative(IAudioVolumeDuckNotification sessionNotification)
        {
            int result = 0;
            if (_volumeDuckNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioVolumeDuckNotification)) : IntPtr.Zero;
                result = InteropCalls.CallI(_basePtr, (void*)ptr, ((void**)(*(void**)_basePtr))[9]);
                _volumeDuckNotifications.Remove(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Deletes the registration to receive ducking notifications.
        /// </summary>
        public void UnregisterDuckNotification(IAudioVolumeDuckNotification sessionNotification)
        {
            CoreAudioAPIException.Try(UnregisterDuckNotificationNative(sessionNotification), c, "UnregisterDuckNotification");
        }
    }
}
