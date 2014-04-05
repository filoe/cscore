using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    [Guid("BFA971F1-4D5E-40BB-935E-967039BFBEE4")]
    public class AudioSessionManager : ComObject
    {
        public AudioSessionManager(IntPtr ptr)
            : base(ptr)
		{
		}

        /// <summary>
        /// The GetAudioSessionControl method retrieves an audio session control.
        /// </summary>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <param name="streamFlags">Specifies the status of the flags for the audio stream.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetAudioSessionControlNative(Guid audioSessionGuid, int streamFlags, out AudioSessionControl sessionControl)
        {
            sessionControl = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &audioSessionGuid, streamFlags, &ptr, ((void**)(*(void**)_basePtr))[3]);
            if (ptr != IntPtr.Zero)
                sessionControl = new AudioSessionControl(ptr);
            return result;
        }

        /// <summary>
        /// The GetAudioSessionControl method retrieves an audio session control.
        /// </summary>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <param name="streamFlags">Specifies the status of the flags for the audio stream.</param>
        public AudioSessionControl GetAudioSessionControl(Guid audioSessionGuid, int streamFlags)
        {
            AudioSessionControl sessionControl;
            CoreAudioAPIException.Try(GetAudioSessionControlNative(audioSessionGuid, streamFlags, out sessionControl), "IAudioSessionManager", "GetAudioSessionControl");
            return sessionControl;
        }

        /// <summary>
        /// The GetSimpleAudioVolume method retrieves a simple audio volume control.
        /// </summary>
        /// <param name="crossProcessSession">Specifies whether the request is for a cross-process session. Set to TRUE if the session is cross-process. Set to FALSE if the session is not cross-process.</param>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetSimpleAudioVolumeNative(Guid audioSessionGuid, NativeBool crossProcessSession, out SimpleAudioVolume audioVolume)
        {
            audioVolume = null;
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &audioSessionGuid, crossProcessSession, &ptr, ((void**)(*(void**)_basePtr))[4]);
            if (ptr != IntPtr.Zero)
                audioVolume = new SimpleAudioVolume(ptr);
            return result;
        }

        /// <summary>
        /// The GetSimpleAudioVolume method retrieves a simple audio volume control.
        /// </summary>
        /// <param name="crossProcessSession">Specifies whether the request is for a cross-process session. Set to TRUE if the session is cross-process. Set to FALSE if the session is not cross-process.</param>
        /// <param name="audioSessionGuid">If the GUID does not identify a session that has been previously opened, the call opens a new but empty session. If the value is Guid.Empty, the method assigns the stream to the default session.</param>
        public SimpleAudioVolume GetSimpleAudioVolume(Guid audioSessionGuid, bool crossProcessSession)
        {
            SimpleAudioVolume v;
            CoreAudioAPIException.Try(GetSimpleAudioVolumeNative(audioSessionGuid, (NativeBool)crossProcessSession, out v), "IAudioSessionManager", "GetSimpleAudioVolume");
            return v;
        }
    }
}
