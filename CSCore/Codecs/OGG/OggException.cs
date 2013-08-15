using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.OGG
{
    public class OggException : Exception
    {
        public OggException(string message, OggExceptionLayer layer)
            : base(message + " " + layer.ToString())
        {
        }
    }

    public enum OggExceptionLayer
    {
        Page,
        Packet,
        Segment
    }
}