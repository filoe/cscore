using System;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Provides data for the <see cref="ISoundIn.DataAvailable"/> event.
    /// </summary>
    public class DataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the available data.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the number of available bytes.
        /// </summary>
        public int ByteCount { get; private set; }

        /// <summary>
        /// Gets the zero-based offset inside of the <see cref="Data"/> array at which the available data starts.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Gets the format of the available <see cref="Data"/>.
        /// </summary>
        public WaveFormat Format { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="data">A byte array which contains the data.</param>
        /// <param name="offset">The offset inside of the <see cref="Data"/> array at which the available data starts.</param>
        /// <param name="bytecount">The number of available bytes.</param>
        /// <param name="format">The format of the <paramref name="data"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// data
        /// or
        /// format
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// offset must not be less than zero. 
        /// bytecount must not be or equal to zero.
        /// </exception>
        public DataAvailableEventArgs(byte[] data, int offset, int bytecount, WaveFormat format)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (bytecount <= 0 || bytecount > data.Length)
                throw new ArgumentOutOfRangeException("bytecount");
            if (format == null)
                throw new ArgumentNullException("format");

            Offset = offset;
            Data = data;
            ByteCount = bytecount;
            Format = format;
        }
    }
}