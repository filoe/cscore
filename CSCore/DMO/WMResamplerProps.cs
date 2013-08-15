using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    [Guid("E7E9984F-F09F-4da4-903F-6E2E0EFE56B5")]
    public class WMResamplerProps : ComObject
    {
        public WMResamplerProps(IntPtr ptr)
            : base(ptr)
        {
            System.Diagnostics.Debug.WriteLine("WMResamplerProps created");
        }

        /// <summary>
        /// Specifies the quality of the output.
        /// </summary>
        /// <param name="quality">Specifies the quality of the output. The valid range is 1 to 60,
        /// inclusive.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetHalfFilterLength(int quality)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, quality, ((void**)(*(void**)_basePtr))[3]);
        }

        /// <summary>
        /// http: //msdn.microsoft.com/en-us/library/windows/desktop/ff819252(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUserChannelMtx(float[] channelConverstionMatrix)
        {
            fixed (void* pccm = &channelConverstionMatrix[0])
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pccm, ((void**)(*(void**)_basePtr))[4]);
            }
        }
    }
}