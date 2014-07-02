using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Defines a <see cref="ISoundIn"/> provider which offers a <see cref="RecordingState"/> property.
    /// </summary>
    public interface ISoundRecorder : ISoundIn
    {
        /// <summary>
        /// Gets the current <see cref="SoundIn.RecordingState"/> of the <see cref="ISoundRecorder"/>.
        /// </summary>
        RecordingState RecordingState { get; }
    }
}
