using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.SoundIn
{
    public interface ISoundRecorder : ISoundIn
    {
        RecordingState RecordingState { get; }
    }
}
