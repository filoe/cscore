using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSCore.DMO
{
    /// <summary>
    ///     Base class for all Dmo based streams.
    /// </summary>
    public abstract class DmoStream : IWaveSource
    {
        /// <summary>
        ///     The default inputStreamIndex to use.
        /// </summary>
        protected readonly int InputIndex = 0;

        /// <summary>
        ///     The default outputStreamIndex to use.
        /// </summary>
        protected readonly int OutputIndex = 0;

        private bool _disposed;

        private byte[] _inputBuffer;
        private MediaBuffer _inputDataBuffer;
        private WaveFormat _inputFormat;
        private bool _isInitialized;
        private MediaObject _mediaObject;
        private DmoOutputDataBuffer _outputDataBuffer;

        private int _outputDataBufferOffset;
        private int _outputDataBufferOverflows;
        private WaveFormat _outputFormat;
        private double _ratio;

        /// <summary>
        ///     Gets the input format of the <see cref="DmoStream" />.
        /// </summary>
        public WaveFormat InputFormat
        {
            get { return _inputFormat; }
        }

        /// <summary>
        ///     Reads a sequence of bytes from the stream.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the read bytes.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the stream</param>
        /// <returns>The actual number of read bytes.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual int Read(byte[] buffer, int offset, int count)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("DmoStream is not initialized.");

            int read = 0;
            while (read < count)
            {
                if (_disposed)
                    break;

                //check for overflows
                if (_outputDataBufferOverflows != 0)
                {
                    int overflowsRead = _outputDataBuffer.Read(buffer, offset + read, count - read,
                        _outputDataBufferOffset);
                    read += overflowsRead;

                    _outputDataBufferOverflows -= overflowsRead;
                    _outputDataBufferOffset += overflowsRead;

                    continue;
                }

                bool isIncomplete = (_outputDataBuffer.Status & OutputDataBufferFlags.Incomplete) ==
                                    OutputDataBufferFlags.Incomplete;
                bool isReadyForInput = _mediaObject.IsReadyForInput(InputIndex);

                //Process data if
                //  the MediaObject is ready for input
                //  there is no data to process left
                if (isReadyForInput && isIncomplete == false)
                {
                    var bytesToRead = (int) OutputToInput(Math.Max(0, count - read));
                    int bytesRead = GetInputData(ref _inputBuffer, bytesToRead);
                    if (bytesRead == 0)
                        break;

                    if (_disposed)
                        break;

                    if (_inputDataBuffer.MaxLength < bytesRead)
                    {
                        _inputDataBuffer.Dispose();
                        _inputDataBuffer = new MediaBuffer(bytesRead);
                    }
                    _inputDataBuffer.Write(_inputBuffer, 0, bytesRead);

                    _mediaObject.ProcessInput(0, _inputDataBuffer);
                }
                else if (isReadyForInput == false && isIncomplete == false) //no data available and not ready for input
                {
                    Debug.WriteLine("Unknown behavior: No data available and not ready for input.");
                    break; //todo: implement any better solution
                }
                else
                    Debugger.Break();

                //If there is no data left
                //   -> reset the outputDataBuffer
                if (isIncomplete == false)
                    _outputDataBuffer.Reset();

                //Process output data
                _mediaObject.ProcessOutput(ProcessOutputFlags.None, _outputDataBuffer);

                _outputDataBufferOffset = 0;
                _outputDataBufferOverflows = _outputDataBuffer.Length;

                if (_outputDataBuffer.Length <= 0)
                {
                    Debug.WriteLine("No data in output buffer.");
                    continue; //todo:
                }

                //if there is less data available than requested (count) -> outputDataRead will be the actual number of bytes read.
                int outputDataRead = _outputDataBuffer.Read(buffer, offset + read, count - read);
                read += outputDataRead;

                _outputDataBufferOverflows -= outputDataRead;
                _outputDataBufferOffset += outputDataRead;
            }

            return read;
        }

        /// <summary>
        ///     Gets or sets the position of the stream.
        /// </summary>
        public abstract long Position { get; set; }

        /// <summary>
        ///     Gets the length of the stream.
        /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public abstract bool CanSeek { get; }

        /// <summary>
        ///     Disposes the <see cref="DmoStream" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the output format of the <see cref="DmoStream" />.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _outputFormat; }
        }

        /// <summary>
        ///     Gets inputData to feed the Dmo MediaObject with.
        /// </summary>
        /// <param name="inputDataBuffer">
        ///     InputDataBuffer which receives the inputData.
        ///     If this parameter is null or the length is less than the amount of inputData, a new byte array will be applied.
        /// </param>
        /// <param name="requested">The requested number of bytes.</param>
        /// <returns>The number of bytes read. The number of actually read bytes does not have to be the number of requested bytes.</returns>
        protected abstract int GetInputData(ref byte[] inputDataBuffer, int requested);

        /// <summary>
        ///     Creates and returns a new <see cref="MediaObject"/> instance to use for processing audio data. This can be a decoder, effect, ...
        /// </summary>
        /// <param name="inputFormat">The input format of the <see cref="MediaObject"/> to create.</param>
        /// <param name="outputFormat">The output format of the <see cref="MediaObject"/> to create.</param>
        /// <returns>The created <see cref="MediaObject"/> to use for processing audio data.</returns>
        protected abstract MediaObject CreateMediaObject(WaveFormat inputFormat, WaveFormat outputFormat);

        /// <summary>
        ///     Gets the input format to use.
        /// </summary>
        /// <returns>The input format.</returns>
        protected abstract WaveFormat GetInputFormat();

        /// <summary>
        ///     Gets the output format to use.
        /// </summary>
        /// <returns>The output format.</returns>
        protected abstract WaveFormat GetOutputFormat();

        /// <summary>
        ///     Initializes the DmoStream. Important: This has to be called before using the DmoStream.
        /// </summary>
        protected void Initialize()
        {
            _inputFormat = GetInputFormat();
            _outputFormat = GetOutputFormat();

            _ratio = _outputFormat.BytesPerSecond / (double) _inputFormat.BytesPerSecond;

            _mediaObject = CreateMediaObject(_inputFormat, _outputFormat);

            //Setup Mediatype
            if (_mediaObject.SupportsInputFormat(InputIndex, _inputFormat) == false)
                throw new NotSupportedException("Inputformat is not supported.");
            _mediaObject.SetInputType(InputIndex, _inputFormat);

            if (_mediaObject.SupportsOutputFormat(OutputIndex, _outputFormat) == false)
                throw new NotSupportedException("Outputformat is not supported.");
            _mediaObject.SetOutputType(OutputIndex, _outputFormat);

            //Create Mediabuffers
            _inputDataBuffer = new MediaBuffer(_inputFormat.BytesPerSecond / 2);
            _outputDataBuffer = new DmoOutputDataBuffer(_outputFormat.BytesPerSecond / 2);

            _isInitialized = true;
        }

        /// <summary>
        ///     Converts a position of the inputstream to the equal position in the outputstream.
        /// </summary>
        /// <param name="position">Any position/offset of the inputstream, in bytes.</param>
        /// <returns>Position in the outputstream, in bytes.</returns>
        protected virtual long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            var result = (long) (position * _ratio);
            result -= (result % _outputFormat.BlockAlign);
            return result;
        }

        /// <summary>
        ///     Translates a position of the outputstream to the equal position in the inputstream.
        /// </summary>
        /// <param name="position">Any position/offset of the outputstream, in bytes.</param>
        /// <returns>Position in the inputstream, in bytes.</returns>
        protected virtual long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            var result = (long) (position / _ratio);
            result -= (result % _inputFormat.BlockAlign);
            return result;
        }

        /// <summary>
        ///     Resets the overflowbuffer.
        /// </summary>
        protected void ResetOverflowBuffer()
        {
            _outputDataBufferOverflows = 0;
            _outputDataBufferOffset = 0;
        }

        /// <summary>
        /// Releases the <see cref="DmoStream"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                DisposeAndResetObject(ref _mediaObject);
                DisposeAndResetObject(ref _inputDataBuffer);
                _outputDataBuffer.Dispose();

                _inputBuffer = null;
            }
            _disposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DmoStream"/> class.
        /// </summary>
        ~DmoStream()
        {
            Dispose(false);
        }

        private void DisposeAndResetObject<T>(ref T disposable) where T : class, IDisposable
        {
            if (disposable != null)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (ObjectDisposedException)
                {
                }

                disposable = null;
            }
        }
    }
}