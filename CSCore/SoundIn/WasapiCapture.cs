using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSCore.SoundIn
{
    //http://msdn.microsoft.com/en-us/library/dd370800(v=vs.85).aspx
    /// <summary>
    /// Captures audio data from a audio device (through Wasapi Apis). To capture audio from an output device, use the <see cref="WasapiLoopbackCapture"/> class.
    /// Minimum supported OS: Windows Vista (see <see cref="IsSupportedOnCurrentPlatform"/> property).
    /// </summary>
    public class WasapiCapture : ISoundIn
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="WasapiCapture"/> class is supported on the current platform.
        /// If <b>true</b>, it is supported; otherwise false.
        /// </summary>
        public static bool IsSupportedOnCurrentPlatform
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        /// <summary>
        /// Reference time units per millisecond.
        /// </summary>
        public const int ReftimesPerMillisecond = 10000; //see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370800(v=vs.85).aspx
        /// <summary>
        /// Reference time units per second.
        /// </summary>
        public const int ReftimesPerSecond = ReftimesPerMillisecond * 1000;

        /// <summary>
        /// Occurs when new data got captured and is available. 
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// Occurs when <see cref="WasapiCapture"/> stopped capturing audio.
        /// </summary>
        public event EventHandler<RecordingStoppedEventArgs> Stopped;

        private AudioClient _audioClient;
        private AudioCaptureClient _audioCaptureClient;
        private MMDevice _device;
        private readonly AudioClientShareMode _shareMode;
        private WaveFormat _waveFormat;
        private EventWaitHandle _eventWaitHandle;
        private Thread _recordThread;
        private readonly ThreadPriority _captureThreadPriority;
        private readonly SynchronizationContext _synchronizationContext;
        private volatile RecordingState _recordingState;

        private int _latency;
        private readonly bool _eventSync;
        private bool _disposed;

        private volatile bool _isInitialized;

        private readonly object _lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiCapture"/> class.
        /// CaptureThreadPriority = AboveNormal. 
        /// DefaultFormat = null. 
        /// Latency = 100ms. 
        /// EventSync = true.
        /// SharedMode = Shared.
        /// </summary>
        public WasapiCapture()
            : this(true, AudioClientShareMode.Shared)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiCapture"/> class.
        /// CaptureThreadPriority = AboveNormal. 
        /// DefaultFormat = null.
        /// Latency = 100ms.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior. Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, the device can only be used once on the whole system. Don't use exclusive mode in combination with eventSync.</param>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode)
            : this(eventSync, shareMode, 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiCapture"/> class.
        /// CaptureThreadPriority = AboveNormal. 
        /// DefaultFormat = null.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior. Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, the device can only be used once on the whole system. Don't use exclusive mode in combination with eventSync.</param>
        /// <param name="latency">Latency of the capture specified in milliseconds.</param>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode, int latency)
            : this(eventSync, shareMode, latency, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiCapture"/> class.
        /// CaptureThreadPriority = AboveNormal.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior. Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, the device can only be used once on the whole system. Don't use exclusive mode in combination with eventSync.</param>
        /// <param name="latency">Latency of the capture specified in milliseconds.</param>
        /// <param name="defaultFormat">The default WaveFormat to use for the capture. If this parameter is set to null, the best available format will be chosen automatically.</param>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode, int latency, WaveFormat defaultFormat)
            : this(eventSync, shareMode, latency, defaultFormat, ThreadPriority.AboveNormal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiCapture"/> class. SynchronizationContext = null.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior. Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, the device can only be used once on the whole system. Don't use exclusive mode in combination with eventSync.</param>
        /// <param name="latency">Latency of the capture specified in milliseconds.</param>
        /// <param name="captureThreadPriority">ThreadPriority of the capturethread which runs in background and provides the audiocapture itself.</param>
        /// <param name="defaultFormat">The default WaveFormat to use for the capture. If this parameter is set to null, the best available format will be chosen automatically.</param>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode, int latency, WaveFormat defaultFormat, ThreadPriority captureThreadPriority)
            : this(eventSync, shareMode, latency, defaultFormat, captureThreadPriority, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiCapture"/> class.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior. Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, the device can only be used once on the whole system. Don't use exclusive mode in combination with eventSync.</param>
        /// <param name="latency">Latency of the capture specified in milliseconds.</param>
        /// <param name="captureThreadPriority">ThreadPriority of the capturethread which runs in background and provides the audiocapture itself.</param>
        /// <param name="defaultFormat">The default WaveFormat to use for the capture. If this parameter is set to null, the best available format will be chosen automatically.</param>
        /// <param name="synchronizationContext">The <see cref="SynchronizationContext"/> to use to fire events on.</param>
        /// <exception cref="PlatformNotSupportedException">The current platform does not support Wasapi. For more details see: <see cref="IsSupportedOnCurrentPlatform"/>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="eventSync"/> parameter is set to true while the <paramref name="shareMode"/> is set to <see cref="AudioClientShareMode.Exclusive"/>.</exception>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode, int latency, WaveFormat defaultFormat,
            ThreadPriority captureThreadPriority, SynchronizationContext synchronizationContext)
        {
            if (!IsSupportedOnCurrentPlatform)
                throw new PlatformNotSupportedException("Wasapi is only supported on Windows Vista and above.");
            if (eventSync && shareMode == AudioClientShareMode.Exclusive)
                throw new ArgumentException("Don't use eventSync in combination with exclusive mode.");

            _eventSync = eventSync;
            _shareMode = shareMode;
            _waveFormat = defaultFormat;

            _latency = latency;
            _captureThreadPriority = captureThreadPriority;
            _synchronizationContext = synchronizationContext;

            _recordingState = RecordingState.Stopped;
        }

        /// <summary>
        /// Initializes WasapiCapture and prepares all resources for recording.
        /// Note that properties like Device, etc. won't affect WasapiCapture after calling Initialize.
        /// </summary>
        public void Initialize()
        {
            CheckForInvalidThreadCall();
            lock (_lockObj)
            {
                CheckForDisposed();

                if (RecordingState != RecordingState.Stopped)
                    throw new InvalidOperationException(
                        "RecordingState has to be Stopped. Call WasapiCapture::Stop to stop the wasapicapture.");

                _recordThread.WaitForExit();

                UninitializeAudioClients();
                InitializeInternal();
                _isInitialized = true;

                Debug.WriteLine(String.Format("Initialized WasapiCapture[Mode: {0}; Latency: {1}; OutputFormat: {2}]",
                    _shareMode, _latency, _waveFormat));
            }
        }

        /// <summary>
        /// Start recording.
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
                    using (var waitHandle = new AutoResetEvent(false))
                    {
                        _recordThread = new Thread(CaptureProc)
                        {
                            Name = "WASAPI Capture-Thread; ID = " + DebuggingId,
                            Priority = _captureThreadPriority
                        };
                        _recordThread.Start(waitHandle);
                        waitHandle.WaitOne();
                    }
                }
            }
        }

        /// <summary>
        /// Stop recording.
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
                    _recordingState = RecordingState.Stopped;
                    _recordThread.WaitForExit(); //possible deadlock
                    _recordThread = null;
                }
                else if (RecordingState == RecordingState.Stopped && _recordThread != null)
                {
                    _recordThread.WaitForExit();
                    _recordThread = null;
                }
            }
        }

        //based on http://msdn.microsoft.com/en-us/library/windows/desktop/dd370800(v=vs.85).aspx
        private void CaptureProc(object param)
        {
            var playbackStartedEventWaitHandle = param as EventWaitHandle;

            Exception exception = null;
            try
            {
                int bufferSize = _audioClient.BufferSize;
                int frameSize = WaveFormat.Channels * WaveFormat.BytesPerSample;

                long actualDuration = (long) ((double) ReftimesPerSecond * bufferSize / WaveFormat.SampleRate);
                int actualLatency = (int) (actualDuration / ReftimesPerMillisecond);
                int sleepDuration = actualLatency / 8;

                byte[] buffer = new byte[bufferSize * frameSize];

                WaitHandle[] eventWaitHandleArray = {_eventWaitHandle};

                _audioClient.Start();
                _recordingState = RecordingState.Recording;

                if (playbackStartedEventWaitHandle != null)
                {
                    playbackStartedEventWaitHandle.Set();
                    playbackStartedEventWaitHandle = null;
                }

                while (RecordingState != RecordingState.Stopped)
                {
                    if (_eventSync)
                    {
                        int eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, actualLatency, false);
                        if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
                            continue;
                    }
                    else
                    {
                        Thread.Sleep(sleepDuration);
                    }

                    if (RecordingState == RecordingState.Recording)
                    {
                        ReadData(buffer, _audioCaptureClient, (uint) frameSize);
                    }
                }

                Thread.Sleep(actualLatency / 2);

                _audioClient.Stop();
                _audioClient.Reset();

            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (playbackStartedEventWaitHandle != null)
                    playbackStartedEventWaitHandle.Set();
                RaiseStopped(exception);
            }
        }

        private void ReadData(byte[] buffer, AudioCaptureClient captureClient, uint frameSize)
        {
            int nextPacketSize = captureClient.GetNextPacketSize();
            int read = 0;
            int offset = 0;

            while (nextPacketSize != 0)
            {
                int framesAvailable;
                AudioClientBufferFlags flags;

                IntPtr nativeBuffer = captureClient.GetBuffer(out framesAvailable, out flags);

                int bytesAvailable = (int)(framesAvailable * frameSize);
                int bytesToCopy = Math.Min(bytesAvailable, buffer.Length);

                if (Math.Max(buffer.Length - read, 0) < bytesAvailable && read > 0)
                {
                    RaiseDataAvilable(buffer, 0, read);
                    read = offset = 0;
                }

                if ((flags & AudioClientBufferFlags.Silent) == AudioClientBufferFlags.Silent)
                {
                    Array.Clear(buffer, offset, bytesToCopy);
                }
                else
                {
                    Marshal.Copy(nativeBuffer, buffer, offset, bytesToCopy);
                }

                read += bytesToCopy;
                offset += bytesToCopy;

                captureClient.ReleaseBuffer(framesAvailable);
                nextPacketSize = captureClient.GetNextPacketSize();
            }

            RaiseDataAvilable(buffer, 0, read);
        }

        private void InitializeInternal()
        {
            var defaultFormat = _waveFormat;

            _audioClient = AudioClient.FromMMDevice(Device);
            /*if (_shareMode == AudioClientShareMode.Exclusive)
            {
                _waveFormat = _waveFormat ?? _audioClient.MixFormat;
            }
            else
            {
                _waveFormat = _waveFormat ?? _audioClient.MixFormat;
            }*/
            _waveFormat = _waveFormat ?? _audioClient.MixFormat;

            _waveFormat = SetupWaveFormat(_waveFormat, _audioClient);

            if (!_eventSync)
            {
                _audioClient.Initialize(_shareMode, AudioClientStreamFlags.None | GetStreamFlags(), _latency * ReftimesPerMillisecond, 0, _waveFormat, Guid.Empty);
            }
            else
            {
                if (_shareMode == AudioClientShareMode.Exclusive)
                {
                    try
                    {
                        _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback | GetStreamFlags(), _latency * ReftimesPerMillisecond, _latency * ReftimesPerMillisecond, _waveFormat, Guid.Empty);
                    }
                    catch (CoreAudioAPIException e)
                    {
                        if (e.ErrorCode == unchecked((int)0x88890019)) //AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED
                        {
                            int bufferSize = _audioClient.BufferSize;
                            _audioClient.Dispose();
                            long hnsRequestedDuration = (long)(((double)ReftimesPerMillisecond * 1000 / _waveFormat.SampleRate * bufferSize) + 0.5);
                            _audioClient = AudioClient.FromMMDevice(Device);
                            if (defaultFormat == null)
                                _waveFormat = _audioClient.MixFormat;
                            _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback | GetStreamFlags(), hnsRequestedDuration, hnsRequestedDuration, _waveFormat, Guid.Empty);
                        }
                    }
                }
                else
                {
                    _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback | GetStreamFlags(), 0, 0, _waveFormat, Guid.Empty);
                    if(_audioClient.StreamLatency > 0) 
                    {
                        _latency = (int) (_audioClient.StreamLatency / ReftimesPerMillisecond);
                    }
                }

                _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                _audioClient.SetEventHandle(_eventWaitHandle.SafeWaitHandle.DangerousGetHandle());
            }

            _audioCaptureClient = AudioCaptureClient.FromAudioClient(_audioClient);
        }

        private void RaiseDataAvilable(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return;
            if (DataAvailable != null)
            {
                var e = new DataAvailableEventArgs(buffer, offset, count, WaveFormat);
                if (_synchronizationContext != null)
                    _synchronizationContext.Post(o => DataAvailable(this, e), null); //use post instead of send to avoid deadlocks
                else
                    DataAvailable(this, e);
            }
        }

        private void RaiseStopped(Exception exception)
        {
            if (Stopped != null)
            {
                var e = new RecordingStoppedEventArgs(exception);
                if(_synchronizationContext != null)
                    _synchronizationContext.Post(o => Stopped(this, e), null); //use post instead of send to avoid deadlocks
                else
                    Stopped(this, e);
            }
        }

        /// <summary>
        /// Gets the RecordingState.
        /// </summary>
        public RecordingState RecordingState
        {
            get { return _recordingState; }
        }

        /// <summary>
        /// Gets or sets the capture device to use.
        /// Set this property before calling Initialize.
        /// </summary>
        public MMDevice Device
        {
            get
            {
                return _device ?? GetDefaultDevice();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _device = value;
            }
        }

        /// <summary>
        /// Gets the OutputFormat.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        /// Random ID based on internal audioclients memory address for debugging purposes. 
        /// </summary>
        public long DebuggingId
        {
            get { return _audioCaptureClient != null ? _audioCaptureClient.BasePtr.ToInt64() : -1; }
        }

        private WaveFormat SetupWaveFormat(WaveFormat waveFormat, AudioClient audioClient)
        {
            WaveFormat closestMatch;
            WaveFormat finalFormat = waveFormat;
            if (!audioClient.IsFormatSupported(_shareMode, waveFormat, out closestMatch))
            {
                if (closestMatch == null)
                {
                    WaveFormat mixformat = audioClient.GetMixFormat();
                    if (mixformat == null || !audioClient.IsFormatSupported(_shareMode, mixformat))
                    {
                        WaveFormatExtensible[] possibleFormats =
                        {
                            new WaveFormatExtensible(waveFormat.SampleRate, 32, waveFormat.Channels, AudioSubTypes.IeeeFloat),
                            new WaveFormatExtensible(waveFormat.SampleRate, 24, waveFormat.Channels, AudioSubTypes.Pcm),
                            new WaveFormatExtensible(waveFormat.SampleRate, 16, waveFormat.Channels, AudioSubTypes.Pcm),
                            new WaveFormatExtensible(waveFormat.SampleRate, 8,  waveFormat.Channels, AudioSubTypes.Pcm)
                        };

                        if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                        {
                            //no format found...
                            possibleFormats = new[]
                            {
                                new WaveFormatExtensible(waveFormat.SampleRate, 32, 2, AudioSubTypes.IeeeFloat),
                                new WaveFormatExtensible(waveFormat.SampleRate, 24, 2, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 16, 2, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 8,  2, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 32, 1, AudioSubTypes.IeeeFloat),
                                new WaveFormatExtensible(waveFormat.SampleRate, 24, 1, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 16, 1, AudioSubTypes.Pcm),
                                new WaveFormatExtensible(waveFormat.SampleRate, 8,  1, AudioSubTypes.Pcm)
                            };

                            if (CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                            {
                                throw new NotSupportedException("Could not find a supported format.");
                            }
                        }
                    }

                    finalFormat = mixformat;
                }
                else
                {
                    finalFormat = closestMatch;
                }
            }

            return finalFormat;
        }

        private bool CheckForSupportedFormat(AudioClient audioClient, IEnumerable<WaveFormatExtensible> waveFormats, out WaveFormat foundMatch)
        {
            foundMatch = null;
            foreach (var format in waveFormats)
            {
                if (audioClient.IsFormatSupported(_shareMode, format))
                {
                    foundMatch = format;
                    return true;
                }
            }
            return false;
        }

        private void UninitializeAudioClients()
        {
            if (_audioClient != null)
            {
                _audioClient.Dispose();
                _audioClient = null;
            }
            if (_audioCaptureClient != null)
            {
                _audioCaptureClient.Dispose();
                _audioCaptureClient = null;
            }
            if(_eventWaitHandle != null)
            {
                _eventWaitHandle.Close();
                _eventWaitHandle = null;
            }

            _isInitialized = false;
        }

        /// <summary>
        /// Returns the default device.
        /// </summary>
        /// <returns>The default device.</returns>
        protected virtual MMDevice GetDefaultDevice()
        {
            return MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Capture, Role.Console);
        }

        /// <summary>
        /// Returns the stream flags to use for the audioclient initialization.
        /// </summary>
        /// <returns>The stream flags to use for the audioclient initialization.</returns>
        protected virtual AudioClientStreamFlags GetStreamFlags()
        {
            return AudioClientStreamFlags.None;
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("WasapiCapture");
        }

        private void CheckForInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Not initialized.");
        }

        private void CheckForInvalidThreadCall()
        {
            if (Thread.CurrentThread == _recordThread)
                throw new InvalidOperationException("You must not access this method from the CaptureThread.");
        }

        /// <summary>
        /// Stops the capture and frees all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                if (!_disposed)
                {
                    Stop();
                    UninitializeAudioClients();
                    //todo: dispose device?
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WasapiCapture"/> class.
        /// </summary>
        ~WasapiCapture()
        {
            Dispose(false);
            Debug.Assert(false, "WasapiCapture was not disposed correctly.");
        }
    }
}