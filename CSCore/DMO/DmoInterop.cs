using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
