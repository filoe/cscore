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
        /// Gets a pointer to the audio session enumerator object.
        /// <seealso cref="RegisterDuckNotification"/>
        /// </summary>
        /// <param name="sessionEnumerator">Retrieves a session enumerator object that the client can use to enumerate audio sessions on the audio device.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>The client is responsible for releasing the <paramref name="sessionEnumerator"/>.</remarks>
        public unsafe int GetSessionEnumeratorNative(out AudioSessionEnumerator sessionEnumerator)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(UnsafeBasePtr, &ptr, ((void**) (*(void**) UnsafeBasePtr))[5]);
            sessionEnumerator = ptr != IntPtr.Zero ? new AudioSessionEnumerator(ptr) : null;
            return result;
        }

        /// <summary>
        /// Gets a pointer to the audio session enumerator object.
        /// </summary>
        /// <returns>a session enumerator object that the client can use to enumerate audio sessions on the audio device.</returns>
        /// <remarks>The client is responsible for releasing the returned <see cref="AudioSessionEnumerator"/>.</remarks>
        public AudioSessionEnumerator GetSessionEnumerator()
        {
            AudioSessionEnumerator sessionEnumerator;
            CoreAudioAPIException.Try(GetSessionEnumeratorNative(out sessionEnumerator), InterfaceName,
                "GetSessionEnumerator");
            return sessionEnumerator;
        }

        /// <summary>
        /// Registers the application to receive a notification when a session is created.
        /// <seealso cref="RegisterSessionNotificationNative"/>
        /// </summary>
        /// <param name="sessionNotification">The application's implementation of the <see cref="IAudioSessionNotification"/> interface.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        /// Use the <see cref="AudioSessionNotification"/> class as the default implementation for the <paramref name="sessionNotification"/> parameter.
        /// 
        /// <c>Note:</c> Make sure to call the <see cref="RegisterSessionNotificationNative"/> from an MTA-Thread. Also make sure to enumerate all sessions after calling this method. 
        /// </remarks>
        public unsafe int RegisterSessionNotificationNative(IAudioSessionNotification sessionNotification)
        {
            int result = 0;
            if (!_sessionNotifications.Contains(sessionNotification))
            {
                result = InteropCalls.CallI(
                    UnsafeBasePtr,
                    sessionNotification != null
                        ? Marshal.GetComInterfaceForObject(sessionNotification, typeof (IAudioSessionNotification))
                        : IntPtr.Zero,
                    ((void**) (*(void**) UnsafeBasePtr))[6]);
                _sessionNotifications.Add(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Registers the application to receive a notification when a session is created.
        /// </summary>
        /// <param name="sessionNotification">The application's implementation of the <see cref="IAudioSessionNotification"/> interface.</param>
        ///         /// <remarks>
        /// Use the <see cref="AudioSessionNotification"/> class as the default implementation for the <paramref name="sessionNotification"/> parameter.
        /// 
        /// <c>Note:</c> Make sure to call the <see cref="RegisterSessionNotification"/> from an MTA-Thread. Also make sure to enumerate all sessions after calling this method. 
        /// </remarks>
        public void RegisterSessionNotification(IAudioSessionNotification sessionNotification)
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.MTA)
                throw new InvalidOperationException("RegisterSessionNotification has to be called from an MTA-Thread.");
            CoreAudioAPIException.Try(RegisterSessionNotificationNative(sessionNotification), InterfaceName,
                "RegisterSessionNotification");
        }

        /// <summary>
        /// Deletes the registration to receive a notification when a session is created.
        /// <seealso cref="UnregisterSessionNotification"/>
        /// </summary>
        /// <param name="sessionNotification">
        /// The application's implementation of the <see cref="IAudioSessionNotification"/> interface. 
        /// Pass the same object that was specified to the session manager in a previous call <see cref="RegisterSessionNotification"/> to register for notification.</param>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterSessionNotificationNative(IAudioSessionNotification sessionNotification)
        {
            int result = 0;
            if (_sessionNotifications.Contains(sessionNotification))
            {
                result = InteropCalls.CallI(
                    UnsafeBasePtr,
                    sessionNotification != null
                        ? Marshal.GetComInterfaceForObject(sessionNotification, typeof (IAudioSessionNotification))
                        : IntPtr.Zero,
                    ((void**) (*(void**) UnsafeBasePtr))[7]);
                _sessionNotifications.Remove(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Deletes the registration to receive a notification when a session is created.
        /// </summary>
        /// <param name="sessionNotification">
        /// The application's implementation of the <see cref="IAudioSessionNotification"/> interface. 
        /// Pass the same object that was specified to the session manager in a previous call <see cref="RegisterSessionNotification"/> to register for notification.</param>
        public void UnregisterSessionNotification(IAudioSessionNotification sessionNotification)
        {
            CoreAudioAPIException.Try(UnregisterSessionNotificationNative(sessionNotification), InterfaceName,
                "UnregisterSessionNotification");
        }

        //--

        /// <summary>
        /// Registers the application to receive ducking notifications.
        /// <seealso cref="RegisterDuckNotification"/>
        /// </summary>
        /// <param name="sessionId">A string that contains a session instance identifier. Applications that are playing a media stream and want to provide custom stream attenuation or ducking behavior, pass their own session instance identifier.
        /// Other applications that do not want to alter their streams but want to get all the ducking notifications must pass NULL.</param>
        /// <param name="sessionNotification">Instance of any object which implements the <see cref="IAudioVolumeDuckNotification"/> and which should receive duck notifications.</param>
        /// <returns>HRESULT</returns>
        public unsafe int RegisterDuckNotificationNative(string sessionId,
            IAudioVolumeDuckNotification sessionNotification)
        {
            int result = 0;
            if (!_volumeDuckNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null
                    ? Marshal.GetComInterfaceForObject(sessionNotification, typeof (IAudioVolumeDuckNotification))
                    : IntPtr.Zero;
                IntPtr ptr0 = sessionId != null ? Marshal.StringToHGlobalUni(sessionId) : IntPtr.Zero;
                result = InteropCalls.CallI(UnsafeBasePtr, (void*) ptr0, (void*) ptr,
                    ((void**) (*(void**) UnsafeBasePtr))[8]);
                if (ptr0 != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr0);

                _volumeDuckNotifications.Add(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Registers the application to receive ducking notifications.
        /// </summary>
        /// <param name="sessionId">A string that contains a session instance identifier. Applications that are playing a media stream and want to provide custom stream attenuation or ducking behavior, pass their own session instance identifier.
        /// Other applications that do not want to alter their streams but want to get all the ducking notifications must pass NULL.</param>
        /// <param name="sessionNotification">Instance of any object which implements the <see cref="IAudioVolumeDuckNotification"/> and which should receive duck notifications.</param>
        public void RegisterDuckNotification(string sessionId, IAudioVolumeDuckNotification sessionNotification)
        {
            CoreAudioAPIException.Try(RegisterDuckNotificationNative(sessionId, sessionNotification), InterfaceName,
                "RegisterDuckNotification");
        }

        /// <summary>
        /// Deletes the registration to receive ducking notifications.
        /// <seealso cref="UnregisterDuckNotification"/>
        /// </summary>
        /// <param name="sessionNotification">
        /// The <see cref="IAudioVolumeDuckNotification"/> interface that is implemented by the application. Pass the same interface pointer that was specified to the session manager in a previous call to the <see cref="RegisterDuckNotification"/> method.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterDuckNotificationNative(IAudioVolumeDuckNotification sessionNotification)
        {
            int result = 0;
            if (_volumeDuckNotifications.Contains(sessionNotification))
            {
                IntPtr ptr = sessionNotification != null
                    ? Marshal.GetComInterfaceForObject(sessionNotification, typeof (IAudioVolumeDuckNotification))
                    : IntPtr.Zero;
                result = InteropCalls.CallI(UnsafeBasePtr, (void*) ptr, ((void**) (*(void**) UnsafeBasePtr))[9]);
                _volumeDuckNotifications.Remove(sessionNotification);
            }
            return result;
        }

        /// <summary>
        /// Deletes the registration to receive ducking notifications.
        /// </summary>
        /// <param name="sessionNotification">
        /// The <see cref="IAudioVolumeDuckNotification"/> interface that is implemented by the application. Pass the same interface pointer that was specified to the session manager in a previous call to the <see cref="RegisterDuckNotification"/> method.
        /// </param>
        public void UnregisterDuckNotification(IAudioVolumeDuckNotification sessionNotification)
        {
            CoreAudioAPIException.Try(UnregisterDuckNotificationNative(sessionNotification), InterfaceName,
                "UnregisterDuckNotification");
        }

        /// <summary>
        /// Releases the COM object and unregisters all session notifications and all volume duck notifications.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            for (int i = _sessionNotifications.Count - 1; i >= 0; i--)
            {
                UnregisterSessionNotification(_sessionNotifications[i]);
                _sessionNotifications.RemoveAt(i);
            }

            for (int i = _volumeDuckNotifications.Count - 1; i >= 0; i--)
            {
                UnregisterDuckNotification(_volumeDuckNotifications[i]);
                _volumeDuckNotifications.RemoveAt(i);
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        private bool _disposed;
    }
}