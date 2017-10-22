using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="IAudioVolumeDuckNotification"/> interface is used to by the system to send notifications about stream attenuation changes.
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd371012(v=vs.85).aspx"/>.</remarks>
    [ComImport]
    [Guid("C3B284D4-6D39-4359-B3CF-B56DDB3BB39C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IAudioVolumeDuckNotification
    {
        /// <summary>
        /// Sends a notification about a pending system ducking event.
        /// </summary>
        /// <param name="sessionId">A string containing the session instance identifier of the communications session that raises the the auto-ducking event.</param>
        /// <param name="countCommunicationSessions">The number of active communications sessions. If there are n sessions, the sessions are numbered from 0 to –1.</param>
        /// <returns>HRESULT</returns>
        int OnVolumeDuckNotification([In, MarshalAs(UnmanagedType.LPWStr)] string sessionId,
            int countCommunicationSessions);

        /// <summary>
        /// Sends a notification about a pending system unducking event. 
        /// </summary>
        /// <param name="sessionId">A string containing the session instance identifier of the terminating communications session that intiated the ducking.</param>
        /// <param name="countCommunicationSessions">The number of active communications sessions. If there are n sessions, they are numbered from 0 to n-1.</param>
        /// <returns>HRESULT</returns>
        int OnVolumeUnduckNotification([In, MarshalAs(UnmanagedType.LPWStr)] string sessionId,
            int countCommunicationSessions);
    }
}