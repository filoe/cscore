using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionNotification.
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd370969(v=vs.85).aspx.
    /// </summary>
    [Guid("641DD20B-4D41-49CC-ABA3-174B9477BB08")]
    public class AudioSessionNotification : IAudioSessionNotification
    {
        /// <summary>
        /// Notifies the registered processes that the audio session has been created.
        /// </summary>
        public event EventHandler<SessionCreatedEventArgs> SessionCreated;

        int IAudioSessionNotification.OnSessionCreated(IntPtr newSession)
        {
            if (SessionCreated != null)
                SessionCreated(this, new SessionCreatedEventArgs(new AudioSessionControl(newSession)));
            return (int)Win32.HResult.S_OK;
        }
    }

    /// <summary>
    /// SessionCreatedEventArgs
    /// </summary>
    public class SessionCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// AudioSessionControl interface of the audio session that was created.
        /// </summary>
        public AudioSessionControl NewSession { get; private set; }

        public SessionCreatedEventArgs(AudioSessionControl newSession)
        {
            if (newSession == null)
                throw new ArgumentNullException("newSession");
            NewSession = newSession;
        }
    }
}
