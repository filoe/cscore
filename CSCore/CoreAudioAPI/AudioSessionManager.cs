using System;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="AudioSessionManager"/> class enables a client to access the session controls and volume controls for both cross-process and process-specific audio sessions.
    /// </summary>
    [Guid("BFA971F1-4D5E-40BB-935E-967039BFBEE4")]
    public class AudioSessionManager : ComObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionManager"/> class.
        /// </summary>
        /// <param name="ptr">Native pointer to the <see cref="AudioSessionManager"/> object.</param>
        public AudioSessionManager(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Retrieves an audio session control.
        /// </summary>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <param name="streamFlags">Specifies the status of the flags for the audio stream.</param>
        /// <param name="sessionControl">The <see cref="AudioSessionControl"/> of the specified <paramref name="audioSessionGuid"/>.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetAudioSessionControlNative(Guid audioSessionGuid, int streamFlags,
            out AudioSessionControl sessionControl)
        {
            sessionControl = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(UnsafeBasePtr, &audioSessionGuid, streamFlags, &ptr,
                ((void**) (*(void**) UnsafeBasePtr))[3]);
            if (ptr != IntPtr.Zero)
                sessionControl = new AudioSessionControl(ptr);
            return result;
        }

        /// <summary>
        /// Retrieves an audio session control.
        /// </summary>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <param name="streamFlags">Specifies the status of the flags for the audio stream.</param>
        /// <returns><see cref="AudioSessionControl"/> instance.</returns>
        public AudioSessionControl GetAudioSessionControl(Guid audioSessionGuid, int streamFlags)
        {
            AudioSessionControl sessionControl;
            CoreAudioAPIException.Try(GetAudioSessionControlNative(audioSessionGuid, streamFlags, out sessionControl),
                "IAudioSessionManager", "GetAudioSessionControl");
            return sessionControl;
        }

        /// <summary>
        /// Retrieves a simple audio volume control.
        /// </summary>
        /// <param name="crossProcessSession">Specifies whether the request is for a cross-process session. Set to TRUE if the session is cross-process. Set to FALSE if the session is not cross-process.</param>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <param name="audioVolume"><see cref="SimpleAudioVolume"/> of the audio volume control object.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSimpleAudioVolumeNative(Guid audioSessionGuid, NativeBool crossProcessSession,
            out SimpleAudioVolume audioVolume)
        {
            audioVolume = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(UnsafeBasePtr, &audioSessionGuid, crossProcessSession, &ptr,
                ((void**) (*(void**) UnsafeBasePtr))[4]);
            if (ptr != IntPtr.Zero)
                audioVolume = new SimpleAudioVolume(ptr);
            return result;
        }

        /// <summary>
        /// Retrieves a simple audio volume control.
        /// </summary>
        /// <param name="crossProcessSession">Specifies whether the request is for a cross-process session. Set to TRUE if the session is cross-process. Set to FALSE if the session is not cross-process.</param>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <returns><see cref="SimpleAudioVolume"/> instance.</returns>
        public SimpleAudioVolume GetSimpleAudioVolume(Guid audioSessionGuid, bool crossProcessSession)
        {
            SimpleAudioVolume v;
            CoreAudioAPIException.Try(GetSimpleAudioVolumeNative(audioSessionGuid, crossProcessSession, out v),
                "IAudioSessionManager", "GetSimpleAudioVolume");
            return v;
        }
    }
}