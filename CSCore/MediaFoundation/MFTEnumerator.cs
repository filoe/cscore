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
        /// <param name="inputType">Specifies an input media type to match. This parameter can be <c>null</c>. If <c>null</c>, all input types are matched.</param>
        /// <param name="outputType">Specifies an output media type to match. This parameter can be <c>null</c>. If <c>null</c>, all output types are matched.</param>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the MFTs.</returns>
        public static IEnumerable<MFActivate> EnumerateTransformsEx(Guid category, MFTEnumFlags flags, MFTRegisterTypeInfo inputType = null, MFTRegisterTypeInfo outputType = null)
        {
            if(!(Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1))
                throw new PlatformNotSupportedException("The EnumerateTransformsEx method requires Windows 7/Windows Server 2008 R2 or above.");

            IntPtr ptr;
            int count;
            int res = NativeMethods.MFTEnumEx(category, flags, inputType, outputType, out ptr, out count);
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

        /// <summary>
        /// Enumerates Media Foundation transforms (MFTs) in the registry.
        /// </summary>
        /// <param name="category">A <see cref="Guid" /> that specifies the category of MFTs to enumerate.
        /// For a list of MFT categories, see <see cref="MFTCategories" />.</param>
        /// <param name="inputType">Specifies an input media type to match. This parameter can be <c>null</c>. If <c>null</c>, all input types are matched.</param>
        /// <param name="outputType">Specifies an output media type to match. This parameter can be <c>null</c>. If <c>null</c>, all output types are matched.</param>
        /// <returns>An array of CLSIDs. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms701774%28v=vs.85%29.aspx"/>.</returns>
        /// <remarks>On Windows 7/Windows Server 2008 R2, use the <see cref="EnumerateTransformsEx"/> method instead.</remarks>
        public static unsafe Guid[] EnumerateTransforms(Guid category, MFTRegisterTypeInfo inputType = null, MFTRegisterTypeInfo outputType = null)
        {
            IntPtr ptr;
            int count;
            int res = NativeMethods.MFTEnum(category, 0, inputType, outputType, IntPtr.Zero, out ptr, out count);

            try
            {
                MediaFoundationException.Try(res, "Interops", "MFTEnum");
                Guid[] result = new Guid[count];
                Guid* p = (Guid*)ptr;
                for (int i = 0; i < count; i++)
                {
                    result[i] = p[i];
                }
                return result;
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }
    }
}