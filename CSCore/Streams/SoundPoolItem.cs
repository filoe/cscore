using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class SoundPoolItem
    {
        public ISampleSource Sound { get; set; }
        public bool AutoDequeueOnEndOfStream { get; set; }

        public SoundPoolItem()
        {
            AutoDequeueOnEndOfStream = false;
        }

        public SoundPoolItem(ISampleSource sound, bool autoDequeueOnEndOfStream)
            : this()
        {
            if (sound == null)
                throw new ArgumentNullException("sound");

            Sound = sound;
            AutoDequeueOnEndOfStream = autoDequeueOnEndOfStream;
        }
    }
}
