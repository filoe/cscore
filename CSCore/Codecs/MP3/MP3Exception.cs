using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.MP3
{
    public class MP3Exception : Exception
    {
        public MP3Exception(String message)
            : base(message)
        {
        }
    }
}
