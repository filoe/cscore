using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CSCore.Win32;

namespace CSCore.MediaFoundation
{
    public static class MFInterops
    {
        [DllImport("mfplat.dll")]
        public static extern int MFCreateMFByteStreamOnStream(IStream stream, out IMFByteStream byteStream);

        public static IMFByteStream IStreamToByteStream(IStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            IMFByteStream result;
            MediaFoundationException.Try(MFCreateMFByteStreamOnStream(stream, out result), "Interops", "MFCreateMFByteStreamOnStreamEx");
            return result;
        }
    }
}
