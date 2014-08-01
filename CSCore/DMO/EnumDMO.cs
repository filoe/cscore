using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.DMO
{
    /// <summary>
    ///     Provides methods for enumerating Microsoft DirectX Media Objects.
    /// </summary>
    [Guid("2c3cd98a-2bfa-4a53-9c27-5249ba64ba0f")]
    public class EnumDmo : ComObject
    {
        private const string InterfaceName = "IEnumDMO";

        /// <summary>
        ///     Creates a new DMO Enumerator based on its pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public EnumDmo(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Enumerates DMOs listed in the registry. The caller can search by category, media type, or both.
        /// </summary>
        /// <param name="category">
        ///     GUID that specifies which category of DMO to search. Use Guid.Empty to search every category.
        ///     See <see cref="DmoEnumeratorCategories" /> for a list of category guids.
        /// </param>
        /// <param name="flags">Flags that specify search criteria.</param>
        /// <param name="inputTypes">Array of input-Mediatypes.</param>
        /// <param name="outputTypes">Array of output-Mediatypes.</param>
        /// <returns>EnumDMO</returns>
        public static EnumDmo EnumerateDMOs(Guid category, DmoEnumFlags flags, DmoPartialMediaType[] inputTypes,
            DmoPartialMediaType[] outputTypes)
        {
            IntPtr ptr;
            int result = NativeMethods.DMOEnum(ref category, flags, inputTypes != null ? inputTypes.Length : 0, inputTypes,
                outputTypes != null ? inputTypes.Length : 0, outputTypes, out ptr);
            DmoException.Try(result, "Interops", "DMOEnum");

            return new EnumDmo(ptr);
        }

        /// <summary>
        ///     Enumerates DMOs listed in the registry.
        /// </summary>
        /// <param name="category">
        ///     GUID that specifies which category of DMO to search. Use Guid.Empty to search every category.
        ///     See <see cref="DmoEnumeratorCategories" /> for a list of category guids.
        /// </param>
        /// <param name="flags">Flags that specify search criteria.</param>
        public static IEnumerable<DmoEnumItem> EnumerateDMOs(Guid category, DmoEnumFlags flags)
        {
            using (EnumDmo enumerator = EnumerateDMOs(category, flags, null, null))
            {
                var item = new DmoEnumItem[1];
                while ((item = enumerator.Next(1)).Length > 0)
                {
                    yield return item[0];
                }
            }
        }

        /// <summary>
        ///     Retrieves a specified number of items in the enumeration sequence.
        /// </summary>
        /// <param name="itemsToFetch">Number of items to retrieve.</param>
        /// <param name="clsids">Array that is filled with the CLSIDs of the enumerated DMOs.</param>
        /// <param name="names">Array that is filled with the friendly names of the enumerated DMOs.</param>
        /// <param name="itemsFetched">Actual number of items retrieved.</param>
        /// <returns>HRESULT</returns>
        public unsafe int NextNative(int itemsToFetch, out Guid[] clsids, out string[] names, out int itemsFetched)
        {
            if (itemsToFetch <= 0)
                throw new ArgumentOutOfRangeException("itemsToFetch");

            int result = 0;
            clsids = new Guid[itemsToFetch];
            names = new string[itemsToFetch];

            var pnames = new IntPtr[itemsToFetch];

            fixed (void* p1 = &clsids[0], p2 = &pnames[0], p3 = &itemsFetched)
            {
                result = InteropCalls.CalliMethodPtr(UnsafeBasePtr, itemsToFetch, p1, p2, p3,
                    ((void**) (*(void**) UnsafeBasePtr))[3]);
            }

            if (result != (int) HResult.S_FALSE && result != (int) HResult.S_OK)
                return result;

            for (int i = 0; i < itemsFetched; i++)
            {
                names[i] = Marshal.PtrToStringUni(pnames[i]);
                Marshal.FreeCoTaskMem(pnames[i]);
            }

            return result;
        }

        /// <summary>
        ///     Retrieves a specified number of items in the enumeration sequence.
        /// </summary>
        /// <param name="itemsToFetch">Number of items to retrieve.</param>
        /// <returns>Array of enumerated DMOs.</returns>
        public DmoEnumItem[] Next(int itemsToFetch)
        {
            Guid[] clsids;
            string[] names;
            int itemsFetched;

            int result = NextNative(itemsToFetch, out clsids, out names, out itemsFetched);
            if (result != (int) HResult.S_FALSE && result != (int) HResult.S_OK)
                DmoException.Try(result, InterfaceName, "Next");

            var items = new DmoEnumItem[itemsFetched];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new DmoEnumItem
                {
                    CLSID = clsids[i],
                    Name = names[i]
                };
            }

            return items;
        }

        //---

        /// <summary>
        ///     Skips over a specified number of items in the enumeration sequence.
        /// </summary>
        /// <param name="itemsToSkip">Number of items to skip.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SkipNative(int itemsToSkip)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, itemsToSkip, ((void**) (*(void**) UnsafeBasePtr))[4]);
        }

        /// <summary>
        ///     Skips over a specified number of items in the enumeration sequence.
        /// </summary>
        /// <param name="itemsToSkip">Number of items to skip.</param>
        public void Skip(int itemsToSkip)
        {
            DmoException.Try(SkipNative(itemsToSkip), InterfaceName, "Skip");
        }

        //---

        /// <summary>
        ///     Resets the enumeration sequence to the beginning.
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int ResetNative()
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, ((void**) (*(void**) UnsafeBasePtr))[5]);
        }

        /// <summary>
        ///     Resets the enumeration sequence to the beginning.
        /// </summary>
        public void Reset()
        {
            DmoException.Try(ResetNative(), InterfaceName, "Reset");
        }

        //---

        /// <summary>
        ///     This method is not implemented.
        /// </summary>
        public unsafe int CloneNative(out IntPtr pEnum)
        {
            fixed (void* p = &pEnum)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, p, ((void**) (*(void**) UnsafeBasePtr))[6]);
            }
        }

        /// <summary>
        ///     This method is not implemented.
        /// </summary>
        public EnumDmo Clone()
        {
            IntPtr p;
            DmoException.Try(CloneNative(out p), InterfaceName, "Clone");
            return new EnumDmo(p);
        }
    }
}