using CSCore.DMO;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSCore.DSP
{
    /// <summary>
    /// Resampler based on the DmoResampler. Supported since Windows XP.
    /// </summary>
    public class DmoResampler : WaveAggregatorBase
    {
        protected WaveFormat _outputformat;
        protected WMResampler _resampler;
        protected MediaBuffer _inputBuffer;
        protected DmoOutputDataBuffer _outputBuffer;

        protected double _ratio;
        private int _quality = 30;

        protected object _lockObj;

        /// <summary>
        /// Resampler based on wavesource and new samplerate.
        /// </summary>
        /// <param name="source">Source which has to get resampled.</param>
        /// <param name="destSampleRate">Samplerate, the stream will be resampled to.</param>
        public DmoResampler(IWaveSource source, int destSampleRate)
            : this(source, new WaveFormat(source.WaveFormat.SampleRate, source.WaveFormat.BitsPerSample, source.WaveFormat.Channels, source.WaveFormat.GetWaveFormatTag()))
        {
        }

        /// <summary>
        /// Resampler based on wavesource and new outputFormat.
        /// </summary>
        /// <param name="source">Source which has to get resampled.</param>
        /// <param name="outputFormat">Waveformat, which specifies the new format. Note, that by far not all formats are supported.</param>
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
            _ratio = (double)outputformat.BytesPerSecond / (double)inputformat.BytesPerSecond;
            InitCom(inputformat, outputformat);
        }

        protected void InitCom(WaveFormat inputformat, WaveFormat outputformat)
        {
            lock (_lockObj)
            {
                var source = BaseStream;
                _resampler = new WMResampler();

                MediaObject mediaObject = _resampler.MediaObject;
                if (!mediaObject.SupportsInputFormat(0, inputformat))
                {
                    throw new NotSupportedException("Inputformat not supported.");
                }
                mediaObject.SetInputType(0, inputformat);

                if (!mediaObject.SupportsOutputFormat(0, outputformat))
                {
                    throw new NotSupportedException("Outputformat not supported.");
                }
                mediaObject.SetOutputType(0, outputformat);

                _inputBuffer = new MediaBuffer(inputformat.BytesPerSecond / 2);
                _outputBuffer = new DmoOutputDataBuffer(outputformat.BytesPerSecond / 2);
            }
        }

        /// <summary>
        /// Read from the resampled source.
        /// </summary>
        /// <returns>Amount of read bytes.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_lockObj)
            {
                int read = 0;
                while (read < count)
                {
                    MediaObject mediaObject = _resampler.MediaObject;
                    if (mediaObject.IsReadyForInput(0))
                    {
                        int bytesToRead = (int)OutputToInput(count - read);
                        byte[] inputData = new byte[bytesToRead];
                        int bytesRead = base.Read(inputData, 0, inputData.Length);
                        if (bytesRead <= 0)
                            break;

                        if (_disposed)
                            break;

                        if (_inputBuffer.MaxLength < bytesRead)
                        {
                            _inputBuffer.Dispose();
                            _inputBuffer = new MediaBuffer(bytesRead);
                        }
                        _inputBuffer.Write(inputData, 0, bytesRead);

                        mediaObject.ProcessInput(0, _inputBuffer);

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

                            mediaObject.ProcessOutput(ProcessOutputFlags.None, new DmoOutputDataBuffer[] { _outputBuffer }, 1);

                            if (_outputBuffer.Length <= 0)
                            {
                                Debug.WriteLine("DmoResampler::Read: No data in output buffer.");
                                break;
                            }

                            _outputBuffer.Read(buffer, offset + read);
                            read += _outputBuffer.Length;
                        }
                        while (/*_outputBuffer.DataAvailable*/false); //todo: Implement DataAvailable
                    }
                    else
                    {
                        Debug.WriteLine("Case of not ready for input is not implemented yet."); //todo: .
                    }
                }

                return read;
            }
        }

        /// <summary>
        /// Gets the new output format.
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return _outputformat; }
        }

        /// <summary>
        /// Gets or sets the position of the source. Note that the position gets calculated with the new output format. See <see cref="WaveFormat"/>.
        /// </summary>
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

        /// <summary>
        /// Gets the length of the source. Note that the length gets calculated with the new output format. See <see cref="WaveFormat"/>. 
        /// </summary>
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

        /// <summary>
        /// Disposes the allocated resources of the resampler but does not dispose the unerlying source.
        /// </summary>
        public void DisposeResamplerOnly()
        {
            DisposeBaseSource = false;
            Dispose();
        }

        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                DisposeBaseSource = false;
            base.Dispose(disposing);

            DisposeAndReset(ref _resampler);
            _outputBuffer.Dispose();
            DisposeAndReset(ref _inputBuffer);

            _disposed = true;
        }

        private void DisposeAndReset<T>(ref T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }
    }
}