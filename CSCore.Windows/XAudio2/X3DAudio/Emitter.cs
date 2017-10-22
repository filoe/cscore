using System;
using System.Runtime.InteropServices;
using CSCore.Utils;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    /// Defines a single-point or multiple-point 3D audio source that is used with an arbitrary number of sound channels.
    /// </summary>
    public class Emitter
    {
        internal EmitterNative NativeInstance = new EmitterNative();

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct EmitterNative
        {
            public IntPtr ConePtr;
            public Vector3 OrientFront;
            public Vector3 OrientTop;
            public Vector3 Position;
            public Vector3 Velocity;

            public float InnerRadius;
            public float InnerRadiusAngle;
            public int ChannelCount;
            public float ChannelRadius;
            public IntPtr ChannelAzimuthsPtr;

            public IntPtr VolumeCurvePtr;
            public IntPtr LFECurvePtr;
            public IntPtr LPFDirectCurvePtr;
            public IntPtr LPFReverbCurvePtr;
            public IntPtr ReverbCurvePtr;

            public float CurveDistanceScaler;
            public float DopplerScaler;

            public void FreeMemory()
            {
                if (ChannelAzimuthsPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ChannelAzimuthsPtr);
                    ChannelAzimuthsPtr = IntPtr.Zero;
                }
                if (VolumeCurvePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(VolumeCurvePtr);
                    VolumeCurvePtr = IntPtr.Zero;
                }
                if (LFECurvePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(LFECurvePtr);
                    LFECurvePtr = IntPtr.Zero;
                }
                if (LPFDirectCurvePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(LPFDirectCurvePtr);
                    LPFDirectCurvePtr = IntPtr.Zero;
                }
                if (LPFReverbCurvePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(LPFReverbCurvePtr);
                    LPFReverbCurvePtr = IntPtr.Zero;
                }
                if (ReverbCurvePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ReverbCurvePtr);
                    ReverbCurvePtr = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Gets or sets the sound cone. Used only with single-channel emitters for matrix, LPF (both direct and reverb paths), and reverb calculations. NULL specifies the emitter is omnidirectional.
        /// </summary>
        public Cone? Cone { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the front direction. This value must be orthonormal with <see cref="OrientTop"/>. <see cref="OrientFront"/> must be normalized when used. For single-channel emitters without cones <see cref="OrientFront"/> is only used for emitter angle calculations. For multi channel emitters or single-channel with cones <see cref="OrientFront"/> is used for matrix, LPF (both direct and reverb paths), and reverb calculations.
        /// </summary>
        public Vector3 OrientFront
        {
            get { return NativeInstance.OrientFront; }
            set { NativeInstance.OrientFront = value; }
        }

        /// <summary>
        /// Gets or sets the orientation of the top direction. This value must be orthonormal with <see cref="OrientFront"/>. <see cref="OrientTop"/> is only used with multi-channel emitters for matrix calculations.
        /// </summary>
        public Vector3 OrientTop
        {
            get { return NativeInstance.OrientTop; }
            set { NativeInstance.OrientTop = value; }
        }

        /// <summary>
        /// Gets or sets the position in user-defined world units. This value does not affect <see cref="Velocity"/>.
        /// </summary>
        public Vector3 Position
        {
            get { return NativeInstance.Position; }
            set { NativeInstance.Position = value; }
        }

        /// <summary>
        /// Gets or sets the velocity vector in user-defined world units/second. This value is used only for doppler calculations. It does not affect <see cref="Position"/>.
        /// </summary>
        public Vector3 Velocity
        {
            get { return NativeInstance.Velocity; }
            set { NativeInstance.Velocity = value; }
        }

        /// <summary>
        /// Gets or sets the value to be used for the inner radius calculations. If <see cref="InnerRadius"/> is 0, then no inner radius is used, but <see cref="InnerRadiusAngle"/> may still be used. This value must be between 0.0f and FLT_MAX.
        /// </summary>
        public float InnerRadius
        {
            get { return NativeInstance.InnerRadius; }
            set { NativeInstance.InnerRadius = value; }
        }

        /// <summary>
        /// Gets or sets the value to be used for the inner radius angle calculations. This value must be between 0.0f and <see cref="Math.PI"/>/4.0 (which equals 45°).
        /// </summary>
        public float InnerRadiusAngle
        {
            get { return NativeInstance.InnerRadiusAngle; }
            set { NativeInstance.InnerRadiusAngle = value; }
        }

        /// <summary>
        /// Gets or sets the number of emitters defined by the <see cref="Emitter"/> class. Must be greater than 0.
        /// </summary>
        public int ChannelCount
        {
            get { return NativeInstance.ChannelCount; }
            set { NativeInstance.ChannelCount = value; }
        }

        /// <summary>
        /// Gets or sets the distance from <see cref="Position"/> that channels will be placed if <see cref="ChannelCount"/> is greater than 1. <see cref="ChannelRadius"/> is only used with multi-channel emitters for matrix calculations. Must be greater than or equal to 0.0f.
        /// </summary>
        public float ChannelRadius
        {
            get { return NativeInstance.ChannelRadius; }
            set { NativeInstance.ChannelRadius = value; }
        }

        /// <summary>
        /// Gets or sets the table of channel positions, expressed as an azimuth in radians along the channel radius with respect to the front orientation vector in the plane orthogonal to the top orientation vector. An azimuth of 2*<see cref="Math.PI"/> specifies a channel is a low-frequency effects (LFE) channel. LFE channels are positioned at the emitter base and are calculated with respect to <see cref="LowFrequencyEffectCurve"/> only, never <see cref="VolumeCurve"/>. <see cref="ChannelAzimuths"/> must have at least <see cref="ChannelCount"/> elements, but can be NULL if <see cref="ChannelCount"/> = 1. The table values must be within 0.0f to 2*<see cref="Math.PI"/>. <see cref="ChannelAzimuths"/> is used with multi-channel emitters for matrix calculations.
        /// </summary>
        public float[] ChannelAzimuths { get; set; }

        /// <summary>
        /// Gets or sets the volume-level distance curve, which is used only for matrix calculations. NULL specifies a specialized default curve that conforms to the inverse square law, such that when distance is between 0.0f and <see cref="CurveDistanceScaler" />× 1.0f, no attenuation is applied. When distance is greater than <see cref="CurveDistanceScaler" />× 1.0f, the amplification factor is (<see cref="CurveDistanceScaler" />× 1.0f)/distance. At a distance of <see cref="CurveDistanceScaler" />× 2.0f, the sound will be at half volume or -6 dB, at a distance of <see cref="CurveDistanceScaler" />× 4.0f, the sound will be at one quarter volume or -12 dB, and so on. <see cref="VolumeCurve" /> and <see cref="LowFrequencyEffectCurve"/> are independent of each other. <see cref="VolumeCurve" /> does not affect LFE channel volume.
        /// </summary>
        public CurvePoint[] VolumeCurve { get; set; }

        /// <summary>
        /// Gets or sets the LFE roll-off distance curve, or NULL to use default curve: [0.0f, <see cref="CurveDistanceScaler" /> ×1.0f], [<see cref="CurveDistanceScaler" /> ×1.0f, 0.0f]. A NULL value for <see cref="LowFrequencyEffectCurve"/> specifies a default curve that conforms to the inverse square law with distances &lt;= <see cref="CurveDistanceScaler" /> clamped to no attenuation. <see cref="VolumeCurve" /> and <see cref="LowFrequencyEffectCurve"/> are independent of each other. <see cref="LowFrequencyEffectCurve"/> does not affect non LFE channel volume.
        /// </summary>
        public CurvePoint[] LowFrequencyEffectCurve { get; set; }

        /// <summary>
        /// Gets or sets the low-pass filter (LPF) direct-path coefficient distance curve, or NULL to use the default curve: [0.0f, 1.0f], [1.0f, 0.75f]. <see cref="LowPassFilterDirectCurve"/> is only used for LPF direct-path calculations.
        /// </summary>
        public CurvePoint[] LowPassFilterDirectCurve { get; set; }

        /// <summary>
        /// Gets or sets the LPF reverb-path coefficient distance curve, or NULL to use default curve: [0.0f, 0.75f], [1.0f, 0.75f]. <see cref="LowPassFilterReverbCurve"/> is only used for LPF reverb path calculations.
        /// </summary>
        public CurvePoint[] LowPassFilterReverbCurve { get; set; }

        /// <summary>
        /// Gets or sets the reverb send level distance curve, or NULL to use default curve: [0.0f, 1.0f], [1.0f, 0.0f].
        /// </summary>
        public CurvePoint[] ReverbCurve;

        /// <summary>
        /// Gets or sets the curve distance scaler that is used to scale normalized distance curves to user-defined world units, and/or to exaggerate their effect. This does not affect any other calculations. The value must be within the range FLT_MIN to FLT_MAX. <see cref="CurveDistanceScaler"/> is only used for matrix, LPF (both direct and reverb paths), and reverb calculations.
        /// </summary>
        public float CurveDistanceScaler
        {
            get { return NativeInstance.CurveDistanceScaler; }
            set { NativeInstance.CurveDistanceScaler = value; }
        }

        /// <summary>
        /// Doppler shift scaler that is used to exaggerate Doppler shift effect. <see cref="DopplerScaler"/> is only used for Doppler calculations and does not affect any other calculations. The value must be within the range 0.0f to FLT_MAX.
        /// </summary>
        public float DopplerScaler
        {
            get { return NativeInstance.DopplerScaler; }
            set { NativeInstance.DopplerScaler = value; }
        }
    }
}