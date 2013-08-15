using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.VORBIS
{
    public class VorbisException : Exception
    {
        public VorbisException(string message)
            : base(message)
        {
        }

        public VorbisException(string message, params object[] obj)
            : this(String.Format(message, obj))
        {
        }

        public VorbisException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public VorbisException(string message, Exception innerException, params object[] obj)
            : this(String.Format(message, obj), innerException)
        {
        }
    }
}