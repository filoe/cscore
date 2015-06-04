using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     <see cref="XAudio2" /> is the class for the XAudio2 object that manages all audio engine states, the audio
    ///     processing thread, the voice graph, and so forth.
    /// </summary>
    [Guid("60d8dac8-5aa1-4e8e-b597-2f5e2883d484")]
// ReSharper disable once InconsistentNaming
    public class XAudio2_8 : XAudio2
    {
        /// <summary>
        /// The denominator of a quantum unit. In 10ms chunks (= 1/100 seconds). 
        /// </summary>
        public new const int QuantumDenominator = 100;
        /// <summary>
        /// Minimum sample rate is 1000 Hz.
        /// </summary>
        public new const int MinimumSampleRate = 1000;
        /// <summary>
        /// Maximum sample rate is 200 kHz.
        /// </summary>
        public new const int MaximumSampleRate = 200000;
        /// <summary>
        /// The minimum frequency ratio is 1/1024.
        /// </summary>
        public new const float MinFrequencyRatio = (1/1024.0f);
        /// <summary>
        /// Maximum frequency ratio is 1024.
        /// </summary>
        public new const float MaxFrequencyRatio = 1024.0f;
        /// <summary>
        /// The default value for the frequency ratio is 4.
        /// </summary>
        public new const float DefaultFrequencyRatio = 4.0f;
        /// <summary>
        /// The maximum number of supported channels is 64.
        /// </summary>
        public new const int MaxAudioChannels = 64;
        /// <summary>
        /// Value which indicates that the default number of channels should be used.
        /// </summary>
        public new const int DefaultChannels = 0;
        /// <summary>
        /// Values which indicates that the default sample rate should be used.
        /// </summary>
        public new const int DefaultSampleRate = 0;

        /// <summary>
        /// Value which can be used in combination with the <see cref="XAudio2.CommitChanges(int)"/> method to commit all changes.
        /// </summary>
        public new const int CommitAll = 0;
        /// <summary>
        /// Values which indicates that the made changes should be commited instantly.
        /// </summary>
        public new const int CommitNow = 0;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2_8" /> class.
        /// </summary>
        /// <param name="ptr">Native pointer of the <see cref="XAudio2_8" /> object.</param>
        public XAudio2_8(IntPtr ptr)
            : base(ptr, XAudio2Version.XAudio2_8)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2_8" /> class.
        /// </summary>
        public XAudio2_8()
            : this(XAudio2Processor.Xaudio28DefaultProcessor)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XAudio2_8" /> class.
        /// </summary>
        /// <param name="processor">
        ///     Specifies which CPU to use. Use <see cref="XAudio2Processor.Xaudio28DefaultProcessor" /> as
        ///     default value.
        /// </param>
        public unsafe XAudio2_8(XAudio2Processor processor)
        {
            IntPtr ptr = IntPtr.Zero;
            var pptr = new IntPtr(&ptr);
            int result = NativeMethods.XAudio2Create(pptr, 0, processor);
            XAudio2Exception.Try(result, "Interop", "XAudio2Create");

            Version = XAudio2Version.XAudio2_8;
            BasePtr = ptr;
        }

        /// <summary>
        ///     Adds an <see cref="IXAudio2EngineCallback" /> from the <see cref="XAudio2" /> engine callback list.
        /// </summary>
        /// <param name="callback">
        ///     <see cref="IXAudio2EngineCallback" /> object to add to the <see cref="XAudio2" /> engine
        ///     callback list.
        /// </param>
        /// <returns>HRESULT</returns>
        public override unsafe int RegisterForCallbacksNative(IXAudio2EngineCallback callback)
        {
            IntPtr ptr = IntPtr.Zero;
            if (callback != null)
            {
                ptr = Marshal.GetComInterfaceForObject(callback, typeof (IXAudio2EngineCallback));
                ptr = Utils.Utils.GetComInterfaceForObjectWithAdjustedVtable(ptr, 3, 3);
            }
            return InteropCalls.CallI(UnsafeBasePtr, ptr.ToPointer(), ((void**) (*(void**) UnsafeBasePtr))[3]);
        }

        /// <summary>
        ///     Removes an <see cref="IXAudio2EngineCallback" /> from the <see cref="XAudio2" /> engine callback list.
        /// </summary>
        /// <param name="callback">
        ///     <see cref="IXAudio2EngineCallback" /> object to remove from the <see cref="XAudio2" /> engine
        ///     callback list. If the given interface is present more than once in the list, only the first instance in the list
        ///     will be removed.
        /// </param>
        public override unsafe void UnregisterForCallbacks(IXAudio2EngineCallback callback)
        {
            InteropCalls.CallI6(UnsafeBasePtr, callback, ((void**) (*(void**) UnsafeBasePtr))[4]);
        }

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
        ///     source voice. If <paramref name="sendList" /> is NULL, the send list defaults to a single output to the first mastering
        ///     voice created.
        /// </param>
        /// <param name="effectChain">
        ///     List of <see cref="EffectChain" /> structures that describe an effect chain to use in the
        ///     source voice. This parameter is optional and can be null.
        /// </param>
        /// <returns>HRESULT</returns>
        public override unsafe int CreateSourceVoiceNative(
            out IntPtr pSourceVoice,
            IntPtr sourceFormat,
            VoiceFlags flags,
            float maxFrequencyRatio,
            IXAudio2VoiceCallback voiceCallback,
            VoiceSends? sendList, //out
            EffectChain? effectChain
            )
        {
            VoiceSends value0 = sendList.HasValue ? sendList.Value : new VoiceSends();
            EffectChain value1 = effectChain.HasValue ? effectChain.Value : new EffectChain();

            IntPtr p = IntPtr.Zero;
            if (voiceCallback != null)
            {
                p = Marshal.GetComInterfaceForObject(voiceCallback, typeof (IXAudio2VoiceCallback));
                p = Utils.Utils.GetComInterfaceForObjectWithAdjustedVtable(p, 7, 3);
            }

            fixed (void* ptr = &pSourceVoice)
            {
                return InteropCalls.CallI(UnsafeBasePtr,
                    ptr,
                    sourceFormat,
                    flags,
                    maxFrequencyRatio,
                    p.ToPointer(),
                    sendList.HasValue ? &value0 : (void*) IntPtr.Zero,
                    effectChain.HasValue ? &value1 : (void*) IntPtr.Zero,
                    ((void**) (*(void**) UnsafeBasePtr))[5]);
            }
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
        ///     <paramref name="processingStage" /> value. Voices that include the same <paramref name="processingStage" /> value are
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
        public override unsafe int CreateSubmixVoiceNative(out IntPtr pSubmixVoice, int inputChannels,
            int inputSampleRate, VoiceFlags flags,
            int processingStage, VoiceSends? sendList, EffectChain? effectChain)
        {
            VoiceSends value0 = sendList.HasValue ? sendList.Value : new VoiceSends();
            EffectChain value1 = effectChain.HasValue ? effectChain.Value : new EffectChain();
            fixed (void* ptr = &pSubmixVoice)
            {
                return InteropCalls.CallI(UnsafeBasePtr,
                    ptr,
                    inputChannels,
                    inputSampleRate,
                    flags,
                    processingStage,
                    sendList.HasValue ? &value0 : (void*) IntPtr.Zero,
                    effectChain.HasValue ? &value1 : (void*) IntPtr.Zero,
                    ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        ///     Creates and configures a mastering voice.
        /// </summary>
        /// <param name="pMasteringVoice">If successful, returns a pointer to the new <see cref="XAudio2MasteringVoice" /> object.</param>
        /// <param name="inputChannels">
        ///     Number of channels the mastering voice expects in its input audio. <paramref name="inputChannels" /> must be less than
        ///     or equal to <see cref="MaxAudioChannels" />.
        ///     You can set InputChannels to <see cref="DefaultChannels" />, which causes XAudio2 to try to detect the system
        ///     speaker configuration setup.
        /// </param>
        /// <param name="inputSampleRate">
        ///     Sample rate of the input audio data of the mastering voice. This rate must be a multiple of
        ///     <see cref="QuantumDenominator" />. <paramref name="inputSampleRate" /> must be between <see cref="MinimumSampleRate" />
        ///     and <see cref="MaximumSampleRate" />.
        ///     You can set InputSampleRate to <see cref="DefaultSampleRate" />, with the default being determined by the current
        ///     platform.
        /// </param>
        /// <param name="flags">Flags that specify the behavior of the mastering voice. Must be 0.</param>
        /// <param name="deviceId">
        ///     Identifier of the device to receive the output audio. Specifying the default value of NULL
        ///     causes XAudio2 to select the global default audio device.
        /// </param>
        /// <param name="effectChain">
        ///     <see cref="EffectChain" /> structure that describes an effect chain to use in the mastering
        ///     voice, or NULL to use no effects.
        /// </param>
        /// <param name="streamCategory">The audio stream category to use for this mastering voice.</param>
        /// <returns>HRESULT</returns>
        public override unsafe int CreateMasteringVoiceNative(out IntPtr pMasteringVoice, int inputChannels,
            int inputSampleRate, int flags,
            object deviceId, EffectChain? effectChain, AudioStreamCategory streamCategory)
        {
            if (deviceId != null && !(deviceId is string))
                throw new ArgumentException("DeviceId has to be a string.", "deviceId");

            var device = deviceId as string;
            IntPtr pdeviceId = IntPtr.Zero;
            try
            {
                EffectChain value1 = effectChain.HasValue ? effectChain.Value : new EffectChain();
                if (device != null)
                    pdeviceId = Marshal.StringToHGlobalUni(device);

                fixed (void* ptr = &pMasteringVoice)
                {
                    return InteropCalls.CallI(
                        UnsafeBasePtr,
                        ptr,
                        inputChannels,
                        inputSampleRate,
                        flags,
                        (void*) pdeviceId,
                        effectChain.HasValue ? &value1 : (void*) IntPtr.Zero, 
                        streamCategory,
                        ((void**) (*(void**) UnsafeBasePtr))[7]);
                }
            }
            finally
            {
                if (pdeviceId != IntPtr.Zero)
                    Marshal.FreeHGlobal(pdeviceId);
            }
        }

        /// <summary>
        ///     Starts the audio processing thread.
        /// </summary>
        /// <returns>HRESULT</returns>
        public override unsafe int StartEngineNative()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[8]);
        }

        /// <summary>
        ///     Stops the audio processing thread.
        /// </summary>
        public override unsafe void StopEngine()
        {
            InteropCalls.CallI7(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[9]);
        }

        /// <summary>
        ///     Atomically applies a set of operations that are tagged with a given identifier.
        /// </summary>
        /// <param name="operationSet">
        ///     Identifier of the set of operations to be applied. To commit all pending operations, pass
        ///     <see cref="CommitAll" />.
        /// </param>
        /// <returns>HRESULT</returns>
        public override unsafe int CommitChangesNative(int operationSet)
        {
            return InteropCalls.CallI(UnsafeBasePtr, operationSet, ((void**) (*(void**) UnsafeBasePtr))[10]);
        }

        /// <summary>
        ///     Returns current resource usage details, such as available memory or CPU usage.
        /// </summary>
        /// <param name="performanceData">
        ///     On success, pointer to an <see cref="CSCore.XAudio2.PerformanceData" /> structure that is
        ///     returned.
        /// </param>
        /// <returns>HRESULT</returns>
        public override unsafe void GetPerformanceDataNative(out PerformanceData performanceData)
        {
            performanceData = default(PerformanceData); //initialize performanceData to fix compiler error
            fixed (void* p = &performanceData)
            {
                InteropCalls.CallI5(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[11]);
            }
        }

        /// <summary>
        ///     Changes <b>global</b> debug logging options for XAudio2.
        /// </summary>
        /// <param name="debugConfiguration"><see cref="DebugConfiguration" /> structure that contains the new debug configuration.</param>
        /// <param name="reserved">Reserved parameter. Must me NULL.</param>
        /// <returns>HRESULT</returns>
        public override unsafe void SetDebugConfigurationNative(DebugConfiguration debugConfiguration, IntPtr reserved)
        {
            InteropCalls.CallI4(UnsafeBasePtr, &debugConfiguration, reserved.ToPointer(), ((void**) (*(void**) UnsafeBasePtr))[12]);
        }

        /// <summary>
        /// Returns the default device.
        /// </summary>
        /// <returns>The default device.</returns>
        protected override object GetDefaultDevice()
        {
            return null;
        }
    }
}