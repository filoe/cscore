using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    internal class DmoInterop
    {
        private const string dll = "msdmo.dll";

        [DllImport(dll)]
        public static extern int DMOEnum
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