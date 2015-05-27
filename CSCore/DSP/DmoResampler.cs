using System;
using System.Diagnostics;
using CSCore.DMO;

namespace CSCore.DSP
{
    /// <summary>
    ///     Resampler based on the DmoResampler. Supported since Windows XP.
    /// </summary>
    public class DmoResampler : WaveAggregatorBase
    {
        internal MediaBuffer InputBuffer;
        internal object LockObj = new object();
        internal DmoOutputDataBuffer OutputBuffer;
        internal WaveFormat Outputformat;

        internal double Ratio;
        internal WMResampler Resampler;
        private bool _disposed;
        private int _quality = 30;

        private static WaveFormat GetWaveFormatWithChangedSampleRate(IWaveSource source, int destSampleRate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var waveFormat = (WaveFormat)source.WaveFormat.Clone();
            waveFormat.SampleRate = destSampleRate;
            return waveFormat;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DmoResampler" /> class.
        /// </summary>
        /// <param name="source"><see cref="IWaveSource" /> which has to get resampled.</param>
        /// <param name="destinationSampleRate">The new output samplerate specified in Hz.</param>
        public DmoResampler(IWaveSource source, int destinationSampleRate)
            : this(
                source,
                GetWaveFormatWithChangedSampleRate(source, destinationSampleRate))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DmoResampler" /> class.
        /// </summary>
        /// <param name="source"><see cref="IWaveSource" /> which has to get resampled.</param>
        /// <param name="outputFormat">Waveformat, which specifies the new format. Note, that by far not all formats are supported.</param>
        public DmoResampler(IWaveSource source, WaveFormat outputFormat)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (outputFormat == null)
                throw new ArgumentNullException("outputFormat");

            Initialize(source.WaveFormat, outputFormat);
            Outputformat = outputFormat;
        }

        /// <summary>
        ///     Gets the new output format.
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return Outputformat; }
        }

        /// <summary>
        ///     Gets or sets the position of the source.
        /// </summary>
        public override long Position
        {
            get { return InputToOutput(base.Position); }
            set { base.Position = OutputToInput(value); }
        }

        /// <summary>
        ///     Gets the length of the source.
        /// </summary>
        public override long Length
        {
            get { return InputToOutput(base.Length); }
        }

        /// <summary>
        ///     Specifies the quality of the output. The valid range is from 1 to 60.
        /// </summary>
        /// <value>Specifies the quality of the resampled output. The valid range is: <code>1 &gt;= value &lt;= 60</code>.</value>
        public int Quality
        {
            get { return _quality; }
            set
            {
                if (value < 1 || value > 60)
                    throw new ArgumentOutOfRangeException("value");
                _quality = value;
                Resampler.ResamplerProps.SetHalfFilterLength(value);
            }
        }

        internal void Initialize(WaveFormat inputformat, WaveFormat outputformat)
        {
            Ratio = (double) outputformat.BytesPerSecond / inputformat.BytesPerSecond;
            lock (LockObj)
            {
                Resampler = new WMResampler();

                MediaObject mediaObject = Resampler.MediaObject;
                if (!mediaObject.SupportsInputFormat(0, inputformat))
                    throw new NotSupportedException("Inputformat not supported.");
                mediaObject.SetInputType(0, inputformat);

                if (!mediaObject.SupportsOutputFormat(0, outputformat))
                    throw new NotSupportedException("Outputformat not supported.");
                mediaObject.SetOutputType(0, outputformat);

                InputBuffer = new MediaBuffer(inputformat.BytesPerSecond / 2);
                OutputBuffer = new DmoOutputDataBuffer(outputformat.BytesPerSecond / 2);
            }
        }


        /// <summary>
        ///     Reads a resampled sequence of bytes from the <see cref="DmoResampler" /> and advances the position within the
        ///     stream by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the current source.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (LockObj)
            {
                int read = 0;
                while (read < count)
                {
                    MediaObject mediaObject = Resampler.MediaObject;
                    if (mediaObject.IsReadyForInput(0))
                    {
                        var bytesToRead = (int) OutputToInput(count - read);
                        var inputData = new byte[bytesToRead];
                        int bytesRead = base.Read(inputData, 0, inputData.Length);
                        if (bytesRead <= 0)
                            break;

                        if (_disposed)
                            break;

                        if (InputBuffer.MaxLength < bytesRead)
                        {
                            InputBuffer.Dispose();
                            InputBuffer = new MediaBuffer(bytesRead);
                        }
                        InputBuffer.Write(inputData, 0, bytesRead);

                        mediaObject.ProcessInput(0, InputBuffer);

                        OutputBuffer.Reset();
                        do
                        {
                            var outputBuffer = (MediaBuffer) OutputBuffer.Buffer;
                            if (outputBuffer.MaxLength < count)
                            {
                                outputBuffer.Dispose();
                                OutputBuffer.Buffer = new MediaBuffer(count);
                            }
                            OutputBuffer.Buffer.SetLength(0);

                            mediaObject.ProcessOutput(ProcessOutputFlags.None, new[] {OutputBuffer}, 1);

                            if (OutputBuffer.Length <= 0)
                            {
                                Debug.WriteLine("DmoResampler::Read: No data in output buffer.");
                                break;
                            }

                            OutputBuffer.Read(buffer, offset + read);
                            read += OutputBuffer.Length;
                        } while ( /*_outputBuffer.DataAvailable*/false); //todo: Implement DataAvailable
                    }
                    else
                        Debug.WriteLine("Case of not ready for input is not implemented yet."); //todo: .
                }

                return read;
            }
        }

        internal long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            var result = (long) (position * Ratio);
            result -= (result % Outputformat.BlockAlign);
            return result;
        }

        internal long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            var result = (long) (position / Ratio);
            result -= (result % BaseSource.WaveFormat.BlockAlign);
            return result;
        }

        /// <summary>
        ///     Disposes the allocated resources of the resampler but does not dispose the underlying source.
        /// </summary>
        public void DisposeResamplerOnly()
        {
            DisposeBaseSource = false;
            Dispose();
        }

        /// <summary>
        ///     Disposes the <see cref="DmoResampler" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                DisposeBaseSource = false;
            base.Dispose(disposing);

            DisposeAndReset(ref Resampler);
            OutputBuffer.Dispose();
            DisposeAndReset(ref InputBuffer);

            _disposed = true;
        }

        private void DisposeAndReset<T>(ref T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                try
                {
                    obj.Dispose();
                }
                catch (ObjectDisposedException)
                {
                }
                obj = null;
            }
        }
    }
}