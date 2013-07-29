using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/dd376684(v=vs.85).aspx
	/// </summary>
	public class MediaBuffer : IMediaBuffer, IDisposable
	{
		const string n = "IMediaBuffer";

		IntPtr _buffer;
		int _maxlength;
		int _length;

		public MediaBuffer(int maxlength)
		{
			if (maxlength < 1)
				throw new ArgumentOutOfRangeException("maxlength");
			_maxlength = maxlength;
			_buffer = Marshal.AllocCoTaskMem(maxlength);
			if (_buffer == IntPtr.Zero)
				throw new OutOfMemoryException("Could not allocate memory");
		}

		public int MaxLength { get { return _maxlength; } }

		public int Length
		{
			get { return _length; }
			set
			{
				if (_length > MaxLength || value <= 0)
					throw new ArgumentOutOfRangeException("value", "length can not be less than one or greater than maxlength");
				_length = value;
			}
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			Length = count;
			Marshal.Copy(buffer, offset, _buffer, count);
		}

		public void Read(byte[] buffer, int offset)
		{
			Read(buffer, offset, Length);
		}

		public void Read(byte[] buffer, int offset, int count)
		{
			if (count > Length)
			{
				throw new ArgumentOutOfRangeException("count", "count is greater than MaxLength");
			}
			Marshal.Copy(_buffer, buffer, offset, count);
		}

		int IMediaBuffer.SetLength(int length)
		{
			if (length > MaxLength)
			{
				return (int)HResult.E_INVALIDARG;
			}
			_length = length;
			return (int)HResult.S_OK;
		}

		int IMediaBuffer.GetMaxLength(out int length)
		{
			length = _maxlength;
			return (int)HResult.S_OK;
		}

		int IMediaBuffer.GetBufferAndLength(IntPtr ppBuffer, IntPtr validDataByteLength)
		{
			//if (ppBuffer == IntPtr.Zero && validDataByteLength == IntPtr.Zero)
			//    return (int)Utils.HResult.E_POINTER;
			if (ppBuffer != IntPtr.Zero)
			{
				Marshal.WriteIntPtr(ppBuffer, _buffer);
			}
			if (validDataByteLength != IntPtr.Zero)
			{
				Marshal.WriteInt32(validDataByteLength, _length);
			}
			return (int)HResult.S_OK;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_buffer != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(_buffer);
				_buffer = IntPtr.Zero;
			}
		}

		~MediaBuffer()
		{
			Dispose(false);
		}
	}
}
