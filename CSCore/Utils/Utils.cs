using System;
using System.Collections.Generic;

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
                IntPtr prev = pp[i];

                pp[i] = pp[i + replaceCount];

                IntPtr after = pp[i];
                //Console.WriteLine("{0} -> {1}", prev, after); //just for debugging
            }
            return ptr;
        }
    }
}