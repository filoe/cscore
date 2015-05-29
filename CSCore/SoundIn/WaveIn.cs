using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CSCore.SoundOut.MMInterop;
using System.Collections.Generic;

namespace CSCore.SoundIn
{
    /// <summary>
    /// Captures audio from a audio device (through WaveIn Apis).
    /// </summary>
    public class WaveIn : ISoundIn
    {
        private const int BufferCount = 2;
        private readonly WaveCallback _callback;
        private readonly object _lockObj = new object();
        private int _activeBuffers;
        private WaveInBuffer[] _buffers;
        private Thread _callbackThread;
        private WaveInDevice _device;

        private bool _disposed;
        private bool _isInitialized;
        private int _latency = 150;
        private IntPtr _waveInHandle;
        private readonly Queue<int> _failedBuffers = new Queue<int>();

        /// <summary>
        ///     Gets or sets the <see cref="Device" /> which should be used for capturing audio.
        ///     The <see cref="Device" /> property has to be set before initializing. The systems default recording device is used
        ///     as default value
        ///     of the <see cref="Device" /> property.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value must not be null.</exception>
        public WaveInDevice Device
        {
            get { return _device; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _device = value;
            }
        }

        /// <summary>
        ///     Gets or sets the latency of the wavein specified in milliseconds.
        ///     The <see cref="Latency" /> property has to be set before initializing.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Must be greater than zero.");
                _latency = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveIn"/> class using the a default format (44.1kHz, 16 bit, 2 channels, pcm).
        /// </summary>
        public WaveIn()
            : this(new WaveFormat(44100, 16, 2, AudioEncoding.Pcm))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveIn"/> class.
        /// </summary>
        /// <param name="waveFormat">The default format to use. The final format must not equal the specified <paramref name="waveFormat"/>.</param>
        /// <exception cref="System.ArgumentNullException">waveFormat</exception>
        public WaveIn(WaveFormat waveFormat)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            WaveFormat = waveFormat;
            _callback = Callback;
            RecordingState = RecordingState.Stopped;
            Device = WaveInDevice.DefaultDevice;
        }

        /// <summary>
        /// Occurs when new data got captured and is available.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// Occurs when the recording stopped.
        /// </summary>
        public event EventHandler<RecordingStoppedEventArgs> Stopped;

        /// <summary>
        /// Gets the format of the captured audio data.
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Initializes the <see cref="WaveIn" /> instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"><see cref="RecordingState"/> has to be <see cref="SoundIn.RecordingState.Stopped"/>. Call <see cref="Stop"/> to stop.</exception>
        public void Initialize()
        {
            CheckForInvalidThreadCall();
            lock (_lockObj)
            {
                CheckForDisposed();

                if (RecordingState != RecordingState.Stopped)
                    throw new InvalidOperationException("RecordingState has to be Stopped. Call WaveIn::Stop to stop.");

                WaitForBuffersToFinish();

                CleanupResources();
                InitializeInternal();

                _isInitialized = true;
            }
        }

        /// <summary>
        /// Starts recording.
        /// </summary>
        public void Start()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForInitialized();

                if (RecordingState == RecordingState.Stopped)
                {
                    foreach (WaveInBuffer buffer in _buffers)
                    {
                        if (!buffer.IsInQueue)
                            FireUpBuffer(buffer);
                    }
                    MmException.Try(NativeMethods.waveInStart(_waveInHandle), "waveInStart");
                    RecordingState = RecordingState.Recording;
                }
            }
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        public void Stop()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                //don't check for initialized since disposing without init would cause an exception

                if (RecordingState != RecordingState.Stopped)
                {
                    MmException.Try(NativeMethods.waveInReset(_waveInHandle), "waveInStop");
                    WaitForBuffersToFinish();

                    RecordingState = RecordingState.Stopped;
                }
                else if (RecordingState == RecordingState.Stopped && _activeBuffers != 0)
                {
                    WaitForBuffersToFinish();
                }
            }
        }

        /// <summary>
        /// Gets the current <see cref="SoundIn.RecordingState" />.
        /// </summary>
        public RecordingState RecordingState { get; private set; }

        private void CheckForInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("WaveIn is not initialized.");
        }

        private void InitializeInternal()
        {
            var supportedFormats = new Queue<WaveFormat>(Device.SupportedFormats
                .OrderBy(x => Math.Abs(x.SampleRate - WaveFormat.SampleRate))
                .ThenBy(x => Math.Abs(x.BitsPerSample - WaveFormat.BitsPerSample))
                .ThenBy(x => Math.Abs(x.Channels - WaveFormat.Channels)));
            var finalFormat = WaveFormat;
            do
            {
                try
                {
                    _waveInHandle = CreateWaveInHandle(finalFormat);
                }
                catch (MmException exception)
                {
                    if (exception.Result == MmResult.BadFormat && supportedFormats.Count > 0)
                        finalFormat = supportedFormats.Dequeue();
                    else if (exception.Result == MmResult.BadFormat && supportedFormats.Count == 0)
                        throw new Exception("No valid format could be found.", exception);
                    else
                        throw;
                }
            } while (_waveInHandle == IntPtr.Zero);

            _failedBuffers.Clear();
            var bufferSize = (int) WaveFormat.MillisecondsToBytes(_latency);
            _buffers = new WaveInBuffer[BufferCount];
            for (int i = 0; i < _buffers.Length; i++)
            {
                _buffers[i] = new WaveInBuffer(_waveInHandle, bufferSize, (IntPtr) i);
            }
        }

        /// <summary>
        ///     Creates and returns the WaveOut handle.
        /// </summary>
        /// <param name="waveFormat">The waveformat to use.</param>
        /// <returns>A valid WaveOut handle.</returns>
        protected virtual IntPtr CreateWaveInHandle(WaveFormat waveFormat)
        {
            IntPtr handle;
            MmException.Try(
                NativeMethods.waveInOpen(out handle, (IntPtr)_device.DeviceId, waveFormat, _callback, IntPtr.Zero,
                    NativeMethods.WaveInOutOpenFlags.CALLBACK_FUNCTION), "waveInOpen");
            return handle;
        }

        private void Callback(IntPtr handle, WaveMsg msg, IntPtr user, WaveHeader header, IntPtr reserved)
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + "|" + msg);
            if (_waveInHandle != handle)
                return;

            if (msg == WaveMsg.WIM_DATA)
            {
                if (_callbackThread == null)
                    _callbackThread = Thread.CurrentThread;

                //Debug.Assert(_callbackThread == Thread.CurrentThread, "Strange thread?");
                if(_callbackThread != Thread.CurrentThread)
                    Debugger.Break();

                var index = (int) header.userData;
                WaveInBuffer buffer = _buffers[index];

                Interlocked.Decrement(ref _activeBuffers);

                if (Monitor.TryEnter(_lockObj, 10))
                {
                    try
                    {
                        callback0:
                        //only add buffer to queue again, if we are still recording
                        if (buffer != null && RecordingState != RecordingState.Stopped)
                        {
                            try
                            {
                                //todo: should we care about recordingstate when firing dataavailable?
                                RaiseDataAvailable(buffer);
                                FireUpBuffer(buffer);
                            }
                            catch (Exception exception)
                            {
                                StopFromCallback(exception);
                            }
                        }

                        if (_failedBuffers.Count > 0)
                        {
                            while (_failedBuffers.Count > 0
                                   && (buffer = _buffers[(index = _failedBuffers.Dequeue())]).IsInQueue == false)
                            {
                                Debug.WriteLine("Already queued.");
                            }
                            if (buffer != null && !buffer.IsInQueue)
                            {
                                Debug.WriteLine("Failed buffer: " + index);
                                goto callback0;
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(_lockObj);
                    }
                }
                else
                {
                    _failedBuffers.Enqueue(index);
                }

                if (_activeBuffers <= 0)
                    StopFromCallback(null);

                _callbackThread = null;
            }
            else if (msg == WaveMsg.WIM_CLOSE)
            {
                if (RecordingState != RecordingState.Stopped)
                    StopFromCallback(null);
            }
        }

        private void FireUpBuffer(WaveInBuffer buffer)
        {
            buffer.AddBufferToQueue();
            Interlocked.Increment(ref _activeBuffers);
        }

        private void StopFromCallback(Exception exception)
        {
            RecordingState = RecordingState.Stopped;
            RaiseStopped(exception);
        }

        private void RaiseStopped(Exception exception)
        {
            if (Stopped != null)
                Stopped(this, new RecordingStoppedEventArgs(exception));
        }

        private void RaiseDataAvailable(WaveInBuffer buffer)
        {
            if (DataAvailable != null && buffer.WaveHeader.bytesRecorded > 0)
            {
                DataAvailable(this,
                    new DataAvailableEventArgs(buffer.Buffer, 0, buffer.WaveHeader.bytesRecorded, WaveFormat));
            }
        }

        private void CleanupResources()
        {
            if (_waveInHandle == IntPtr.Zero)
                return;

            DisposeBuffers();
            MmException.Try(NativeMethods.waveInClose(_waveInHandle), "waveInClose");
            _waveInHandle = IntPtr.Zero;
            _callbackThread = null;

            _isInitialized = false;
        }

        private void DisposeBuffers()
        {
            for (int i = 0; i < _buffers.Length; i++)
            {
                if (_buffers[i] != null)
                {
                    _buffers[i].Dispose();
                    _buffers[i] = null;
                }
            }
        }

        private void WaitForBuffersToFinish()
        {
            //do we have volatile to _activeBuffers???
            while (Interlocked.CompareExchange(ref _activeBuffers, 0, 0) != 0 &&
                   RecordingState != RecordingState.Stopped)
            {
                Thread.Sleep(10);
            }
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        private void CheckForInvalidThreadCall()
        {
            if (_callbackThread == null)
                return;
            if (Thread.CurrentThread == _callbackThread)
                throw new InvalidOperationException("You must not access this method from the Capture-thread.");
        }

        /// <summary>
        /// Disposes and stops the <see cref="WaveIn"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes and stops the <see cref="WaveIn"/> instance.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                if (!_disposed)
                {
                    if (_waveInHandle == IntPtr.Zero)
                        return;

                    Debug.WriteLine("Disposing WaveIn");
                    Stop();
                    CleanupResources();

                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WaveIn"/> class.
        /// </summary>
        ~WaveIn()
        {
            Dispose(false);
        }
    }
}