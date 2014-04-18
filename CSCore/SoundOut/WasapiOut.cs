using CSCore.CoreAudioAPI;
using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CSCore.SoundOut
{
	/// <summary>
	/// Provides audioplayback through Wasapi.
	/// Minimum supported OS: Windows Vista (see IsSupportedOnCurrentPlatform property).
	/// </summary>
	public class WasapiOut : ISoundOut
	{
		private IWaveSource _source;
		private volatile PlaybackState _playbackState;
		private bool _isInitialized;

		private int _latency;
		private readonly bool _eventSync;
		private bool _createdResampler;
		private readonly AudioClientShareMode _shareMode;
		private AudioClient _audioClient;
		private WaveFormat _outputFormat;
		private EventWaitHandle _eventWaitHandle;
		private AudioRenderClient _renderClient;
		private SimpleAudioVolume _simpleAudioVolume;
		private MMDevice _device;

		private Thread _playbackThread;
		private readonly SynchronizationContext _syncContext;
		private readonly ThreadPriority _playbackThreadPriority;

		/// <summary>
		/// Thrown whenever Stop is called or the source goes end of stream.
		/// </summary>
		public event EventHandler Stopped;

		/// <summary>
		/// Gets whether Wasapi is supported on the current Platform.
		/// </summary>
		public static bool IsSupportedOnCurrentPlatform
		{
			get { return Environment.OSVersion.Version.Major >= 6; }
		}

		/// <summary>
		/// Creates a new WasapiOut instance. 
		/// EventSyncContext = SynchronizationContext.Current. 
		/// PlaybackThreadPriority = AboveNormal. 
		/// Latency = 100ms. 
		/// EventSync = False. 
		/// ShareMode = Shared. 
		/// </summary>
		public WasapiOut()
			: this(false, AudioClientShareMode.Shared, 100) //100 ms default
		{
		}

		/// <summary>
		/// Creates a new WasapiOut instance. 
		/// EventSyncContext = SynchronizationContext.Current. 
		/// PlaybackThreadPriority = AboveNormal.
		/// </summary>
		/// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
		/// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, only one single playback for the specified device is possible at once.</param>
		/// <param name="latency">Latency of the playback specified in milliseconds.</param>
		public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency)
			: this(eventSync, shareMode, 100, ThreadPriority.AboveNormal)
		{
		}

		/// <summary>
		/// Creates a new WasapiOut instance. 
		/// EventSyncContext = SynchronizationContext.Current.
		/// </summary>
		/// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
		/// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, only one single playback for the specified device is possible at once.</param>
		/// <param name="latency">Latency of the playback specified in milliseconds.</param>
		/// <param name="playbackThreadPriority">ThreadPriority of the playbackthread which runs in background and feeds the device with data.</param>
		public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency, ThreadPriority playbackThreadPriority)
			: this(eventSync, shareMode, latency, playbackThreadPriority, SynchronizationContext.Current)
		{
		}

		/// <summary>
		/// Creates a new WasapiOut instance.
		/// </summary>
		/// <param name="eventSync">True, to use eventsynchronization instead of a simple loop and sleep behavior.</param>
		/// <param name="shareMode">Specifies how to open the audio device. Note that if exclusive mode is used, only one single playback for the specified device is possible at once.</param>
		/// <param name="latency">Latency of the playback specified in milliseconds.</param>
		/// <param name="playbackThreadPriority">ThreadPriority of the playbackthread which runs in background and feeds the device with data.</param>
		/// <param name="eventSyncContext">The synchronizationcontext which is used to raise any events like the "Stopped"-event. If the passed value is not null, the events will be called async through the SynchronizationContext.Post() method.</param>
		public WasapiOut(bool eventSync, AudioClientShareMode shareMode, int latency, ThreadPriority playbackThreadPriority, SynchronizationContext eventSyncContext)
		{
			if (!IsSupportedOnCurrentPlatform)
				throw new PlatformNotSupportedException("Wasapi is only supported on Windows Vista and above.");

			if (latency <= 0)
				throw new ArgumentOutOfRangeException("latency");

			_latency = latency;
			_shareMode = shareMode;
			_eventSync = eventSync;
			_playbackThreadPriority = playbackThreadPriority;
			_syncContext = SynchronizationContext.Current;
		}

		/// <summary>
		/// Initializes WasapiOut and prepares all resources for playback.
		/// Note that properties like Device, Latency,... won't affect WasapiOut after calling Initialize.
		/// </summary>
		/// <param name="source">The source to prepare for playback.</param>
		public void Initialize(IWaveSource source)
		{
			CheckForDisposed();
			CheckForInvalidThreadCall();

			if (source == null)
				throw new ArgumentNullException("source");

			if (_playbackState != PlaybackState.Stopped)
				throw new InvalidOperationException("PlaybackState has to be Stopped. Call WasapiOut::Stop to stop the playback.");

			_playbackThread.WaitForExit();

			//if (_isInitialized)
			//    throw new InvalidOperationException("Wasapi is already initialized. Call WasapiOut::Stop to uninitialize Wasapi.");

			_source = source;
			CleanupResources();
			InitializeInternal();
			_isInitialized = true;
		}

		/// <summary>
		/// Starts the playback.
		/// Note: Initialize has to get called before calling Play. 
		/// If PlaybackState is Paused, Resume() will be called automatically. 
		/// </summary>
		public void Play()
		{
			CheckForDisposed();
			CheckForInvalidThreadCall();
			CheckForIsInitialized();

			if (PlaybackState == SoundOut.PlaybackState.Stopped)
			{
				using (var waitHandle = new AutoResetEvent(false))
				{
					_playbackThread.WaitForExit(); //just to be sure that the thread finished already. Should not be necessary because after Stop(), Initialize() has to be called which already waits until the playbackthread stopped.
					_playbackThread = new Thread(new ParameterizedThreadStart(PlaybackProc))
					{
						Name = "WASAPI Playback-Thread; ID = " + DebuggingID,
						Priority = _playbackThreadPriority
					};

					_playbackThread.Start(waitHandle);
					waitHandle.WaitOne();
				}
			}
			else if (PlaybackState == SoundOut.PlaybackState.Paused)
			{
				Resume();
			}
			else
			{
				//do nothing :)
			}
		}

		/// <summary>
		/// Stops the playback and frees all allocated resources. 
		/// After calling the caller has to call Initialize again before another playback can be started.
		/// </summary>
		public void Stop()
		{
			CheckForDisposed();
			CheckForInvalidThreadCall();

			if (_playbackState != SoundOut.PlaybackState.Stopped && _playbackThread != null)
			{
				_playbackState = SoundOut.PlaybackState.Stopped;
				_playbackThread.WaitForExit(); //possible deadlock
				_playbackThread = null;
			}
			else if (_playbackState == SoundOut.PlaybackState.Stopped && _playbackThread != null)
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
			{
				Debug.WriteLine("Wasapi is already stopped.");
			}
		}

		/// <summary>
		/// Resumes the paused playback.
		/// </summary>
		public void Resume()
		{
			CheckForDisposed();
			CheckForInvalidThreadCall();
			CheckForIsInitialized();

			if (_playbackState == SoundOut.PlaybackState.Paused)
			{
				_playbackState = SoundOut.PlaybackState.Playing;
			}
		}

		/// <summary>
		/// Pauses the playback.
		/// </summary>
		public void Pause()
		{
			CheckForDisposed();
			CheckForInvalidThreadCall();

			if (PlaybackState == SoundOut.PlaybackState.Playing)
			{
				_playbackState = SoundOut.PlaybackState.Paused;
			}
			else
			{
				//do nothing :)
			}
		}

		private void PlaybackProc(object playbackStartedEventWaithandle)
		{
			try
			{
				int bufferSize;
				int frameSize;
				byte[] buffer;
				int eventWaitHandleIndex;
				WaitHandle[] eventWaitHandleArray;

				bufferSize = _audioClient.BufferSize;
				frameSize = _outputFormat.Channels * _outputFormat.BytesPerSample;

				buffer = new byte[bufferSize * frameSize];

				eventWaitHandleIndex = WaitHandle.WaitTimeout;
				eventWaitHandleArray = new WaitHandle[] { _eventWaitHandle };

                //001
				/*if (!FeedBuffer(_renderClient, buffer, bufferSize, frameSize)) //todo: might cause a deadlock: play() is waiting on eventhandle but FeedBuffer got already called
				{
					_playbackState = PlaybackState.Stopped;
					if (playbackStartedEventWaithandle is EventWaitHandle)
					{
						((EventWaitHandle)playbackStartedEventWaithandle).Set();
						playbackStartedEventWaithandle = null;
					}
				}
				else
				{*/
					_audioClient.Start();
					_playbackState = SoundOut.PlaybackState.Playing;

					if (playbackStartedEventWaithandle is EventWaitHandle) 
					{
						((EventWaitHandle)playbackStartedEventWaithandle).Set();
						playbackStartedEventWaithandle = null;
					}

					while (PlaybackState != PlaybackState.Stopped)
					{
						if (_eventSync) //based on the "RenderSharedEventDriven"-Sample: http://msdn.microsoft.com/en-us/library/dd940520(v=vs.85).aspx
						{
							eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, 3 * _latency, false);
							//3 * latency = see msdn: recommended timeout
							if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
								continue;
						}
						else //based on the "RenderSharedTimerDriven"-Sample: http://msdn.microsoft.com/en-us/library/dd940521(v=vs.85).aspx
						{
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
									_playbackState = PlaybackState.Stopped; //TODO: Fire Stopped-event here?
								}
							}
						}
					}

					Thread.Sleep(_latency / 2);

					_audioClient.Stop();
					_audioClient.Reset();
				//}
			}
			finally
			{
				//CleanupResources();
				if (playbackStartedEventWaithandle is EventWaitHandle)
					((EventWaitHandle)playbackStartedEventWaithandle).Set();
				RaiseStopped();
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
			_audioClient = AudioClient.FromMMDevice(Device);
			_outputFormat = SetupWaveFormat(_source.WaveFormat, _audioClient);

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
			_simpleAudioVolume.MasterVolume = 1f;
		}

		private void CleanupResources()
		{
			if (_createdResampler && _source is DmoResampler)
			{
				((DmoResampler)_source).DisposeResamplerOnly();
				_source = null;
			}

			if (_renderClient != null)
			{
				_renderClient.Dispose();
				_renderClient = null;
			}
			if (_audioClient != null)
			{
				try
				{
					_audioClient.Reset();
				}
				catch (CoreAudioAPIException ex)
				{
					if (ex.ErrorCode != unchecked((int)0x88890001)) //AUDCLNT_E_NOT_INITIALIZED
					{
						throw;
					}
				}
				_audioClient.Dispose();
				_audioClient = null;
			}
			if (_simpleAudioVolume != null)
			{
				_simpleAudioVolume.Dispose();
				_simpleAudioVolume = null;
			}
			if (_eventWaitHandle != null)
			{
				_eventWaitHandle.Close();
				_eventWaitHandle = null;
			}

			_isInitialized = false;
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
					//todo: implement channel matrix
					DmoResampler resampler = new DmoResampler(_source, finalFormat);
					resampler.Quality = 60;
					_source = resampler;
					_createdResampler = true;
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

		private bool FeedBuffer(AudioRenderClient renderClient, byte[] buffer, int numFramesCount, int frameSize)
		{
			int count = numFramesCount * frameSize;
			count -= (count % _source.WaveFormat.BlockAlign);
			if (count <= 0)
				return true;

			int read = _source.Read(buffer, 0, count);

			var ptr = renderClient.GetBuffer(numFramesCount);
			Marshal.Copy(buffer, 0, ptr, read);
			renderClient.ReleaseBuffer((int)(read / frameSize), AudioClientBufferFlags.None);

			return read > 0;
		}

		private void RaiseStopped()
		{
			if (Stopped == null)
				return;

			if (_syncContext != null)
				_syncContext.Post(x => Stopped(this, EventArgs.Empty), null); //maybe post?
			else
				Stopped(this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets or sets the Device which should be used for playback. 
		/// The Device property has to be set before initializing. The systems default playback device is used as default value of the Device property.
		/// </summary>
		/// <remarks>
		/// Be sure to set only activated render devices.
		/// </remarks>
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
				_device = value;
			}
		}

		/// <summary>
		/// Gets the current PlaybackState of the playback.
		/// </summary>
		public PlaybackState PlaybackState
		{
			get { return _playbackState; }
		}

		/// <summary>
		/// Random ID based on internal audioclients memory address for debugging purposes. 
		/// </summary>
		public long DebuggingID
		{
			get { return _audioClient != null ? _audioClient.BasePtr.ToInt64() : -1; }
		}

		/// <summary>
		/// Gets or sets the volume of the playback. 
		/// Valid values are in the range from 0.0 to 1.0. 1.0 equals 100%. 
		/// </summary>
		public float Volume
		{
			get
			{
				return _simpleAudioVolume != null ? _simpleAudioVolume.MasterVolume : 1;
			}
			set
			{
				CheckForDisposed();
				if (_simpleAudioVolume != null)
					_simpleAudioVolume.MasterVolume = value;
			}
		}

		/// <summary>
		/// Latency of the playback specified in milliseconds.
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
		/// The currently initialized source.
		/// To change the WaveSource property, call Initialize().
		/// </summary>
		/// <remarks>
		/// The value of the WaveSource might not be the value which was passed to the Initialize method, because
		/// WasapiOut (depending on the waveformat of the source) has to use a DmoResampler.
		/// </remarks>
		public IWaveSource WaveSource
		{
			get { return _source; }
		}

		private bool _disposed;

		/// <summary>
		/// Stops the playback (if playing) and cleans up all used resources. 
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
				Debug.WriteLine("Disposing WasapiOut.");
				Stop();
				CleanupResources();
			}
			_disposed = true;
		}

		~WasapiOut()
		{
			Dispose(false);
		}
	}
}
