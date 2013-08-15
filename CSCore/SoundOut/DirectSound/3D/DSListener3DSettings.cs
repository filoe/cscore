using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DSListener3DSettings
    {
        public int dwSize;
        public D3DVector Position;
        public D3DVector Velocity;
        public D3DVector OrientFront;
        public D3DVector OrientTop;
        public float DistanceFactor;
        public float RolloffFactor;
    }
}