using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    internal static class NativeMethods
    {
        [DllImport("msdmo.dll")]
        internal static extern int MoInitMediaType([In, Out] ref MediaType mediaType, int formatBlockBytes);

        [DllImport("msdmo.dll")]
        internal static extern int MoFreeMediaType([In] ref MediaType mediaType);

        [DllImport("msdmo.dll")]
        internal static extern int DMOEnum
            (
            ref Guid guidCategory,
            DmoEnumFlags flags,
            int numberOfInputTypes,
            [In] DmoPartialMediaType[] inputTypes,
            int numberOfOutputTypes,
            [In] DmoPartialMediaType[] outputTypes,
            out IntPtr dmoEnum
            );
    }
}