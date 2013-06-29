using System;
using System.Collections.Generic;
using System.IO;

namespace CSCore.Codecs.WAV
{
	public class WaveFileReader : IWaveSource
	{
		Stream _stream;
		Int32 _fileLength;
		List<WaveFileChunk> _chunks;
		WaveFormat _waveFormat;

		object lockObj;

		long _dataInitPosition;

		public List<WaveFileChunk> Chunks
		{
			get { return _chunks; }
		}

		public WaveFileReader(string fileName)
			: this(File.OpenRead(fileName))
		{
		}

		public WaveFileReader(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			if (!stream.CanRead) throw new ArgumentException("stream is not readable");

			_stream = stream;

			BinaryReader reader = new BinaryReader(stream);
			if (new String(reader.ReadChars(4)) == "RIFF")
			{
				_fileLength = reader.ReadInt32(); //FileLength
				char[] rifftype = reader.ReadChars(4); //RiffType WAVE
			}

			_chunks = ReadChunks(stream);
			lockObj = new object();
		}

		private List<WaveFileChunk> ReadChunks(Stream stream)
		{
			List<WaveFileChunk> chunks = new List<WaveFileChunk>();
			WaveFileChunk tmp;
			do
			{
				tmp = WaveFileChunk.FromStream(stream);
				chunks.Add(tmp);

				if (tmp is FMTChunk)
					_waveFormat = (tmp as FMTChunk).WaveFormat;
				else if(!(tmp is DataChunk))
				{
					stream.Position += tmp.ChunkDataSize;
				}

			} while (!(tmp is DataChunk));
			_dataInitPosition = stream.Position;

			return chunks;
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			lock (lockObj)
			{
				count -= count % WaveFormat.BlockAlign;
				return _stream.Read(buffer, offset, count);
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
				return _stream.Position - _dataInitPosition;
			}
			set
			{
				lock (lockObj)
				{
					value = Math.Min(value, Length);
					value -= (value % WaveFormat.BlockAlign);
					_stream.Position = value + _dataInitPosition;
				}
			}
		}

		public long Length
		{
			get { return _stream.Length - _dataInitPosition; }
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			lock (lockObj)
			{
				if (disposing)
				{
					//dispose managed
				}
				if (_stream != null)
				{
					_stream.Dispose();
					_stream = null;
				}
				Context.Current.Logger.Info("WaveFile disposed");
			}
		}

		~WaveFileReader()
		{
			Dispose(false);
		}
	}
}
