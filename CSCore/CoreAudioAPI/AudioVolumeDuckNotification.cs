using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The default implementation of the <see cref="IAudioVolumeDuckNotification"/> interface.
    /// </summary>
    [Guid("C3B284D4-6D39-4359-B3CF-B56DDB3BB39C")]
    public sealed class AudioVolumeDuckNotification : IAudioVolumeDuckNotification
    {
        /// <summary>
        /// Occurs when a pending system ducking event gets fired.
        /// </summary>
        public event EventHandler<VolumeDuckNotificationEventArgs> VolumeDuckNotification;

        /// <summary>
        /// Occurs when a pending system unducking event gets fired.
        /// </summary>
        public event EventHandler<VolumeDuckNotificationEventArgs> VolumeUnDuckNotification;

        /// <summary>
        /// Sends a notification about a pending system ducking event.
        /// </summary>
        /// <param name="sessionId">A string containing the session instance identifier of the communications session that raises the the auto-ducking event.</param>
        /// <param name="countCommunicationSessions">The number of active communications sessions. If there are n sessions, the sessions are numbered from 0 to –1.</param>
        /// <returns>HRESULT</returns>
        int IAudioVolumeDuckNotification.OnVolumeDuckNotification(string sessionId, int countCommunicationSessions)
        {
            if (VolumeDuckNotification != null)
                VolumeDuckNotification(this, new VolumeDuckNotificationEventArgs(sessionId, countCommunicationSessions));
            return (int) HResult.S_OK;
        }

        /// <summary>
        /// Sends a notification about a pending system unducking event. 
        /// </summary>
        /// <param name="sessionId">A string containing the session instance identifier of the terminating communications session that intiated the ducking.</param>
        /// <param name="countCommunicationSessions">The number of active communications sessions. If there are n sessions, they are numbered from 0 to n-1.</param>
        /// <returns></returns>
        int IAudioVolumeDuckNotification.OnVolumeUnduckNotification(string sessionId, int countCommunicationSessions)
        {
            if (VolumeUnDuckNotification != null)
            {
                VolumeUnDuckNotification(this,
                    new VolumeDuckNotificationEventArgs(sessionId, countCommunicationSessions));
            }
            return (int) HResult.S_OK;
        }
    }
}