using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.DMO
{
    /// <summary>
    /// Represents a DmoOutputDataBuffer. Fore more details see http://msdn.microsoft.com/en-us/library/windows/desktop/dd375507(v=vs.85).aspx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct DmoOutputDataBuffer : IDisposable
    {
        /// <summary>
        /// Pointer to the IMediaBuffer interface of a buffer allocated by the application.
        /// </summary>
        [MarshalAs(UnmanagedType.Interface)]
        public IMediaBuffer Buffer;

        /// <summary>
        /// Status flags. After processing output, the DMO sets this member to a bitwise combination
        /// of zero or more DMO_OUTPUT_DATA_BUFFER_FLAGS flags.
        /// </summary>
        public OutputDataBufferFlags Status;

        /// <summary>
        /// Time stamp that specifies the start time of the data in the buffer. If the buffer has a
        /// valid time stamp, the DMO sets this member and also sets the
        /// DMO_OUTPUT_DATA_BUFFERF_TIME flag in the dwStatus member. Otherwise, ignore this member.
        /// </summary>
        public long Timestamp;

        /// <summary>
        /// Reference time specifying the length of the data in the buffer. If the DMO sets this
        /// member to a valid value, it also sets the DMO_OUTPUT_DATA_BUFFERF_TIMELENGTH flag in the
        /// dwStatus member. Otherwise, ignore this member.
        /// </summary>
        public long TimeLength;

        /// <summary>
        /// Creates a new DmoOutputDataBuffer.
        /// </summary>
        /// <param name="bufferSize">The maxlength (in bytes) of the internally used MediaBuffer.</param>
        public DmoOutputDataBuffer(int bufferSize)
        {
            Buffer = new MediaBuffer(bufferSize);
            Status = OutputDataBufferFlags.None;
            Timestamp = 0;
            TimeLength = 0;
        }

        /*public bool DataAvailable
        {
            get { return Status.HasFlag(OutputDataBufferFlags.Incomplete); }
        }*/

        /// <summary>
        /// Gets the length of the MediaBuffer. See <see cref="MediaBuffer.Length"/>.
        /// </summary>
        public int Length
        {
            get { return (Buffer as MediaBuffer).Length; }
        }

        /// <summary>
        /// Reads a sequence of bytes from the MediaBuffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">Zero-based byte offset in the specified buffer at which to begin storing the data read from the buffer.</param>
        public void Read(byte[] buffer, int offset)
        {
            (Buffer as MediaBuffer).Read(buffer, offset);
        }

        /// <summary>
        /// Reads a sequence of bytes from the MediaBuffer.
        /// </summary>
        /// <param name="buffer">Array of bytes to store the read bytes in.</param>
        /// <param name="offset">Zero-based byte offset in the specified buffer at which to begin storing the data read from the buffer.</param>
        /// <param name="count">The maximum number of bytes to read from the buffer.</param>
        public int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, Length);

            (Buffer as MediaBuffer).Read(buffer, offset, count);

            return count;
        }

        public int Read(byte[] buffer, int offset, int count, int sourceOffset)
        {
            count = Math.Min(count, Length - sourceOffset);

            (Buffer as MediaBuffer).Read(buffer, offset, count, sourceOffset);

            return count;
        }

        /// <summary>
        /// Resets the Buffer. Sets the length of the MediaBuffer (see <see cref="MediaBuffer"/>) to zero and sets the <see cref="Status"/> to <see cref="OutputDataBufferFlags.None"/>.
        /// </summary>
        public void Reset()
        {
            Buffer.SetLength(0);
            Status = OutputDataBufferFlags.None;
        }

        /// <summary>
        /// Disposes the internally used MediaBuffer.
        /// </summary>
        public void Dispose()
        {
            if (Buffer != null)
            {
                (Buffer as MediaBuffer).Dispose();
                Buffer = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
