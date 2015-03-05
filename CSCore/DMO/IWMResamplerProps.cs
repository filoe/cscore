using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/ff819251(v=vs.85).aspx
    [ComImport]
    [Guid("E7E9984F-F09F-4da4-903F-6E2E0EFE56B5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IWMResamplerProps
    {
        /// <summary>
        ///     Specifies the quality of the output.
        /// </summary>
        /// <param name="quality">
        ///     Specifies the quality of the output. The valid range is 1 to 60,
        ///     inclusive.
        /// </param>
        /// <returns>HRESULT</returns>
        int SetHalfFilterLength(int quality);

        //http://msdn.microsoft.com/en-us/library/windows/desktop/ff819252(v=vs.85).aspx
        int SetUserChannelMtx([In] float[] channelConversionMatrix);
    }
}