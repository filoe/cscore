using System;
using System.Collections.Generic;

namespace CSCore.DMO
{
    public static class MediaTypes
    {
        //http://msdn.microsoft.com/en-us/library/windows/desktop/dd317599(v=vs.85).aspx
        public static readonly Guid MediaTypeAudio = new Guid("73647561-0000-0010-8000-00AA00389B71");

        public static readonly Guid MEDIASUBTYPE_PCM = new Guid("00000001-0000-0010-8000-00AA00389B71");
        public static readonly Guid MEDIASUBTYPE_IEEE_FLOAT = new Guid("00000003-0000-0010-8000-00aa00389b71");
        public static readonly Guid WMMEDIASUBTYPE_MP3 = new Guid("00000055-0000-0010-8000-00AA00389B71");

        static Dictionary<AudioEncoding, Guid> _values;

        static MediaTypes()
        {
            _values = new Dictionary<AudioEncoding, Guid>();
            _values.Add(AudioEncoding.WAVE_FORMAT_RAW_AAC1, new System.Guid(0x000000FF, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.WAVE_FORMAT_MPEG_HEAAC, new System.Guid((int)AudioEncoding.WAVE_FORMAT_MPEG_HEAAC, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.WAVE_FORMAT_DOLBY_AC3_SPDIF, new System.Guid((int)AudioEncoding.WAVE_FORMAT_DOLBY_AC3_SPDIF, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.Drm, new System.Guid((int)AudioEncoding.Drm, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.Dts, new System.Guid((int)AudioEncoding.Dts, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.IeeeFloat, new System.Guid((int)AudioEncoding.IeeeFloat, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.MpegLayer3, WMMEDIASUBTYPE_MP3);
            _values.Add(AudioEncoding.Mpeg, new System.Guid((int)AudioEncoding.Mpeg, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.WAVE_FORMAT_WMAVOICE9, new System.Guid((int)AudioEncoding.WAVE_FORMAT_WMAVOICE9, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.Pcm, MEDIASUBTYPE_PCM);
            _values.Add(AudioEncoding.WAVE_FORMAT_WMASPDIF, new System.Guid((int)AudioEncoding.WAVE_FORMAT_WMASPDIF, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.WAVE_FORMAT_WMAUDIO_LOSSLESS, new System.Guid((int)AudioEncoding.WAVE_FORMAT_WMAUDIO_LOSSLESS, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.WAVE_FORMAT_WMAUDIO2, new System.Guid((int)AudioEncoding.WAVE_FORMAT_WMAUDIO2, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
            _values.Add(AudioEncoding.WAVE_FORMAT_WMAUDIO3, new System.Guid((int)AudioEncoding.WAVE_FORMAT_WMAUDIO3, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71));
        }

        public static AudioEncoding EncodingFromMediaType(Guid mediaType)
        {
            foreach (var item in _values)
            {
                if (item.Value == mediaType)
                    return item.Key;
            }
            
            throw new NotSupportedException("Not supported mediatype.");
        }

        public static Guid MediaTypeFromEncoding(AudioEncoding encoding)
        {
            if (_values.ContainsKey(encoding))
                return _values[encoding];
            
            throw new NotSupportedException("Not supported encoding.");
        }
    }
}