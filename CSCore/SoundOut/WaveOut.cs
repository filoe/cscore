using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CSCore.DSP;
using CSCore.SoundOut.MMInterop;
using CSCore.Streams;

namespace CSCore.SoundOut
{
    /// <summary>
    ///     Provides audioplayback through the WaveOut api.
    /// </summary>
    public class WaveOut : ISoundOut
    {
        private readonly WaveCallback _callback;
        private readonly Queue<int> _failedBuffers = new Queue<int>();
        private readonly object _lockObject = new object();
        private int _activeBuffers;
        private WaveOutBuffer[] _buffers;
        private Thread _callbackThread;
        private WaveOutDevice _device;
        private bool _disposed;
        private bool _isInitialized;
        private int _latency;
        private volatile PlaybackState _playbackState;
        private IWaveSource _source;
        private VolumeSource _volumeSource;
        private IntPtr _waveOutHandle;

        private const int BufferCount = 2;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaveOut" /> class with a latency of 100 ms.
        /// </summary>
        public WaveOut()
            : this(100)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaveOut" /> class.
        /// </summary>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">latency must not be less or equal to zero.</exception>
        public WaveOut(int latency)
        {
            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");

            _latency = latency;
            _callback = Callback;
            Device = WaveOutDevice.DefaultDevice;
            UseChannelMixingMatrices = true;
        }

        /// <summary>
        ///     Gets or sets the <see cref="Device" /> which should be used for playback.
        ///     The <see cref="Device" /> property has to be set before initializing. The systems default playback device is used
        ///     as default value
        ///     of the <see cref="Device" /> property.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value must not be null.</exception>
        public WaveOutDevice Device
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
        ///     Gets or sets the latency of the playback specified in milliseconds.
        ///     The <see cref="Latency" /> property has to be set before initializing.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
            set
            {
                if (value <= 0)
                    throw new ArgumentNullException("value");
                _latency = value;
            }
        }

        /// <summary>
        ///     Starts the playback.
        ///     Note: <see cref="Initialize" /> has to get called before calling Play.
        ///     If <see cref="PlaybackState" /> is <see cref="SoundOut.PlaybackState.Paused" />, <see cref="Resume" /> will be
        ///     called automatically.
        /// </summary>
        public void Play()
        {
            CheckForInvalidThreadCall();

            lock (_lockObject)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Stopped)
                {
                    _playbackState = PlaybackState.Playing;
                    foreach (WaveOutBuffer buffer in _buffers.Where(x => !x.IsQueued))
                    {
                        try
                        {
                            if (!FireUpBuffer(buffer))
                                StopInternal(null);
                        }
                        catch (Exception exception)
                        {
                            StopInternal(exception);
                        }
                    }
                }
                else if (PlaybackState == PlaybackState.Paused)
                    Resume();
            }
        }

        /// <summary>
        ///     Pauses the playback.
        /// </summary>
        public void Pause()
        {
            CheckForInvalidThreadCall();

            lock (_lockObject)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Playing)
                {
                    MmException.Try(NativeMethods.waveOutPause(_waveOutHandle), "waveOutPause");
                    _playbackState = PlaybackState.Paused;
                }
            }
        }

        /// <summary>
        ///     Resumes the paused playback.
        /// </summary>
        public void Resume()
        {
            CheckForInvalidThreadCall();

            lock (_lockObject)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Paused)
                {
                    MmException.Try(NativeMethods.waveOutRestart(_waveOutHandle), "waveOutRestart");
                    _playbackState = PlaybackState.Playing;
                }
            }
        }

        /// <summary>
        ///     Stops the playback and frees most of allocated resources.
        /// </summary>
        public void Stop()
        {
            CheckForInvalidThreadCall();

            lock (_lockObject)
            {
                CheckForDisposed();
                //don't check for isinitialized here (we don't want the Dispose method to throw an exception)

                if (PlaybackState != PlaybackState.Stopped)
                {
                    MmException.Try(NativeMethods.waveOutReset(_waveOutHandle), "waveOutReset");
                    WaitForBuffersToFinish();
                    _playbackState = PlaybackState.Stopped;
                }
                else if (PlaybackState == PlaybackState.Stopped && _activeBuffers != 0)
                {
                    WaitForBuffersToFinish();
                }
            }
        }

        /// <summary>
        ///     Initializes WaveOut instance and prepares all resources for playback.
        ///     Note that properties like <see cref="Device" />, <see cref="Latency" />,... won't affect WaveOut after calling
        ///     <see cref="Initialize" />.
        /// </summary>
        /// <param name="source">The source to prepare for playback.</param>
        public void Initialize(IWaveSource source)
        {
            CheckForInvalidThreadCall();
            lock (_lockObject)
            {
                CheckForDisposed();
                if (source == null)
                    throw new ArgumentNullException("source");

                if (PlaybackState != PlaybackState.Stopped)
                {
                    throw new InvalidOperationException(
                        "PlaybackState has to be Stopped. Call WaveOut::Stop to stop the playback.");
                }

                WaitForBuffersToFinish();

                source = new InterruptDisposingChainSource(source);
                _volumeSource = new VolumeSource(source.ToSampleSource());
                _source = _volumeSource.ToWaveSource();

                CleanupResources();
                InitializeInternal();

                _isInitialized = true;
            }
        }

        /// <summary>
        ///     Gets or sets the volume of the playback.
        ///     Valid values are in the range from 0.0 (0%) to 1.0 (100%).
        /// </summary>
        public float Volume
        {
            get { return _volumeSource != null ? _volumeSource.Volume : 1; }
            set
            {
                CheckForDisposed();
                CheckForIsInitialized();
                _volumeSource.Volume = value;
            }
        }

        /// <summary>
        ///     The currently initialized source.
        ///     To change the WaveSource property, call <see cref="Initialize" />.
        /// </summary>
        /// <remarks>
        ///     The value of the WaveSource might not be the value which was passed to the <see cref="Initialize" /> method,
        ///     because
        ///     WaveOut uses the <see cref="VolumeSource" /> class to control the volume of the playback.
        /// </remarks>
        public IWaveSource WaveSource
        {
            get { return _source; }
        }

        /// <summary>
        ///     Gets the current <see cref="SoundOut.PlaybackState" /> of the playback.
        /// </summary>
        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
        }

        /// <summary>
        ///     Occurs when the playback stops.
        /// </summary>
        public event EventHandler<PlaybackStoppedEventArgs> Stopped;

        /// <summary>
        ///     Stops the playback (if playing) and cleans up all used resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void StopInternal(Exception exception)
        {
            _playbackState = PlaybackState.Stopped;
            RaiseStopped(exception);
        }

        private void RaiseStopped(Exception exception)
        {
            if (Stopped != null)
                Stopped(this, new PlaybackStoppedEventArgs(exception));
        }

        private void CheckForIsInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("WaveOut is not initialized.");
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

        private void InitializeInternal()
        {
            Debug.WriteLine("Initialize, thread id: " + Thread.CurrentThread.ManagedThreadId);
            _callbackThread = null;
            var supportedFormats = new Queue<WaveFormat>(Device.SupportedFormats
                .OrderBy(x => Math.Abs(x.SampleRate - _source.WaveFormat.SampleRate))
                .ThenBy(x => Math.Abs(x.BitsPerSample - _source.WaveFormat.BitsPerSample))
                .ThenBy(x => Math.Abs(x.Channels - _source.WaveFormat.Channels)));

            var finalFormat = _source.WaveFormat;
            do
            {
                try
                {
                    _waveOutHandle = CreateWaveOutHandle(finalFormat);
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
            } while (_waveOutHandle == IntPtr.Zero);

            if (finalFormat != _source.WaveFormat)
            {
                //the original format of the source is not supported
                //we have to convert the source
                //todo: test channel matrix conversion
                ChannelMatrix channelMatrix = null;
                if (UseChannelMixingMatrices)
                {
                    try
                    {
                        channelMatrix = ChannelMatrix.GetMatrix(_source.WaveFormat, finalFormat);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("No channelmatrix was found.");
                    }
                }
                DmoResampler resampler = channelMatrix != null
                    ? new DmoChannelResampler(_source, channelMatrix, finalFormat)
                    : new DmoResampler(_source, finalFormat);
                resampler.Quality = 60;

                _source = resampler;
            }

            _failedBuffers.Clear();
            var bufferSize = (int) WaveSource.WaveFormat.MillisecondsToBytes(_latency);
            _buffers = new WaveOutBuffer[BufferCount];
            for (int i = 0; i < _buffers.Length; i++)
            {
                _buffers[i] = new WaveOutBuffer(_waveOutHandle, bufferSize, (IntPtr) i);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="WaveOut"/> should try to use all available channels.
        /// </summary>
        public bool UseChannelMixingMatrices { get; set; }

        private void Callback(IntPtr handle, WaveMsg msg, IntPtr user, WaveHeader header, IntPtr reserved)
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + "|" + msg);

            if (_waveOutHandle != handle)
                return;

            if (msg == WaveMsg.WOM_DONE)
            {
                if (_callbackThread == null)
                {
                    _callbackThread = Thread.CurrentThread;
                    Debug.WriteLine("CallbackThread: " + Thread.CurrentThread.ManagedThreadId);
                }

                Debug.Assert(_callbackThread == Thread.CurrentThread, "Strange thread?");
                var index = (int) header.userData;
                WaveOutBuffer buffer = _buffers[index];

                Interlocked.Decrement(ref _activeBuffers);

                /*
                 * The Play method starts all buffers. 
                 * In order to do that, it calls the waveOutWrite function.
                 * If there is a delay between buffer0 and buffer1 
                 * and buffer0 gets fired through this callback, the
                 * waveOutWrite method for buffer1 can't be called
                 * -> deadlock since the callback method waits until the
                 * Play method releases the lock. 
                 * In order to avoid that, we are using a timeout of 10ms.
                 * If the timeout exceeds, the index of the buffer gets 
                 * saved and fired within the next callback.
                 * 
                 */
                if (Monitor.TryEnter(_lockObject, 10))
                {
                    try
                    {
                        callback0:
                        if (buffer != null && PlaybackState != PlaybackState.Stopped)
                        {
                            try
                            {
                                FireUpBuffer(buffer);
                            }
                            catch (Exception exception)
                            {
                                StopFromCallback(exception);
                            }
                        }

                        if (_failedBuffers.Count > 0)
                        {
                            //continue until we find a buffer that failed and is not in queue.
                            while (_failedBuffers.Count > 0 &&
                                   (buffer = _buffers[(index = _failedBuffers.Dequeue())]).IsQueued == false)
                            {
                                Debug.WriteLine("Already queued:" + index);
                            }
                            if (buffer != null && !buffer.IsQueued)
                            {
                                Debug.WriteLine("Failed buffer: " + index);
                                goto callback0;
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(_lockObject);
                    }
                }
                else
                {
                    _failedBuffers.Enqueue(index);
                }

                if (_activeBuffers <= 0)
                    StopFromCallback(null);
            }
            else if (msg == WaveMsg.WOM_CLOSE)
            {
                if (PlaybackState != PlaybackState.Stopped)
                    StopFromCallback(null);
            }
            else if (msg == WaveMsg.WOM_OPEN)
                Debug.WriteLine("open");
        }

        private void StopFromCallback(Exception exception)
        {
            _playbackState = PlaybackState.Stopped;
            RaiseStopped(exception);
        }

        private void WaitForBuffersToFinish()
        {
            //do we have volatile to _activeBuffers???
            while (Interlocked.CompareExchange(ref _activeBuffers, 0, 0) != 0 && PlaybackState != PlaybackState.Stopped)
            {
                Thread.Sleep(10);
            }
        }

        private void CleanupResources()
        {
            if (_waveOutHandle == IntPtr.Zero)
                return;

            DisposeBuffers();
            MmException.Try(NativeMethods.waveOutClose(_waveOutHandle), "waveOutClose");
            _waveOutHandle = IntPtr.Zero;
            _callbackThread = null;

            _isInitialized = false;
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
            Debug.WriteLine("Check thread id: " + _callbackThread.ManagedThreadId);
            if (Thread.CurrentThread == _callbackThread)
                throw new InvalidOperationException("You must not access this method from the PlaybackThread.");
        }

        private bool FireUpBuffer(WaveOutBuffer buffer)
        {
            if (buffer.Refill(_source))
            {
                Interlocked.Increment(ref _activeBuffers);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Creates and returns the WaveOut handle.
        /// </summary>
        /// <param name="waveFormat">The waveformat to use.</param>
        /// <returns>A valid WaveOut handle.</returns>
        protected virtual IntPtr CreateWaveOutHandle(WaveFormat waveFormat)
        {
            IntPtr handle;
            MmException.Try(NativeMethods.waveOutOpen(out handle,
                (IntPtr) Device.DeviceId,
                waveFormat,
                _callback,
                IntPtr.Zero,
                NativeMethods.WaveInOutOpenFlags.CALLBACK_FUNCTION), "waveOutOpen");

            return handle;
        }

        /// <summary>
        ///     Disposes and stops the <see cref="WaveOut" /> instance.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            CheckForInvalidThreadCall();

            lock (_lockObject)
            {
                if (!_disposed)
                {
                    if (_waveOutHandle == IntPtr.Zero)
                        return;

                    Debug.WriteLine("Disposing WaveOut");
                    Stop();
                    CleanupResources();

                    _disposed = true;
                }
            }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="WaveOut" /> class.
        /// </summary>
        ~WaveOut()
        {
            Dispose(false);
        }

        private class InterruptDisposingChainSource : WaveAggregatorBase
        {
            public InterruptDisposingChainSource(IWaveSource source)
                : base(source)
            {
                if (source == null)
                    throw new ArgumentNullException("source");
                DisposeBaseSource = false;
            }
        }
    }
}