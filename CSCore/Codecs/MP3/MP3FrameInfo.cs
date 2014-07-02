using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.MP3
{
    internal class Mp3FrameInfo
    {
        public long StreamPosition;
        public int SampleIndex;
        public int SampleAmount;
        public int Size;
    }
}
