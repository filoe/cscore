using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Utils
{
    /// <summary>
    /// Defines a 3D vector.
    /// </summary>
    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        /// <summary>
        /// Retrieves or sets the x component of the 3D vector.
        /// </summary>
        public float X;
        /// <summary>
        /// Retrieves or sets the y component of the 3D vector.
        /// </summary>
        public float Y;
        /// <summary>
        /// Retrieves or sets the z component of the 3D vector.
        /// </summary>
        public float Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="value">The value to use for the x, y and z component of the 3D vector.</param>
        public Vector3(float value)
            : this(value, value, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="x">The x component of the 3D vector.</param>
        /// <param name="y">The y component of the 3D vector..</param>
        /// <param name="z">The z component of the 3D vector.</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns a string that represents the 3D vector.
        /// </summary>
        /// <returns>A string that represents the 3D vector.</returns>
        public override string ToString()
        {
            return "{" + X + ";" + Y + ";" + Z + "}";
        }

        //TODO: Add some mathematical stuff.
    }
}
