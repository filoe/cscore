using System;

namespace CSCore.Tags.ID3
{
    /// <summary>
    /// Exception class for all ID3-Tag related Exceptions.
    /// </summary>
    [Serializable]
    public class ID3Exception : Exception
    {
        internal ID3Exception(String message, params Object[] args)
            : this(String.Format(message, args))
        {
        }

        internal ID3Exception(String message)
            : base(message)
        {
        }

        internal ID3Exception(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}