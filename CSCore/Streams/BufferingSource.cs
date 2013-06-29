using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams
{
	public class BufferingSource : IWaveSource
	{
		WaveFormat _waveFormat;
		FixedSizeBuffer<byte> _buffer;
		volatile object _bufferlock = new object();

		public BufferingSource(WaveFormat waveFormat)
		{
			_waveFormat = waveFormat;
			_buffer = new FixedSizeBuffer<byte>(waveFormat.BytesPerSecond * 5);
		}

		public int Write(byte[] buffer, int offset, int count)
		{
			lock (_bufferlock)
			{
				return _buffer.Write(buffer, offset, count);
			}
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			lock (_bufferlock)
			{
				int read = _buffer.Read(buffer, offset, count);
				if (read < count)
					Array.Clear(buffer, offset + read, count - read);
				return count;
			}
		}

		public WaveFormat WaveFormat
		{
			get { return _waveFormat; }
		}

		public long Position
		{
			get
			{
				return -1;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public long Length
		{
			get { return -1; }
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
			if(disposing)
			{
				//dispose managed
				_buffer.Dispose();
				_buffer = null;
			}
		}

		~BufferingSource()
		{
			Dispose(false);
		}
	}
}
