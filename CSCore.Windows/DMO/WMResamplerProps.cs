using System;
using System.Runtime.InteropServices;
using CSCore.DSP;
using CSCore.Win32;

namespace CSCore.DMO
{
    /// <summary>
    ///     Sets properties on the audio resampler DSP.
    /// </summary>
    [Guid("E7E9984F-F09F-4da4-903F-6E2E0EFE56B5")]
    public class WMResamplerProps : ComObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WMResamplerProps" /> class.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public WMResamplerProps(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        ///     Specifies the quality of the output.
        /// </summary>
        /// <param name="quality">
        ///     Specifies the quality of the output. The valid range is 1 to 60,
        ///     inclusive.
        /// </param>
        public void SetHalfFilterLength(int quality)
        {
            if (quality < 1 || quality > 60)
                throw new ArgumentOutOfRangeException("quality");
            DmoException.Try(SetHalfFilterLengthNative(quality), "IWMResamplerProps", "SetHalfFilterLength");
        }

        /// <summary>
        ///     Specifies the channel matrix.
        /// </summary>
        /// <param name="channelConversitionMatrix">An array of floating-point values that represents a channel conversion matrix.</param>
        /// <remarks>
        ///     Use the <see cref="ChannelMatrix" /> class to build the channel-conversation-matrix and its
        ///     <see cref="ChannelMatrix.GetOneDimensionalMatrix" /> method to convert the channel-conversation-matrix into a
        ///     compatible array which can be passed as value for the <paramref name="channelConversitionMatrix" /> parameter.
        ///     For more information,
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff819252(v=vs.85).aspx" />.
        /// </remarks>
        public void SetUserChannelMtx(float[] channelConversitionMatrix)
        {
            if (channelConversitionMatrix == null)
                throw new ArgumentNullException("channelConversitionMatrix");

            DmoException.Try(SetUserChannelMtxNative(channelConversitionMatrix), "IWMResamplerProps",
                "SetUserChannelMtxNative");
        }

        /// <summary>
        ///     Specifies the quality of the output.
        /// </summary>
        /// <param name="quality">
        ///     Specifies the quality of the output. The valid range is 1 to 60,
        ///     inclusive.
        /// </param>
        /// <returns>HRESULT</returns>
        public unsafe int SetHalfFilterLengthNative(int quality)
        {
            return InteropCalls.CalliMethodPtr(UnsafeBasePtr, quality, ((void**) (*(void**) UnsafeBasePtr))[3]);
        }

        /// <summary>
        ///     Specifies the channel matrix.
        /// </summary>
        /// <param name="channelConversitionMatrix">An array of floating-point values that represents a channel conversion matrix.</param>
        /// <returns>HRESULT</returns>
        /// <remarks>
        ///     Use the <see cref="ChannelMatrix" /> class to build the channel-conversation-matrix and its
        ///     <see cref="ChannelMatrix.GetOneDimensionalMatrix" /> method to convert the channel-conversation-matrix into a
        ///     compatible array which can be passed as value for the <paramref name="channelConversitionMatrix" /> parameter.
        ///     For more information,
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff819252(v=vs.85).aspx" />.
        /// </remarks>
        public unsafe int SetUserChannelMtxNative(float[] channelConversitionMatrix)
        {
            fixed (void* pccm = &channelConversitionMatrix[0])
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, pccm, ((void**) (*(void**) UnsafeBasePtr))[4]);
            }
        }
    }
}