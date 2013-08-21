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
    public class WasapiCapture : ISoundRecorder
    {
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
        private RecordingState _recordingState;

        private int _latency;
        private bool _eventSync;
        private bool _disposed;

        /// <summary>
        /// </summary>
        /// <param name="eventSync">Don't use this in combination with exclusive mode.</param>
        /// <param name="shareMode">Don't use exclusive mode in combination with eventSync.</param>
        /// <param name="defaultFormat"></param>
        public WasapiCapture(bool eventSync, AudioClientShareMode shareMode, WaveFormat defaultFormat = null)
        {
            _eventSync = eventSync;
            _shareMode = shareMode;
            _waveFormat = defaultFormat;

            _latency = 100;

            _recordingState = SoundIn.RecordingState.Stopped;
        }

        public void Initialize()
        {
            UninitializeAudioClients();

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
            Debug.WriteLine(String.Format("Initialized WasapiCapture[Mode: {0}; Latency: {1}; OutputFormat: {2}]", _shareMode, _latency, _waveFormat));
        }

        public void Start()
        {
            if (RecordingState != SoundIn.RecordingState.Recording)
            {
                if (RecordingState == SoundIn.RecordingState.Stopped && _recordThread == null)
                {
                    _recordThread = new Thread(new ThreadStart(CaptureProc));
                    _recordThread.Name = "WASAPI Capture-Thread; ID = " + DebuggingID;
                    _recordThread.Priority = ThreadPriority.AboveNormal;
                    _recordThread.Start();
                }
            }
        }

        public void Stop()
        {
            if (RecordingState != SoundIn.RecordingState.Stopped)
            {
                _recordingState = SoundIn.RecordingState.Stopped;
                _recordThread.Join();
                _recordThread = null;
            }
        }

        //based on http://msdn.microsoft.com/en-us/library/windows/desktop/dd370800(v=vs.85).aspx
        private void CaptureProc()
        {
            try
            {
                _recordingState = SoundIn.RecordingState.Recording;
                int bufferSize = _audioClient.BufferSize;
                int frameSize = _waveFormat.Channels * _waveFormat.BytesPerSample;

                byte[] buffer = new byte[bufferSize * frameSize];

                int eventWaitHandleIndex = WaitHandle.WaitTimeout;
                WaitHandle[] eventWaitHandleArray = new WaitHandle[] { _eventWaitHandle };

                long actualDuration = (long)((double)REFTIMES_PER_SEC * bufferSize / WaveFormat.SampleRate);
                int actualLatency = (int)(actualDuration / REFTIMES_PER_MILLISEC);
                int sleepDuration = actualLatency / 8;

                _audioClient.Start();

                while (RecordingState != SoundIn.RecordingState.Stopped)
                {
                    if (_eventSync)
                    {
                        eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, actualLatency, false);
                        if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
                            continue;
                    }
                    else
                    {
                        Thread.Sleep(sleepDuration);
                    }

                    if (RecordingState == SoundIn.RecordingState.Recording)
                    {
                        ReadData(buffer, _audioCaptureClient, (uint)frameSize);
                    }
                }

                Thread.Sleep(actualLatency / 2);
                _audioClient.Stop();
                _audioClient.ResetNative();
            }
            catch (Exception e)
            {
                Debug.WriteLine("WasapiCapture::CaptureProc: " + e.ToString());
                if (System.Diagnostics.Debugger.IsAttached)
                    throw new Exception("Unhandled exception in wasapi capture proc.", e);
            }
            finally
            {
                Stop();
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

        private void RaiseDataAvilable(byte[] buffer, int offset, int count)
        {
            if (DataAvailable != null)
                DataAvailable(this, new DataAvailableEventArgs(buffer, offset, count, WaveFormat));
        }

        private void RaiseStopped()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }

        public RecordingState RecordingState
        {
            get { return _recordingState; }
        }

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

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

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
        }

        protected virtual MMDevice GetDefaultDevice()
        {
            return MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Capture, Role.Console);
        }

        protected virtual AudioClientStreamFlags GetStreamFlags()
        {
            return AudioClientStreamFlags.None;
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