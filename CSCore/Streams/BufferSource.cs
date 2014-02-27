using CSCore;
using CSCore.Utils.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSCore.Streams
{
    [Obsolete("Still experimental.")]
    public class BufferSource : WaveAggregatorBase
    {
        private readonly int _bufferSize;
        private readonly Thread _bufferThread;
        private readonly Object _lockObject;

        private FixedSizeBuffer<byte> _buffer;
        private bool _disposing = false;

        /// <param name="source"></param>
        /// <param name="bufferSize">Buffersize in bytes.</param>
        public BufferSource(IWaveSource source, int bufferSize)
            : base(source)
        {
            if (bufferSize <= 0 || bufferSize % source.WaveFormat.BlockAlign != 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            _bufferSize = bufferSize;
            _buffer = new FixedSizeBuffer<byte>(bufferSize);
            _lockObject = new Object();

            _bufferThread = new Thread(new ParameterizedThreadStart(BufferProc))
            {
                Priority = ThreadPriority.Normal,
                IsBackground = false
            };
            _bufferThread.Start();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read0 = 0;
            int read1 = 0;
            Array.Clear(buffer, offset, count - offset);

            do
            {
                read1 = _buffer.Read(buffer, offset + read0, count - read0);
                read0 += read1;
            } while (read0 < count/* && (read1 != 0 && Position == Length)*/); //todo: abort if read1 = 0?

            return read0;
        }

        public void ResetBuffer()
        {
            lock (_lockObject)
            {
                _buffer.Clear();
            }
        }

        private void BufferProc(object o)
        {
            byte[] byteBuffer = new byte[WaveFormat.BytesPerSecond / 2];

            do
            {
                if (_buffer.Buffered >= _buffer.Length * 0.85 && !_disposing)
                {
                    Thread.Sleep(50);
                    continue;
                }
                lock (_lockObject)
                {
                    int bytesToRead = Math.Min(byteBuffer.Length, _buffer.Length - _buffer.Buffered);
                    int read = base.Read(byteBuffer, 0, bytesToRead);
                    int written = _buffer.Write(byteBuffer, 0, read);
                }
            } while (!_disposing);
        }

        public override long Position
        {
            get
            {
                lock (_lockObject)
                {
                    return Math.Max(0, Math.Min(BaseStream.Position - _buffer.Buffered, Length));
                }
            }
            set
            {
                lock (_lockObject) //todo: need lock? may cause laggy position trackbars etc.
                {
                    base.Position = value;
                    ResetBuffer();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _disposing = true;
            _bufferThread.WaitForExit(400);
            base.Dispose(disposing);
        }
    }
}
