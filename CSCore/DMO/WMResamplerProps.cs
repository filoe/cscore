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
        }

        /// <summary>
        /// Specifies the quality of the output.
        /// </summary>
        /// <param name="quality">Specifies the quality of the output. The valid range is 1 to 60,
        /// inclusive.</param>
        public void SetHalfFilterLength(int quality)
        {
            if (quality < 1 || quality > 60)
                throw new ArgumentOutOfRangeException("quality");
            DmoException.Try(SetHalfFilterLengthNative(quality), "IWMResamplerProps", "SetHalfFilterLength");
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/ff819252(v=vs.85).aspx
        /// </summary>
        public void SetUserChannelMtx(float[] channelConversitionMatrix)
        {
            if (channelConversitionMatrix == null)
                throw new ArgumentNullException("channelConversitionMatrix");

            DmoException.Try(SetUserChannelMtxNative(channelConversitionMatrix), "IWMResamplerProps", "SetUserChannelMtxNative");
        }

        /// <summary>
        /// Specifies the quality of the output.
        /// </summary>
        /// <param name="quality">Specifies the quality of the output. The valid range is 1 to 60,
        /// inclusive.</param>
        /// <returns>HRESULT</returns>
        public unsafe int SetHalfFilterLengthNative(int quality)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, quality, ((void**)(*(void**)_basePtr))[3]);
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ff819252(v=vs.85).aspx
        /// </summary>
        /// <returns>HRESULT</returns>
        public unsafe int SetUserChannelMtxNative(float[] channelConversitionMatrix)
        {
            fixed (void* pccm = &channelConversitionMatrix[0])
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pccm, ((void**)(*(void**)_basePtr))[4]);
            }
        }
    }
}