using System;
using System.Runtime.InteropServices;

namespace CSCore.Compression.ACM
{
    public class AcmHeader : IDisposable
    {
        IntPtr _handle;
        int _sourceBufferSize, _destinationBufferSize;
        byte[] _sourceBuffer, _destinationBuffer;
        GCHandle _sourceBufferPtr, _destinationBufferPtr;

        WaveFormat _sourceFormat;

        NativeAcmHeader _header;

        AcmConvertFlags _flags = AcmConvertFlags.ACM_STREAMCONVERTF_START | AcmConvertFlags.ACM_STREAMCONVERTF_BLOCKALIGN;

        public AcmHeader(IntPtr acmStreamHandle, WaveFormat sourceFormat, int sourceBufferSize, int destinationBufferSize)
        {
            if (acmStreamHandle == IntPtr.Zero)
                throw new ArgumentNullException("acmStreamHandle");
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceForamt");
            if (sourceBufferSize <= 0)
                throw new ArgumentOutOfRangeException("sourceBufferSize");
            if (destinationBufferSize <= 0)
                throw new ArgumentOutOfRangeException("destinationBufferSize");

            _handle = acmStreamHandle;
            _sourceBufferSize = sourceBufferSize;
            _destinationBufferSize = destinationBufferSize;

            _sourceFormat = sourceFormat;

            _sourceBuffer = new byte[sourceBufferSize];
            _destinationBuffer = new byte[destinationBufferSize];
            _sourceBufferPtr = GCHandle.Alloc(_sourceBuffer, GCHandleType.Pinned);
            _destinationBufferPtr = GCHandle.Alloc(_destinationBuffer, GCHandleType.Pinned);

            _header = new NativeAcmHeader();
        }

        public void Convert(byte[] sourceBuffer, int count)
        {
            const string loggerLocation = "AcmHeader.Convert(byte[], int)";
            if (count % _sourceFormat.BlockAlign != 0)
            {
                Context.Current.Logger.Error(String.Format("No valid number of bytes to convert. Parameter: count"), loggerLocation);
                count -= (count % _sourceFormat.BlockAlign);
            }

            Array.Copy(sourceBuffer, _sourceBuffer, count);

            _header.inputBufferLength = count;
            _header.inputBufferLengthUsed = count;

            Context.Current.Logger.MMResult(AcmInterop.acmStreamConvert(
                _handle, _header, _flags), "acmStreamConvert", loggerLocation);
            _flags = AcmConvertFlags.ACM_STREAMCONVERTF_BLOCKALIGN;
        }

        public void BeginConvert()
        {
            _header.cbStruct = Marshal.SizeOf(_header);
            SetupHeader(_header);

            Context.Current.Logger.MMResult(AcmInterop.acmStreamPrepareHeader(
                _handle, _header), "acmStreamPrepareHeader", "AcmHeader.BeginConverter()");
        }

        public AcmResult EndConvert()
        {
            SetupHeader(_header);

            Context.Current.Logger.MMResult(AcmInterop.acmStreamUnprepareHeader(
                _handle, _header), "acmStreamUnprepareHeader", "AcmHeader.EndConvert()");

            return new AcmResult(_header.inputBufferLengthUsed, _header.outputBufferLengthUsed,
                _header.outputBufferLengthUsed == _destinationBuffer.Length, _destinationBuffer);
        }

        private void SetupHeader(NativeAcmHeader header)
        {
            header.outputBufferLength = _destinationBufferSize;
            header.inputBufferLength = _sourceBufferSize;
            header.outputBufferPointer = _destinationBufferPtr.AddrOfPinnedObject();
            header.inputBufferPointer = _sourceBufferPtr.AddrOfPinnedObject();
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
			}
            if (_sourceBufferPtr.IsAllocated)
                _sourceBufferPtr.Free();
            if (_destinationBufferPtr.IsAllocated)
                _destinationBufferPtr.Free();
        }

        ~AcmHeader()
        {
            Dispose(false);
        }
    }
}
