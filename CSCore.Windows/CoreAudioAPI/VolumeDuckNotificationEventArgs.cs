using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioVolumeDuckNotification.VolumeDuckNotification"/> and the <see cref="AudioVolumeDuckNotification.VolumeUnDuckNotification"/> event. 
    /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd371010(v=vs.85).aspx"/>.
    /// </summary>
    public class VolumeDuckNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// A string containing the session instance identifier of the communications session that raises the auto-ducking event.
        /// </summary>
        public string SessionID { get; private set; }

        /// <summary>
        /// The number of active communications sessions. If there are n sessions, the sessions are numbered from 0 to –1.
        /// </summary>
        public int CountCommunicationSessions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDuckNotificationEventArgs"/> class.
        /// </summary>
        /// <param name="sessionID">The session instance identifier of the communications session that raises the the auto-ducking event.</param>
        /// <param name="countCommunicationSessions">number of active communications sessions.</param>
        /// <exception cref="System.ArgumentNullException">sessionID is null or empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">countCommunicationSessions is less than zero.</exception>
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