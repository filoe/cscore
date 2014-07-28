using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    /// Receives the results from a call to <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)"/>. 
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.x3daudio.x3daudio_dsp_settings%28v=vs.85%29.aspx for more details.
    /// </summary>
    public sealed class DspSettings
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct DspSettingsNative
        {
            /// <summary>
            ///     Caller provided array that will be initialized with the volume level of each source channel present in each
            ///     destination channel. The array must have at least (<see cref="SrcChannelCount" /> × <see cref="DstChannelCount" />)
            ///     elements. The array is arranged with the source channels as the column index of the array and the destination
            ///     channels as the row index of the array.
            /// </summary>
            public IntPtr MatrixCoefficientsPtr;

            /// <summary>
            ///     Caller provided delay time array, which receives delays for each destination channel in milliseconds. This array
            ///     must have at least <see cref="DstChannelCount" /> elements. X3DAudio doesn't actually perform the delay. It simply
            ///     returns the
            ///     coefficients that may be used to adjust a delay DSP effect placed in the effect chain. The
            ///     <see cref="DelayTimesPtr" /> member can
            ///     be NULL if the <see cref="CalculateFlags.Delay" /> flag is not specified when calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)"/>.
            ///     Note  This member is only returned when X3DAudio is initialized for stereo output. For typical Xbox 360 usage, it
            ///     will not return any data at all.
            /// </summary>
            public IntPtr DelayTimesPtr;

            /// <summary>
            ///     Number of source channels. This must be initialized to the number of emitter channels before calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public int SrcChannelCount;

            /// <summary>
            ///     Number of source channels. This must be initialized to the number of emitter channels before calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public int DstChannelCount;

            /// <summary>
            ///     LPF direct-path coefficient. Only calculated if the <see cref="CalculateFlags.LpfDirect" /> flag is specified when
            ///     calling <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            ///     When using X3DAudio with XAudio2 the value returned in the LPFDirectCoefficient member would be applied to a low
            ///     pass filter on a source voice with <see cref="XAudio2Voice.SetFilterParameters" />.
            /// </summary>
            public float LPFDirectCoefficient;

            /// <summary>
            ///     LPF reverb-path coefficient. Only calculated if the <see cref="CalculateFlags.LpfReverb" /> flag is specified when
            ///     calling <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public float LPFReverbCoefficient;

            /// <summary>
            ///     Reverb send level. Only calculated if the <see cref="CalculateFlags.Reverb" /> flag is specified when calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public float ReverbLevel;

            /// <summary>
            ///     Doppler shift factor. Scales the resampler ratio for Doppler shift effect, where:
            ///     <code>effective_frequency = DopplerFactor × original_frequency</code>.
            ///     Only calculated if the <see cref="CalculateFlags.Doppler" /> flag is specified when calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            ///     When using X3DAudio with XAudio2 the value returned in the DopplerFactor would be applied to a source voice with
            ///     <see cref="XAudio2SourceVoice.SetFrequencyRatio(float)" />.
            /// </summary>
            public float DopplerFactor;

            /// <summary>
            ///     Emitter-to-listener interior angle, expressed in radians with respect to the emitter's front orientation.
            ///     Only calculated if the <see cref="CalculateFlags.EmitterAngle" /> flag is specified when calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public float EmitterToListenerAngle;

            /// <summary>
            ///     Distance in user-defined world units from the listener to the emitter base position.
            /// </summary>
            public float EmitterToListenerDistance;

            /// <summary>
            ///     Component of emitter velocity vector projected onto emitter-to-listener vector in user-defined world units per
            ///     second.
            ///     Only calculated if the <see cref="CalculateFlags.Doppler" /> flag is specified when calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public float EmitterVelocityComponent;

            /// <summary>
            ///     Component of listener velocity vector projected onto the emitter->listener vector in user-defined world units per
            ///     second. Only calculated if the <see cref="CalculateFlags.Doppler" /> flag is specified when calling
            ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
            /// </summary>
            public float ListenerVelocityComponent;
        }

        internal DspSettingsNative NativeInstance;

        /// <summary>
        ///     Gets the caller provided array that will be initialized with the volume level of each source channel present in each
        ///     destination channel. The array must have at least (<see cref="SrcChannelCount" /> × <see cref="DstChannelCount" />)
        ///     elements. The array is arranged with the source channels as the column index of the array and the destination
        ///     channels as the row index of the array.
        /// </summary>
        public float[] MatrixCoefficients { get; private set; }

        /// <summary>
        ///     Gets the caller provided delay time array, which receives delays for each destination channel in milliseconds. This array
        ///     must have at least <see cref="DstChannelCount" /> elements. X3DAudio doesn't actually perform the delay. It simply
        ///     returns the
        ///     coefficients that may be used to adjust a delay DSP effect placed in the effect chain. This won't be calculated if the <see cref="CalculateFlags.Delay" /> flag is not specified when calling
        ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        /// </summary>
        public float[] DelayTimes { get; private set; }

        /// <summary>
        ///     Gets the number of source channels.
        /// </summary>
        public int SrcChannelCount
        {
            get { return NativeInstance.SrcChannelCount; }
            private set { NativeInstance.SrcChannelCount = value; }
        }

        /// <summary>
        ///     Gets the number of source channels. 
        /// </summary>
        public int DstChannelCount
        {
            get { return NativeInstance.DstChannelCount; }
            private set { NativeInstance.DstChannelCount = value; }
        }

        /// <summary>
        ///     Gets the LPF direct-path coefficient. Only calculated if the <see cref="CalculateFlags.LpfDirect" /> flag is specified when
        ///     calling <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        ///     When using X3DAudio with XAudio2 the value returned in the LPFDirectCoefficient member would be applied to a low
        ///     pass filter on a source voice with <see cref="XAudio2Voice.SetFilterParameters" />.
        /// </summary>
        public float LPFDirectCoefficient
        {
            get { return NativeInstance.LPFDirectCoefficient; }
        }

        /// <summary>
        ///     Gets the LPF reverb-path coefficient. Only calculated if the <see cref="CalculateFlags.LpfReverb" /> flag is specified when
        ///     calling <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        /// </summary>
        public float LPFReverbCoefficient
        {
            get { return NativeInstance.LPFReverbCoefficient; }
        }

        /// <summary>
        ///     Gets the reverb send level. Only calculated if the <see cref="CalculateFlags.Reverb" /> flag is specified when calling
        ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        /// </summary>
        public float ReverbLevel
        {
            get { return NativeInstance.ReverbLevel; }
        }

        /// <summary>
        ///     Gets the doppler shift factor. Scales the resampler ratio for Doppler shift effect, where:
        ///     <code>effective_frequency = DopplerFactor × original_frequency</code>.
        ///     Only calculated if the <see cref="CalculateFlags.Doppler" /> flag is specified when calling
        ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        ///     When using X3DAudio with XAudio2 the value returned in the DopplerFactor would be applied to a source voice with
        ///     <see cref="XAudio2SourceVoice.SetFrequencyRatio(float)" />.
        /// </summary>
        public float DopplerFactor
        {
            get { return NativeInstance.DopplerFactor; }
        }

        /// <summary>
        ///     Gets the emitter-to-listener interior angle, expressed in radians with respect to the emitter's front orientation.
        ///     Only calculated if the <see cref="CalculateFlags.EmitterAngle" /> flag is specified when calling
        ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        /// </summary>
        public float EmitterToListenerAngle
        {
            get { return NativeInstance.EmitterToListenerAngle; }
        }

        /// <summary>
        ///     Gets the distance in user-defined world units from the listener to the emitter base position.
        /// </summary>
        public float EmitterToListenerDistance
        {
            get { return NativeInstance.EmitterToListenerDistance; }
        }

        /// <summary>
        ///     Gets the component of emitter velocity vector projected onto emitter-to-listener vector in user-defined world units per
        ///     second.
        ///     Only calculated if the <see cref="CalculateFlags.Doppler" /> flag is specified when calling
        ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        /// </summary>
        public float EmitterVelocityComponent
        {
            get { return NativeInstance.EmitterVelocityComponent; }
        }

        /// <summary>
        ///     Gets the component of listener velocity vector projected onto the emitter->listener vector in user-defined world units per
        ///     second. Only calculated if the <see cref="CalculateFlags.Doppler" /> flag is specified when calling
        ///     <see cref="X3DAudioCore.X3DAudioCalculate(Listener,Emitter,CalculateFlags,DspSettings)" />.
        /// </summary>
        public float ListenerVelocityComponent
        {
            get { return NativeInstance.ListenerVelocityComponent; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DspSettings"/> class.
        /// </summary>
        /// <param name="sourceChannelCount">The number of source channels.</param>
        /// <param name="destinationChannelCount">The number of destination channels.</param>
        public DspSettings(int sourceChannelCount, int destinationChannelCount)
        {
            NativeInstance = new DspSettingsNative();
            SrcChannelCount = sourceChannelCount;
            DstChannelCount = destinationChannelCount;
            MatrixCoefficients = new float[sourceChannelCount * destinationChannelCount];
            DelayTimes = new float[destinationChannelCount];
        }
    }
}