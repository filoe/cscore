using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Specifies reasons that a audio session was disconnected.
    /// </summary>
    /// <remarks>For more information about WTS sessions, see the Windows SDK documentation or <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370941(v=vs.85).aspx"/>.</remarks>
    public enum AudioSessionDisconnectReason
    {
        /// <summary>
        /// The user removed the audio endpoint device.
        /// </summary>
        DisconnectReasonDeviceRemoval = 0,

        /// <summary>
        /// The Windows audio service has stopped.
        /// </summary>
        DisconnectReasonServerShutdown = (DisconnectReasonDeviceRemoval + 1),

        /// <summary>
        /// The stream format changed for the device that the audio session is connected to.
        /// </summary>
        DisconnectReasonFormatChanged = (DisconnectReasonServerShutdown + 1),

        /// <summary>
        /// The user logged off the Windows Terminal Services (WTS) session that the audio session was running in.
        /// </summary>
        DisconnectReasonSessionLogoff = (DisconnectReasonFormatChanged + 1),

        /// <summary>
        /// The WTS session that the audio session was running in was disconnected.
        /// </summary>
        DisconnectReasonSessionDisconnected = (DisconnectReasonSessionLogoff + 1),

        /// <summary>
        /// The (shared-mode) audio session was disconnected to make the audio endpoint device available for an exclusive-mode connection.
        /// </summary>
        DisconnectReasonExclusiveModeOverride = (DisconnectReasonSessionDisconnected + 1)
    }
}