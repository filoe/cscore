using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    internal static class NativeMethods
    {
        [DllImport("msdmo.dll")]
        public static extern int MoInitMediaType([In, Out] ref MediaType mediaType, int formatBlockBytes);

        [DllImport("msdmo.dll")]
        public static extern int MoFreeMediaType([In] ref MediaType mediaType);
    }
}