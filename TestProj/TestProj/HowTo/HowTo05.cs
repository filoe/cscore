using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestProj.HowTo
{
    public class HowTo05
    {
        public void SetVolumeOfAnISoundOutInterface(ISoundOut soundOut, float newVolume)
        {
            soundOut.Volume = newVolume;
            Debug.WriteLine("Adjusted the volume to {0:P}.", newVolume);

            Debug.WriteLine("Current volume: {0:P}.", soundOut.Volume);
        }
    }
}
