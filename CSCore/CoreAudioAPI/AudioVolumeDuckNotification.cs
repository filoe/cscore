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
}
