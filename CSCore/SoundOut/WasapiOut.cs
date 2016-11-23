using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Utils;
using CSCore.Win32;

namespace CSCore.SoundOut
{
    /// <summary>
    ///     Provides audioplayback through Wasapi.
    ///     Minimum supported OS: Windows Vista (see <see cref="IsSupportedOnCurrentPlatform" /> property).
    /// </summary>
    public class WasapiOut : ISoundOut
    {
        private const Role DeviceRoleNotSet = (Role) 0xFF;

        private readonly bool _eventSync;
        private readonly ThreadPriority _playbackThreadPriority;
        private readonly AudioClientShareMode _shareMode;
        private readonly SynchronizationContext _syncContext;
        private AudioClient _audioClient;
        private bool _createdResampler;
        private MMDevice _device;
        private bool _disposed;
        private EventWaitHandle _eventWaitHandle;
        private bool _isInitialized;

        private int _latency;
        private WaveFormat _outputFormat;
        private volatile PlaybackState _playbackState;
        private Thread _playbackThread;
        private AudioRenderClient _renderClient;
        private VolumeSource _volumeSource;
        private IWaveSource _source;
        private StreamRoutingOptions _streamRoutingOptions = StreamRoutingOptions.OnFormatChange | StreamRoutingOptions.OnDeviceDisconnect;

        private Role _deviceRole = DeviceRoleNotSet;

        private readonly object _lockObj = new object();

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        ///     PlaybackThreadPriority = AboveNormal.
        ///     Latency = 100ms.
        ///     EventSync = False.
        ///     ShareMode = Shared.
        /// </summary>
        public WasapiOut()
            : this(false, AudioClientShareMode.Shared, 100) //100 ms default
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        ///     PlaybackThreadPriority = AboveNormal.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
        /// <param name="shareMode">
        ///     Specifies how to open the audio device. Note that if exclusive mode is used, only one single
        ///     playback for the specified device is possible at once.
        /// </param>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency)
            : this(eventSync, shareMode, latency, ThreadPriority.AboveNormal)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        ///     EventSyncContext = SynchronizationContext.Current.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
        /// <param name="shareMode">
        ///     Specifies how to open the audio device. Note that if exclusive mode is used, only one single
        ///     playback for the specified device is possible at once.
        /// </param>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <param name="playbackThreadPriority">
        ///     ThreadPriority of the playbackthread which runs in background and feeds the device
        ///     with data.
        /// </param>
        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency,
            ThreadPriority playbackThreadPriority)
            : this(eventSync, shareMode, latency, playbackThreadPriority, SynchronizationContext.Current)
        {
        }

        /// <summary>
        ///     Initializes an new instance of <see cref="WasapiOut" /> class.
        /// </summary>
        /// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
        /// <param name="shareMode">
        ///     Specifies how to open the audio device. Note that if exclusive mode is used, only one single
        ///     playback for the specified device is possible at once.
        /// </param>
        /// <param name="latency">Latency of the playback specified in milliseconds.</param>
        /// <param name="playbackThreadPriority">
        ///     <see cref="ThreadPriority"/> of the playbackthread which runs in background and feeds the device
        ///     with data.
        /// </param>
        /// <param name="eventSyncContext">
        ///     The <see cref="SynchronizationContext"/> which is used to raise any events like the <see cref="Stopped"/>-event.
        ///     If the passed value is not null, the events will be called async through the <see cref="SynchronizationContext.Post"/> method.
        /// </param>
        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency,
            ThreadPriority playbackThreadPriority, SynchronizationContext eventSyncContext)
        {
            if (!IsSupportedOnCurrentPlatform)
                throw new PlatformNotSupportedException("Wasapi is only supported on Windows Vista and above.");

            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");

            _latency = latency;
            _shareMode = shareMode;
            _eventSync = eventSync;
            _playbackThreadPriority = playbackThreadPriority;
            _syncContext = eventSyncContext;

            UseChannelMixingMatrices = false;

            /*
             * In order to avoid significant changes without
             * a new release, set All as default on CSCore 1.2 and above.
             */
            if (CSCoreUtils.IsGreaterEqualCSCore12)
            {
                _streamRoutingOptions = StreamRoutingOptions.All;
            }
        }

        /// <summary>
        ///     Gets a value which indicates whether Wasapi is supported on the current Platform. True means that the current
        ///     platform supports <see cref="WasapiOut" />; False means that the current platform does not support
        ///     <see cref="WasapiOut" />.
        /// </summary>
        public static bool IsSupportedOnCurrentPlatform
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        /// <summary>
        /// Sets a value indicating whether the Desktop Window Manager (DWM) has to opt in to or out of Multimedia Class Schedule Service (MMCSS)
        /// scheduling while the current process is alive.
        /// </summary>
        /// <value>
        /// <c>True</c> to instruct the Desktop Window Manager to participate in MMCSS scheduling; <c>False</c> to opt out or end participation in MMCSS scheduling.
        /// </value>
        /// <remarks>DWM will be scheduled by the MMCSS as long as any process that called DwmEnableMMCSS to enable MMCSS is active and has not previously called DwmEnableMMCSS to disable MMCSS.</remarks>
        public static bool EnableDwmMmcssScheduling
        {
            set { Marshal.ThrowExceptionForHR(NativeMethods.DwmEnableMMCSS(value)); }
        }

        /// <summary>
        ///     Gets or sets the stream routing options.
        /// </summary>
        /// <value>
        ///     The stream routing options. 
        /// </value>
        /// <remarks>
        ///     The <see cref="SoundOut.StreamRoutingOptions.OnDefaultDeviceChange"/> flag can only be used
        ///     if the <see cref="Device"/> is the default device.
        ///     That behavior can be changed by overriding the <see cref="UpdateStreamRoutingOptions"/> method.
        /// </remarks>
        public StreamRoutingOptions StreamRoutingOptions
        {
            get { return _streamRoutingOptions; }
            set
            {
                _streamRoutingOptions = value;
                UpdateStreamRoutingOptions();
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Device" /> which should be used for playback.
        ///     The <see cref="Device" /> property has to be set before initializing. The systems default playback device is used
        ///     as default value
        ///     of the <see cref="Device" /> property.
        /// </summary>
        /// <remarks>
        ///     Make sure to set only activated render devices.
        /// </remarks>
        public MMDevice Device
        {
            get
            {
                return _device ?? (_device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, DeviceRole));
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _device = value;
                UpdateStreamRoutingOptions();
            }
        }

        /// <summary>
        ///     Gets a random ID based on internal audioclients memory address for debugging purposes.
        /// </summary>
        public long DebuggingId
        {
            get { return _audioClient != null ? _audioClient.BasePtr.ToInt64() : -1; }
        }

        /// <summary>
        ///     Gets or sets the latency of the playback specified in milliseconds.
        /// The <see cref="Latency" /> property has to be set before initializing.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _latency = value;
            }
        }

        /// <summary>
        ///     Occurs when the playback stops.
        /// </summary>
        public event EventHandler<PlaybackStoppedEventArgs> Stopped;

        /// <summary>
        ///     Initializes WasapiOut instance and prepares all resources for playback.
        ///     Note that properties like <see cref="Device" />, <see cref="Latency" />,... won't affect WasapiOut after calling
        ///     <see cref="Initialize" />.
        /// </summary>
        /// <param name="source">The source to prepare for playback.</param>
        public void Initialize(IWaveSource source)
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();

                if (source == null)
                    throw new ArgumentNullException("source");

                if (PlaybackState != PlaybackState.Stopped)
                {
                    throw new InvalidOperationException(
                        "PlaybackState has to be Stopped. Call WasapiOut::Stop to stop the playback.");
                }

                _playbackThread.WaitForExit();

                //prevent that the original source will be disposed anyway
                source = new InterruptDisposingChainWaveSource(source);
                
                _volumeSource = new VolumeSource(source.ToSampleSource());

                _source = WrapVolumeSource(_volumeSource);

                CleanupResources();
                InitializeInternal();
                _isInitialized = true;
            }
        }

        private IWaveSource WrapVolumeSource(VolumeSource volumeSource)
        {
            //since we want to reuse the volumesource in the case of a streamswitch (see streamrouting)
            //we prevent the source chain from disposing our volume source.
            return new InterruptDisposingChainSampleSource(volumeSource).ToWaveSource();
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

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Stopped)
                {
                    using (var waitHandle = new AutoResetEvent(false))
                    {
                        //just to be sure that the thread finished already. Should not be necessary because after Stop(), Initialize() has to be called which already waits until the playbackthread stopped.
                        _playbackThread.WaitForExit();
                        _playbackThread = new Thread(PlaybackProc)
                        {
                            Name = "WASAPI Playback-Thread; ID = " + DebuggingId,
                            Priority = _playbackThreadPriority
                        };

                        _playbackThread.Start(waitHandle);
                        waitHandle.WaitOne();
                    }
                }
                else if (PlaybackState == PlaybackState.Paused)
                {
                    Resume();
                }
            }
        }

        /// <summary>
        ///     Stops the playback and frees most of allocated resources.
        /// </summary>
        public void Stop()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                //don't check for isinitialized here (we don't want the Dispose method to throw an exception)

                if (_playbackState != PlaybackState.Stopped && _playbackThread != null)
                {
                    _playbackState = PlaybackState.Stopped;
                    _playbackThread.WaitForExit(); //possible deadlock
                    _playbackThread = null;
                }
                else if (_playbackState == PlaybackState.Stopped && _playbackThread != null)
                {
                    /*
                    * On EOF playbackstate is Stopped, but thread is not stopped. => 
                    * New Session can be started while cleaning up old one => unknown behavior. =>
                    * Always call Stop() to make sure, you wait until the thread is finished cleaning up.
                    */
                    _playbackThread.WaitForExit();
                    _playbackThread = null;
                }
                else
                    Debug.WriteLine("Wasapi is already stopped.");
            }
        }

        /// <summary>
        ///     Resumes the paused playback.
        /// </summary>
        public void Resume()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (_playbackState == PlaybackState.Paused)
                    _playbackState = PlaybackState.Playing;
            }
        }

        /// <summary>
        ///     Pauses the playback.
        /// </summary>
        public void Pause()
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                CheckForDisposed();
                CheckForIsInitialized();

                if (PlaybackState == PlaybackState.Playing)
                    _playbackState = PlaybackState.Paused;
            }
        }

        /// <summary>
        ///     Gets the current <see cref="SoundOut.PlaybackState"/> of the playback.
        /// </summary>
        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
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
        ///     To change the WaveSource property, call <see cref="Initialize"/>.
        /// </summary>
        /// <remarks>
        ///     The value of the WaveSource might not be the value which was passed to the <see cref="Initialize"/> method, because
        ///     WasapiOut (depending on the waveformat of the source) has to use a DmoResampler.
        /// </remarks>
        public IWaveSource WaveSource
        {
            get { return _source; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="WasapiOut"/> should try to use all available channels.
        /// </summary>
        public bool UseChannelMixingMatrices { get; set; }

        /// <summary>
        ///     Stops the playback (if playing) and cleans up all used resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void PlaybackProc(object playbackStartedEventWaithandle)
        {
            Exception exception = null;
            IntPtr avrtHandle = IntPtr.Zero;
            string mmcssType = Latency > 25 ? "Audio" : "Pro Audio";
            int taskIndex = 0;
            try
            {
                //we need a least one wait handle for the streamrouting stuff
                //if eventsync is configured, we're using a second event for refilling the buffer
                var eventWaitHandleArray = _eventSync ? new WaitHandle[] {_streamSwitchEvent, _eventWaitHandle} : new WaitHandle[] { _streamSwitchEvent };
                
                //the wait time for event sync has the default value of latency * 3
                //if we're using a pure render loop, the wait time has to be much lower
                //lets say latency / 8. otherwise we might loose too much time
                int waitTime = _eventSync ? _latency * 3 : _latency / 8;
                waitTime = Math.Max(1, waitTime);


                //start the audio client
                _audioClient.Start();

                //update the playbackstate of the wasapiout instance
                _playbackState = PlaybackState.Playing;

                //enable mmcss
                avrtHandle = NativeMethods.AvSetMmThreadCharacteristics(mmcssType, ref taskIndex);

                //if a eventwaithandle got passed as argument, set it
                //this is necessary to signal the Play method that the
                //playbackthread is ready to play some audio data
                if (playbackStartedEventWaithandle is EventWaitHandle)
                {
                    ((EventWaitHandle)playbackStartedEventWaithandle).Set();
                    playbackStartedEventWaithandle = null;
                }

                bool isAudioClientStopped = false;

                byte[] buffer = null;
                int bufferSize = 0;
                int frameSize = 0;

                while (PlaybackState != PlaybackState.Stopped)
                {
                    try
                    {
                        //initialize the buffer within the audio render loop
                        //this is necessary because the buffer has to be rebuilt
                        //if a streamswitch (see streamrouting) was done
                        if (buffer == null)
                        {
                            bufferSize = _audioClient.BufferSize;
                            frameSize = _outputFormat.Channels * _outputFormat.BytesPerSample;

                            buffer = new byte[bufferSize * frameSize];
                        }

                        int waitResult = WaitHandle.WaitAny(eventWaitHandleArray, waitTime);
                        if (waitResult == WaitHandle.WaitTimeout)
                        {
                            if (_eventSync)
                            {
                                //guarantee that the stopped audio client (in exclusive and eventsync mode) can be
                                //restarted below
                                if (PlaybackState != PlaybackState.Playing && !isAudioClientStopped)
                                    continue;
                            }
                            else
                            {
                                //no streamswitch event occurred
                                //do nothing ... continue with processing
                            }
                        }
                        else if (waitResult == 0)
                        {
                            //streamswitch
                            if (!HandleStreamSwitchEvent())
                            {
                                //stop playback
                                _playbackState = PlaybackState.Stopped;
                            }

                            //set the buffer to null 
                            //this will re-initialize the 
                            //buffer, bufferSize and frameSize variables
                            buffer = null;
                            continue;
                        }

                        if (PlaybackState == PlaybackState.Playing)
                        {
                            //if the audio client got stopped in order to avoid the "repetitive" sound
                            //we have to restart it ->
                            if (isAudioClientStopped)
                            {
                                //restart the audioclient if it is still paused in exclusive mode
                                //belongs to the bugfix described below. http://cscore.codeplex.com/workitem/23
                                _audioClient.Start();
                                isAudioClientStopped = false;
                            }

                            //get the current padding
                            int padding;
                            if (_eventSync && _shareMode == AudioClientShareMode.Exclusive)
                            {
                                /*
                             * "For an exclusive-mode rendering or capture stream that was 
                             * initialized with the AUDCLNT_STREAMFLAGS_EVENTCALLBACK flag, 
                             * the client typically has no use for the padding value reported 
                             * by GetCurrentPadding. Instead, the client accesses an entire 
                             * buffer during each processing pass." 
                             * (see https://msdn.microsoft.com/en-us/library/windows/desktop/dd370868(v=vs.85).aspx)
                             */
                                padding = 0;
                            }
                            else
                            {
                                if (_audioClient.GetCurrentPaddingNative(out padding) != (int) HResult.S_OK)
                                    continue;

                            }

                            int framesReadyToFill = bufferSize - padding;

                            //avoid conversion errors
                            if (framesReadyToFill <= 5 ||
                                ((_source is DmoResampler) &&
                                 ((DmoResampler) _source).OutputToInput(framesReadyToFill * frameSize) <= 0))
                                continue;

                            if (!FeedBuffer(_renderClient, buffer, framesReadyToFill, frameSize))
                                _playbackState = PlaybackState.Stopped; //source is eof

                        }
                        else if (PlaybackState == PlaybackState.Paused &&
                                 _shareMode == AudioClientShareMode.Exclusive &&
                                 !isAudioClientStopped)
                        {
                            //stop the audioclient on paused if the sharemode is set to exclusive
                            //otherwise there would be a "repetitive" sound. see http://cscore.codeplex.com/workitem/23
                            _audioClient.Stop();
                            isAudioClientStopped = true;
                        }
                    }
                    catch (CoreAudioAPIException ex)
                    {
                        //this error can occur while if stream routing has not finished yet
                        if (ex.ErrorCode != (int) HResult.AUDCLNT_E_DEVICE_INVALIDATED)
                        {
                            throw;
                        }
                    }
                }

                if (avrtHandle != IntPtr.Zero)
                {
                    NativeMethods.AvRevertMmThreadCharacteristics(avrtHandle);
                    avrtHandle = IntPtr.Zero;
                }


                Thread.Sleep(_latency / 2);

                _audioClient.Stop();
                _audioClient.Reset();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                //set the playbackstate to stopped
                _playbackState = PlaybackState.Stopped;
                if (avrtHandle != IntPtr.Zero)
                    NativeMethods.AvRevertMmThreadCharacteristics(avrtHandle);

                //set the eventWaitHandle since the Play() method maybe still waits on it (only possible if there were any errors during the initialization)
                var eventWaitHandle = playbackStartedEventWaithandle as EventWaitHandle;
                if (eventWaitHandle != null)
                    eventWaitHandle.Set();

                RaiseStopped(exception);
            }
        }

        private void CheckForInvalidThreadCall()
        {
            if (Thread.CurrentThread == _playbackThread)
                throw new InvalidOperationException("You must not access this method from the PlaybackThread.");
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("WasapiOut");
        }

        private void CheckForIsInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("WasapiOut is not initialized.");
        }

        private void InitializeInternal()
        {
            const int reftimesPerMillisecond = 10000;

            _audioClient = AudioClient.FromMMDevice(Device);
            _outputFormat = SetupWaveFormat(_source, _audioClient);

            long latency = _latency * reftimesPerMillisecond;
        AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED_TRY_AGAIN:
            try
            {

                if (!_eventSync)
                    _audioClient.Initialize(_shareMode, AudioClientStreamFlags.None, latency, 0, _outputFormat,
                        Guid.Empty);
                else //event sync
                {
                    if (_shareMode == AudioClientShareMode.Exclusive) //exclusive
                    {
                        _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback, latency,
                            latency, _outputFormat, Guid.Empty);
                    }
                    else //shared
                    {
                        _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlagsEventCallback, 0, 0,
                            _outputFormat, Guid.Empty);
                        //latency = (int)(_audioClient.StreamLatency / reftimesPerMillisecond);
                    }
                }
            }
            catch (CoreAudioAPIException exception)
            {
                if (exception.ErrorCode == unchecked((int)0x88890019)) //AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED
                {
                    const long reftimesPerSec = 10000000;
                    int framesInBuffer = _audioClient.GetBufferSize();
                    // ReSharper disable once PossibleLossOfFraction
                    latency = (int)(reftimesPerSec * framesInBuffer / _outputFormat.SampleRate + 0.5);
                    goto AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED_TRY_AGAIN;
                }
                throw;
            }

            if (_audioClient.StreamLatency != 0) //windows 10 returns zero, got no idea why => https://github.com/filoe/cscore/issues/11
            {
                Latency = (int)(_audioClient.StreamLatency / reftimesPerMillisecond);   
            }

            if (_eventSync)
            {
                _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                _audioClient.SetEventHandle(_eventWaitHandle.SafeWaitHandle.DangerousGetHandle());
            }

            _renderClient = AudioRenderClient.FromAudioClient(_audioClient);

            if (_streamSwitchEvent == null)
            {
                _streamSwitchEvent = new AutoResetEvent(false);
            }

            InitializeStreamRouting();
        }

        private void CleanupResources(bool streamSwitch = false)
        {
            if (_createdResampler && _source is DmoResampler)
            {
                //dispose the source -> the volume source won't get touched
                //because of the interruption
                _source.Dispose(); 
                _source = streamSwitch ? WrapVolumeSource(_volumeSource) : null;
            }

            if (_renderClient != null)
            {
                _renderClient.Dispose();
                _renderClient = null;
            }
            if (_audioClient != null && _audioClient.BasePtr != IntPtr.Zero)
            {
                try
                {
                    _audioClient.StopNative();
                    _audioClient.Reset();
                }
                catch (CoreAudioAPIException ex)
                {
                    if (ex.ErrorCode != unchecked((int)0x88890001)) //AUDCLNT_E_NOT_INITIALIZED
                        throw;
                }
                _audioClient.Dispose();
                _audioClient = null;
            }
            if (_eventWaitHandle != null)
            {
                _eventWaitHandle.Close();
                _eventWaitHandle = null;
            }

            TerminateStreamRouting();

            _isInitialized = false;
        }

        private WaveFormat SetupWaveFormat(IWaveSource source, AudioClient audioClient)
        {
            WaveFormat waveFormat = source.WaveFormat;
            WaveFormat closestMatch;
            WaveFormat finalFormat = waveFormat;
            //check whether initial format is supported
            if (!audioClient.IsFormatSupported(_shareMode, waveFormat, out closestMatch))
            {
                //initial format is not supported -> maybe there was some kind of close match ...
                if (closestMatch == null)
                {
                    //no match ... check whether the format of the windows audio mixer is supported
                    //yes ... this gets executed for shared and exclusive mode streams
                    WaveFormat mixformat = audioClient.GetMixFormat();
                    if (mixformat == null || !audioClient.IsFormatSupported(_shareMode, mixformat))
                    {
                        //mixformat is not supported
                        //start generating possible formats

                        mixformat = null;
                        WaveFormatExtensible[] possibleFormats;
                        if (_shareMode == AudioClientShareMode.Exclusive)
                        {
                            //for exclusive mode streams use the DeviceFormat of the initialized MMDevice
                            //as base for further possible formats
                            var deviceFormat = Device.DeviceFormat;

                            //generate some possible formats based on the samplerate of the DeviceFormat
                            possibleFormats = GetPossibleFormats(deviceFormat.SampleRate, deviceFormat.Channels);
                            if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                            {
                                //none of the tested formats were supported
                                //try some different samplerates
                                List<WaveFormatExtensible> waveFormats = new List<WaveFormatExtensible>();
                                foreach (var sampleRate in new[] {44100, 48000, 96000, 192000})
                                {
                                    waveFormats.AddRange(GetPossibleFormats(sampleRate, deviceFormat.Channels));
                                }

                                //assign the generated formats with samplerates 44.1kHz, 48kHz, 96kHz and 192kHz to 
                                //the possibleFormats array which will be used below
                                possibleFormats = waveFormats.ToArray();
                            }
                        }
                        else
                        {
                            //for shared mode streams, generate some formats based on the initial waveFormat
                            possibleFormats = GetPossibleFormats(waveFormat.SampleRate, waveFormat.Channels);
                        }

                        if (mixformat == null)
                        {
                            if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                            {
                                throw new NotSupportedException("Could not find a supported format.");
                            }
                        }
                    }

                    finalFormat = mixformat;
                }
                else
                    finalFormat = closestMatch;

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
                _createdResampler = true;

                return finalFormat;
            }

            return finalFormat;
        }

        private WaveFormatExtensible[] GetPossibleFormats(int sampleRate, int suggestedNumberOfChannels)
        {
            return new[]
            {
                new WaveFormatExtensible(sampleRate, 32, suggestedNumberOfChannels,
                    AudioSubTypes.IeeeFloat),
                new WaveFormatExtensible(sampleRate, 24, suggestedNumberOfChannels,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 16, suggestedNumberOfChannels,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 8, suggestedNumberOfChannels,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 32, 2,
                    AudioSubTypes.IeeeFloat),
                new WaveFormatExtensible(sampleRate, 24, 2,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 16, 2,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 8, 2,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 32, 1,
                    AudioSubTypes.IeeeFloat),
                new WaveFormatExtensible(sampleRate, 24, 1,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 16, 1,
                    AudioSubTypes.Pcm),
                new WaveFormatExtensible(sampleRate, 8, 1,
                    AudioSubTypes.Pcm)
            };
        }

        private bool CheckForSupportedFormat(AudioClient audioClient, IEnumerable<WaveFormatExtensible> waveFormats,
            out WaveFormat foundMatch)
        {
            foundMatch = null;
            foreach (WaveFormatExtensible format in waveFormats)
            {
                if (audioClient.IsFormatSupported(_shareMode, format))
                {
                    foundMatch = format;
                    return true;
                }
            }
            return false;
        }

        private bool FeedBuffer(AudioRenderClient renderClient, byte[] buffer, int numFramesCount, int frameSize)
        {
            //calculate the number of bytes to "feed"
            int count = numFramesCount * frameSize;
            count -= (count % _source.WaveFormat.BlockAlign);
            //if the driver did not request enough data, return true to continue playback
            if (count <= 0)
                return true;

            //get the requested data
            int read = _source.Read(buffer, 0, count);
            //if the source did not provide enough data, we abort the playback by returning false
            if (read <= 0)
                return false;

            //calculate the number of FRAMES to request
            int actualNumFramesCount = read / frameSize;

            //again there are some special requirements for exclusive mode AND eventsync
            if (_shareMode == AudioClientShareMode.Exclusive && _eventSync &&
                read < count)
            {
                /* The caller can request a packet size that is less than or equal to the amount
                 * of available space in the buffer (except in the case of an exclusive-mode stream
                 * that uses event-driven buffering; for more information, see IAudioClient::Initialize).
                 * see https://msdn.microsoft.com/en-us/library/windows/desktop/dd368243%28v=vs.85%29.aspx - remarks*/

                //since we have to provide exactly the requested number of frames, we clear the rest of the array
                Array.Clear(buffer, read, count - read);
                //set the number of frames to request memory for, to the number of requested frames
                actualNumFramesCount = numFramesCount;
            }

            IntPtr ptr = renderClient.GetBuffer(actualNumFramesCount);

            //we may should introduce a try-finally statement here, but the Marshal.Copy method should not
            //throw any relevant exceptions ... so we should be able to always release the packet
            Marshal.Copy(buffer, 0, ptr, read);
            renderClient.ReleaseBuffer(actualNumFramesCount, AudioClientBufferFlags.None);

            return true;
        }

        private void RaiseStopped(Exception exception)
        {
            EventHandler<PlaybackStoppedEventArgs> handler = Stopped;
            if (handler != null) {
                if (_syncContext != null)
                    //since Send could cause deadlocks better use Post instead
                    _syncContext.Post(x => handler(this, new PlaybackStoppedEventArgs(exception)), null);
                else
                    handler(this, new PlaybackStoppedEventArgs(exception));
            }
        }

        private EventWaitHandle _streamSwitchCompleteEvent;
        private AudioSessionControl _audioSessionControl;
        private MMDeviceEnumerator _deviceEnumerator;
        private WasapiEventHandler _eventHandler;
        private bool _inStreamSwitch;
        private EventWaitHandle _streamSwitchEvent;

        private void InitializeStreamRouting()
        {
            _eventHandler = new WasapiEventHandler(this);

            _audioSessionControl = new AudioSessionControl(_audioClient);
            _deviceEnumerator = new MMDeviceEnumerator();

            if (_streamSwitchCompleteEvent == null)
            {
                _streamSwitchCompleteEvent = new ManualResetEvent(false);
            }

            _audioSessionControl.RegisterAudioSessionNotification(_eventHandler);
            _deviceEnumerator.RegisterEndpointNotificationCallback(_eventHandler);
        }

        private void TerminateStreamRouting()
        {
            if (_audioSessionControl != null)
            {
                _audioSessionControl.UnregisterAudioSessionNotification(_eventHandler);
                CSCoreUtils.SafeRelease(ref _audioSessionControl);
            }

            if (_deviceEnumerator != null)
            {
                _deviceEnumerator.UnregisterEndpointNotificationCallback(_eventHandler);
                CSCoreUtils.SafeRelease(ref _deviceEnumerator);
            }

            if (_streamSwitchCompleteEvent != null)
            {
                _streamSwitchCompleteEvent.Close();
                _streamSwitchCompleteEvent = null;
            }
        }

        private bool HandleStreamSwitchEvent()
        {
            Debug.Assert(_inStreamSwitch);

            //Step 1: Stop rendering
            _audioClient.Stop();

            //Step 3: Wait for the default device to change.
            if (!_streamSwitchCompleteEvent.WaitOne(750))
            {
                Debug.WriteLine("Stream switch timeout - aborting...");
                goto StreamSwitchErrorExit;
            }

            //Step 2: Release resources. 
            _audioSessionControl.UnregisterAudioSessionNotification(_eventHandler);

            CSCoreUtils.SafeRelease(ref _audioSessionControl);
            CleanupResources(true);
            CSCoreUtils.SafeRelease(ref _device);

            //Step 4: Try to get new endpoint
            try
            {
                _deviceEnumerator = _deviceEnumerator ?? new MMDeviceEnumerator();
                _device = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, DeviceRole);
            }
            catch (CoreAudioAPIException exception)
            {
                Debug.WriteLine("Unable to retrieve new default device during stream switch: 0x" + exception.ErrorCode.ToString("x8"));
                goto StreamSwitchErrorExit;
            }

            //Step 5: Re-instantiate the audio client on the new endpoint.
            InitializeInternal();

            _audioClient.Start();

            _inStreamSwitch = false;
            return true;

StreamSwitchErrorExit:
            _inStreamSwitch = false;
            return false;
        }

        private Role DeviceRole
        {
            get { return _deviceRole == DeviceRoleNotSet ? Role.Multimedia : _deviceRole; }
        }

        private bool IsDefaultDevice(MMDevice device, DataFlow dataFlow, Role role)
        {
            var defaultAudioEndpoint = MMDeviceEnumerator.DefaultAudioEndpoint(dataFlow, role);
            if (defaultAudioEndpoint != null && device != null)
            {
                return defaultAudioEndpoint.DeviceID == device.DeviceID;
            }

            return false;
        }
        
        /// <summary>
        /// Updates the stream routing options.
        /// </summary>
        /// <remarks>
        /// If the current <see cref="Device"/> is not the default device, 
        /// the <see cref="SoundOut.StreamRoutingOptions.OnDefaultDeviceChange"/> flag will be removed.
        /// </remarks>
        protected virtual void UpdateStreamRoutingOptions()
        {
            if (!IsDefaultDevice(Device, DataFlow.Render, DeviceRole) &&
                (StreamRoutingOptions & StreamRoutingOptions.OnDefaultDeviceChange) ==
                StreamRoutingOptions.OnDefaultDeviceChange)
            {
                //new device is not the default device 
                //AND OnDefaultDeviceChange is set
                //=> remove OnDefaultDeviceChange flag
                StreamRoutingOptions &= ~StreamRoutingOptions.OnDefaultDeviceChange;
            }
        }

        /// <summary>
        /// Disposes and stops the <see cref="WasapiOut"/> instance.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            CheckForInvalidThreadCall();

            lock (_lockObj)
            {
                if (!_disposed)
                {
                    Debug.WriteLine("Disposing WasapiOut.");
                    Stop();
                    CleanupResources();
                    if (_streamSwitchEvent != null)
                    {
                        _streamSwitchEvent.Close();
                        _streamSwitchEvent = null;
                    }
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WasapiOut"/> class.
        /// </summary>
        ~WasapiOut()
        {
            Dispose(false);
        }

        private class InterruptDisposingChainWaveSource : WaveAggregatorBase
        {
            public InterruptDisposingChainWaveSource(IWaveSource source)
                : base(source)
            {
                if (source == null)
                    throw new ArgumentNullException("source");
                DisposeBaseSource = false;
            }
        }

        private class InterruptDisposingChainSampleSource : SampleAggregatorBase
        {
            public InterruptDisposingChainSampleSource(ISampleSource source)
                : base(source)
            {
                if (source == null)
                    throw new ArgumentNullException("source");
                DisposeBaseSource = false;
            }
        }

        private class WasapiEventHandler : IAudioSessionEvents, IMMNotificationClient
        {
            private readonly WasapiOut _wasapiOut;

            public WasapiEventHandler(WasapiOut wasapiOut)
            {
                _wasapiOut = wasapiOut;
            }

            public void OnDisplayNameChanged(string newDisplayName, ref Guid eventContext)
            {
            }

            public void OnIconPathChanged(string newIconPath, ref Guid eventContext)
            {
            }

            public void OnSimpleVolumeChanged(float newVolume, bool newMute, ref Guid eventContext)
            {
            }

            public void OnChannelVolumeChanged(int channelCount, float[] newChannelVolumeArray, int changedChannel, ref Guid eventContext)
            {
            }

            public void OnGroupingParamChanged(ref Guid newGroupingParam, ref Guid eventContext)
            {
            }

            public void OnStateChanged(AudioSessionState newState)
            {
            }

            public void OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason)
            {
                if (_wasapiOut._streamSwitchEvent != null && !_wasapiOut._inStreamSwitch)
                {
                    if (disconnectReason == AudioSessionDisconnectReason.DisconnectReasonDeviceRemoval)
                    {
                        //check whether OnDeviceDisconnect StreamRoutingOptions is set
                        if ((_wasapiOut.StreamRoutingOptions &
                             StreamRoutingOptions.OnDeviceDisconnect) != StreamRoutingOptions.OnDeviceDisconnect)
                            return;

                        _wasapiOut._inStreamSwitch = true;
                        _wasapiOut._streamSwitchEvent.Set();

                        //if the current device is currently not the default device, 
                        //the OnDefaultDeviceChanged event won't be triggered
                        //=> the streamSwitchCompleteEvent won't be set
                        //=> set it within the OnSessionDisconnected event
                        //otherwise wait for the final DefaultDeviceSwitch
                        if (!_wasapiOut.IsDefaultDevice(_wasapiOut.Device, DataFlow.Render, _wasapiOut.DeviceRole))
                        {
                            _wasapiOut._streamSwitchCompleteEvent.Set();
                        }
                    }
                    if (disconnectReason == AudioSessionDisconnectReason.DisconnectReasonFormatChanged)
                    {
                        if ((_wasapiOut.StreamRoutingOptions &
                             StreamRoutingOptions.OnFormatChange) != StreamRoutingOptions.OnFormatChange)
                            return;

                        _wasapiOut._inStreamSwitch = true;
                        _wasapiOut._streamSwitchEvent.Set();
                        _wasapiOut._streamSwitchCompleteEvent.Set();
                    }
                }
            }

            public void OnDeviceStateChanged(string deviceId, DeviceState deviceState)
            {
            }

            public void OnDeviceAdded(string deviceId)
            {
            }

            public void OnDeviceRemoved(string deviceId)
            {
            }

            public void OnDefaultDeviceChanged(DataFlow dataFlow, Role role, string deviceId)
            {
                bool defaultDeviceSwitingEnabled =
                    (_wasapiOut.StreamRoutingOptions & StreamRoutingOptions.OnDefaultDeviceChange) ==
                    StreamRoutingOptions.OnDefaultDeviceChange;

                if (dataFlow == DataFlow.Render && role == _wasapiOut.DeviceRole)
                {

                    //check whether DefaultDeviceSwitching is enabled
                    if (!_wasapiOut._inStreamSwitch && defaultDeviceSwitingEnabled)
                    {
                        //we're not in stream switch already ... initiate stream switch
                        _wasapiOut._inStreamSwitch = true;
                        _wasapiOut._streamSwitchEvent.Set();
                    }

                    //signal the render thread to re-initialize the audio renderer
                    if (_wasapiOut._inStreamSwitch)
                    {
                        //if defaultDeviceSwitching is not enabled, inStreamSwitch is always false
                        //except the DeviceRemoval event was triggered
                        //for that case, set the streamSwitchCompleteEvent
                        _wasapiOut._streamSwitchCompleteEvent.Set();
                    }
                }
            }

            public void OnPropertyValueChanged(string deviceId, PropertyKey key)
            {
            }
        }
    }

    /// <summary>
    /// Defines options for wasapi-streamrouting.
    /// </summary>
    [Flags]
    public enum StreamRoutingOptions
    {
        /// <summary>
        /// Disable stream routing.
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// Use stream routing when the device format has changed.
        /// </summary>
        OnFormatChange = 1,
        /// <summary>
        /// Use stream routing when the default device has changed.
        /// </summary>
        OnDefaultDeviceChange = 2,
        /// <summary>
        /// Use stream routing when the current device was disconnected.
        /// </summary>
        OnDeviceDisconnect = 4,
        /// <summary>
        /// Combination of <see cref="OnFormatChange"/>, <see cref="OnDefaultDeviceChange"/> and <see cref="OnDeviceDisconnect"/>.
        /// </summary>
        All = OnFormatChange | OnDefaultDeviceChange | OnDeviceDisconnect
    }
}