﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Extends the the <see cref="XAudio2SourceVoice" /> to enable real-time audio streaming.
    /// </summary>
    public class StreamingSourceVoice : XAudio2SourceVoice
    {
        private const int MaxBufferCount = 3;

        private readonly byte[] _buffer;
        private readonly XAudio2Buffer[] _buffers = new XAudio2Buffer[MaxBufferCount];
        private readonly object _lockObj = new object();

        private readonly VoiceCallback _voiceCallback;
        private readonly IWaveSource _waveSource;
        private int _currentBufferIndex;

        private volatile bool _disposed;
        private EventWaitHandle _waitHandle;

        private static IntPtr CreateSourceVoice(XAudio2 xaudio2, IWaveSource waveSource, VoiceCallback callback)
        {
            if (xaudio2 == null)
                throw new ArgumentNullException("xaudio2");
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            return xaudio2.CreateSourceVoicePtr(waveSource.WaveFormat, VoiceFlags.None,
                XAudio2.DefaultFrequencyRatio, callback,
                null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingSourceVoice"/> class with a default buffer size of 100ms.
        /// </summary>
        /// <param name="xaudio2">Instance of the <see cref="XAudio2" /> class, used to create the <see cref="XAudio2SourceVoice"/>.</param>
        /// <param name="waveSource">The <see cref="IWaveSource" /> instance which provides audio data to play.</param>
        public StreamingSourceVoice(XAudio2 xaudio2, IWaveSource waveSource)
            : this(xaudio2, waveSource, 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingSourceVoice"/> class.
        /// </summary>
        /// <param name="xaudio2">Instance of the <see cref="XAudio2" /> class, used to create the <see cref="XAudio2SourceVoice"/>.</param>
        /// <param name="waveSource">The <see cref="IWaveSource" /> instance which provides audio data to play.</param>
        /// <param name="bufferSize">
        ///     Buffersize of the internal buffers, in milliseconds. Values in the range from 70ms to
        ///     200ms are recommended.
        /// </param>
        public StreamingSourceVoice(XAudio2 xaudio2, IWaveSource waveSource, int bufferSize)
            : this(xaudio2, waveSource, new VoiceCallback(), bufferSize)
        {
        }

        internal StreamingSourceVoice(XAudio2 xaudio2, IWaveSource waveSource, VoiceCallback voiceCallback, int bufferSize)
            : base(CreateSourceVoice(xaudio2, waveSource, voiceCallback), xaudio2.Version)
        {
            _voiceCallback = voiceCallback;
            _waveSource = waveSource;

            var maxBufferBytes = (int)waveSource.WaveFormat.MillisecondsToBytes(bufferSize);
            _buffer = new byte[maxBufferBytes];

            for (int i = 0; i < _buffers.Length; i++)
            {
                var buffer = new XAudio2Buffer(maxBufferBytes);
                _buffers[i] = buffer;
            }

            InitializeForStreaming();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StreamingSourceVoice" /> class.
        /// </summary>
        /// <param name="ptr">Pointer to a <see cref="XAudio2SourceVoice" /> object.</param>
        /// <param name="voiceCallback">
        ///     <see cref="VoiceCallback" /> instance which receives notifications from the
        ///     <see cref="XAudio2SourceVoice" /> which got passed as a pointer (see the <paramref name="ptr" /> argument).
        /// </param>
        /// <param name="waveSource"><see cref="IWaveSource" /> which provides the audio data to stream.</param>
        /// <param name="bufferSize">
        ///     Buffersize of the internal used buffers in milliseconds. Values in the range from 70ms to
        ///     200ms are recommended.
        /// </param>
        /// <remarks>It is recommended to use the <see cref="Create(XAudio2,IWaveSource,int)" /> method instead of the this constructor.</remarks>
        internal StreamingSourceVoice(IntPtr ptr, VoiceCallback voiceCallback, IWaveSource waveSource, int bufferSize)
        {
            BasePtr = ptr;
            _voiceCallback = voiceCallback;
            _waveSource = waveSource;

            var maxBufferBytes = (int) waveSource.WaveFormat.MillisecondsToBytes(bufferSize);
            _buffer = new byte[maxBufferBytes];

            for (int i = 0; i < _buffers.Length; i++)
            {
                var buffer = new XAudio2Buffer(maxBufferBytes);
                _buffers[i] = buffer;
            }

            InitializeForStreaming();
        }

        internal EventWaitHandle BufferEndWaitHandle
        {
            get { return _waitHandle; }
        }

        /// <summary>
        ///     Creates an instance of the <see cref="StreamingSourceVoice" /> class.
        /// </summary>
        /// <param name="xaudio2">Instance of the <see cref="XAudio2" /> class.</param>
        /// <param name="waveSource"><see cref="IWaveSource" /> which provides the audio data to stream.</param>
        /// <param name="bufferSize">
        ///     Buffersize of the internal used buffers in milliseconds. Values in the range from 70ms to
        ///     200ms are recommended.
        /// </param>
        /// <returns>Configured <see cref="StreamingSourceVoice" /> instance.</returns>
        [Obsolete("Use the constructor of the StreamingSourceVoice-class.")]
        public static StreamingSourceVoice Create(XAudio2 xaudio2, IWaveSource waveSource, int bufferSize = 100)
        {
            var voiceCallback = new VoiceCallback();
            IntPtr ptr = xaudio2.CreateSourceVoicePtr(waveSource.WaveFormat, VoiceFlags.None,
                XAudio2.DefaultFrequencyRatio, voiceCallback,
                null, null);

            return new StreamingSourceVoice(ptr, voiceCallback, waveSource, bufferSize);
        }

        public bool Loop { get; set; }

        /// <summary>
        ///     Occurs when the playback stops and no more data is available.
        /// </summary>
        /// <remarks>This event occurs whenever the <see cref="VoiceCallback.StreamEnd" /> event occurs.</remarks>
        public event EventHandler Stopped
        {
            add { _voiceCallback.StreamEnd += value; }
            remove { _voiceCallback.StreamEnd -= value; }
        }

        private void InitializeForStreaming()
        {
            _waitHandle = new AutoResetEvent(true); //set the initial state to true to start streaming

            _voiceCallback.BufferEnd += (s, e) => _waitHandle.Set();
            //Start(); //start the playback
        }

        /// <summary>
        ///     Notifies the <see cref="StreamingSourceVoice" /> class that new data got requested. If there are any buffers which
        ///     are currently not queued and the underlying <see cref="IWaveSource" /> holds any more data, this data refills the
        ///     internal used buffers and provides audio data to play.
        /// </summary>
        public virtual void Refill()
        {
            lock (_lockObj) //make sure that nothing gets disposed while anything is still in use.
            {
                if (_disposed)
                {
                    _waitHandle.Close();
                    return;
                }

                int buffersQueued = GetState(GetVoiceStateFlags.NoSamplesPlayed).BuffersQueued;
                if (buffersQueued >= MaxBufferCount)
                    return;

                int read = _waveSource.Read(_buffer, 0, _buffer.Length);
                if (read == 0)
                    return;

                XAudio2Buffer nbuffer = _buffers[_currentBufferIndex];
                nbuffer.AudioBytes = read;

                //bug: could be critical since some wave sources don't provide length and position
                if (_waveSource.Position >= _waveSource.Length)
                {
                    if (!Loop)
                        nbuffer.Flags = XAudio2BufferFlags.EndOfStream;
                    else
                    {
                        nbuffer.Flags = XAudio2BufferFlags.None;
                        _waveSource.Position = 0;
                    }
                }
                else
                    nbuffer.Flags = XAudio2BufferFlags.None;

                using (var stream = nbuffer.GetStream())
                {
                    stream.Write(_buffer, 0, read);
                }

                Debug.WriteLine(String.Format("Submit: {0};{1}", nbuffer.Flags, nbuffer.AudioBytes));
                SubmitSourceBuffer(nbuffer);

                _currentBufferIndex++;
                _currentBufferIndex %= MaxBufferCount;
            }
        }

        /// <summary>
        ///     Stops and disposes the <see cref="XAudio2SourceVoice" />, closes the internal used waithandle and frees the
        ///     allocated memory of all used buffers.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                if (_disposed)
                    return;

                _waitHandle.Reset();
                Stop(SourceVoiceStopFlags.None, XAudio2.CommitNow);

                foreach (XAudio2Buffer buffer in _buffers)
                {
                    buffer.Dispose();
                }

                base.Dispose(disposing);
                
                _voiceCallback.Dispose();

                _disposed = true;
            }
        }
    }
}