using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSCore.ACM
{
    public class AcmHeader : IDisposable
    {
        private IntPtr _handle;
        private int _sourceBufferSize, _destinationBufferSize;
        private byte[] _sourceBuffer, _destinationBuffer;
        private GCHandle _sourceBufferPtr, _destinationBufferPtr;

        private WaveFormat _sourceFormat;

        private NativeAcmHeader _header;

        private AcmConvertFlags _flags = AcmConvertFlags.ACM_STREAMCONVERTF_START | AcmConvertFlags.ACM_STREAMCONVERTF_BLOCKALIGN;

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
            if (count % _sourceFormat.BlockAlign != 0 || count == 0)
            {
                Debug.WriteLine("No valid number of bytes to convert. Parameter: count");
                count -= (count % _sourceFormat.BlockAlign);
            }

            Array.Copy(sourceBuffer, _sourceBuffer, count);

            _header.inputBufferLength = count;
            _header.inputBufferLengthUsed = count;

            AcmException.Try(AcmInterop.acmStreamConvert(
                _handle, _header, _flags), "acmStreamConvert");
            _flags = AcmConvertFlags.ACM_STREAMCONVERTF_BLOCKALIGN;
        }

        public void BeginConvert()
        {
            _header.cbStruct = Marshal.SizeOf(_header);
            SetupHeader(_header);

            MmException.Try(AcmInterop.acmStreamPrepareHeader(
                _handle, _header), "acmStreamPrepareHeader");
        }

        public AcmResult EndConvert()
        {
            SetupHeader(_header);

            MmException.Try(AcmInterop.acmStreamUnprepareHeader(
                _handle, _header), "acmStreamUnprepareHeader");

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
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
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