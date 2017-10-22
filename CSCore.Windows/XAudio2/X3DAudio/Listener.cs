using System;
using System.Runtime.InteropServices;
using CSCore.Utils;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    ///     Defines a point of 3D audio reception.
    /// </summary>
    /// <remarks>
    ///     A listener's front and top vectors must be orthonormal. To be considered orthonormal, a pair of vectors must
    ///     have a magnitude of 1 +- 1x10-5 and a dot product of 0 +- 1x10-5.
    /// </remarks>
    public class Listener
    {
        internal ListenerNative NativeInstance = new ListenerNative();

        /// <summary>
        ///     Gets or sets the orientation of front direction. When <see cref="Cone" /> is NULL OrientFront is used only for
        ///     matrix and delay calculations. When <see cref="Cone" /> is not NULL OrientFront is used for matrix, LPF (both
        ///     direct and reverb paths), and reverb calculations. This value must be orthonormal with <see cref="OrientTop" />
        ///     when used.
        /// </summary>
        public Vector3 OrientFront
        {
            get { return NativeInstance.OrientFront; }
            set { NativeInstance.OrientFront = value; }
        }

        /// <summary>
        ///     Gets or sets the orientation of top direction, used only for matrix and delay calculations. This value must be
        ///     orthonormal with <see cref="OrientFront" /> when used.
        /// </summary>
        public Vector3 OrientTop
        {
            get { return NativeInstance.OrientTop; }
            set { NativeInstance.OrientTop = value; }
        }

        /// <summary>
        ///     Gets or sets the position in user-defined world units. This value does not affect <see cref="Velocity" />.
        /// </summary>
        public Vector3 Position
        {
            get { return NativeInstance.Position; }
            set { NativeInstance.Position = value; }
        }

        /// <summary>
        ///     Gets or sets the velocity vector in user-defined world units per second, used only for doppler calculations. This
        ///     value does not affect <see cref="Position" />.
        /// </summary>
        public Vector3 Velocity
        {
            get { return NativeInstance.Velocity; }
            set { NativeInstance.Velocity = value; }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Cone" /> to use. Providing a listener cone will specify that additional calculations
        ///     are performed when determining the volume and filter DSP parameters for individual sound sources. A NULL
        ///     <see cref="Cone" /> value specifies an omnidirectional sound and no cone processing is applied. <see cref="Cone" />
        ///     is only used for matrix, LPF (both direct and reverb paths), and reverb calculations.
        /// </summary>
        public Cone? Cone { get; set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct ListenerNative
        {
            public Vector3 OrientFront;
            public Vector3 OrientTop;
            public Vector3 Position;
            public Vector3 Velocity;

            public IntPtr ConePtr;
        }
    }
}