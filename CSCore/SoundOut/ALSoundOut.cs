using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CSCore.SoundOut.AL;
using CSCore.Streams;

namespace CSCore.SoundOut
{
	/// <summary>
	///     Provides audioplayback through OpenAL.
	/// </summary>
	/// <remarks>
	/// This SoundOut provider runs on multiple platforms. 
	/// But since the OpenAL implementation on Windows platforms, has some different
	/// handling what context switching concerns, it is not possible to play sounds on multiple 
	/// devices at once through OpenAL. 
	/// </remarks>
	// ReSharper disable once InconsistentNaming
	public class ALSoundOut : ISoundOut
	{
		private const int NumberOfBuffers = 4;
		private readonly object _lockObj = new object();
		private readonly ThreadPriority _playbackPriority;
		private readonly SynchronizationContext _syncContext;
		private ALSource _alSource;
		private uint[] _buffers;
		private int _bufferSize;

		private ALDevice _device;
		private bool _disposed;
		private bool _isInitialized;
		private int _latency;
		private ALFormat _playbackFormat;
		private PlaybackState _playbackState;

		private Thread _playbackThread;
		private ALDevice _playingDevice;
		private IWaveSource _source;
		private VolumeSource _volumeSource;
		private ALContext _context;

		/// <summary>
		///     Initializes a new instance of the <see cref="ALSoundOut" /> class.
		/// </summary>
		public ALSoundOut()
			: this(50)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ALSoundOut" /> class with a initial latency.
		/// </summary>
		/// <param name="latency">The playback latency in milliseconds.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">latency</exception>
		public ALSoundOut(int latency)
			: this(latency, ThreadPriority.AboveNormal)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ALSoundOut" /> class with a initial latency
		///     and <see cref="ThreadPriority" /> of the playback thread.
		/// </summary>
		/// <param name="latency">The playback latency in milliseconds.</param>
		/// <param name="playbackThreadPriority">The <see cref="ThreadPriority" /> of the playback thread.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">latency</exception>
		public ALSoundOut(int latency, ThreadPriority playbackThreadPriority)
			: this(latency, playbackThreadPriority, SynchronizationContext.Current)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="ALSoundOut" /> class based on a initial latency,
		///     the <see cref="ThreadPriority" /> of the playback thread and the <see cref="SynchronizationContext" /> used to
		///     raise events.
		/// </summary>
		/// <param name="latency">The playback latency in milliseconds.</param>
		/// <param name="playbackThreadPriority">The <see cref="ThreadPriority" /> of the playback thread.</param>
		/// <param name="eventSyncContext">
		///     The <see cref="SynchronizationContext" /> which is used to raise any events like the <see cref="Stopped" />-event.
		///     If the passed value is not null, the events will be called async through the
		///     <see cref="SynchronizationContext.Post" /> method.
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">latency</exception>
		public ALSoundOut(int latency, ThreadPriority playbackThreadPriority, SynchronizationContext eventSyncContext)
		{
			if (latency <= 0)
				throw new ArgumentOutOfRangeException("latency");

			_latency = latency;
			_playbackPriority = playbackThreadPriority;
			_syncContext = eventSyncContext;

            if (!ALInterops.IsSupported())
            {
                throw new PlatformNotSupportedException("openAL is not supported by the current platform. Consider installing openAL on the current platform.");
            }
		}

		/// <summary>
		///     Gets or sets the <see cref="Device" /> which should be used for playback.
		///     The <see cref="Device" /> property has to be set before initializing.
		///     The systems default playback device is used as default
		///     value of the <see cref="Device" /> property.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">value is less than one</exception>
		public ALDevice Device
		{
			get { return _device ?? (_device = ALDevice.DefaultDevice); }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				lock (_lockObj)
				{
					_device = value;
				}
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
					throw new ArgumentOutOfRangeException("value");
				lock (_lockObj)
				{
					_latency = value;
				}
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
		///     Gets the <see cref="IWaveSource" /> which provides
		///     the waveform-audio data and was used to <see cref="Initialize" />
		///     the <see cref="ALSoundOut" /> instance.
		/// </summary>
		public IWaveSource WaveSource
		{
			get { return _source; }
		}

		/// <summary>
		///     Gets the <see cref="SoundOut.PlaybackState" />.
		///     The playback state indicates whether the playback is currently playing, paused or stopped.
		/// </summary>
		public PlaybackState PlaybackState
		{
			get { return _playbackState; }
		}

		/// <summary>
		///     Gets the Context used for the playback.
		/// </summary>
		protected ALContext Context
		{
			get { return _context; }
		}

		/// <summary>
		///     Occurs when the playback stops.
		/// </summary>
		public event EventHandler<PlaybackStoppedEventArgs> Stopped;

		/// <summary>
		///     Starts the playback.
		///     Note: The <see cref="Initialize" /> method has to get called before calling <see cref="Play" />.
		///     If the <see cref="PlaybackState" /> is <see cref="CSCore.SoundOut.PlaybackState.Paused" />, the
		///     <see cref="Resume" />
		///     will be called automatically.
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
					using (var waitHandle = new ManualResetEvent(false))
					{
						_playbackThread.WaitForExit();
						_playbackThread = new Thread(PlaybackProc)
						{
							Name = "OpenAL Playback-Thread",
							Priority = _playbackPriority
						};

						_playbackThread.Start(waitHandle);
						waitHandle.WaitOne();
					}
				}
				else if (PlaybackState == PlaybackState.Paused)
					Resume();
			}
		}

		/// <summary>
		///     Pauses the audio playback.
		/// </summary>
		public void Pause()
		{
			CheckForInvalidThreadCall();

			lock (_lockObj)
			{
				CheckForDisposed();
				CheckForIsInitialized();

				if (PlaybackState == PlaybackState.Playing)
				{
					_alSource.Pause();

					_playbackState = PlaybackState.Paused;
				}
			}
		}

		/// <summary>
		///     Resumes the audio playback.
		/// </summary>
		public void Resume()
		{
			CheckForInvalidThreadCall();

			lock (_lockObj)
			{
				CheckForDisposed();
				CheckForIsInitialized();

				if (PlaybackState == PlaybackState.Paused)
				{
					_alSource.Play();

					_playbackState = PlaybackState.Playing;
				}
			}
		}

		/// <summary>
		///     Stops the audio playback and releases most of allocated resources.
		/// </summary>
		public void Stop()
		{
			CheckForInvalidThreadCall();

			lock (_lockObj)
			{
				CheckForDisposed();
				//don't check for isinitialized here (we don't want the Dispose method to throw an exception)

				if (PlaybackState != PlaybackState.Stopped)
				{
					if (_alSource != null)
						_alSource.Stop();

					_playbackState = PlaybackState.Stopped;
				}

				if (_playbackThread != null)
				{
					/*
                    * On EOF playbackstate is Stopped, but thread is not stopped. => 
                    * New Session can be started while cleaning up old one => unknown behavior. =>
                    * Always call Stop() to make sure, you wait until the thread is finished cleaning up.
                    */
					_playbackThread.WaitForExit();
					_playbackThread = null;
				}
			}
		}

		/// <summary>
		///     Initializes the <see cref="ALSoundOut" /> instance for playing a <paramref name="source" />.
		/// </summary>
		/// <param name="source"><see cref="IWaveSource" /> which provides waveform-audio data to play.</param>
		/// <exception cref="System.ArgumentNullException">source</exception>
		/// <exception cref="System.InvalidOperationException">
		///     <see cref="PlaybackState" /> is not
		///     <see cref="SoundOut.PlaybackState.Stopped" />.
		/// </exception>
		public void Initialize(IWaveSource source)
		{
			CheckForInvalidThreadCall();

			lock (_lockObj)
			{
				CheckForDisposed();

				if (source == null)
					throw new ArgumentNullException("source");

				source = new InterruptDisposingChainSource(source);
				if (PlaybackState != PlaybackState.Stopped)
				{
					throw new InvalidOperationException(
						"PlaybackState has to be Stopped. Call ALSoundOut.Stop to stop the playback.");
				}

				//wait for the playbackthread to finish
				_playbackThread.WaitForExit();
				//after the playbackthread finished, release the resources 
				CleanupResources();
				//start creating new resources including new context and so on.
				_playingDevice = Device;
				_context = new ALContext(_playingDevice);

				source = new InterruptDisposingChainSource(source);
				_volumeSource = new VolumeSource(source.ToSampleSource());

				int numberOfBitsPerSample = FindBestBitDepth(source.WaveFormat);
				_source = _volumeSource.ToWaveSource(numberOfBitsPerSample);

				InitializeInternal();

				_isInitialized = true;
			}
		}

		/// <summary>
		///     Stops the playback (if playing) and releases all allocated resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void PlaybackProc(object args)
		{
			Exception exception = null;
			EventWaitHandle waitHandle = args as EventWaitHandle;
			IList<BufferedAudioData> byteBuffers;
			uint[] unqueuedBuffers;

			using (Context.LockContext())
			{
				//if we run eof, and we did not call initialize, the buffers are still queued
				//make sure the buffers are unququed before trying to fill them
				if (_alSource.BuffersQueued == 0 && _alSource.BuffersProcessed == 0)
				{
					unqueuedBuffers = _buffers;
				}
				else
				{
					while ((unqueuedBuffers = _alSource.UnqueueBuffers(_alSource.BuffersProcessed)).Length <= 0)
					{
						Thread.Sleep(Latency / 5);
					}
				}
			}

			if ((byteBuffers = GetBufferedData(unqueuedBuffers.Length)).Count <= 0)
			{
				_playbackState = PlaybackState.Stopped;
			}
			else
			{
				using (Context.LockContext())
				{

					FillBuffers(unqueuedBuffers, byteBuffers);
					_alSource.Play();

					_playbackState = PlaybackState.Playing;
					if (waitHandle != null)
					{
						waitHandle.Set();
						waitHandle = null;
					}
				}
			}

			try
			{
				while (PlaybackState != PlaybackState.Stopped)
				{
					if (PlaybackState == PlaybackState.Paused)
					{
						Thread.Sleep(Latency / 5);
						continue;
					}

					//locks and unlocks context!
					int numberOfProcessedBuffers = _alSource.BuffersProcessed;
					if (numberOfProcessedBuffers == 0)
					{
						Thread.Sleep(Latency / 5);
						continue;
					}

					if ((byteBuffers = GetBufferedData(numberOfProcessedBuffers)).Count <= 0)
					{
						_playbackState = PlaybackState.Stopped;
					}
					else
					{
						using (Context.LockContext())
						{
							unqueuedBuffers = _alSource.UnqueueBuffers(numberOfProcessedBuffers);
							FillBuffers(unqueuedBuffers, byteBuffers);

							//locks and unlocks context!
							if (_alSource.SourceState == ALSourceState.Stopped)
								_alSource.Play();
						}
					}
				}
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				_playbackState = PlaybackState.Stopped;

				if (waitHandle != null)
					waitHandle.Set();

				RaiseStopped(exception);
			}
		}

		private void RaiseStopped(Exception exception)
		{
			EventHandler<PlaybackStoppedEventArgs> handler = Stopped;
			if (handler != null)
			{
				if (_syncContext != null)
					_syncContext.Post(x => handler(this, new PlaybackStoppedEventArgs(exception)), null);
				else
					handler(this, new PlaybackStoppedEventArgs(exception));
			}
		}

		private void InitializeInternal()
		{
			using (Context.LockContext())
			{
				_playbackFormat = FindALFormat(_source.WaveFormat);
				_alSource = new ALSource(Context);

				_buffers = new uint[NumberOfBuffers];
				ALException.Try(
					() =>
					ALInterops.alGenBuffers(_buffers.Length, _buffers),
					"alGenBuffers");
			}
			_bufferSize = (int)_source.WaveFormat.MillisecondsToBytes(_latency);
		}

		private void CleanupResources()
		{
			if (!_isInitialized)
				return;

			if (_alSource != null)
			{
				using (Context.LockContext())
				{
					int numberOfProcessedBuffers = _alSource.BuffersProcessed;
					if (numberOfProcessedBuffers > 0)
					{
						//sometimes there are duplicates on window??
						var finishedBuffers = _alSource.UnqueueBuffers(numberOfProcessedBuffers).Distinct().ToArray();
						ALException.Try(
							() =>
							ALInterops.alDeleteBuffers(finishedBuffers.Length, finishedBuffers),
							"alDeleteBuffers");
					}

					_alSource.Dispose();
					_alSource = null;
				}
			}

			if (Context != null)
			{
				Context.Dispose();
				_context = null;
			}

			_isInitialized = false;
		}

		private IList<BufferedAudioData> GetBufferedData(int numberOfBuffers)
		{
			List<BufferedAudioData> byteBuffers = new List<BufferedAudioData>(numberOfBuffers);
			for (int i = 0; i < numberOfBuffers; i++)
			{
				byte[] buffer = new byte[_bufferSize];
				int read = _source.Read(buffer, 0, buffer.Length);
				if (read <= 0)
				{
					continue;
				}

				byteBuffers.Add(new BufferedAudioData()
					{
						Data = buffer,
						Length = read
					});
			}

			return byteBuffers;
		}

		private void FillBuffers(uint[] buffers, IList<BufferedAudioData> audioData)
		{
			for (int i = 0; i < buffers.Length; i++)
			{
				FillBuffer(buffers[i], audioData[i].Data, audioData[i].Length);
			}
		}

		private void FillBuffer(uint bufferHandle, byte[] buffer, int count)
		{
			using (Context.LockContext())
			{
				ALException.Try(
					() =>
					ALInterops.alBufferData(bufferHandle, _playbackFormat, buffer, count,
						(uint) _source.WaveFormat.SampleRate),
					"alBufferData");
				_alSource.QueueBuffer(bufferHandle);
			}
		}

		/// <summary>
		///     Disposes and stops the <see cref="ALSoundOut" /> instance.
		/// </summary>
		/// <param name="disposing">
		///     True to release both managed and unmanaged resources; false to release only unmanaged
		///     resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			CheckForInvalidThreadCall();

			lock (_lockObj)
			{
				if (!_disposed)
				{
					Debug.WriteLine("Disposing ALSoundOut");
					Stop();
					CleanupResources();
				}
				_disposed = true;
			}
		}

		~ALSoundOut()
		{
			Dispose(false);
		}

		private int FindBestBitDepth(WaveFormat waveFormat)
		{
			int bitsPerSample = waveFormat.BitsPerSample;
			var supportedBitsPerSample = new[]
			{
				8, 
				16, 
				Context.Supports32Float ? 32 : 16
			}.OrderBy(x => x);

			foreach (int bits in supportedBitsPerSample)
			{
				if (bits >= bitsPerSample)
					return bits;
			}

			return supportedBitsPerSample.Max();
		}

		private ALFormat FindALFormat(WaveFormat waveFormat)
		{
			if (waveFormat.Channels == 1)
			{
				switch (waveFormat.BitsPerSample)
				{
				case 8:
					return ALFormat.Mono8Bit;
				case 16:
					return ALFormat.Mono16Bit;
				case 32:
					return ALFormat.MonoFloat32Bit;
				default:
					throw new ALException("Invalid BitsPerSample.");
				}
			}
			if (waveFormat.Channels == 2)
			{
				switch (waveFormat.BitsPerSample)
				{
				case 8:
					return ALFormat.Stereo8Bit;
				case 16:
					return ALFormat.Stereo16Bit;
				case 32:
					return ALFormat.StereoFloat32Bit;
				default:
					throw new ALException("Invalid BitsPerSample.");
				}
			}

			throw new ALException("Invalid number of channels.");
		}

		private void CheckForInvalidThreadCall()
		{
			if (Thread.CurrentThread == _playbackThread)
				throw new InvalidOperationException("You must not access this method from the PlaybackThread.");
		}

		private void CheckForDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException("ALSoundOut");
		}

		private void CheckForIsInitialized()
		{
			if (!_isInitialized)
				throw new InvalidOperationException("ALSoundOut is not initialized.");
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

		private struct BufferedAudioData
		{
			public byte[] Data;
			public int Length;
		}
	}
}