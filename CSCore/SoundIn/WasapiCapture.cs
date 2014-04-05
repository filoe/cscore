using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CSCore.SoundIn
{
    //http://msdn.microsoft.com/en-us/library/dd370800(v=vs.85).aspx
    /// <summary>
    /// Provides audiocapture through Wasapi.
    /// Minimum supported OS: Windows Vista (see IsSupportedOnCurrentPlatform property).
    /// </summary>
    public class WasapiCapture : ISoundRecorder
    {
        /// <summary>
        /// Gets whether Wasapi is supported on the current Platform.
        /// </summary>
        public static bool IsSupportedOnCurrentPlatform
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        public const int REFTIMES_PER_MILLISEC = 10000; //see http://msdn.microsoft.com/en-us/library/windows/desktop/dd370800(v=vs.85).aspx
        public const int REFTIMES_PER_SEC = 10000000;

        public event EventHandler<DataAvailableEventArgs> DataAvailable;
        public event EventHandler Stopped;

        private AudioClient _audioClient;
        private AudioCaptureClient _audioCaptureClient;
        private MMDevice _device;
        private AudioClientShareMode _shareMode;
        private WaveFormat _waveFormat;
        private EventWaitHandle _eventWaitHandle;
        private Thread _recordThread;
        private volatile RecordingState _recordingState;

        private int _latency;
        private bool _eventSync;
        private bool _disposed;

        private volatile bool _isInitialized = false;

        /// <summary>
        /// Creates a new WasapiCapture instance.
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
        /// Creates a new WasapiCapture instance.
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
        /// Creates a new WasapiCapture instance.
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
        /// Creates a new WasapiCapture instance.
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
        /// Creates a new WasapiCapture instance.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior. Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, the device can only be used once on the whole system. Don't use exclusive mode in combination with eventSync.</param>
        /// <param name="latency">Latency of the capture specified in milliseconds.</param>
        /// <param name="captureThreadPriority">ThreadPriority of the capturethread which runs in background and provides the audiocapture itself.</param>
        /// <param name="defaultFormat">The default WaveFormat to use for the capture. If this parameter is set to null, the best available format will be chosen automatically.</param>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode, int latency, WaveFormat defaultFormat, ThreadPriority captureThreadPriority)
        {
            if (!IsSupportedOnCurrentPlatform)
                throw new PlatformNotSupportedException("Wasapi is only supported on Windows Vista and above.");
            if (eventSync && shareMode == AudioClientShareMode.Exclusive)
                throw new ArgumentException("Don't use eventSync in combination with exclusive mode.");

            _eventSync = eventSync;
            _shareMode = shareMode;
            _waveFormat = defaultFormat;

            _latency = 100;

            _recordingState = SoundIn.RecordingState.Stopped;
        }

        /// <summary>
        /// Initializes WasapiCapture and prepares all resources for recording.
        /// Note that properties like Device, etc. won't affect WasapiCapture after calling Initialize.
        /// </summary>
        public void Initialize()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (RecordingState != SoundIn.RecordingState.Stopped)
                throw new InvalidOperationException("RecordingState has to be Stopped. Call WasapiCapture::Stop to stop the wasapicapture.");

            _recordThread.WaitForExit();

            UninitializeAudioClients();
            InitializeInternal();
            _isInitialized = true;

            Debug.WriteLine(String.Format("Initialized WasapiCapture[Mode: {0}; Latency: {1}; OutputFormat: {2}]", _shareMode, _latency, _waveFormat));
        }

        /// <summary>
        /// Start Recording.
        /// </summary>
        public void Start()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();
            CheckForInitialized();

            if (RecordingState == SoundIn.RecordingState.Stopped)
            {
                using (var waitHandle = new AutoResetEvent(false))
                {
                    _recordThread = new Thread(new ParameterizedThreadStart(CaptureProc))
                    {
                        Name = "WASAPI Capture-Thread; ID = " + DebuggingID,
                        Priority = ThreadPriority.AboveNormal
                    };
                    _recordThread.Start(waitHandle);
                    waitHandle.WaitOne();
                }
            }
        }

        /// <summary>
        /// Stop Recording.
        /// </summary>
        public void Stop()
        {
            CheckForDisposed();
            CheckForInvalidThreadCall();

            if (RecordingState != SoundIn.RecordingState.Stopped)
            {
                _recordingState = SoundIn.RecordingState.Stopped;
                _recordThread.WaitForExit(); //possible deadlock
                _recordThread = null;
            }
            else if(RecordingState == SoundIn.RecordingState.Stopped && _recordThread != null)
            {
                _recordThread.WaitForExit();
                _recordThread = null;
            }
        }

        //based on http://msdn.microsoft.com/en-us/library/windows/desktop/dd370800(v=vs.85).aspx
        private void CaptureProc(object playbackStartedEventWaitHandle)
        {
            try
            {
                int bufferSize;
                int frameSize;
                long actualDuration;
                int actualLatency;
                int sleepDuration;
                byte[] buffer;
                int eventWaitHandleIndex;
                WaitHandle[] eventWaitHandleArray;

                bufferSize = _audioClient.BufferSize;
                frameSize = WaveFormat.Channels * WaveFormat.BytesPerSample;

                actualDuration = (long)((double)REFTIMES_PER_SEC * bufferSize / WaveFormat.SampleRate);
                actualLatency = (int)(actualDuration / REFTIMES_PER_MILLISEC);
                sleepDuration = actualLatency / 8;

                buffer = new byte[bufferSize * frameSize];

                eventWaitHandleIndex = WaitHandle.WaitTimeout;
                eventWaitHandleArray = new WaitHandle[] { _eventWaitHandle };

                _audioClient.Start();
                _recordingState = SoundIn.RecordingState.Recording;

                if(playbackStartedEventWaitHandle is EventWaitHandle)
                {
                    ((EventWaitHandle)playbackStartedEventWaitHandle).Set();
                    playbackStartedEventWaitHandle = null;
                }

                while (RecordingState != SoundIn.RecordingState.Stopped)
                {
                    if(_eventSync)
                    {
                        eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, actualLatency, false);
                        if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
                            continue;
                    }
                    else
                    {
                        Thread.Sleep(sleepDuration);
                    }

                    if(RecordingState == SoundIn.RecordingState.Recording)
                    {
                        ReadData(buffer, _audioCaptureClient, (uint)frameSize);
                    }
                }

                Thread.Sleep(actualLatency / 2);

                _audioClient.Stop();
                _audioClient.Reset();
                
            }
            finally
            {
                if (playbackStartedEventWaitHandle is EventWaitHandle)
                    ((EventWaitHandle)playbackStartedEventWaitHandle).Set();
                RaiseStopped();
            }
        }

        private void ReadData(byte[] buffer, AudioCaptureClient captureClient, uint frameSize)
        {
            uint nextPacketSize = captureClient.GetNextPacketSize();
            int read = 0;
            int offset = 0;

            while (nextPacketSize != 0)
            {
                uint framesAvailable = 0;
                AudioClientBufferFlags flags;

                IntPtr nativeBuffer = captureClient.GetBuffer(out framesAvailable, out flags);

                int bytesAvailable = (int)(framesAvailable * frameSize);
                int bytesToCopy = Math.Min((int)bytesAvailable, buffer.Length);

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
            if (_shareMode == AudioClientShareMode.Exclusive)
            {
                _waveFormat = _waveFormat ?? _audioClient.MixFormat;
            }
            else
            {
                _waveFormat = _waveFormat ?? _audioClient.MixFormat;
            }

            _waveFormat = SetupWaveFormat(_waveFormat, _audioClient);

            if (!_eventSync)
            {
                _audioClient.Initialize(_shareMode, AudioClientStreamFlags.None | GetStreamFlags(), _latency * REFTIMES_PER_MILLISEC, 0, _waveFormat, Guid.Empty);
            }
            else
            {
                if (_shareMode == AudioClientShareMode.Exclusive)
                {
                    try
                    {
                        _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlags_EventCallback | GetStreamFlags(), _latency * REFTIMES_PER_MILLISEC, _latency * REFTIMES_PER_MILLISEC, _waveFormat, Guid.Empty);
                    }
                    catch (CoreAudioAPIException e)
                    {
                        if (e.ErrorCode == unchecked((int)0x88890019)) //AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED
                        {
                            int bufferSize = _audioClient.BufferSize;
                            _audioClient.Dispose();
                            long hnsRequestedDuration = (long)(((double)REFTIMES_PER_MILLISEC * 1000 / _waveFormat.SampleRate * bufferSize) + 0.5);
                            _audioClient = AudioClient.FromMMDevice(Device);
                            if (defaultFormat == null)
                                _waveFormat = _audioClient.MixFormat;
                            _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlags_EventCallback | GetStreamFlags(), hnsRequestedDuration, hnsRequestedDuration, _waveFormat, Guid.Empty);
                        }
                    }
                }
                else
                {
                    _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlags_EventCallback | GetStreamFlags(), 0, 0, _waveFormat, Guid.Empty);
                    _latency = (int)(_audioClient.StreamLatency / REFTIMES_PER_MILLISEC);
                }

                _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                _audioClient.SetEventHandle(_eventWaitHandle.SafeWaitHandle.DangerousGetHandle());
            }

            _audioCaptureClient = AudioCaptureClient.FromAudioClient(_audioClient);
        }

        private void RaiseDataAvilable(byte[] buffer, int offset, int count)
        {
            if (count < 1)
                return;
            if (DataAvailable != null)
                DataAvailable(this, new DataAvailableEventArgs(buffer, offset, count, WaveFormat));
        }

        private void RaiseStopped()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
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
        public long DebuggingID
        {
            get { return _audioCaptureClient != null ? _audioCaptureClient.BasePtr.ToInt64() : -1; }
        }

        private WaveFormat SetupWaveFormat(WaveFormat waveFormat, AudioClient audioClient)
        {
            WaveFormatExtensible closestMatch;
            WaveFormat finalFormat = waveFormat;
            if (!audioClient.IsFormatSupported(_shareMode, waveFormat, out closestMatch))
            {
                if (closestMatch == null)
                {
                    WaveFormat mixformat = audioClient.GetMixFormat();
                    if (mixformat == null || !audioClient.IsFormatSupported(_shareMode, mixformat))
                    {
                        WaveFormatExtensible[] possibleFormats = new WaveFormatExtensible[]
						{
							new WaveFormatExtensible(waveFormat.SampleRate, 32, waveFormat.Channels, DMO.MediaTypes.MEDIATYPE_IeeeFloat),
							new WaveFormatExtensible(waveFormat.SampleRate, 24, waveFormat.Channels, DMO.MediaTypes.MEDIATYPE_Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 16, waveFormat.Channels, DMO.MediaTypes.MEDIATYPE_Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 8,  waveFormat.Channels, DMO.MediaTypes.MEDIATYPE_Pcm)
						};

                        if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                        {
                            //no format found...
                            possibleFormats = new WaveFormatExtensible[]
							{
								new WaveFormatExtensible(waveFormat.SampleRate, 32, 2, DMO.MediaTypes.MEDIATYPE_IeeeFloat),
								new WaveFormatExtensible(waveFormat.SampleRate, 24, 2, DMO.MediaTypes.MEDIATYPE_Pcm),
								new WaveFormatExtensible(waveFormat.SampleRate, 16, 2, DMO.MediaTypes.MEDIATYPE_Pcm),
								new WaveFormatExtensible(waveFormat.SampleRate, 8,  2, DMO.MediaTypes.MEDIATYPE_Pcm),
								new WaveFormatExtensible(waveFormat.SampleRate, 32, 1, DMO.MediaTypes.MEDIATYPE_IeeeFloat),
								new WaveFormatExtensible(waveFormat.SampleRate, 24, 1, DMO.MediaTypes.MEDIATYPE_Pcm),
								new WaveFormatExtensible(waveFormat.SampleRate, 16, 1, DMO.MediaTypes.MEDIATYPE_Pcm),
								new WaveFormatExtensible(waveFormat.SampleRate, 8,  1, DMO.MediaTypes.MEDIATYPE_Pcm)
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

        protected virtual MMDevice GetDefaultDevice()
        {
            return MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Capture, Role.Console);
        }

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

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Stop();
                UninitializeAudioClients();
                //todo: dispose device?
            }
            _disposed = true;
        }

        ~WasapiCapture()
        {
            System.Diagnostics.Debug.Assert(false, "WasapiCapture was not disposed correctly.");
            Dispose(false);
        }
    }
}