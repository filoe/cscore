using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Win32;

namespace CSCore.SoundOut
{
	/// <summary>
	///     Provides audioplayback through Wasapi.
	///     Minimum supported OS: Windows Vista (see <see cref="IsSupportedOnCurrentPlatform" /> property).
	/// </summary>
	public class WasapiOut : ISoundOut
	{
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

				source = new InterruptDisposingChainSource(source);

				_volumeSource = new VolumeSource(source.ToSampleSource());
				_source = _volumeSource.ToWaveSource();
				CleanupResources();
				InitializeInternal();
				_isInitialized = true;
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
			try
			{
				int bufferSize = _audioClient.BufferSize;
				int frameSize = _outputFormat.Channels * _outputFormat.BytesPerSample;

				var buffer = new byte[bufferSize * frameSize];

				WaitHandle[] eventWaitHandleArray = { _eventWaitHandle };

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
				_playbackState = PlaybackState.Playing;

				string mmcssType = Latency > 25 ? "Audio" : "Pro Audio";

				int taskIndex;
				avrtHandle = NativeMethods.AvSetMmThreadCharacteristics(mmcssType, out taskIndex);


				if (playbackStartedEventWaithandle is EventWaitHandle)
				{
					((EventWaitHandle)playbackStartedEventWaithandle).Set();
					playbackStartedEventWaithandle = null;
				}

				bool isAudioClientStopped = false;

				while (PlaybackState != PlaybackState.Stopped)
				{
					//based on the "RenderSharedEventDriven"-Sample: http://msdn.microsoft.com/en-us/library/dd940520(v=vs.85).aspx
					if (_eventSync)
					{
						//3 * latency = see msdn: recommended timeout
						int eventWaitHandleIndex = WaitHandle.WaitAny(eventWaitHandleArray, 3 * _latency, false);
						if (eventWaitHandleIndex == WaitHandle.WaitTimeout)
						{
							//guarantee that the stopped audio client (in exclusive and eventsync mode) can be
							//restarted below
							if(PlaybackState != PlaybackState.Playing && !isAudioClientStopped)
								continue;
						}
					}
					else
					{
						//based on the "RenderSharedTimerDriven"-Sample: http://msdn.microsoft.com/en-us/library/dd940521(v=vs.85).aspx
						Thread.Sleep(_latency / 8);
					}

					if (PlaybackState == PlaybackState.Playing)
					{
						if (isAudioClientStopped)
						{
							//restart the audioclient if it is still paused in exclusive mode
							//belongs to the bugfix described below. http://cscore.codeplex.com/workitem/23
							_audioClient.Start();
							isAudioClientStopped = false;
						}

						int padding;
						if (_eventSync && _shareMode == AudioClientShareMode.Exclusive)
							padding = 0;
						else
							padding = _audioClient.GetCurrentPadding();

						int framesReadyToFill = bufferSize - padding;
						//avoid conversion errors
						/*if (framesReadyToFill > 5 &&
							!(_source is DmoResampler &&
							  ((DmoResampler) _source).OutputToInput(framesReadyToFill * frameSize) <= 0))
						{
							if (!FeedBuffer(_renderClient, buffer, framesReadyToFill, frameSize))
								_playbackState = PlaybackState.Stopped; //TODO: Fire Stopped-event here?
						}*/

						if(framesReadyToFill <= 5 || 
							((_source is DmoResampler) && ((DmoResampler)_source).OutputToInput(framesReadyToFill * frameSize) <= 0))
							continue;

						if(!FeedBuffer(_renderClient, buffer, framesReadyToFill, frameSize))
							_playbackState = PlaybackState.Stopped; //source is eof

					}
					else if(PlaybackState == PlaybackState.Paused && 
							_shareMode == AudioClientShareMode.Exclusive &&
							!isAudioClientStopped)
					{
						//stop the audioclient on paused if the sharemode is set to exclusive
						//otherwise there would be a "repetitive" sound. see http://cscore.codeplex.com/workitem/23
						_audioClient.Stop();
						isAudioClientStopped = true;
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
				//}
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

			_isInitialized = false;
		}

		private WaveFormat SetupWaveFormat(IWaveSource source, AudioClient audioClient)
		{
			WaveFormat waveFormat = source.WaveFormat;
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
							new WaveFormatExtensible(waveFormat.SampleRate, 32, waveFormat.Channels,
								AudioSubTypes.IeeeFloat),
							new WaveFormatExtensible(waveFormat.SampleRate, 24, waveFormat.Channels,
								AudioSubTypes.Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 16, waveFormat.Channels,
								AudioSubTypes.Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 8, waveFormat.Channels,
								AudioSubTypes.Pcm),

							new WaveFormatExtensible(waveFormat.SampleRate, 32, 2,
								AudioSubTypes.IeeeFloat),
							new WaveFormatExtensible(waveFormat.SampleRate, 24, 2,
								AudioSubTypes.Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 16, 2,
								AudioSubTypes.Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 8, 2,
								AudioSubTypes.Pcm),

							new WaveFormatExtensible(waveFormat.SampleRate, 32, 1, 
								AudioSubTypes.IeeeFloat),
							new WaveFormatExtensible(waveFormat.SampleRate, 24, 1, 
								AudioSubTypes.Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 16, 1, 
								AudioSubTypes.Pcm),
							new WaveFormatExtensible(waveFormat.SampleRate, 8, 1, 
								AudioSubTypes.Pcm)
						};

						if (!CheckForSupportedFormat(audioClient, possibleFormats, out mixformat))
						{
							throw new NotSupportedException("Could not find a supported format.");
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
			if (Stopped == null)
				return;

			if (_syncContext != null)
				//since Send could cause deadlocks better use Post instead
				_syncContext.Post(x => Stopped(this, new PlaybackStoppedEventArgs(exception)), null);
			else
				Stopped(this, new PlaybackStoppedEventArgs(exception));
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