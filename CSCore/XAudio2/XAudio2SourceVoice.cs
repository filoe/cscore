using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Use a source voice to submit audio data to the XAudio2 processing pipeline.You must send voice data to a mastering
    ///     voice to be heard, either directly or through intermediate submix voices.
    /// </summary>
    public class XAudio2SourceVoice : XAudio2Voice
    {
        private const string N = "IXAudio2SourceVoice";

        /// <summary>
        /// Gets the <see cref="VoiceState"/> of the source voice.
        /// </summary>
        public VoiceState State
        {
            get { return GetState(); }
        }

        internal XAudio2SourceVoice()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2SourceVoice" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer of the <see cref="XAudio2SourceVoice" /> object.</param>
        /// <param name="version">The <see cref="XAudio2Version"/> to use.</param>        
        public XAudio2SourceVoice(IntPtr ptr, XAudio2Version version)
            : base(ptr, version)
        {
        }

        /// <summary>
        ///     Starts consumption and processing of audio by the voice. Delivers the result to any connected submix or mastering
        ///     voices, or to the output device.
        /// </summary>
        /// <param name="flags">Flags that control how the voice is started. Must be 0.</param>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int StartNative(int flags, int operationSet)
        {
            return InteropCalls.CallI(UnsafeBasePtr, flags, operationSet, ((void**) (*(void**) UnsafeBasePtr))[19]);
        }

        /// <summary>
        ///     Starts consumption and processing of audio by the voice. Delivers the result to any connected submix or mastering
        ///     voices, or to the output device.
        /// </summary>
        /// <param name="flags">Flags that control how the voice is started. Must be 0.</param>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        public void Start(int flags, int operationSet)
        {
            XAudio2Exception.Try(StartNative(flags, operationSet), N, "Start");
        }

        /// <summary>
        ///     Starts consumption and processing of audio by the voice. Delivers the result to any connected submix or mastering
        ///     voices, or to the output device.
        /// </summary>
        public void Start()
        {
            Start(0, XAudio2.CommitNow);
        }

        /// <summary>
        ///     Stops consumption of audio by the current voice.
        /// </summary>
        /// <param name="flags">
        ///     Flags that control how the voice is stopped. Can be <see cref="SourceVoiceStopFlags.None" /> or
        ///     <see cref="SourceVoiceStopFlags.PlayTails" />.
        /// </param>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int StopNative(SourceVoiceStopFlags flags, int operationSet)
        {
            return InteropCalls.CallI(UnsafeBasePtr, flags, operationSet, ((void**) (*(void**) UnsafeBasePtr))[20]);
        }

        /// <summary>
        ///     Stops consumption of audio by the current voice.
        /// </summary>
        public void Stop()
        {
            Stop(SourceVoiceStopFlags.None, XAudio2.CommitNow);
        }

        /// <summary>
        ///     Stops consumption of audio by the current voice.
        /// </summary>
        /// <param name="flags">
        ///     Flags that control how the voice is stopped. Can be <see cref="SourceVoiceStopFlags.None" /> or
        ///     <see cref="SourceVoiceStopFlags.PlayTails" />.
        /// </param>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        public void Stop(SourceVoiceStopFlags flags, int operationSet)
        {
            XAudio2Exception.Try(StopNative(flags, operationSet), N, "Stop");
        }

        /// <summary>
        ///     Adds a new audio buffer to the voice queue.
        /// </summary>
        /// <param name="buffer">Pointer to an <see cref="XAudio2Buffer" /> structure to queue.</param>
        /// <param name="bufferWma">Pointer to an additional XAudio2BufferWma structure used when submitting WMA data.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     See
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2sourcevoice.ixaudio2sourcevoice.submitsourcebuffer(v=vs.85).aspx.
        /// </remarks>
        public unsafe int SubmitSourceBufferNative(IntPtr buffer, IntPtr bufferWma)
        {
            return InteropCalls.CallI(UnsafeBasePtr, (void*) buffer, (void*) bufferWma,
                ((void**) (*(void**) UnsafeBasePtr))[21]);
        }

        /// <summary>
        ///     Adds a new audio buffer to the voice queue.
        /// </summary>
        /// <param name="buffer"><see cref="XAudio2Buffer" /> structure to queue.</param>
        /// <remarks>
        ///     See
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2sourcevoice.ixaudio2sourcevoice.submitsourcebuffer(v=vs.85).aspx.
        /// </remarks>
        public unsafe void SubmitSourceBuffer(XAudio2Buffer buffer)
        {
            XAudio2Exception.Try(SubmitSourceBufferNative(new IntPtr(&buffer), IntPtr.Zero), N, "SubmitSourceBuffer");
        }

        /// <summary>
        ///     Removes all pending audio buffers from the voice queue. If the voice is started, the buffer that is currently
        ///     playing is not removed from the queue.
        /// </summary>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     See
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2sourcevoice.ixaudio2sourcevoice.flushsourcebuffers(v=vs.85).aspx.
        /// </remarks>
        public unsafe int FlushSourceBuffersNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[22]);
        }

        /// <summary>
        ///     Removes all pending audio buffers from the voice queue. If the voice is started, the buffer that is currently
        ///     playing is not removed from the queue.
        /// </summary>
        /// <remarks>
        ///     See
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2sourcevoice.ixaudio2sourcevoice.flushsourcebuffers(v=vs.85).aspx.
        /// </remarks>
        public void FlushSourceBuffers()
        {
            XAudio2Exception.Try(FlushSourceBuffersNative(), N, "FlushSourceBuffers");
        }

        /// <summary>
        ///     Notifies an XAudio2 voice that no more buffers are coming after the last one that is currently in its queue.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int DiscontinuityNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[23]);
        }

        /// <summary>
        ///     Notifies an XAudio2 voice that no more buffers are coming after the last one that is currently in its queue.
        /// </summary>
        public void Discontinuity()
        {
            XAudio2Exception.Try(DiscontinuityNative(), N, "Discontinuity");
        }

        /// <summary>
        ///     Stops looping the voice when it reaches the end of the current loop region.
        /// </summary>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int ExitLoopNative(int operationSet)
        {
            return InteropCalls.CallI(UnsafeBasePtr, operationSet, ((void**) (*(void**) UnsafeBasePtr))[24]);
        }

        /// <summary>
        ///     Stops looping the voice when it reaches the end of the current loop region.
        /// </summary>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        public void ExitLoop(int operationSet)
        {
            XAudio2Exception.Try(ExitLoopNative(operationSet), N, "ExitLoop");
        }

        /// <summary>
        ///     Stops looping the voice when it reaches the end of the current loop region.
        /// </summary>
        public void ExitLoop()
        {
            ExitLoop(XAudio2.CommitNow);
        }

        /// <summary>
        ///     Returns the voice's current cursor position data.
        /// </summary>
        /// <returns><see cref="VoiceState" /> structure containing the state of the voice.</returns>
        public VoiceState GetState()
        {
            return GetState(GetVoiceStateFlags.Default);
        }

        /// <summary>
        ///     Returns the voice's current cursor position data.
        /// </summary>
        /// <param name="flags">
        ///     <b>XAudio2.8 only:</b> Flags controlling which voice state data should be returned.
        ///     Valid values are <see cref="GetVoiceStateFlags.Default" /> or <see cref="GetVoiceStateFlags.NoSamplesPlayed" />.
        ///     The default value is <see cref="GetVoiceStateFlags.Default" />. If you specify
        ///     <see cref="GetVoiceStateFlags.NoSamplesPlayed" />, GetState
        ///     returns only the buffer state, not the sampler state.
        ///     GetState takes roughly one-third as much time to complete when you specify
        ///     <see cref="GetVoiceStateFlags.NoSamplesPlayed" />.
        /// </param>
        /// <returns><see cref="VoiceState" /> structure containing the state of the voice.</returns>
        /// <remarks>If the <see cref="XAudio2Voice.Version"/> is not <see cref="XAudio2Version.XAudio2_8"/> the <paramref name="flags"/> parameter will be ignored.</remarks>
        public unsafe VoiceState GetState(GetVoiceStateFlags flags)
        {
            if (Version == XAudio2Version.XAudio2_7)
            {
                VoiceState voiceState = default(VoiceState);
                InteropCalls.CallI1(UnsafeBasePtr, &voiceState, ((void**)(*(void**)UnsafeBasePtr))[25]);
                return voiceState;
            }
            if (Version == XAudio2Version.XAudio2_8)
            {
                VoiceState voiceState = default(VoiceState);
                InteropCalls.CallI1(UnsafeBasePtr, &voiceState, flags, ((void**)(*(void**)UnsafeBasePtr))[25]);
                return voiceState;
            }

            throw new Exception("Invalid XAudio2 Version.");
        }

        /// <summary>
        ///     Sets the frequency adjustment ratio of the voice.
        /// </summary>
        /// <param name="ratio">
        ///     Frequency adjustment ratio. This value must be between <see cref="XAudio2.MinFrequencyRatio" /> and
        ///     the MaxFrequencyRatio parameter specified when the voice was created
        ///     <see
        ///         cref="XAudio2.CreateSourceVoice(CSCore.WaveFormat,CSCore.XAudio2.VoiceFlags,float,CSCore.XAudio2.IXAudio2VoiceCallback,System.Nullable{CSCore.XAudio2.VoiceSends},System.Nullable{CSCore.XAudio2.EffectChain})" />
        ///     .
        /// </param>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int SetFrequencyRatioNative(float ratio, int operationSet)
        {
            return InteropCalls.CallI(UnsafeBasePtr, ratio, operationSet, ((void**) (*(void**) UnsafeBasePtr))[26]);
        }

        /// <summary>
        ///     Sets the frequency adjustment ratio of the voice.
        /// </summary>
        /// <param name="ratio">
        ///     Frequency adjustment ratio. This value must be between <see cref="XAudio2.MinFrequencyRatio" /> and
        ///     the MaxFrequencyRatio parameter specified when the voice was created
        ///     <see
        ///         cref="XAudio2.CreateSourceVoice(CSCore.WaveFormat,CSCore.XAudio2.VoiceFlags,float,CSCore.XAudio2.IXAudio2VoiceCallback,System.Nullable{CSCore.XAudio2.VoiceSends},System.Nullable{CSCore.XAudio2.EffectChain})" />
        ///     .
        /// </param>
        /// <param name="operationSet">
        ///     Identifies this call as part of a deferred batch. For more details see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/ee415807(v=vs.85).aspx.
        /// </param>
        public void SetFrequencyRatio(float ratio, int operationSet)
        {
            XAudio2Exception.Try(SetFrequencyRatioNative(ratio, operationSet), N, "SetFrequencyRatio");
        }

        /// <summary>
        ///     Sets the frequency adjustment ratio of the voice.
        /// </summary>
        /// <param name="ratio">
        ///     Frequency adjustment ratio. This value must be between <see cref="XAudio2.MinFrequencyRatio" /> and
        ///     the MaxFrequencyRatio parameter specified when the voice was created
        ///     <see
        ///         cref="XAudio2.CreateSourceVoice(CSCore.WaveFormat,CSCore.XAudio2.VoiceFlags,float,CSCore.XAudio2.IXAudio2VoiceCallback,System.Nullable{CSCore.XAudio2.VoiceSends},System.Nullable{CSCore.XAudio2.EffectChain})" />
        ///     .
        /// </param>
        public void SetFrequencyRatio(float ratio)
        {
            SetFrequencyRatio(ratio, XAudio2.CommitNow);
        }

        /// <summary>
        ///     Returns the frequency adjustment ratio of the voi
        /// </summary>
        /// <returns>Current frequency adjustment ratio if successful.</returns>
        public unsafe float GetFrequencyRatio()
        {
            float value = default(float);
            return InteropCalls.CallI1(UnsafeBasePtr, &value, ((void**) (*(void**) UnsafeBasePtr))[27]);
        }

        /// <summary>
        ///     Reconfigures the voice to consume source data at a different sample rate than the rate specified when the voice was
        ///     created.
        /// </summary>
        /// <param name="newSourceSampleRate">
        ///     The new sample rate the voice should process submitted data at. Valid sample rates
        ///     are 1kHz to 200kHz.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int SetSourceSampleRateNative(int newSourceSampleRate)
        {
            return InteropCalls.CallI(UnsafeBasePtr, newSourceSampleRate, ((void**) (*(void**) UnsafeBasePtr))[28]);
        }

        /// <summary>
        ///     Reconfigures the voice to consume source data at a different sample rate than the rate specified when the voice was
        ///     created.
        /// </summary>
        /// <param name="newSourceSampleRate">
        ///     The new sample rate the voice should process submitted data at. Valid sample rates
        ///     are 1kHz to 200kHz.
        /// </param>
        public void SetSourceSampleRate(int newSourceSampleRate)
        {
            XAudio2Exception.Try(SetSourceSampleRateNative(newSourceSampleRate), N, "SetSourceSampleRate");
        }
    }
}