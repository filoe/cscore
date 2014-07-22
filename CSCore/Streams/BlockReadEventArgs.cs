using System;

namespace CSCore.Streams
{
    /// <summary>
    /// Provides data for the <see cref="NotificationSource.BlockRead"/> event.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Data"/> array.</typeparam>
    public class BlockReadEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the number of read elements.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the array which contains the read data.
        /// </summary>
        public T[] Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockReadEventArgs{T}"/> class.
        /// </summary>
        /// <param name="data">The read data.</param>
        /// <param name="length">The number of read elements.</param>
        public BlockReadEventArgs(T[] data, int length)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (length < 0)
                throw new ArgumentNullException("length");

            Data = data;
            Length = length;
        }
    }
}