using System;

namespace CSCore.CoreAudioAPI
{
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