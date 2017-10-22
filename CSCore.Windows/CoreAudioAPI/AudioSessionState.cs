using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Defines constants that indicate the current state of an audio session.
    /// </summary>
    public enum AudioSessionState
    {
        /// <summary>
        /// The session has no active audio streams.
        /// </summary>
        AudioSessionStateInactive = 0,

        /// <summary>
        /// The session has active audio streams.
        /// </summary>
        AudioSessionStateActive = 1,

        /// <summary>
        /// The session is dormant.
        /// </summary>
        AudioSessionStateExpired = 2
    }
}