using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSCore.Utils
{
    internal static class Utils
    {
        private static readonly List<IntPtr> _patchedVtables = new List<IntPtr>();

        public unsafe static IntPtr GetComInterfaceForObjectWithAdjustedVtable(IntPtr ptr, int finalVtableLength, int replaceCount)
        {
            var pp = (IntPtr*)(void*)ptr;
            pp = (IntPtr*)pp[0];

            IntPtr z = new IntPtr(pp);

            //since the same vtable applies to all com objects of the same type -> make sure to only patch it once
            if (_patchedVtables.Contains(z))
            {
                return ptr;
            }

            _patchedVtables.Add(z);

            for (int i = 0; i < finalVtableLength; i++)
            {
#if DEBUG
                IntPtr prev = pp[i];
#endif
                pp[i] = pp[i + replaceCount];

#if DEBUG
                IntPtr after = pp[i];
                System.Diagnostics.Debug.WriteLine(String.Format("{0} -> {1}", prev, after));
#endif
            }
            return ptr;
        }
    }
}