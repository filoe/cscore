#define static_buffer_array

//#define static_buffer_queue
using System;
using System.Threading;

namespace CSCore.Utils.Buffer
{
    public class FixedSizeBuffer<T> : IDisposable //optional auch von System.IO.Stream ableiten
    {
#if static_buffer_queue
        int _bufferSize;
        Queue<T> _queue;
        public FixedSizeBuffer(int bufferSize)
        {
            _queue = new Queue<T>();
            _bufferSize = bufferSize;
        }

        public int Write(T[] buffer, int offset, int count)
        {
            for (int i = offset; i < count; i++)
            {
                _queue.Enqueue(buffer[i]);
            }

            return count;
        }

        public int Read(T[] buffer, int offset, int count)
        {
            int read = 0;
            for (int i = offset; i < Math.Min(count, _queue.Count); i++)
            {
                buffer[i] = _queue.Dequeue();
                read++;
            }

            return read;
        }

        public int Buffered { get { return _queue.Count; } }
        public int Length { get { return _bufferSize; } }


#error implement clear
        public void Dispose()
        {
            _queue = null;
        }
#endif
#if static_buffer_array
        private T[] _buffer; //buffer welcher immer wieder überschrieben wird
        private int _bufferedBytes = 0; //anzahl der vorhandenen Bytes
        private int _writeOffset = 0; //Schreibeoffset im Buffer
        private int _readOffset = 0; //Leseoffset im Buffer
        private object _lockObj = new object();

        public FixedSizeBuffer(int bufferSize)
        {
            _buffer = new T[bufferSize];
        }

        public int Write(T[] buffer, int offset, int count)
        {
            int written = 0;

            lock (_lockObj)
            {
                if (count > _buffer.Length - _bufferedBytes)
                    count = _buffer.Length - _bufferedBytes;

                int length = Math.Min(count, _buffer.Length - _writeOffset);
                Array.Copy(buffer, offset, _buffer, _writeOffset, length); //copy to buffer
                _writeOffset += length;
                written += length;
                _writeOffset = _writeOffset % _buffer.Length;

                if (written < count)
                {
                    Array.Copy(buffer, offset + written, _buffer, _writeOffset, count - written);
                    _writeOffset += (count - written);
                    written += (count - written);
                }

                _bufferedBytes += written;
            }

            return written;
        }

        public int Read(T[] buffer, int offset, int count)
        {
            int read = 0;

            lock (_lockObj)
            {
                count = Math.Min(count, _bufferedBytes);
                int length = Math.Min(count, _buffer.Length - _readOffset);
                Array.Copy(_buffer, _readOffset, buffer, offset, length); //copy to buffer
                read += length;
                _readOffset += read;
                _readOffset = _readOffset % _buffer.Length;

                if (read < count)
                {
                    Array.Copy(_buffer, _readOffset, buffer, offset + read, count - read);
                    _readOffset += (count - read);
                    read += (count - read);
                }

                _bufferedBytes -= read;
            }

            return read;
        }

        public int Length { get { return _buffer.Length; } }

        public int Buffered { get { return _bufferedBytes; } }

        public void Clear()
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            //reset all offsets
            _bufferedBytes = 0;
            _writeOffset = 0;
            _readOffset = 0;
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _buffer = null;
            }
            _disposed = true;
        }

        ~FixedSizeBuffer()
        {
            Dispose(false);
        }

#endif
    }
}