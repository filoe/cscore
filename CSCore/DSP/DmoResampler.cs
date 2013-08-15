using CSCore.DMO;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DSP
{
    /// <summary>
    /// Resampler based on the DmoResampler. Supportet since Windows XP
    /// </summary>
    public class DmoResampler : WaveAggregatorBase
    {
        protected WaveFormat _outputformat;
        protected WMResampler _resampler;
        protected MediaBuffer _inputBuffer;
        protected DmoOutputDataBuffer _outputBuffer;
        protected MediaObject nativeObject;

        protected double _ratio;
        private int _quality = 30;

        protected object _lockObj;

        /// <summary>
        /// Resampler based on wavesource and new samplerate
        /// </summary>
        /// <param name="source">Source which has to get resampled</param>
        /// <param name="destSampleRate">Samplerate, the stream will be resampled to</param>
        public DmoResampler(IWaveSource source, int destSampleRate)
            : this(source, new WaveFormat(source.WaveFormat, destSampleRate))
        {
        }

        public DmoResampler(IWaveSource source, WaveFormat outputFormat)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (outputFormat == null)
                throw new ArgumentNullException("outputFormat");

            _lockObj = new object();
            Init(source.WaveFormat, outputFormat);
            _outputformat = outputFormat;
        }

        protected void Init(WaveFormat inputformat, WaveFormat outputformat)
        {
            //_ratio = (double)inputformat.BytesPerSecond / (double)outputformat.BytesPerSecond;
            _ratio = (double)outputformat.BytesPerSecond / (double)inputformat.BytesPerSecond;
            InitCom(inputformat, outputformat);
        }

        protected void InitCom(WaveFormat inputformat, WaveFormat outputformat)
        {
            lock (_lockObj)
            {
                var source = BaseStream;
                _resampler = new WMResampler();
                nativeObject = new MediaObject(Marshal.GetComInterfaceForObject(_resampler.MediaObject.NativeObject, typeof(IMediaObject)));

                if (!nativeObject.SupportsInputFormat(0, inputformat))
                {
                    throw new ArgumentException("Not supported source-format");
                }
                nativeObject.SetInputType(0, inputformat);

                if (!nativeObject.SupportsOutputFormat(0, outputformat))
                {
                    throw new ArgumentOutOfRangeException("destSampleRate");
                }
                nativeObject.SetOutputType(0, outputformat);

                _inputBuffer = new MediaBuffer(inputformat.BytesPerSecond / 2);
                _outputBuffer = new DmoOutputDataBuffer(outputformat.BytesPerSecond / 2);
            }
        }

        /// <summary>
        /// Read a block of bytes
        /// </summary>
        /// <returns>Read bytes</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_lockObj)
            {
                int read = 0;
                while (read < count)
                {
                    if (nativeObject.IsReadyForInput(0))
                    {
                        int bytesToRead = (int)OutputToInput(count - read);
                        byte[] inputData = new byte[bytesToRead];
                        int bytesRead = base.Read(inputData, 0, inputData.Length);
                        if (bytesRead <= 0)
                            break;

                        if (_inputBuffer.MaxLength < bytesRead)
                        {
                            _inputBuffer.Dispose();
                            _inputBuffer = new MediaBuffer(bytesRead);
                        }
                        _inputBuffer.Write(inputData, 0, bytesRead);

                        nativeObject.ProcessInput(0, _inputBuffer);

                        _outputBuffer.Reset();
                        do
                        {
                            var outputBuffer = _outputBuffer.Buffer as MediaBuffer;
                            if (outputBuffer.MaxLength < count)
                            {
                                outputBuffer.Dispose();
                                _outputBuffer.Buffer = outputBuffer = new MediaBuffer(count);
                            }
                            _outputBuffer.Buffer.SetLength(0);

                            nativeObject.ProcessOutput(ProcessOutputFlags.None, new DmoOutputDataBuffer[] { _outputBuffer }, 1);

                            if (_outputBuffer.Length <= 0)
                            {
                                Context.Current.Logger.Debug("No data in output buffer", "DmoResampler::Read(byte[], int, int)");
                                break;
                            }

                            _outputBuffer.Read(buffer, offset + read);
                            read += _outputBuffer.Length;
                        }
                        while (/*_outputBuffer.DataAvailable*/false);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(false, "Case of not ready for input is not implemented yet");
                    }
                }

                return read;
            }
        }

        public override WaveFormat WaveFormat
        {
            get { return _outputformat; }
        }

        public override long Position
        {
            get
            {
                return InputToOutput(base.Position);
            }
            set
            {
                base.Position = OutputToInput(value);
            }
        }

        public override long Length
        {
            get
            {
                return OutputToInput(base.Length);
            }
        }

        /// <summary>
        /// Specifies the quality of the output. The valid range is 1 to 60, inclusive.
        /// </summary>
        public int Quality
        {
            get
            {
                return _quality;
            }
            set
            {
                if (value < 1 || value > 60)
                    throw new ArgumentOutOfRangeException("value");
                _quality = value;
                _resampler.ResamplerProps.SetHalfFilterLength(value);
            }
        }

        internal long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position * _ratio);
            result -= (result % _outputformat.BlockAlign);
            return result;
        }

        internal long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position / _ratio);
            result -= (result % BaseStream.WaveFormat.BlockAlign);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _resampler.Dispose();
            _outputBuffer.Dispose();
            _inputBuffer.Dispose();
            nativeObject.Dispose();
        }
    }
}