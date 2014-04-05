using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.DMO
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd375507(v=vs.85).aspx
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
        /// Gets the length of the MediaBuffer. See <see cref="MediaBuffer"/>.
        /// </summary>
        public int Length
        {
            get { return (Buffer as MediaBuffer).Length; }
        }

        public void Read(byte[] buffer, int offset)
        {
            (Buffer as MediaBuffer).Read(buffer, offset);
        }

        public void Reset()
        {
            Buffer.SetLength(0);
            Status = OutputDataBufferFlags.None;
        }

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
