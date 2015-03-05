using CSCore.Utils;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="SimpleAudioVolume"/> object enables a client to control the master volume level of an audio session. 
    /// For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd316535(v=vs.85).aspx"/>.
    /// </summary>
    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8")]
    public class SimpleAudioVolume : ComObject
    {
        private const string InterfaceName = "ISimpleAudioVolume";
        private static readonly Guid IID_SimpleAudioVolume = new Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8");

        /// <summary>
        ///     Creates a new <see cref="SimpleAudioVolume" /> instance by calling the <see cref="AudioClient.GetService" /> method of the
        ///     specified <paramref name="audioClient" />.
        /// </summary>
        /// <param name="audioClient">
        ///     The <see cref="AudioClient" /> which should be used to create the <see cref="SimpleAudioVolume" />-instance
        ///     with.
        /// </param>
        /// <returns>A new instance of the <see cref="SimpleAudioVolume"/> class.</returns>
        public static SimpleAudioVolume FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new SimpleAudioVolume(audioClient.GetService(IID_SimpleAudioVolume));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleAudioVolume"/> class.
        /// </summary>
        /// <param name="nativePtr">The native pointer of the COM object.</param>
        public SimpleAudioVolume(IntPtr nativePtr)
            : base(nativePtr)
        {
        }

        /// <summary>
        /// Gets or sets the master volume level for the audio session. Valid volume levels are in the range 0.0 (=0%) to 1.0 (=100%).
        /// </summary>
        public float MasterVolume
        {
            get
            {
                float volume;
                CoreAudioAPIException.Try(GetMasterVolumeNative(out volume), InterfaceName, "GetMasterVolume");
                return volume;
            }
            set
            {
                if (value < 0f || value > 1f)
                    throw new ArgumentOutOfRangeException("value");
                CoreAudioAPIException.Try(SetMasterVolumeNative(value, Guid.Empty), InterfaceName, "SetMasterVolume");
            }
        }

        /// <summary>
        /// Gets or sets the muting state for the audio session. <c>True</c> indicates that muting is enabled. <c>False</c> indicates that it is disabled.
        /// </summary>
        public bool IsMuted
        {
            get
            {
                NativeBool muted;
                CoreAudioAPIException.Try(GetMuteInternal(out muted), InterfaceName, "GetMute");
                return muted;
            }
            set
            {
                CoreAudioAPIException.Try(SetMuteNative(value, Guid.Empty), InterfaceName, "SetMute");
            }
        }

        /// <summary>
        /// Sets the master volume level for the audio session.
        /// <seealso cref="MasterVolume"/>
        /// </summary>
        /// <param name="volume">The new master volume level. Valid volume levels are in the range 0.0 to 1.0.</param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetMasterVolumeNative(float volume, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, volume, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[3]);
        }

        /// <summary>
        /// Retrieves the client volume level for the audio session.
        /// <seealso cref="MasterVolume"/>
        /// </summary>
        /// <param name="volume">A variable into which the method writes the client volume level. The volume level is a value in the range 0.0 to 1.0.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMasterVolumeNative(out float volume)
        {
            fixed (void* pvolume = &volume)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pvolume, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }

        /// <summary>
        /// Sets the muting state for the audio session.
        /// <seealso cref="IsMuted"/>
        /// </summary>
        /// <param name="muted">The new muting state. TRUE enables muting. FALSE disables muting.</param>
        /// <param name="eventContext">EventContext which can be accessed in the event handler.</param>        
        /// <returns>HRESULT</returns>
        public unsafe int SetMuteNative(NativeBool muted, Guid eventContext)
        {
            return InteropCalls.CallI(UnsafeBasePtr, muted, &eventContext, ((void**) (*(void**) UnsafeBasePtr))[5]);
        }

        /// <summary>
        /// The GetMute method retrieves the current muting state for the audio session.
        /// <seealso cref="IsMuted"/>
        /// </summary>
        /// <param name="isMuted">A variable into which the method writes the muting state. TRUE indicates that muting is enabled. FALSE indicates that it is disabled.</param>
        /// <returns>HRESULT</returns>
        public unsafe int GetMuteInternal(out NativeBool isMuted)
        {
            fixed (void* pismuted = &isMuted)
            {
                return InteropCalls.CallI(UnsafeBasePtr, pismuted, ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }
    }
}