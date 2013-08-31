using CSCore.CoreAudioAPI;
using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSCore.SoundOut
{
    public class WasapiOut : ISoundOut
    {
        public static bool IsSupportedOnCurrentPlatform
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        private bool _eventSync = false;

        private MMDevice _device;
        private IWaveSource _source;
        private AudioClient _audioClient;
        private AudioRenderClient _renderClient;
        private AudioClientShareMode _shareMode = AudioClientShareMode.Shared;

        private WaveFormat _outputFormat;

        private int _latency;
        private PlaybackState _playbackState;

        private Thread _playbackThread;
        private EventWaitHandle _eventWaitHandle;
        private SimpleAudioVolume _simpleAudioVolume;

        public event EventHandler Stopped;

        /// <summary>
        /// EventSync = False; Shared; 100ms Latency
        /// </summary>
        public WasapiOut()
            : this(false, AudioClientShareMode.Shared, 100) //100 ms default
        {
        }

        public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency)
        {
            if (!IsSupportedOnCurrentPlatform)
                throw new PlatformNotSupportedException("Supported since Windows Vista");

            if (latency <= 0)
                throw new ArgumentOutOfRangeException("latency");

            _shareMode = shareMode;
            _latency = latency;
            _eventSync = eventSync;
        }

        public void Initialize(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            _source = source;

            UninitializeAudioClients();

            _audioClient = AudioClient.FromMMDevice(Device);
            _outputFormat = SetupWaveFormat(source.WaveFormat, _audioClient);

            long latency = _latency * 10000;

            if (!_eventSync)
            {
                _audioClient.Initialize(_shareMode, AudioClientStreamFlags.None, latency, 0, _outputFormat, Guid.Empty);
            }
            else //event sync
            {
                if (_shareMode == AudioClientShareMode.Exclusive) //exclusive
                {
                    _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlags_EventCallback, latency, latency, _outputFormat, Guid.Empty);
                }
                else //shared
                {
                    _audioClient.Initialize(_shareMode, AudioClientStreamFlags.StreamFlags_EventCallback, 0, 0, _outputFormat, Guid.Empty);
                    _latency = (int)(_audioClient.StreamLatency / 10000);
                }

                _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                _audioClient.SetEventHandle(_eventWaitHandle.SafeWaitHandle.DangerousGetHandle());
            }

            _renderClient = AudioRenderClient.FromAudioClient(_audioClient);
            _simpleAudioVolume = SimpleAudioVolume.FromAudioClient(_audioClient);
            Debug.WriteLine(String.Format("Initialized WasapiOut[Mode: {0}; Latency: {1}; OutputFormat: {2}]", _shareMode, _latency, _outputFormat));
        }

        public void Play()
        {
            if (PlaybackState != PlaybackState.Playing)
            {
                if (PlaybackState == SoundOut.PlaybackState.Stopped && _playbackThread == null)
                {
                    _playbackThread = new Thread(new ThreadStart(PlaybackProc));
                    _playbackThread.Name = "WASAPI Playback-Thread; ID = " + DebuggingID;
                    _playbackThread.Priority = ThreadPriority.AboveNormal;
                    _playbackThread.Start();
                }
                else if (PlaybackState == SoundOut.PlaybackState.Paused)
                {
                    _playbackState = SoundOut.PlaybackState.Playing;
                }
            }
        }

        public void Pause()
        {
            if (PlaybackState == SoundOut.PlaybackState.Playing)
                _playbackState = SoundOut.PlaybackState.Paused;
        }

        public void Resume()
        {
            Play();
        }

        public void Stop()
        {
            if (PlaybackState != SoundOut.PlaybackState.Stopped)
            {
                _playbackState = SoundOut.PlaybackState.Stopped;
                _playbackThread.Join();
                _playbackThread = null;
            }
        }

        private void PlaybackProc()
        {
            try
            {
                _playbackState = SoundOut.PlaybackState.Playing;

                int bufferSize = _audioClient.BufferSize;
                int frameSize = _outputFormat.Channels * _outputFormat.BytesPerSample;

                byte[] buffer = new byte[bufferSize * frameSize];

                int eventWaitHandleIndex = WaitHandle.WaitTimeout;
                WaitHandle[] eventWaitHandleArray = new WaitHandle[] { _eventWaitHandle };

                if (!FeedBuffer(_renderClient, buffer, bufferSize, frameSize))
                {
                    _playbackState = PlaybackState.Stopped;
                }

                _audioClient.Start();

                while (PlaybackState != PlaybackState.Stopped)
                {
                    if (_eventSync)
                    {
                        eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, 3 * _latency, false); //3 * latency = see msdn: recommended timeout
                        if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
                            continue;
                    }
                    else
                    {
                        //Thread.Sleep(_latency / 8);
                        Thread.Sleep(_latency / 8);
                    }

                    if (PlaybackState == PlaybackState.Playing)
                    {
                        int padding;
                        if (_eventSync && _shareMode == AudioClientShareMode.Exclusive)
                        {
                            padding = 0;
                        }
                        else
                        {
                            padding = _audioClient.GetCurrentPadding();
                        }

                        int framesReadyToFill = bufferSize - padding;
                        if (framesReadyToFill > 5 && 
                            !(_source is DmoResampler && 
                            ((DmoResampler)_source).OutputToInput(framesReadyToFill * frameSize) <= 0)) //avoid conversion errors
                        {
                            if (!FeedBuffer(_renderClient, buffer, framesReadyToFill, frameSize))
                            {
                                _playbackState = PlaybackState.Stopped;
                            }
                        }
                    }
                }

                Thread.Sleep(_latency / 2);
                _audioClient.Stop();
                if (_playbackState == SoundOut.PlaybackState.Stopped)
                {
                    _audioClient.ResetNative();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("WasapiOut::PlaybackProc: " + e.ToString(), "WasapiOut.PlaybackProc");
                if (System.Diagnostics.Debugger.IsAttached)
                    throw new Exception("Unhandled exception in wasapi playback proc. See innerexception for details.", e);
            }
            finally
            {
                Stop();
                //_playbackThread = null;
                RaiseStopped();
            }
        }

        private bool FeedBuffer(AudioRenderClient renderClient, byte[] buffer, int numFramesCount, int frameSize)
        {
            int count = numFramesCount * frameSize;
            count -= (count % _source.WaveFormat.BlockAlign);
            if (count <= 0)
                return true;

            var ptr = renderClient.GetBuffer(numFramesCount);

            int read = _source.Read(buffer, 0, count);

            Marshal.Copy(buffer, 0, ptr, read);

            renderClient.ReleaseBuffer((int)(read / frameSize), AudioClientBufferFlags.None);

            return read > 0;
        }

        private void RaiseStopped()
        {
            if (Stopped != null)
                Stopped(this, new EventArgs());
        }

        public float Volume
        {
            get
            {
                return _simpleAudioVolume.MasterVolume;
            }
            set
            {
                _simpleAudioVolume.MasterVolume = value;
            }
        }

        public IWaveSource WaveSource
        {
            get { return _source; }
        }

        public PlaybackState PlaybackState
        {
            get { return _playbackState; }
        }

        public MMDevice Device
        {
            get
            {
                return _device ?? (_device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Multimedia));
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                //if (_device != null)
                //	_device.Dispose();
                _device = value;
            }
        }

        public long DebuggingID
        {
            get { return _audioClient != null ? _audioClient.BasePtr.ToInt64() : -1; }
        }

        public AudioClientShareMode ShareMode
        {
            get { return _shareMode; }
        }

        public int Latency
        {
            get { return _latency; }
        }

        public bool EventSync
        {
            get { return _eventSync; }
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
							new WaveFormatExtensible(waveFormat.SampleRate, 32, waveFormat.Channels, DMO.MediaTypes.MEDIASUBTYPE_IEEE_FLOAT),
							new WaveFormatExtensible(waveFormat.SampleRate, 24, waveFormat.Channels, DMO.MediaTypes.MEDIASUBTYPE_PCM),
							new WaveFormatExtensible(waveFormat.SampleRate, 16, waveFormat.Channels, DMO.MediaTypes.MEDIASUBTYPE_PCM),
							new WaveFormatExtensible(waveFormat.SampleRate, 8,  waveFormat.Channels, DMO.MediaTypes.MEDIASUBTYPE_PCM)
						};

                        if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                        {
                            //no format found...
                            possibleFormats = new WaveFormatExtensible[]
							{
								new WaveFormatExtensible(waveFormat.SampleRate, 32, 2, DMO.MediaTypes.MEDIASUBTYPE_IEEE_FLOAT),
								new WaveFormatExtensible(waveFormat.SampleRate, 24, 2, DMO.MediaTypes.MEDIASUBTYPE_PCM),
								new WaveFormatExtensible(waveFormat.SampleRate, 16, 2, DMO.MediaTypes.MEDIASUBTYPE_PCM),
								new WaveFormatExtensible(waveFormat.SampleRate, 8,  2, DMO.MediaTypes.MEDIASUBTYPE_PCM),
								new WaveFormatExtensible(waveFormat.SampleRate, 32, 1, DMO.MediaTypes.MEDIASUBTYPE_IEEE_FLOAT),
								new WaveFormatExtensible(waveFormat.SampleRate, 24, 1, DMO.MediaTypes.MEDIASUBTYPE_PCM),
								new WaveFormatExtensible(waveFormat.SampleRate, 16, 1, DMO.MediaTypes.MEDIASUBTYPE_PCM),
								new WaveFormatExtensible(waveFormat.SampleRate, 8,  1, DMO.MediaTypes.MEDIASUBTYPE_PCM)
							};

                            if (CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
                            {
                                throw new NotSupportedException("Could not find a supported format.");
                            }
                        }
                    }

                    finalFormat = mixformat;
                    //todo: implement channel matrix
                    DmoResampler resampler = new DmoResampler(_source, finalFormat);
                    resampler.Quality = 60;
                    _source = resampler;
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
            if (_renderClient != null)
            {
                _renderClient.Dispose();
                _renderClient = null;
            }
            if (_simpleAudioVolume != null)
            {
                _simpleAudioVolume.Dispose();
                _simpleAudioVolume = null;
            }
        }

        private bool _disposed;

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

        ~WasapiOut()
        {
            System.Diagnostics.Debug.Assert(false, "WasapiOut was not disposed correctly.");
            Dispose(false);
        }
    }
}