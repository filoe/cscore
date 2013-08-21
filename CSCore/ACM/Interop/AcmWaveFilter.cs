using System.Runtime.InteropServices;

namespace CSCore.ACM
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class AcmWaveFilter
    {
        public int cbSize = Marshal.SizeOf(typeof(AcmWaveFilter));
        public int dwFilterTags = 0;
        public int fdwFilter = 0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] dwReserved = null;
    }
}