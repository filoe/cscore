using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSCore.DMO
{
    /// <summary>
    /// Base class for all Dmo based streams.
    /// </summary>
    public abstract class DmoStream : IWaveSource
    {
        /// <summary>
        /// The default inputStreamIndex to use.
        /// </summary>
        protected readonly int _inputIndex = 0;
        /// <summary>
        /// The default outputStreamIndex to use.
        /// </summary>
        protected readonly int _outputIndex = 0;

        private MediaObject mediaObject;

        private byte[] inputBuffer;
        private MediaBuffer inputDataBuffer;
        private DmoOutputDataBuffer outputDataBuffer;

        private WaveFormat _inputFormat;
        private WaveFormat _outputFormat;
        private double _ratio;

        private int outputDataBufferOffset = 0;
        private int outputDataBufferOverflows = 0;

        private bool _disposed;
        private bool _isInitialized = false;

        /// <summary>
        /// Gets inputData to feed the Dmo MediaObject with.
        /// </summary>
        /// <param name="inputDataBuffer">InputDataBuffer which receives the inputData. 
        /// If this parameter is null or the length is less than the amount of inputData, a new byte array will be applied. 
        /// </param>
        /// <param name="requested">The requested number of bytes.</param>
        /// <returns>The number of bytes read. The number of actually read bytes does not have to be the number of requested bytes.</returns>
        protected abstract int GetInputData(ref byte[] inputDataBuffer, int requested);

        /// <summary>
        /// Creates a MediaObjec to use. This can be a decoder, effect, ...
        /// </summary>
        protected abstract MediaObject CreateMediaObject(WaveFormat inputFormat, WaveFormat outputFormat);

        /// <summary>
        /// Gets the input format to use.
        /// </summary>
        /// <returns></returns>
        protected abstract WaveFormat GetInputFormat();

        /// <summary>
        /// Gets the output format to use.
        /// </summary>
        /// <returns></returns>
        protected abstract WaveFormat GetOutputFormat();

        /// <summary>
        /// Initializes the DmoStream. Important: This has to be called before using the DmoStream.
        /// </summary>
        protected void Initialize()
        {
            _inputFormat = GetInputFormat();
            _outputFormat = GetOutputFormat();

            _ratio = (double)_outputFormat.BytesPerSecond / (double)_inputFormat.BytesPerSecond;

            mediaObject = CreateMediaObject(_inputFormat, _outputFormat);

            //Setup Mediatype
            if (mediaObject.SupportsInputFormat(_inputIndex, _inputFormat) == false)
                throw new NotSupportedException("Inputformat is not supported.");
            mediaObject.SetInputType(_inputIndex, _inputFormat);

            if (mediaObject.SupportsOutputFormat(_outputIndex, _outputFormat) == false)
                throw new NotSupportedException("Outputformat is not supported.");
            mediaObject.SetOutputType(_outputIndex, _outputFormat);

            //Create Mediabuffers
            inputDataBuffer = new MediaBuffer(_inputFormat.BytesPerSecond / 2);
            outputDataBuffer = new DmoOutputDataBuffer(_outputFormat.BytesPerSecond / 2);

            _isInitialized = true;
        }

        /// <summary>
        /// Reads a sequence of bytes from the stream.
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
            while(read < count)
            {
                if(_disposed)
                    break;

                //check for overflows
                if(outputDataBufferOverflows != 0)
                {
                    int overflowsRead = outputDataBuffer.Read(buffer, offset + read, count - read, outputDataBufferOffset);
                    read += overflowsRead;

                    outputDataBufferOverflows -= overflowsRead;
                    outputDataBufferOffset += overflowsRead;

                    continue;
                }

                bool isIncomplete = (outputDataBuffer.Status & OutputDataBufferFlags.Incomplete) == OutputDataBufferFlags.Incomplete;
                bool isReadyForInput = mediaObject.IsReadyForInput(_inputIndex);

                //Process data if
                //  the MediaObject is ready for input
                //  there is no data to process left
                if (isReadyForInput && isIncomplete == false)
                {
                    int bytesToRead = (int)OutputToInput(Math.Max(0, count - read));
                    int bytesRead = GetInputData(ref inputBuffer, bytesToRead);
                    if (bytesRead == 0)
                        break;

                    if (_disposed)
                        break;

                    if (inputDataBuffer.MaxLength < bytesRead)
                    {
                        inputDataBuffer.Dispose();
                        inputDataBuffer = new MediaBuffer(bytesRead);
                    }
                    inputDataBuffer.Write(inputBuffer, 0, bytesRead);

                    mediaObject.ProcessInput(0, inputDataBuffer);
                }
                else if(isReadyForInput == false && isIncomplete == false) //no data available and not ready for input
                {
                    Debug.WriteLine("Unknown behavior: No data available and not ready for input.");
                    break; //todo: implement any better solution
                }
                else
                {
                    Debugger.Break();
                }

                //If there is no data left
                //   -> reset the outputDataBuffer
                if(isIncomplete == false)
                    outputDataBuffer.Reset();

                //Process output data
                mediaObject.ProcessOutput(ProcessOutputFlags.None, outputDataBuffer);

                outputDataBufferOffset = 0;
                outputDataBufferOverflows = outputDataBuffer.Length;

                if(outputDataBuffer.Length <= 0)
                {
                    Debug.WriteLine("No data in output buffer.");
                    continue; //todo:
                }

                //if there is less data available than requested (count) -> outputDataRead will be the actual number of bytes read.
                int outputDataRead = outputDataBuffer.Read(buffer, offset + read, count - read);
                read += outputDataRead;

                outputDataBufferOverflows -= outputDataRead;
                outputDataBufferOffset += outputDataRead;
            }

            return read;
        }

        /// <summary>
        /// Gets or sets the position of the stream.
        /// </summary>
        public abstract long Position { get; set; }

        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// Translates a position of the inputstream to the position in the outputstream.
        /// </summary>
        /// <param name="position">Any position/offset of the inputstream.</param>
        /// <returns>Position in the outputstream.</returns>
        protected virtual long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position * _ratio);
            result -= (result % _outputFormat.BlockAlign);
            return result;
        }

        /// <summary>
        /// Translates a position of the outputstream to the position in the inputstream.
        /// </summary>
        /// <param name="position">Any position/offset of the outputstream.</param>
        /// <returns>Position in the inputstream.</returns>
        protected virtual long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position / _ratio);
            result -= (result % _inputFormat.BlockAlign);
            return result;
        }

        /// <summary>
        /// Resets the overflowbuffer.
        /// </summary>
        protected void ResetOverflowBuffer()
        {
            outputDataBufferOverflows = 0;
            outputDataBufferOffset = 0;
        }

        /// <summary>
        /// Disposes the <see cref="DmoStream"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the <see cref="DmoStream"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //dispose managed
                }

                DisposeAndResetObject(ref mediaObject);
                DisposeAndResetObject(ref inputDataBuffer);
                outputDataBuffer.Dispose();

                inputBuffer = null;
            }
            _disposed = true;
        }

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

        /// <summary>
        /// Gets the output format of the <see cref="DmoStream"/>.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _outputFormat; }
        }

        /// <summary>
        /// Gets the input format of the <see cref="DmoStream"/>.
        /// </summary>
        public WaveFormat InputFormat
        {
            get { return _inputFormat; }
        }
    }
}
