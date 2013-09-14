using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.SoundOut.MMInterop
{
    [System.Flags]
    public enum WaveCapsSupported : int
    {
        WAVECAPS_PITCH = 1,
        WAVECAPS_PLAYBACKRATE = 2,
        WAVECAPS_VOLUME = 4,
        WAVECAPS_LRVOLUME = 8,
        WAVECAPS_SYNC = 16,
        WAVECAPS_SAMPLEACCURATE = 32,
        WAVECAPS_DIRECTSOUND = 64
    }
}
