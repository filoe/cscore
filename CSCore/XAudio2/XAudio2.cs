using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     <see cref="XAudio2" /> is the class for the XAudio2 object that manages all audio engine states, the audio
    ///     processing thread, the voice graph, and so forth.
    /// </summary>
    [Guid("60d8dac8-5aa1-4e8e-b597-2f5e2883d484")]
    public abstract class XAudio2 : ComObject
    {
        private XAudio2Version _version;
        private const string N = "IXAudio2";

        /// <summary>
        ///     The denominator of a quantum unit. In 10ms chunks (= 1/100 seconds).
        /// </summary>
        public const int QuantumDenominator = 100;

        /// <summary>
        ///     Minimum sample rate is 1000 Hz.
        /// </summary>
        public const int MinimumSampleRate = 1000;

        /// <summary>
        ///     Maximum sample rate is 200 kHz.
        /// </summary>
        public const int MaximumSampleRate = 200000;

        /// <summary>
        ///     The minimum frequency ratio is 1/1024.
        /// </summary>
        public const float MinFrequencyRatio = (1 / 1024.0f);

        /// <summary>
        ///     Maximum frequency ratio is 1024.
        /// </summary>
        public const float MaxFrequencyRatio = 1024.0f;

        /// <summary>
        ///     The default value for the frequency ratio is 4.
        /// </summary>
        public const float DefaultFrequencyRatio = 4.0f;

        /// <summary>
        ///     The maximum number of supported channels is 64.
        /// </summary>
        public const int MaxAudioChannels = 64;

        /// <summary>
        ///     Value which indicates that the default number of channels should be used.
        /// </summary>
        public const int DefaultChannels = 0;

        /// <summary>
        ///     Values which indicates that the default sample rate should be used.
        /// </summary>
        public const int DefaultSampleRate = 0;

        /// <summary>
        ///     Value which can be used in combination with the <see cref="XAudio2.CommitChanges(int)" /> method to commit all
        ///     changes.
        /// </summary>
        public const int CommitAll = 0;

        /// <summary>
        ///     Values which indicates that the made changes should be commited instantly.
        /// </summary>
        public const int CommitNow = 0;

        /// <summary>
        ///     Internal default ctor.
        /// </summary>
        internal XAudio2()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer of the <see cref="XAudio2Voice" /> object.</param>
        /// <param name="version">The XAudio2 subversion to use.</param>
        protected XAudio2(IntPtr ptr, XAudio2Version version)
            : base(ptr)
        {
            _version = version;
        }

        /// <summary>
        ///     Gets current resource usage details, such as available memory or CPU usage.
        /// </summary>
        public PerformanceData PerformanceData
        {
            get
            {
                PerformanceData performanceData;
                GetPerformanceDataNative(out performanceData);
                return performanceData;
            }
        }

        /// <summary>
        /// Gets the default device which can be used to create a mastering voice.
        /// </summary>
        /// <value>Using XAudio2.7 the default device is 0 (as an integer). Using XAudio2.8 the default device is null.</value>
        public object DefaultDevice
        {
            get { return GetDefaultDevice(); }
        }

        /// <summary>
        /// Gets the <see cref="XAudio2Version"/> of the XAudio2 object.
        /// </summary>
        public XAudio2Version Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="XAudio2" /> class.
        ///     If no supported XAudio2 version is available, the CreateXAudio2 method throws an
        ///     <see cref="NotSupportedException" />.
        /// </summary>
        /// <returns>A new <see cref="XAudio2" /> instance.</returns>
        public static XAudio2 CreateXAudio2()
        {
            return CreateXAudio2(null);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="XAudio2" /> class.
        ///     If no supported XAudio2 version is available, the CreateXAudio2 method throws an
        ///     <see cref="NotSupportedException" />.
        /// </summary>
        /// <param name="processor">The <see cref="XAudio2Processor" /> to use.</param>
        /// <returns>A new <see cref="XAudio2" /> instance.</returns>
        public static XAudio2 CreateXAudio2(XAudio2Processor? processor)
        {
            try
            {
                if(processor.HasValue)
                    return new XAudio2_8(processor.Value);
                return new XAudio2_8();
            }
            catch (Exception)
            {
                try
                {
                    if(processor.HasValue)
                        return new XAudio2_7(false, processor.Value);
                    return new XAudio2_7();
                }
                catch (Exception)
                {
                    throw new NotSupportedException("No supported XAudio2 version is installed.");
                }
            }
        }

        /// <summary>
        ///     Adds an <see cref="IXAudio2EngineCallback" /> from the <see cref="XAudio2" /> engine callback list.
        /// </summary>
        /// <param name="callback">
        ///     <see cref="IXAudio2EngineCallback" /> object to add to the <see cref="XAudio2" /> engine
        ///     callback list.
        /// </param>
        /// <returns>HRESULT</returns>
        public abstract int RegisterForCallbacksNative(IXAudio2EngineCallback callback);

        /// <summary>
        ///     Adds an <see cref="IXAudio2EngineCallback" /> from the <see cref="XAudio2" /> engine callback list.
        /// </summary>
        /// <param name="callback">
        ///     <see cref="IXAudio2EngineCallback" /> object to add to the <see cref="XAudio2" /> engine
        ///     callback list.
        /// </param>
        public void RegisterForCallbacks(IXAudio2EngineCallback callback)
        {
            XAudio2Exception.Try(RegisterForCallbacksNative(callback), N, "RegisterForCallbacks");
        }

        /// <summary>
        ///     Removes an <see cref="IXAudio2EngineCallback" /> from the <see cref="XAudio2" /> engine callback list.
        /// </summary>
        /// <param name="callback">
        ///     <see cref="IXAudio2EngineCallback" /> object to remove from the <see cref="XAudio2" /> engine
        ///     callback list. If the given interface is present more than once in the list, only the first instance in the list
        ///     will be removed.
        /// </param>
        public abstract void UnregisterForCallbacks(IXAudio2EngineCallback callback);

        /// <summary>
        ///     Creates and configures a source voice. For more information see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2.ixaudio2.createsourcevoice(v=vs.85).aspx.
        /// </summary>
        /// <param name="pSourceVoice">If successful, returns a pointer to the new <see cref="XAudio2SourceVoice" /> object.</param>
        /// <param name="sourceFormat">
        ///     Pointer to a <see cref="WaveFormat" />. The following formats are supported:
        ///     <ul>
        ///         <li>8-bit (unsigned) integer PCM</li><li>16-bit integer PCM (optimal format for XAudio2)</li>
        ///         <li>20-bit integer PCM (either in 24 or 32 bit containers)</li>
        ///         <li>24-bit integer PCM (either in 24 or 32 bit containers)</li><li>32-bit integer PCM</li>
        ///         <li>32-bit float PCM (preferred format after 16-bit integer)</li>
        ///     </ul>
        ///     The number of channels in a source voice must be less than or equal to <see cref="MaxAudioChannels" />. The sample
        ///     rate of a source voice must be between <see cref="MinimumSampleRate" /> and <see cref="MaximumSampleRate" />.
        /// </param>
        /// <param name="flags">
        ///     <see cref="VoiceFlags" /> that specify the behavior of the source voice. A flag can be
        ///     <see cref="VoiceFlags.None" /> or a combination of one or more of the following.
        ///     Possible values are <see cref="VoiceFlags.NoPitch" />, <see cref="VoiceFlags.NoSampleRateConversition" /> and
        ///     <see cref="VoiceFlags.UseFilter" />. <see cref="VoiceFlags.Music" /> is not supported on Windows.
        /// </param>
        /// <param name="maxFrequencyRatio">
        ///     Highest allowable frequency ratio that can be set on this voice. The value for this
        ///     argument must be between <see cref="MinFrequencyRatio" /> and <see cref="MaxFrequencyRatio" />.
        /// </param>
        /// <param name="voiceCallback">
        ///     Client-provided callback interface, <see cref="IXAudio2VoiceCallback" />. This parameter is
        ///     optional and can be null.
        /// </param>
        /// <param name="sendList">
        ///     List of <see cref="VoiceSends" /> structures that describe the set of destination voices for the
        ///     source voice. If <paramref name="sendList" /> is NULL, the send list defaults to a single output to the first
        ///     mastering
        ///     voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     source voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>HRESULT</returns>
        public abstract int CreateSourceVoiceNative(
            out IntPtr pSourceVoice,
            IntPtr sourceFormat,
            VoiceFlags flags,
            float maxFrequencyRatio,
            IXAudio2VoiceCallback voiceCallback,
            VoiceSends? sendList, //out
            EffectChain? effectChain
            );


        /// <summary>
        ///     Creates and configures a source voice. For more information see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2.ixaudio2.createsourcevoice(v=vs.85).aspx.
        /// </summary>
        /// <param name="sourceFormat">
        ///     Pointer to a <see cref="WaveFormat" />. The following formats are supported:
        ///     <ul>
        ///         <li>8-bit (unsigned) integer PCM</li><li>16-bit integer PCM (optimal format for XAudio2)</li>
        ///         <li>20-bit integer PCM (either in 24 or 32 bit containers)</li>
        ///         <li>24-bit integer PCM (either in 24 or 32 bit containers)</li><li>32-bit integer PCM</li>
        ///         <li>32-bit float PCM (preferred format after 16-bit integer)</li>
        ///     </ul>
        ///     The number of channels in a source voice must be less than or equal to <see cref="MaxAudioChannels" />. The
        ///     sample rate of a source voice must be between <see cref="MinimumSampleRate" /> and <see cref="MaximumSampleRate" />
        ///     .
        /// </param>
        /// <param name="flags">
        ///     <see cref="VoiceFlags" /> that specify the behavior of the source voice. A flag can be
        ///     <see cref="VoiceFlags.None" /> or a combination of one or more of the following.
        ///     Possible values are <see cref="VoiceFlags.NoPitch" />, <see cref="VoiceFlags.NoSampleRateConversition" /> and
        ///     <see cref="VoiceFlags.UseFilter" />. <see cref="VoiceFlags.Music" /> is not supported on Windows.
        /// </param>
        /// <param name="maxFrequencyRatio">
        ///     Highest allowable frequency ratio that can be set on this voice. The value for this
        ///     argument must be between <see cref="MinFrequencyRatio" /> and <see cref="MaxFrequencyRatio" />.
        /// </param>
        /// <param name="voiceCallback">
        ///     Client-provided callback interface, <see cref="IXAudio2VoiceCallback" />. This parameter is
        ///     optional and can be null.
        /// </param>
        /// <param name="sendList">
        ///     List of <see cref="VoiceSends" /> structures that describe the set of destination voices for the
        ///     source voice. If <paramref name="sendList" /> is NULL, the send list defaults to a single output to the first
        ///     mastering
        ///     voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     source voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>If successful, returns a pointer to the new <see cref="XAudio2SourceVoice" /> object.</returns>
        public IntPtr CreateSourceVoicePtr(WaveFormat sourceFormat, VoiceFlags flags, float maxFrequencyRatio,
            IXAudio2VoiceCallback voiceCallback, VoiceSends? sendList, EffectChain? effectChain)
        {
            GCHandle hWaveFormat = GCHandle.Alloc(sourceFormat, GCHandleType.Pinned);
            //todo: do we really need to use GCHandle?
            try
            {
                IntPtr ptr;
                int result = CreateSourceVoiceNative(
                    out ptr,
                    hWaveFormat.AddrOfPinnedObject(),
                    flags,
                    maxFrequencyRatio,
                    voiceCallback,
                    sendList,
                    effectChain);
                XAudio2Exception.Try(result, N, "CreateSourceVoice");

                return ptr;
            }
            finally
            {
                if (hWaveFormat.IsAllocated)
                    hWaveFormat.Free();
            }
        }

        /// <summary>
        ///     Creates and configures a source voice. For more information see
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2.ixaudio2.createsourcevoice(v=vs.85).aspx.
        /// </summary>
        /// <param name="sourceFormat">
        ///     Pointer to a <see cref="WaveFormat" />. The following formats are supported:
        ///     <ul>
        ///         <li>8-bit (unsigned) integer PCM</li><li>16-bit integer PCM (optimal format for XAudio2)</li>
        ///         <li>20-bit integer PCM (either in 24 or 32 bit containers)</li>
        ///         <li>24-bit integer PCM (either in 24 or 32 bit containers)</li><li>32-bit integer PCM</li>
        ///         <li>32-bit float PCM (preferred format after 16-bit integer)</li>
        ///     </ul>
        ///     The number of channels in a source voice must be less than or equal to <see cref="MaxAudioChannels" />. The
        ///     sample rate of a source voice must be between <see cref="MinimumSampleRate" /> and <see cref="MaximumSampleRate" />
        ///     .
        /// </param>
        /// <param name="flags">
        ///     <see cref="VoiceFlags" /> that specify the behavior of the source voice. A flag can be
        ///     <see cref="VoiceFlags.None" /> or a combination of one or more of the following.
        ///     Possible values are <see cref="VoiceFlags.NoPitch" />, <see cref="VoiceFlags.NoSampleRateConversition" /> and
        ///     <see cref="VoiceFlags.UseFilter" />. <see cref="VoiceFlags.Music" /> is not supported on Windows.
        /// </param>
        /// <param name="maxFrequencyRatio">
        ///     Highest allowable frequency ratio that can be set on this voice. The value for this
        ///     argument must be between <see cref="MinFrequencyRatio" /> and <see cref="MaxFrequencyRatio" />.
        /// </param>
        /// <param name="voiceCallback">
        ///     Client-provided callback interface, <see cref="IXAudio2VoiceCallback" />. This parameter is
        ///     optional and can be null.
        /// </param>
        /// <param name="sendList">
        ///     List of <see cref="VoiceSends" /> structures that describe the set of destination voices for the
        ///     source voice. If <paramref name="sendList" /> is NULL, the send list defaults to a single output to the first
        ///     mastering
        ///     voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     source voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>If successful, returns a new <see cref="XAudio2SourceVoice" /> object.</returns>
        public XAudio2SourceVoice CreateSourceVoice(WaveFormat sourceFormat, VoiceFlags flags, float maxFrequencyRatio,
            IXAudio2VoiceCallback voiceCallback, VoiceSends? sendList, EffectChain? effectChain)
        {
            IntPtr ptr = CreateSourceVoicePtr(sourceFormat, flags, maxFrequencyRatio, voiceCallback, sendList,
                effectChain);
            return new XAudio2SourceVoice(ptr, _version);
        }

        /// <summary>
        ///     Creates and configures a source voice.
        /// </summary>
        /// <param name="sourceFormat">
        ///     Pointer to a <see cref="WaveFormat" />. The following formats are supported:
        ///     <ul>
        ///         <li>8-bit (unsigned) integer PCM</li><li>16-bit integer PCM (optimal format for XAudio2)</li>
        ///         <li>20-bit integer PCM (either in 24 or 32 bit containers)</li>
        ///         <li>24-bit integer PCM (either in 24 or 32 bit containers)</li><li>32-bit integer PCM</li>
        ///         <li>32-bit float PCM (preferred format after 16-bit integer)</li>
        ///     </ul>
        ///     The number of channels in a source voice must be less than or equal to <see cref="MaxAudioChannels" />. The
        ///     sample rate of a source voice must be between <see cref="MinimumSampleRate" /> and <see cref="MaximumSampleRate" />
        ///     .
        /// </param>
        /// <param name="flags">
        ///     <see cref="VoiceFlags" /> that specify the behavior of the source voice. A flag can be
        ///     <see cref="VoiceFlags.None" /> or a combination of one or more of the following.
        ///     Possible values are <see cref="VoiceFlags.NoPitch" />, <see cref="VoiceFlags.NoSampleRateConversition" /> and
        ///     <see cref="VoiceFlags.UseFilter" />. <see cref="VoiceFlags.Music" /> is not supported on Windows.
        /// </param>
        /// <returns>If successful, returns a new <see cref="XAudio2SourceVoice" /> object.</returns>
        public XAudio2SourceVoice CreateSourceVoice(WaveFormat sourceFormat, VoiceFlags flags)
        {
            const float defaultFreqRatio = 4.0f;
            return CreateSourceVoice(sourceFormat, flags, defaultFreqRatio, null, null, null);
        }

        /// <summary>
        ///     Creates and configures a source voice.
        /// </summary>
        /// <param name="sourceFormat">
        ///     Pointer to a <see cref="WaveFormat" />. The following formats are supported:
        ///     <ul>
        ///         <li>8-bit (unsigned) integer PCM</li><li>16-bit integer PCM (optimal format for XAudio2)</li>
        ///         <li>20-bit integer PCM (either in 24 or 32 bit containers)</li>
        ///         <li>24-bit integer PCM (either in 24 or 32 bit containers)</li><li>32-bit integer PCM</li>
        ///         <li>32-bit float PCM (preferred format after 16-bit integer)</li>
        ///     </ul>
        ///     The number of channels in a source voice must be less than or equal to <see cref="MaxAudioChannels" />. The
        ///     sample rate of a source voice must be between <see cref="MinimumSampleRate" /> and <see cref="MaximumSampleRate" />
        ///     .
        /// </param>
        /// <returns>If successful, returns a new <see cref="XAudio2SourceVoice" /> object.</returns>
        public XAudio2SourceVoice CreateSourceVoice(WaveFormat sourceFormat)
        {
            return CreateSourceVoice(sourceFormat, VoiceFlags.None);
        }

        /// <summary>
        ///     Creates and configures a submix voice.
        /// </summary>
        /// <param name="pSubmixVoice">On success, returns a pointer to the new <see cref="XAudio2SubmixVoice" /> object.</param>
        /// <param name="inputChannels">
        ///     Number of channels in the input audio data of the submix voice. The
        ///     <paramref name="inputChannels" /> must be less than or equal to <see cref="MaxAudioChannels" />.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of submix voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. InputSampleRate must be between <see cref="MinimumSampleRate" /> and
        ///     <see cref="MaximumSampleRate" />.
        /// </param>
        /// <param name="flags">
        ///     Flags that specify the behavior of the submix voice. It can be <see cref="VoiceFlags.None" /> or
        ///     <see cref="VoiceFlags.UseFilter" />.
        /// </param>
        /// <param name="processingStage">
        ///     An arbitrary number that specifies when this voice is processed with respect to other
        ///     submix voices, if the XAudio2 engine is running other submix voices. The voice is processed after all other voices
        ///     that include a smaller <paramref name="processingStage" /> value and before all other voices that include a larger
        ///     <paramref name="processingStage" /> value. Voices that include the same <paramref name="processingStage" /> value
        ///     are
        ///     processed in any order. A submix voice cannot send to another submix voice with a lower or equal
        ///     <paramref name="processingStage" /> value. This prevents audio being lost due to a submix cycle.
        /// </param>
        /// <param name="sendList">
        ///     List of <see cref="VoiceSends" /> structures that describe the set of destination voices for the
        ///     submix voice. If <paramref name="sendList" /> is NULL, the send list will default to a single output to the first
        ///     mastering voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     submix voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>HRESULT</returns>
        public abstract int CreateSubmixVoiceNative(out IntPtr pSubmixVoice, int inputChannels,
            int inputSampleRate, VoiceFlags flags,
            int processingStage, VoiceSends? sendList, EffectChain? effectChain);

        /// <summary>
        ///     Creates and configures a submix voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels in the input audio data of the submix voice. The
        ///     <paramref name="inputChannels" /> must be less than or equal to <see cref="MaxAudioChannels" />.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of submix voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. InputSampleRate must be between <see cref="MinimumSampleRate" /> and
        ///     <see cref="MaximumSampleRate" />.
        /// </param>
        /// <param name="flags">
        ///     Flags that specify the behavior of the submix voice. It can be <see cref="VoiceFlags.None" /> or
        ///     <see cref="VoiceFlags.UseFilter" />.
        /// </param>
        /// <param name="processingStage">
        ///     An arbitrary number that specifies when this voice is processed with respect to other
        ///     submix voices, if the XAudio2 engine is running other submix voices. The voice is processed after all other voices
        ///     that include a smaller <paramref name="processingStage" /> value and before all other voices that include a larger
        ///     <paramref name="processingStage" /> value. Voices that include the same <paramref name="processingStage" /> value
        ///     are
        ///     processed in any order. A submix voice cannot send to another submix voice with a lower or equal
        ///     <paramref name="processingStage" /> value. This prevents audio being lost due to a submix cycle.
        /// </param>
        /// <param name="sendList">
        ///     List of <see cref="VoiceSends" /> structures that describe the set of destination voices for the
        ///     submix voice. If <paramref name="sendList" /> is NULL, the send list will default to a single output to the first
        ///     mastering voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     submix voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>On success, returns a pointer to the new <see cref="XAudio2SubmixVoice" /> object.</returns>
        public IntPtr CreateSubmixVoicePtr(int inputChannels, int inputSampleRate, VoiceFlags flags,
            int processingStage, VoiceSends? sendList, EffectChain? effectChain)
        {
            IntPtr ptr;
            int result = CreateSubmixVoiceNative(out ptr, inputChannels, inputSampleRate, flags, processingStage,
                sendList,
                effectChain);
            XAudio2Exception.Try(result, N, "CreateSubmixVoiceNative");
            return ptr;
        }

        /// <summary>
        ///     Creates and configures a submix voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels in the input audio data of the submix voice. The
        ///     <paramref name="inputChannels" /> must be less than or equal to <see cref="MaxAudioChannels" />.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of submix voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. InputSampleRate must be between <see cref="MinimumSampleRate" /> and
        ///     <see cref="MaximumSampleRate" />.
        /// </param>
        /// <param name="flags">
        ///     Flags that specify the behavior of the submix voice. It can be <see cref="VoiceFlags.None" /> or
        ///     <see cref="VoiceFlags.UseFilter" />.
        /// </param>
        /// <param name="processingStage">
        ///     An arbitrary number that specifies when this voice is processed with respect to other
        ///     submix voices, if the XAudio2 engine is running other submix voices. The voice is processed after all other voices
        ///     that include a smaller <paramref name="processingStage" /> value and before all other voices that include a larger
        ///     <paramref name="processingStage" /> value. Voices that include the same <paramref name="processingStage" /> value
        ///     are
        ///     processed in any order. A submix voice cannot send to another submix voice with a lower or equal
        ///     <paramref name="processingStage" /> value. This prevents audio being lost due to a submix cycle.
        /// </param>
        /// <param name="sendList">
        ///     List of <see cref="VoiceSends" /> structures that describe the set of destination voices for the
        ///     submix voice. If <paramref name="sendList" /> is NULL, the send list will default to a single output to the first
        ///     mastering voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     submix voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>On success, returns a new <see cref="XAudio2SubmixVoice" /> object.</returns>
        public XAudio2SubmixVoice CreateSubmixVoice(int inputChannels, int inputSampleRate, VoiceFlags flags,
            int processingStage, VoiceSends? sendList, EffectChain? effectChain)
        {
            IntPtr ptr = CreateSubmixVoicePtr(inputChannels, inputSampleRate, flags, processingStage, sendList,
                effectChain);
            return new XAudio2SubmixVoice(ptr, _version);
        }

        /// <summary>
        ///     Creates and configures a submix voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels in the input audio data of the submix voice. The
        ///     <paramref name="inputChannels" /> must be less than or equal to <see cref="MaxAudioChannels" />.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of submix voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. InputSampleRate must be between <see cref="MinimumSampleRate" /> and
        ///     <see cref="MaximumSampleRate" />.
        /// </param>
        /// <param name="flags">
        ///     Flags that specify the behavior of the submix voice. It can be <see cref="VoiceFlags.None" /> or
        ///     <see cref="VoiceFlags.UseFilter" />.
        /// </param>
        /// <returns>On success, returns a new <see cref="XAudio2SubmixVoice" /> object.</returns>
        public XAudio2SubmixVoice CreateSubmixVoice(int inputChannels, int inputSampleRate, VoiceFlags flags)
        {
            IntPtr ptr = CreateSubmixVoicePtr(inputChannels, inputSampleRate, flags, 0, null, null);
            return new XAudio2SubmixVoice(ptr, _version);
        }

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <param name="pMasteringVoice">If successful, returns a pointer to the new <see cref="XAudio2MasteringVoice" /> object.</param>
        /// <param name="inputChannels">
        ///     Number of channels the mastering voice expects in its input audio. <paramref name="inputChannels" /> must be less
        ///     than
        ///     or equal to <see cref="MaxAudioChannels" />.
        ///     You can set InputChannels to <see cref="DefaultChannels" />, which causes XAudio2 to try to detect the system
        ///     speaker configuration setup.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of the mastering voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. <paramref name="inputSampleRate" /> must be between
        ///     <see cref="MinimumSampleRate" />
        ///     and <see cref="MaximumSampleRate" />.
        ///     You can set InputSampleRate to <see cref="DefaultSampleRate" />, with the default being determined by the current
        ///     platform.
        /// </param>
        /// <param name="flags">Flags that specify the behavior of the mastering voice. Must be 0.</param>
        /// <param name="device">
        ///     Identifier of the device to receive the output audio. Specifying the default value of NULL (for XAudio2.8) or 0 (for XAudio2.7) causes
        ///     XAudio2 to select the global default audio device.
        /// </param>
        /// <param name="effectChain">
        ///     <see cref="EffectChain" /> structure that describes an effect chain to use in the mastering
        ///     voice, or NULL to use no effects.
        /// </param>
        /// <param name="streamCategory">The audio stream category to use for this mastering voice.</param>
        /// <returns>HRESULT</returns>
        public abstract int CreateMasteringVoiceNative(out IntPtr pMasteringVoice, int inputChannels,
            int inputSampleRate,
            int flags,
            object device, EffectChain? effectChain, AudioStreamCategory streamCategory);

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels the mastering voice expects in its input audio. <paramref name="inputChannels" /> must be less
        ///     than
        ///     or equal to <see cref="MaxAudioChannels" />.
        ///     You can set InputChannels to <see cref="DefaultChannels" />, which causes XAudio2 to try to detect the system
        ///     speaker configuration setup.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of the mastering voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. <paramref name="inputSampleRate" /> must be between
        ///     <see cref="MinimumSampleRate" />
        ///     and <see cref="MaximumSampleRate" />.
        ///     You can set InputSampleRate to <see cref="DefaultSampleRate" />, with the default being determined by the current
        ///     platform.
        /// </param>
        /// <param name="flags">Flags that specify the behavior of the mastering voice. Must be 0.</param>
        /// <param name="device">
        ///     Identifier of the device to receive the output audio. Specifying the default value of NULL (for XAudio2.8) or 0 (for XAudio2.7) causes
        ///     XAudio2 to select the global default audio device.
        /// </param>
        /// <param name="effectChain">
        ///     <see cref="EffectChain" /> structure that describes an effect chain to use in the mastering
        ///     voice, or NULL to use no effects.
        /// </param>
        /// <param name="streamCategory">The audio stream category to use for this mastering voice.</param>
        /// <returns>If successful, returns a pointer to the new <see cref="XAudio2MasteringVoice" /> object.</returns>
        public IntPtr CreateMasteringVoicePtr(int inputChannels, int inputSampleRate, int flags,
            object device, EffectChain? effectChain, AudioStreamCategory streamCategory)
        {
            IntPtr ptr;
            int result = CreateMasteringVoiceNative(out ptr, inputChannels, inputSampleRate, flags, device,
                effectChain, streamCategory);
            XAudio2Exception.Try(result, N, "CreateMasteringVoice");
            return ptr;
        }

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels the mastering voice expects in its input audio. <paramref name="inputChannels" /> must be less
        ///     than
        ///     or equal to <see cref="MaxAudioChannels" />.
        ///     You can set InputChannels to <see cref="DefaultChannels" />, which causes XAudio2 to try to detect the system
        ///     speaker configuration setup.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of the mastering voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. <paramref name="inputSampleRate" /> must be between
        ///     <see cref="MinimumSampleRate" />
        ///     and <see cref="MaximumSampleRate" />.
        ///     You can set InputSampleRate to <see cref="DefaultSampleRate" />, with the default being determined by the current
        ///     platform.
        /// </param>
        /// <param name="device">
        ///     Identifier of the device to receive the output audio. Specifying the default value of NULL (for XAudio2.8) or 0 (for XAudio2.7) causes
        ///     XAudio2 to select the global default audio device.
        /// </param>
        /// <param name="effectChain">
        ///     <see cref="EffectChain" /> structure that describes an effect chain to use in the mastering
        ///     voice, or NULL to use no effects.
        /// </param>
        /// <param name="streamCategory"><b>XAudio2.8 only:</b> The audio stream category to use for this mastering voice.</param>
        /// <returns>If successful, returns a new <see cref="XAudio2MasteringVoice" /> object.</returns>
        public XAudio2MasteringVoice CreateMasteringVoice(int inputChannels, int inputSampleRate,
            object device, EffectChain? effectChain, AudioStreamCategory streamCategory)
        {
            return
                new XAudio2MasteringVoice(CreateMasteringVoicePtr(inputChannels, inputSampleRate, 0, device, effectChain,
                    streamCategory), _version);
        }

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels the mastering voice expects in its input audio. <paramref name="inputChannels" /> must be less
        ///     than
        ///     or equal to <see cref="MaxAudioChannels" />.
        ///     You can set InputChannels to <see cref="DefaultChannels" />, which causes XAudio2 to try to detect the system
        ///     speaker configuration setup.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of the mastering voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. <paramref name="inputSampleRate" /> must be between
        ///     <see cref="MinimumSampleRate" />
        ///     and <see cref="MaximumSampleRate" />.
        ///     You can set InputSampleRate to <see cref="DefaultSampleRate" />, with the default being determined by the current
        ///     platform.
        /// </param>
        /// <param name="device">
        ///     Identifier of the device to receive the output audio. Specifying the default value of NULL (for XAudio2.8) or 0 (for XAudio2.7) causes
        ///     XAudio2 to select the global default audio device.
        /// </param>
        /// <returns>If successful, returns a new <see cref="XAudio2MasteringVoice" /> object.</returns>
        public XAudio2MasteringVoice CreateMasteringVoice(int inputChannels, int inputSampleRate,
            object device)
        {
            return
                new XAudio2MasteringVoice(CreateMasteringVoicePtr(inputChannels, inputSampleRate, 0, device, null,
                    AudioStreamCategory.GameEffects), _version);
        }

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <param name="inputChannels">
        ///     Number of channels the mastering voice expects in its input audio. <paramref name="inputChannels" /> must be less
        ///     than
        ///     or equal to <see cref="MaxAudioChannels" />.
        ///     You can set InputChannels to <see cref="DefaultChannels" />, which causes XAudio2 to try to detect the system
        ///     speaker configuration setup.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of the mastering voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. <paramref name="inputSampleRate" /> must be between
        ///     <see cref="MinimumSampleRate" />
        ///     and <see cref="MaximumSampleRate" />.
        ///     You can set InputSampleRate to <see cref="DefaultSampleRate" />, with the default being determined by the current
        ///     platform.
        /// </param>
        /// <returns>If successful, returns a new <see cref="XAudio2MasteringVoice" /> object.</returns>
        public XAudio2MasteringVoice CreateMasteringVoice(int inputChannels, int inputSampleRate)
        {
            return
                new XAudio2MasteringVoice(CreateMasteringVoicePtr(inputChannels, inputSampleRate, 0, GetDefaultDevice(),
                    null, AudioStreamCategory.GameEffects), _version);
        }

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <returns>If successful, returns a new <see cref="XAudio2MasteringVoice" /> object.</returns>
        public XAudio2MasteringVoice CreateMasteringVoice()
        {
            return CreateMasteringVoice(DefaultChannels, DefaultSampleRate);
        }

        /// <summary>
        ///     Starts the audio processing thread.
        /// </summary>
        /// <returns>HRESULT</returns>
        public abstract int StartEngineNative();

        /// <summary>
        ///     Starts the audio processing thread.
        /// </summary>
        public void StartEngine()
        {
            XAudio2Exception.Try(StartEngineNative(), N, "StartEngine");
        }

        /// <summary>
        ///     Stops the audio processing thread.
        /// </summary>
        public abstract void StopEngine();

        /// <summary>
        ///     Atomically applies a set of operations that are tagged with a given identifier.
        /// </summary>
        /// <param name="operationSet">
        ///     Identifier of the set of operations to be applied. To commit all pending operations, pass
        ///     <see cref="CommitAll" />.
        /// </param>
        /// <returns>HRESULT</returns>
        public abstract int CommitChangesNative(int operationSet);

        /// <summary>
        ///     Atomically applies a set of operations that are tagged with a given identifier.
        /// </summary>
        /// <param name="operationSet">
        ///     Identifier of the set of operations to be applied. To commit all pending operations, pass
        ///     <see cref="CommitAll" />.
        /// </param>
        public void CommitChanges(int operationSet)
        {
            XAudio2Exception.Try(CommitChangesNative(operationSet), N, "CommitChanges");
        }

        /// <summary>
        ///     Atomically applies a set of operations that are tagged with a given identifier.
        /// </summary>
        public void CommitChanges()
        {
            CommitChanges(CommitAll);
        }

        /// <summary>
        ///     Returns current resource usage details, such as available memory or CPU usage.
        /// </summary>
        /// <param name="performanceData">
        ///     On success, pointer to an <see cref="CSCore.XAudio2.PerformanceData" /> structure that is
        ///     returned.
        /// </param>
        /// <returns>HRESULT</returns>
        public abstract void GetPerformanceDataNative(out PerformanceData performanceData);

        /// <summary>
        ///     Changes <b>global</b> debug logging options for XAudio2.
        /// </summary>
        /// <param name="debugConfiguration"><see cref="DebugConfiguration" /> structure that contains the new debug configuration.</param>
        /// <param name="reserved">Reserved parameter. Must me NULL.</param>
        /// <returns>HRESULT</returns>
        public abstract void SetDebugConfigurationNative(DebugConfiguration debugConfiguration, IntPtr reserved);

        /// <summary>
        ///     Changes <b>global</b> debug logging options for XAudio2.
        /// </summary>
        /// <param name="debugConfiguration"><see cref="DebugConfiguration" /> structure that contains the new debug configuration.</param>
        public void SetDebugConfiguration(DebugConfiguration debugConfiguration)
        {
            SetDebugConfigurationNative(debugConfiguration, IntPtr.Zero);
        }

        /// <summary>
        ///     Returns the default device.
        /// </summary>
        /// <returns>The default device.</returns>
        protected abstract object GetDefaultDevice();
    }
}