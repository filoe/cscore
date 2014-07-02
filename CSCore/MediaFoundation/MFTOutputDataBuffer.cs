using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MFTOutputDataBuffer
    {
        public int StreamID;

        [MarshalAs(UnmanagedType.IUnknown)]
        public IMFSample Sample;

        public int Status;

        [MarshalAs(UnmanagedType.IUnknown)]
        public IMFCollection Events;
    }
}