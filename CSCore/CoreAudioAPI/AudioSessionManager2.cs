using System.Threading;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Enables an application to manage submixes for the audio device.
    /// </summary>
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F")]
    public class AudioSessionManager2 : AudioSessionManager
    {
        private const string InterfaceName = "IAudioSessionManager2";
// ReSharper disable once InconsistentNaming
        private static readonly Guid IID_IAudioSessionManager2 = new Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F");
        private readonly List<IAudioSessionNotification> _sessionNotifications;
        private readonly List<IAudioVolumeDuckNotification> _volumeDuckNotifications;

        /// <summary>
        /// Creates a new instance of <see cref="AudioSessionManager2"/> based on a <see cref="MMDevice"/>.
        /// </summary>
        /// <param name="device">Device to use to activate the <see cref="AudioSessionManager2"/>.</param>
        /// <returns><see cref="AudioSessionManager2"/> instance for the specified <paramref name="device"/>.</returns>
// ReSharper disable once InconsistentNaming
        public static AudioSessionManager2 FromMMDevice(MMDevice device)
        {
            return new AudioSessionManager2(device.Activate(IID_IAudioSessionManager2, CLSCTX.CLSCTX_ALL, IntPtr.Zero));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionManager2"/> class.
        /// </summary>
        /// <param name="ptr">The native pointer.</param>
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
            int result = InteropCalls.CallI(UnsafeBasePtr, &ptr, ((void**)(*(void**)UnsafeBasePtr))[5]);
            sessionEnumerator = ptr != IntPtr.Zero ? new AudioSessionEnumerator(ptr) : null;
            return result;
        }

        /// <summary>
        /// The <see cref="GetSessionEnumerator"/> method gets a pointer to the audio session enumerator object.
        /// </summary>
        /// <returns><see cref="AudioSessionEnumerator"/> which enumerates audio sessions.</returns>
        public AudioSessionEnumerator GetSessionEnumerator()
        {
            AudioSessionEnumerator sessionEnumerator;
            CoreAudioAPIException.Try(GetSessionEnumeratorNative(out sessionEnumerator), InterfaceName, "GetSessionEnumerator");
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
                result = InteropCalls.CallI(
                    UnsafeBasePtr, 
                    sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioSessionNotification)) : IntPtr.Zero, 
                    ((void**)(*(void**)UnsafeBasePtr))[6]);
                _sessionNotifications.Add(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// The RegisterSessionNotification method registers the application to receive a notification when a session is created.
        /// IMPORTANT: Make sure to call this method from an MTA-Thread. Also make sure to enumerate all sessions after calling this method. For example with the following code: 
        /// <code>audioSessionManager2.GetSessionEnumerator().ToArray();</code>
        /// </summary>
        public void RegisterSessionNotification(IAudioSessionNotification sessionNotification)
        {
            if(Thread.CurrentThread.GetApartmentState() != ApartmentState.MTA)
                throw new InvalidOperationException("RegisterSessionNotification has to be called from an MTA-Thread.");
            CoreAudioAPIException.Try(RegisterSessionNotificationNative(sessionNotification), InterfaceName, "RegisterSessionNotification");
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
                result = InteropCalls.CallI(
                    UnsafeBasePtr,
                    sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioSessionNotification)) : IntPtr.Zero, 
                    ((void**)(*(void**)UnsafeBasePtr))[7]);
                _sessionNotifications.Remove(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// The UnregisterSessionNotification method deletes the registration to receive a notification when a session is created.
        /// </summary>
        public void UnregisterSessionNotification(IAudioSessionNotification sessionNotification)
        {
            CoreAudioAPIException.Try(UnregisterSessionNotificationNative(sessionNotification), InterfaceName, "UnregisterSessionNotification");
        }

        //--

        /// <summary>
        /// Registers the application to receive ducking notifications.
        /// </summary>
        /// <param name="sessionId"> Applications that are playing a media stream and want to provide custom stream attenuation or ducking behavior, pass their own session instance identifier.
        /// Other applications that do not want to alter their streams but want to get all the ducking notifications must pass NULL.</param>
        /// <param name="sessionNotification">Instance of any object which implements the <see cref="IAudioVolumeDuckNotification"/> and which should receive duck notifications.</param>
        /// <returns>HRESULT</returns>
        public unsafe int RegisterDuckNotificationNative(string sessionId, IAudioVolumeDuckNotification sessionNotification)
        {
            int result = 0;
            if (!_volumeDuckNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null ? Marshal.GetComInterfaceForObject(sessionNotification, typeof(IAudioVolumeDuckNotification)) : IntPtr.Zero;
                IntPtr ptr0 = sessionId != null ? Marshal.StringToHGlobalUni(sessionId) : IntPtr.Zero;
                result = InteropCalls.CallI(UnsafeBasePtr, (void*)ptr0, (void*)ptr, ((void**)(*(void**)UnsafeBasePtr))[8]);
                if (ptr0 != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr0);

                _volumeDuckNotifications.Add(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Registers the application to receive ducking notifications.
        /// </summary>
        /// <param name="sessionId"> Applications that are playing a media stream and want to provide custom stream attenuation or ducking behavior, pass their own session instance identifier.
        /// Other applications that do not want to alter their streams but want to get all the ducking notifications must pass NULL.</param>
        /// <param name="sessionNotification">Instance of any object which implements the <see cref="IAudioVolumeDuckNotification"/> and which should receive duck notifications.</param>
        public void RegisterDuckNotification(string sessionId, IAudioVolumeDuckNotification sessionNotification)
        {
            CoreAudioAPIException.Try(RegisterDuckNotificationNative(sessionId, sessionNotification), InterfaceName, "RegisterDuckNotification");
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
                result = InteropCalls.CallI(UnsafeBasePtr, (void*)ptr, ((void**)(*(void**)UnsafeBasePtr))[9]);
                _volumeDuckNotifications.Remove(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Deletes the registration to receive ducking notifications.
        /// </summary>
        public void UnregisterDuckNotification(IAudioVolumeDuckNotification sessionNotification)
        {
            CoreAudioAPIException.Try(UnregisterDuckNotificationNative(sessionNotification), InterfaceName, "UnregisterDuckNotification");
        }
    }
}
