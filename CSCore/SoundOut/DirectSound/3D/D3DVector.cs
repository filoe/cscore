using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct D3DVector
    {
        public float X;
        public float Y;
        public float Z;

        public D3DVector(float defaultValue)
            : this(defaultValue, defaultValue, defaultValue)
        {
        }

        public D3DVector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}