using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// Exception which can occur while working with MP3 data.
    /// </summary>
    public class Mp3Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mp3Exception"/> class.
        /// </summary>
        /// <param name="message">Message of the <see cref="Mp3Exception"/>.</param>
        public Mp3Exception(String message)
            : base(message)
        {
        }
    }
}
