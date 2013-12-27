using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    [Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD")]
    public class AudioSessionControl : ComObject
    {
        const string c = "IAudioSessionControl";

        private List<IAudioSessionEvents> _sessionEventHandler;

        public AudioSessionControl(IntPtr ptr)
            : base(ptr)
		{
            _sessionEventHandler = new List<IAudioSessionEvents>();
		}

        /// <summary>
        /// Gets the current state of the audio session.
        /// </summary>
        public AudioSessionState SessionState
        {
            get 
            {
                AudioSessionState sessionState;
                CoreAudioAPIException.Try(GetStateNative(out sessionState), c, "GetState");
                return sessionState;
            }
        }

        /// <summary>
        /// Gets or sets the display name for the audio session.
        /// </summary>
        public string DisplayName
        {
            get
            {
                string displayName;
                CoreAudioAPIException.Try(GetDisplayNameNative(out displayName), c, "GetDisplayName");
                return displayName;
            }
            set
            {
                CoreAudioAPIException.Try(SetDisplayNameNative(value, Guid.Empty), c, "SetDisplayName"); 
            }
        }

        /// <summary>
        /// Gets or sets the path for the display icon for the audio session.
        /// </summary>
        public string IconPath
        {
            get
            {
                string iconPath;
                CoreAudioAPIException.Try(GetIconPathNative(out iconPath), c, "GetIconPath");
                return iconPath;
            }
            set
            {
                CoreAudioAPIException.Try(SetIconPathNative(value, Guid.Empty), c, "SetIconPath");
            }
        }

        /// <summary>
        /// Gets or sets the grouping parameter of the audio session.
        /// </summary>
        public Guid GroupingParam
        {
            get
            {
                Guid gp;
                CoreAudioAPIException.Try(GetGroupingParamNative(out gp), c, "GetGroupingParam");
                return gp;
            }
            set
            {
                CoreAudioAPIException.Try(SetGroupingParamNative(value, Guid.Empty), c, "SetGroupingParam");
            }
        }

        /// <summary>
        /// The GetState method retrieves the current state of the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetStateNative(out AudioSessionState state)
        {
            fixed (void* p = &state)
            {
                return InteropCalls.CallI(_basePtr, p, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        /// <summary>
        /// The GetDisplayName method retrieves the display name for the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int GetDisplayNameNative(out string displayName)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &ptr, ((void**)(*(void**)_basePtr))[4]);
            if (result == 0 && ptr != IntPtr.Zero)
            {
                displayName = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
            }
            else
            {
                displayName = null;
            }
            return result;
        }

        /// <summary>
        /// The SetDisplayName method assigns a display name to the current session.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetDisplayNameNative(string displayName, Guid eventContext)
        {
            IntPtr p = displayName != null ? Marshal.StringToHGlobalUni(displayName) : IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, p.ToPointer(), &eventContext, ((void**)(*(void**)_basePtr))[5]);
            Marshal.FreeHGlobal(p);
            return result;
        }

        /// <summary>
        /// The GetIconPath method retrieves the path for the display icon for the audio session.
        /// </summary>
        /// <param name="iconPath">See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368261(v=vs.85).aspx </param>
        /// <returns>HRESULT</returns>
        public unsafe int GetIconPathNative(out string iconPath)
        {
            IntPtr ptr = IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, &ptr, ((void**)(*(void**)_basePtr))[6]);
            if (result == 0 && ptr != IntPtr.Zero)
            {
                iconPath = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
            }
            else
            {
                iconPath = null;
            }
            return result;
        }

        /// <summary>
        /// The SetDisplayName method assigns a display name to the current session.
        /// </summary>
        /// <param name="iconPath">See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368274(v=vs.85).aspx </param>
        /// <returns>HRESULT</returns>
        public unsafe int SetIconPathNative(string iconPath, Guid eventContext)
        {
            IntPtr p = iconPath != null ? Marshal.StringToHGlobalUni(iconPath) : IntPtr.Zero;
            int result = InteropCalls.CallI(_basePtr, p.ToPointer(), &eventContext, ((void**)(*(void**)_basePtr))[7]);
            Marshal.FreeHGlobal(p);
            return result;
        }

        /// <summary>
        /// The GetGroupingParam method retrieves the grouping parameter of the audio session.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>For some more information about Grouping Parameters see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370848(v=vs.85).aspx </remarks>
        public unsafe int GetGroupingParamNative(out Guid groupingParam)
        {
            fixed (void* p = &groupingParam)
            {
                return InteropCalls.CallI(_basePtr, p, ((void**)(*(void**)_basePtr))[8]);
            }
        }

        /// <summary>
        /// The SetGroupingParam method assigns a session to a grouping of sessions.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>For some more information about Grouping Parameters see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370848(v=vs.85).aspx </remarks>
        public unsafe int SetGroupingParamNative(Guid groupingParam, Guid eventContext)
        {
            return InteropCalls.CallI(_basePtr, &groupingParam, &eventContext, ((void**)(*(void**)_basePtr))[9]);
        }

        /// <summary>
        /// The RegisterAudioSessionNotification method registers the client to receive notifications of session events, including changes in the stream state.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int RegisterAudioSessionNotificationNative(IAudioSessionEvents notifications)
        {
            int result = 0;
            if (!_sessionEventHandler.Contains(notifications))
            {
                IntPtr ptr = notifications != null ? Marshal.GetComInterfaceForObject(notifications, typeof(IAudioSessionEvents)) : IntPtr.Zero;
                result = InteropCalls.CallI(_basePtr, ptr.ToPointer(), ((void**)(*(void**)_basePtr))[10]);
                _sessionEventHandler.Add(notifications);
            }
            return result;
        }

        /// <summary>
        /// The RegisterAudioSessionNotification method registers the client to receive notifications of session events, including changes in the stream state.
        /// </summary>
        public void RegisterAudioSessionNotification(IAudioSessionEvents notifications)
        {
            CoreAudioAPIException.Try(RegisterAudioSessionNotificationNative(notifications), c, "RegisterAudioSessionNotification");
        }

        /// <summary>
        /// The UnregisterAudioSessionNotification method deletes a previous registration by the client to receive notifications.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int UnregisterAudioSessionNotificationNative(IAudioSessionEvents notifications)
        {
            int result = 0;
            if (_sessionEventHandler.Contains(notifications))
            {
                IntPtr ptr = notifications != null ? Marshal.GetComInterfaceForObject(notifications, typeof(IAudioSessionEvents)) : IntPtr.Zero;
                result = InteropCalls.CallI(_basePtr, ptr.ToPointer(), ((void**)(*(void**)_basePtr))[11]);
                _sessionEventHandler.Remove(notifications);
            }
            return result;
        }

        /// <summary>
        /// The UnregisterAudioSessionNotification method deletes a previous registration by the client to receive notifications.
        /// </summary>
        public void UnregisterAudioSessionNotification(IAudioSessionEvents notifications)
        {
            CoreAudioAPIException.Try(UnregisterAudioSessionNotificationNative(notifications), c, "UnregisterAudioSessionNotification");
        }
    }
}
