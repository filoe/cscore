using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DSBuffer3DSettings
    {
        public int Size;
        public D3DVector Position;
        public D3DVector Velocity;
        public int InsideConeAngle;
        public int OutsideConeAngle;
        public D3DVector ConeOrientation;
        public int ConeOutsideVolume;
        public float MinDistance;
        public float MaxDistance;
        public DSMode3D Mode;
    }
}