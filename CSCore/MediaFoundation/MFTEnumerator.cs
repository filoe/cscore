using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Provides the functionality to enumerate Mediafoundation-Transforms.
    /// </summary>
    public static class MFTEnumerator
    {
        /// <summary>
        /// Enumerates Mediafoundation-Transforms that match the specified search criteria.
        /// </summary>
        /// <param name="category">A <see cref="Guid" /> that specifies the category of MFTs to enumerate.
        /// For a list of MFT categories, see <see cref="MFTCategories" />.</param>
        /// <param name="flags">The bitwise OR of zero or more flags from the <see cref="MFTEnumFlags" /> enumeration.</param>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the MFTs.</returns>
        public static IEnumerable<MFActivate> EnumerateTransforms(Guid category, MFTEnumFlags flags)
        {
            IntPtr ptr;
            int count;
            int res = NativeMethods.MFTEnumEx(category, flags, null, null, out ptr, out count);
            try
            {
                MediaFoundationException.Try(res, "Interops", "MFTEnumEx");
                for (int i = 0; i < count; i++)
                {
                    var ptr0 = ptr;
                    var ptr1 = Marshal.ReadIntPtr(new IntPtr(ptr0.ToInt64() + i * Marshal.SizeOf(ptr0)));
                    yield return new MFActivate(ptr1);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }
    }
}