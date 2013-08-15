using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    public static class DmoInterop
    {
        [DllImport("msdmo.dll")]
        public static extern int MoInitMediaType([In, Out] ref MediaType mediaType, int formatBlockBytes);

        [DllImport("msdmo.dll")]
        public static extern int MoFreeMediaType([In] ref MediaType mediaType);
    }
}