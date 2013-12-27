using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioVolumeDuckNotification
    /// </summary>
    [Guid("C3B284D4-6D39-4359-B3CF-B56DDB3BB39C")]
    public class AudioVolumeDuckNotification : IAudioVolumeDuckNotification
    {
        /// <summary>
        /// Notification about a pending system ducking event.
        /// </summary>
        public event EventHandler<VolumeDuckNotificationEventArgs> VolumeDuckNotification;
        /// <summary>
        /// Notification about a pending system unducking event.
        /// </summary>
        public event EventHandler<VolumeDuckNotificationEventArgs> VolumeUnDuckNotification;

        int IAudioVolumeDuckNotification.OnVolumeDuckNotification(string sessionID, int countCommunicationSessions)
        {
            if (VolumeDuckNotification != null)
                VolumeDuckNotification(this, new VolumeDuckNotificationEventArgs(sessionID, countCommunicationSessions));
            return (int)Win32.HResult.S_OK;
        }

        int IAudioVolumeDuckNotification.OnVolumeUnduckNotification(string sessionID, int countCommunicationSessions)
        {
            if (VolumeUnDuckNotification != null)
                VolumeUnDuckNotification(this, new VolumeDuckNotificationEventArgs(sessionID, countCommunicationSessions));
            return (int)Win32.HResult.S_OK;
        }
    }

    /// <summary>
    /// VolumeDuckNotificationEventArgs. Fore more see http://msdn.microsoft.com/en-us/library/windows/desktop/dd371010(v=vs.85).aspx.
    /// </summary>
    public class VolumeDuckNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// A string containing the session instance identifier of the communications session that raises the the auto-ducking event. To get the session instance identifier, call IAudioSessionControl2::GetSessionInstanceIdentifier.
        /// </summary>
        public string SessionID { get; private set; }
        /// <summary>
        /// The number of active communications sessions. If there are n sessions, the sessions are numbered from 0 to –1.
        /// </summary>
        public int CountCommunicationSessions { get; private set; }

        public VolumeDuckNotificationEventArgs(string sessionID, int countCommunicationSessions)
        {
            if (sessionID == null)
                throw new ArgumentNullException("sessionID");
            if (countCommunicationSessions < 0)
                throw new ArgumentOutOfRangeException("countCommunicationSessions");

            SessionID = sessionID;
            CountCommunicationSessions = countCommunicationSessions;
        }
    }
}
