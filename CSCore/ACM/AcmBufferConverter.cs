using System;
using System.Runtime.InteropServices;

namespace CSCore.ACM
{
    public class AcmBufferConverter : IDisposable
    {
        public static int StreamSize(IntPtr acmStreamHandle, int value, AcmStreamSizeFlags flags)
        {
            if (value == 0) return 0;
            int tmp = 0;
            AcmException.Try(AcmInterop.acmStreamSize(
                acmStreamHandle,
                value,
                out tmp,
                flags), "acmStreamSize");
            return tmp;
        }

        public static WaveFormat SuggestFormat(WaveFormat sourceFormat)
        {
            WaveFormat result = new WaveFormat(sourceFormat.SampleRate, 16, sourceFormat.Channels); //todo: 16bits fix
            AcmException.Try(AcmInterop.acmFormatSuggest(IntPtr.Zero, sourceFormat, result,
                Marshal.SizeOf(result), AcmFormatSuggestFlags.FormatTag), "acmFormatSuggest");
            return result;
        }

        private IntPtr _handle = IntPtr.Zero;
        private AcmHeader _header;

        public AcmBufferConverter(WaveFormat sourceFormat, WaveFormat destinationFormat)
            : this(sourceFormat, destinationFormat, IntPtr.Zero)
        {
        }

        public AcmBufferConverter(WaveFormat sourceFormat, WaveFormat destinationFormat, IntPtr driver)
        {
            AcmException.Try(AcmInterop.acmStreamOpen(
                    out _handle,
                    driver,
                    sourceFormat,
                    destinationFormat,
                    null,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    AcmStreamOpenFlags.ACM_STREAMOPENF_NONREALTIME),
                "acmStreamOpen");

            int sourceBufferSize = Math.Max(UInt16.MaxValue + 1 /*65536*/, sourceFormat.BytesPerSecond);
            sourceBufferSize -= (sourceBufferSize % sourceFormat.BlockAlign);

            int destinationBufferSize = StreamSize(_handle, sourceBufferSize, AcmStreamSizeFlags.Input);
            _header = new AcmHeader(_handle, sourceFormat, sourceBufferSize, destinationBufferSize);
        }

        public AcmResult Convert(byte[] sourceBuffer, int count)
        {
            _header.BeginConvert();
            _header.Convert(sourceBuffer, count);
            return _header.EndConvert();
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
                if (_handle != IntPtr.Zero)
                {
                    AcmInterop.acmStreamClose(_handle);
                    _handle = IntPtr.Zero;
                }

                if (_header != null)
                {
                    _header.Dispose();
                    _header = null;
                }
            }
            _disposed = true;
        }

        ~AcmBufferConverter()
        {
            Dispose(false);
        }
    }
}