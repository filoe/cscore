using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSCore.DMO
{
    public abstract class DmoStream : IWaveSource
    {
        protected readonly int _inputIndex = 0;
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

        protected abstract int GetInputData(ref byte[] inputDataBuffer, int requested);
        protected abstract MediaObject CreateMediaObject(WaveFormat inputFormat, WaveFormat outputFormat);
        protected abstract WaveFormat GetInputFormat();
        protected abstract WaveFormat GetOutputFormat();

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

        public abstract long Position { get; set; }

        public abstract long Length { get; }

        protected virtual long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position * _ratio);
            result -= (result % _outputFormat.BlockAlign);
            return result;
        }

        protected virtual long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position / _ratio);
            result -= (result % _inputFormat.BlockAlign);
            return result;
        }

        protected void ResetOverflowBuffer()
        {
            outputDataBufferOverflows = 0;
            outputDataBufferOffset = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        public WaveFormat WaveFormat
        {
            get { return _outputFormat; }
        }

        public WaveFormat InputFormat
        {
            get { return _inputFormat; }
        }
    }
}
