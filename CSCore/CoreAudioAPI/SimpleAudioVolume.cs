using CSCore.Utils;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd316531(v=vs.85).aspx
    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8")]
    public class SimpleAudioVolume : ComObject
    {
        private const string c = "ISimpleAudioVolume";
        private static readonly Guid IID_SimpleAudioVolume = new Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8");

        public static SimpleAudioVolume FromAudioClient(AudioClient audioClient)
        {
            if (audioClient == null)
                throw new ArgumentNullException("audioClient");

            return new SimpleAudioVolume(audioClient.GetService(IID_SimpleAudioVolume));
        }

        public SimpleAudioVolume(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Gets or sets the master volume level for the audio session.
        /// </summary>
        public float MasterVolume
        {
            get
            {
                float volume;
                CoreAudioAPIException.Try(GetMasterVolumeInternal(out volume), c, "GetMasterVolume");
                return volume;
            }
            set
            {
                if (value < 0f || value > 1f)
                    throw new ArgumentOutOfRangeException("value");
                CoreAudioAPIException.Try(SetMasterVolumeInternal(value, Guid.Empty), c, "SetMasterVolume");
            }
        }

        /// <summary>
        /// Gets or sets the muting state for the audio session.
        /// </summary>
        public bool IsMuted
        {
            get
            {
                NativeBool muted;
                CoreAudioAPIException.Try(GetMuteInternal(out muted), c, "GetMute");
                return muted;
            }
            set
            {
                CoreAudioAPIException.Try(SetMuteInternal(value, Guid.Empty), c, "SetMute");
            }
        }

        /// <summary>
        /// The SetMasterVolume method sets the master volume level for the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetMasterVolumeInternal(float volume, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, volume, &eventContext, ((void**)(*(void**)_basePtr))[3]);
        }

        /// <summary>
        /// The GetMasterVolume method retrieves the client volume level for the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetMasterVolumeInternal(out float volume)
        {
            fixed (void* pvolume = &volume)
            {
                return InteropCalls.CallI(_basePtr, pvolume, ((void**)(*(void**)_basePtr))[4]);
            }
        }

        /// <summary>
        /// The SetMute method sets the muting state for the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetMuteInternal(NativeBool muted, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, muted, &eventContext, ((void**)(*(void**)_basePtr))[5]);
        }

        /// <summary>
        /// The GetMute method retrieves the current muting state for the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetMuteInternal(out NativeBool ismuted)
        {
            fixed (void* pismuted = &ismuted)
            {
                return InteropCalls.CallI(_basePtr, pismuted, ((void**)(*(void**)_basePtr))[6]);
            }
        }
    }
}