using System;
using System.Runtime.InteropServices;

namespace CSCore.Compression.ACM
{
    public class AcmBufferConverter : IDisposable
    {
        public static int StreamSize(IntPtr acmStreamHandle, int value, AcmStreamSizeFlags flags)
        {
            if (value == 0) return 0;
            int tmp = 0;
            Context.Current.Logger.MMResult(AcmInterop.acmStreamSize(
                acmStreamHandle,
                value,
                out tmp,
                flags), "acmStreamSize", "AcmBufferConverter.StreamSize(IntPtr, int, AcmStreamSizeFlags)");
            return tmp;
        }

        public static WaveFormat SuggestFormat(WaveFormat sourceFormat)
        {
            WaveFormat result = new WaveFormat(sourceFormat.SampleRate, 16, sourceFormat.Channels); //todo: 16bits fix
            Context.Current.Logger.MMResult(AcmInterop.acmFormatSuggest(IntPtr.Zero, sourceFormat, result,
                Marshal.SizeOf(result), AcmFormatSuggestFlags.FormatTag), "acmFormatSuggest", "AcmBufferConverter.SuggestFormat(WaveFormat)");
            return result;
        }

        IntPtr _handle = IntPtr.Zero;
        AcmHeader _header;

        public AcmBufferConverter(WaveFormat sourceFormat, WaveFormat destinationFormat)
            : this(sourceFormat, destinationFormat, IntPtr.Zero)
        {
        }

        public AcmBufferConverter(WaveFormat sourceFormat, WaveFormat destinationFormat, IntPtr driver)
        {
            Context.Current.Logger.MMResult(AcmInterop.acmStreamOpen(
                    out _handle,
                    driver,
                    sourceFormat,
                    destinationFormat,
                    null,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    AcmStreamOpenFlags.ACM_STREAMOPENF_NONREALTIME),
                "acmStreamOpen", "AcmBufferConverter.ctor(WaveFormat, WaveFormat, IntPtr", 
                Utils.Logger.LogDispatcher.MMLogFlag.ThrowOnError | Utils.Logger.LogDispatcher.MMLogFlag.LogAlways);

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

        public void Dispose()
        {
            const string loggerLocation = "AcmBufferConverter.Dispose()";
            if (_handle != IntPtr.Zero)
            {
                Context.Current.Logger.MMResult(AcmInterop.acmStreamClose(_handle), "acmStreamClose", loggerLocation, Utils.Logger.LogDispatcher.MMLogFlag.ThrowNever);
                _handle = IntPtr.Zero;
            }

            if (_header != null)
            {
                _header.Dispose();
                _header = null;
            }
        }
    }
}
