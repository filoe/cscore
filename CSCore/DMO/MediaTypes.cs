using System;

namespace CSCore.DMO
{
    public static class MediaTypes
    {
        //http://msdn.microsoft.com/en-us/library/windows/desktop/dd317599(v=vs.85).aspx
        public static readonly Guid MediaTypeAudio = new Guid("73647561-0000-0010-8000-00AA00389B71");

        public static readonly Guid MEDIASUBTYPE_PCM = new Guid("00000001-0000-0010-8000-00AA00389B71");
        public static readonly Guid MEDIASUBTYPE_IEEE_FLOAT = new Guid("00000003-0000-0010-8000-00aa00389b71");
        public static readonly Guid WMMEDIASUBTYPE_MP3 = new Guid("00000055-0000-0010-8000-00AA00389B71");

        public static AudioEncoding EncodingFromMediaType(Guid mediaType)
        {
            if (mediaType == MEDIASUBTYPE_PCM)
            {
                return AudioEncoding.Pcm;
            }
            else if (mediaType == MEDIASUBTYPE_IEEE_FLOAT)
            {
                return AudioEncoding.IeeeFloat;
            }
            else if (mediaType == WMMEDIASUBTYPE_MP3)
            {
                return AudioEncoding.MpegLayer3;
            }
            else
            {
                throw new NotSupportedException("Not supported mediatype.");
            }
        }
    }
}