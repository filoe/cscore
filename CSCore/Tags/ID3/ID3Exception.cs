using System;

namespace CSCore.Tags.ID3
{
    public class ID3Exception : Exception
    {
        public ID3Exception(String message, params Object[] args)
            : this(String.Format(message, args))
        {
        }

        public ID3Exception(String message)
            : base(message)
        {
        }

        public ID3Exception(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}