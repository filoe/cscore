using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSCore.Utils
{
    internal static class CSCoreUtils
    {
        private static readonly Dictionary<IntPtr, PatchedVtable> _patchedVtables = new Dictionary<IntPtr, PatchedVtable>();

        public static bool IsGreaterEqualCSCore12
        {
            get
            {
                var version = typeof (CSCoreUtils).Assembly.GetName().Version;
                var version12 = new Version(1, 2);
                return version.CompareTo(version12) >= 0;
            }
        }

        public static unsafe IntPtr GetComInterfaceForObjectWithAdjustedVtable(IntPtr ptr, int finalVtableLength, int replaceCount, bool isIUnknown = true)
        {
            var pp = (IntPtr*)(void*)ptr;
            pp = (IntPtr*)pp[0];

            IntPtr z = new IntPtr(pp);

            //since the same vtable applies to all com objects of the same type -> make sure to only patch it once
            if (_patchedVtables.ContainsKey(z))
            {
                return ptr;
            }

            _patchedVtables.Add(z, new PatchedVtable(ptr, pp));

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

        public static int Release(IntPtr ptr)
        {
            PatchedVtable vtable;
            if (_patchedVtables.TryGetValue(ptr, out vtable))
            {
                return vtable.ReleaseFunc(vtable.Ptr);
            }

            return 0;
        }

        internal static void SafeRelease<T>(ref T @object) where T : IDisposable
        {
            var obj = @object;
            if (obj != null)
            {
                obj.Dispose();
            }

            @object = default(T);
        }

        private class PatchedVtable
        {
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate int Release(IntPtr thisPtr);

            public IntPtr Ptr { get; private set; }

            private IntPtr ReleasePtr { get; set; }

            public Release ReleaseFunc { get; private set; }

            public unsafe PatchedVtable(IntPtr thisPtr, IntPtr* ptr)
            {
                Ptr = thisPtr;

                ReleasePtr = ptr[2];
                ReleaseFunc = (Release)Marshal.GetDelegateForFunctionPointer(ReleasePtr, typeof (Release));
            }
        }
    }
}