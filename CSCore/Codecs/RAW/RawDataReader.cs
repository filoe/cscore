using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSCore.Codecs.RAW
{
    public class RawDataReader : IWaveSource
    {
        WaveFormat _waveFormat;
        Stream _stream;

        long _startPosition = 0;

        public RawDataReader(Stream stream, WaveFormat waveFormat)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (!stream.CanRead)
                throw new ArgumentException("stream is not readable", "stream");

            if (stream.CanSeek)
                _startPosition = stream.Position;

            if (waveFormat.WaveFormatTag != AudioEncoding.Pcm &&
               waveFormat.WaveFormatTag != AudioEncoding.IeeeFloat &&
               waveFormat.WaveFormatTag != AudioEncoding.Extensible)
            {
                throw new ArgumentException("Not supported encoding: {" + waveFormat.WaveFormatTag.ToString() + "}");
            }

            _stream = stream;
            _waveFormat = waveFormat;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            count -= (count & WaveFormat.BlockAlign);
            return _stream.Read(buffer, offset, count);
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public long Position
        {
            get
            {
                return _stream.Position - _startPosition;
            }
            set
            {
                _stream.Position = _startPosition + value;
            }
        }

        public long Length
        {
            get { return _stream.Length - _startPosition; }
        }

        private bool _disposed;
		public void Dispose()
        {
			if(!_disposed)
			{
				_disposed = true;
				
				Dispose(true);
				GC.SuppressFinalize(this);
			}
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        ~RawDataReader()
        {
            Dispose(false);
        }
    }
}
