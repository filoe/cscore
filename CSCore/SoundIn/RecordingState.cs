using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Defines the states of a <see cref="ISoundIn"/>.
    /// </summary>
    public enum RecordingState
    {
        /// <summary>
        /// The <see cref="ISoundIn"/> is currently recording.
        /// </summary>
        Recording,
        /// <summary>
        /// The <see cref="ISoundIn"/> is currently stopped.
        /// </summary>
        Stopped
    }
}
