using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    /// <summary>
    ///     Represents a Dmo output data buffer. For more details see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd375507(v=vs.85).aspx"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct DmoOutputDataBuffer : IDisposable
    {
        /// <summary>
        ///     Pointer to the <see cref="IMediaBuffer"/> interface of a buffer allocated by the application.
        /// </summary>
        [MarshalAs(UnmanagedType.Interface)] public IMediaBuffer Buffer;

        /// <summary>
        ///     Status flags. After processing output, the DMO sets this member to a bitwise combination
        ///     of <see cref="OutputDataBufferFlags.None"/> or more <see cref="OutputDataBufferFlags"/> flags.
        /// </summary>
        public OutputDataBufferFlags Status;

        /// <summary>
        ///     Time stamp that specifies the start time of the data in the buffer. If the buffer has a
        ///     valid time stamp, the DMO sets this member and also sets the
        ///     <see cref="OutputDataBufferFlags.Time"/> flag in the dwStatus member. Otherwise, ignore this member.
        /// </summary>
        public long Timestamp;

        /// <summary>
        ///     Reference time specifying the length of the data in the buffer. If the DMO sets this
        ///     member to a valid value, it also sets the <see cref="OutputDataBufferFlags.TimeLength"/> flag in the
        ///     dwStatus member. Otherwise, ignore this member.
        /// </summary>
        public long TimeLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmoOutputDataBuffer"/> struct.
        /// </summary>
        /// <param name="bufferSize">The maxlength (in bytes) of the internally used <see cref="MediaBuffer"/>.</param>
        public DmoOutputDataBuffer(int bufferSize)
        {
            Buffer = new MediaBuffer(bufferSize);
            Status = OutputDataBufferFlags.None;
            Timestamp = 0;
            TimeLength = 0;
        }

        /*public bool IsDataAvailable
        {
            get { return Status.HasFlag(OutputDataBufferFlags.Incomplete); }
        }*/

        /// <summary>
        ///     Gets the length of the <see cref="MediaBuffer"/>.
        /// </summary>
        public int Length
        {
            get { return ((MediaBuffer) Buffer).Length; }
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="MediaBuffer"/>.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">
        ///     Zero-based byte offset in the specified buffer at which to begin storing the data read from the
        ///     buffer.
        /// </param>
        /// <returns>The number of read bytes.</returns>        
        public void Read(byte[] buffer, int offset)
        {
            ((MediaBuffer) Buffer).Read(buffer, offset);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="MediaBuffer"/>.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">
        ///     Zero-based byte offset in the specified buffer at which to begin storing the data read from the
        ///     buffer.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the buffer.</param>
        /// <returns>The number of read bytes.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, Length);

            ((MediaBuffer) Buffer).Read(buffer, offset, count);

            return count;
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="MediaBuffer"/>.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">
        ///     Zero-based byte offset in the specified buffer at which to begin storing the data read from the
        ///     buffer.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the buffer.</param>
        /// <param name="sourceOffset">Zero-based offset inside of the source buffer at which to begin copying data.</param>
        /// <returns>The number of read bytes.</returns>
        internal int Read(byte[] buffer, int offset, int count, int sourceOffset)
        {
            count = Math.Min(count, Length - sourceOffset);

            ((MediaBuffer) Buffer).Read(buffer, offset, count, sourceOffset);

            return count;
        }

        /// <summary>
        ///     Resets the Buffer. Sets the length of the <see cref="MediaBuffer"/> to zero and sets the
        ///     <see cref="Status" /> to <see cref="OutputDataBufferFlags.None" />.
        /// </summary>
        public void Reset()
        {
            Buffer.SetLength(0);
            Status = OutputDataBufferFlags.None;
        }

        /// <summary>
        ///     Disposes the internally used <see cref="MediaBuffer"/>.
        /// </summary>
        public void Dispose()
        {
            if (Buffer != null)
            {
                ((MediaBuffer) Buffer).Dispose();
                Buffer = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}