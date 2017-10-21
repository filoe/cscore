#pragma warning disable CS0649
#pragma warning disable IDE1006

using System;
using System.Runtime.InteropServices;

namespace CSCore.Ffmpeg.Interops
{
    internal unsafe partial struct AVRational
    {
        internal int @num;
        internal int @den;
    }
    
    internal unsafe partial struct av_intfloat32
    {
        internal uint @i;
        internal float @f;
    }
    
    internal unsafe partial struct av_intfloat64
    {
        internal ulong @i;
        internal double @f;
    }
    
    internal unsafe partial struct AVOptionRanges
    {
    }
    
    internal unsafe partial struct AVClass
    {
        internal sbyte* @class_name;
        internal IntPtr @item_name;
        internal AVOption* @option;
        internal int @version;
        internal int @log_level_offset_offset;
        internal int @parent_log_context_offset;
        internal IntPtr @child_next;
        internal IntPtr @child_class_next;
        internal AVClassCategory @category;
        internal IntPtr @get_category;
        internal IntPtr @query_ranges;
    }
    
    internal unsafe partial struct AVOption
    {
    }
    
    internal unsafe partial struct AVOptionRanges
    {
    }
    
    internal unsafe partial struct AVFifoBuffer
    {
        internal sbyte* @buffer;
        internal sbyte* @rptr;
        internal sbyte* @wptr;
        internal sbyte* @end;
        internal uint @rndx;
        internal uint @wndx;
    }
    
    internal unsafe partial struct AVAudioFifo
    {
    }
    
    internal unsafe partial struct AVBPrint
    {
    }
    
    internal unsafe partial struct AVOptionRanges
    {
    }
    
    internal unsafe partial struct AVBuffer
    {
    }
    
    internal unsafe partial struct AVBufferRef
    {
        internal AVBuffer* @buffer;
        internal sbyte* @data;
        internal int @size;
    }
    
    internal unsafe partial struct AVBufferPool
    {
    }
    
    internal unsafe partial struct AVDictionaryEntry
    {
        internal sbyte* @key;
        internal sbyte* @value;

        public string Key
        {
            get { return Marshal.PtrToStringAnsi((IntPtr) key); }
        }

        public string Value
        {
            get { return Marshal.PtrToStringAnsi((IntPtr) value); }
        }
    }
    
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVFrameSideData
    {
        internal AVFrameSideDataType @type;
        internal sbyte* @data;
        internal int @size;
        internal AVDictionary* @metadata;
        internal AVBufferRef* @buf;
    }
    
    internal unsafe partial struct AVFrame
    {
        internal sbyte* @data0; internal sbyte* @data1; internal sbyte* @data2; internal sbyte* @data3; internal sbyte* @data4; internal sbyte* @data5; internal sbyte* @data6; internal sbyte* @data7; 
        internal fixed int @linesize[8]; 
        internal byte** @extended_data;
        internal int @width;
        internal int @height;
        internal int @nb_samples;
        internal int @format;
        internal int @key_frame;
        internal AVPictureType @pict_type;
        internal AVRational @sample_aspect_ratio;
        internal long @pts;
        internal long @pkt_pts;
        internal long @pkt_dts;
        internal int @coded_picture_number;
        internal int @display_picture_number;
        internal int @quality;
        internal void* @opaque;
        internal fixed ulong @error[8]; 
        internal int @repeat_pict;
        internal int @interlaced_frame;
        internal int @top_field_first;
        internal int @palette_has_changed;
        internal long @reordered_opaque;
        internal int @sample_rate;
        internal ulong @channel_layout;
        internal AVBufferRef* @buf0; internal AVBufferRef* @buf1; internal AVBufferRef* @buf2; internal AVBufferRef* @buf3; internal AVBufferRef* @buf4; internal AVBufferRef* @buf5; internal AVBufferRef* @buf6; internal AVBufferRef* @buf7; 
        internal AVBufferRef** @extended_buf;
        internal int @nb_extended_buf;
        internal AVFrameSideData** @side_data;
        internal int @nb_side_data;
        internal int @flags;
        internal AVColorRange @color_range;
        internal AVColorPrimaries @color_primaries;
        internal AVColorTransferCharacteristic @color_trc;
        internal AVColorSpace @colorspace;
        internal AVChromaLocation @chroma_location;
        internal long @best_effort_timestamp;
        internal long @pkt_pos;
        internal long @pkt_duration;
        internal AVDictionary* @metadata;
        internal int @decode_error_flags;
        internal int @channels;
        internal int @pkt_size;
        internal sbyte* @qscale_table;
        internal int @qstride;
        internal int @qscale_type;
        internal AVBufferRef* @qp_table_buf;
        internal AVBufferRef* @hw_frames_ctx;
    }
    
    internal unsafe partial struct AVOptionRanges
    {
    }
    
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVOption
    {
        internal sbyte* @name;
        internal sbyte* @help;
        internal int @offset;
        internal AVOptionType @type;
        internal default_val @default_val;
        internal double @min;
        internal double @max;
        internal int @flags;
        internal sbyte* @unit;
    }
    
    internal unsafe partial struct default_val
    {
        internal long @i64;
        internal double @dbl;
        internal sbyte* @str;
        internal AVRational @q;
    }
    
    internal unsafe partial struct AVOptionRange
    {
        internal sbyte* @str;
        internal double @value_min;
        internal double @value_max;
        internal double @component_min;
        internal double @component_max;
        internal int @is_range;
    }
    
    internal unsafe partial struct AVOptionRanges
    {
        internal AVOptionRange** @range;
        internal int @nb_ranges;
        internal int @nb_components;
    }
    
    internal unsafe partial struct AVComponentDescriptor
    {
        internal int @plane;
        internal int @step;
        internal int @offset;
        internal int @shift;
        internal int @depth;
        internal int @step_minus1;
        internal int @depth_minus1;
        internal int @offset_plus1;
    }
    
    internal unsafe partial struct AVPixFmtDescriptor
    {
        internal sbyte* @name;
        internal sbyte @nb_components;
        internal sbyte @log2_chroma_w;
        internal sbyte @log2_chroma_h;
        internal ulong @flags;
        internal AVComponentDescriptor @comp0; internal AVComponentDescriptor @comp1; internal AVComponentDescriptor @comp2; internal AVComponentDescriptor @comp3; 
        internal sbyte* @alias;
    }
    
    internal enum AVMediaType : int
    {
        @AVMEDIA_TYPE_UNKNOWN = -1,
        @AVMEDIA_TYPE_VIDEO = 0,
        @AVMEDIA_TYPE_AUDIO = 1,
        @AVMEDIA_TYPE_DATA = 2,
        @AVMEDIA_TYPE_SUBTITLE = 3,
        @AVMEDIA_TYPE_ATTACHMENT = 4,
        @AVMEDIA_TYPE_NB = 5,
    }
    
    internal enum AVPictureType : int
    {
        @AV_PICTURE_TYPE_NONE = 0,
        @AV_PICTURE_TYPE_I = 1,
        @AV_PICTURE_TYPE_P = 2,
        @AV_PICTURE_TYPE_B = 3,
        @AV_PICTURE_TYPE_S = 4,
        @AV_PICTURE_TYPE_SI = 5,
        @AV_PICTURE_TYPE_SP = 6,
        @AV_PICTURE_TYPE_BI = 7,
    }
    
    internal enum AVRounding : int
    {
        @AV_ROUND_ZERO = 0,
        @AV_ROUND_INF = 1,
        @AV_ROUND_DOWN = 2,
        @AV_ROUND_UP = 3,
        @AV_ROUND_NEAR_INF = 5,
        @AV_ROUND_PASS_MINMAX = 8192,
    }
    
    internal enum AVClassCategory : int
    {
        @AV_CLASS_CATEGORY_NA = 0,
        @AV_CLASS_CATEGORY_INPUT = 1,
        @AV_CLASS_CATEGORY_OUTPUT = 2,
        @AV_CLASS_CATEGORY_MUXER = 3,
        @AV_CLASS_CATEGORY_DEMUXER = 4,
        @AV_CLASS_CATEGORY_ENCODER = 5,
        @AV_CLASS_CATEGORY_DECODER = 6,
        @AV_CLASS_CATEGORY_FILTER = 7,
        @AV_CLASS_CATEGORY_BITSTREAM_FILTER = 8,
        @AV_CLASS_CATEGORY_SWSCALER = 9,
        @AV_CLASS_CATEGORY_SWRESAMPLER = 10,
        @AV_CLASS_CATEGORY_DEVICE_VIDEO_OUTPUT = 40,
        @AV_CLASS_CATEGORY_DEVICE_VIDEO_INPUT = 41,
        @AV_CLASS_CATEGORY_DEVICE_AUDIO_OUTPUT = 42,
        @AV_CLASS_CATEGORY_DEVICE_AUDIO_INPUT = 43,
        @AV_CLASS_CATEGORY_DEVICE_OUTPUT = 44,
        @AV_CLASS_CATEGORY_DEVICE_INPUT = 45,
        @AV_CLASS_CATEGORY_NB = 46,
    }
    
    internal enum AVPixelFormat : int
    {
        @AV_PIX_FMT_NONE = -1,
        @AV_PIX_FMT_YUV420P = 0,
        @AV_PIX_FMT_YUYV422 = 1,
        @AV_PIX_FMT_RGB24 = 2,
        @AV_PIX_FMT_BGR24 = 3,
        @AV_PIX_FMT_YUV422P = 4,
        @AV_PIX_FMT_YUV444P = 5,
        @AV_PIX_FMT_YUV410P = 6,
        @AV_PIX_FMT_YUV411P = 7,
        @AV_PIX_FMT_GRAY8 = 8,
        @AV_PIX_FMT_MONOWHITE = 9,
        @AV_PIX_FMT_MONOBLACK = 10,
        @AV_PIX_FMT_PAL8 = 11,
        @AV_PIX_FMT_YUVJ420P = 12,
        @AV_PIX_FMT_YUVJ422P = 13,
        @AV_PIX_FMT_YUVJ444P = 14,
        @AV_PIX_FMT_XVMC_MPEG2_MC = 15,
        @AV_PIX_FMT_XVMC_MPEG2_IDCT = 16,
        @AV_PIX_FMT_XVMC = 16,
        @AV_PIX_FMT_UYVY422 = 17,
        @AV_PIX_FMT_UYYVYY411 = 18,
        @AV_PIX_FMT_BGR8 = 19,
        @AV_PIX_FMT_BGR4 = 20,
        @AV_PIX_FMT_BGR4_BYTE = 21,
        @AV_PIX_FMT_RGB8 = 22,
        @AV_PIX_FMT_RGB4 = 23,
        @AV_PIX_FMT_RGB4_BYTE = 24,
        @AV_PIX_FMT_NV12 = 25,
        @AV_PIX_FMT_NV21 = 26,
        @AV_PIX_FMT_ARGB = 27,
        @AV_PIX_FMT_RGBA = 28,
        @AV_PIX_FMT_ABGR = 29,
        @AV_PIX_FMT_BGRA = 30,
        @AV_PIX_FMT_GRAY16BE = 31,
        @AV_PIX_FMT_GRAY16LE = 32,
        @AV_PIX_FMT_YUV440P = 33,
        @AV_PIX_FMT_YUVJ440P = 34,
        @AV_PIX_FMT_YUVA420P = 35,
        @AV_PIX_FMT_VDPAU_H264 = 36,
        @AV_PIX_FMT_VDPAU_MPEG1 = 37,
        @AV_PIX_FMT_VDPAU_MPEG2 = 38,
        @AV_PIX_FMT_VDPAU_WMV3 = 39,
        @AV_PIX_FMT_VDPAU_VC1 = 40,
        @AV_PIX_FMT_RGB48BE = 41,
        @AV_PIX_FMT_RGB48LE = 42,
        @AV_PIX_FMT_RGB565BE = 43,
        @AV_PIX_FMT_RGB565LE = 44,
        @AV_PIX_FMT_RGB555BE = 45,
        @AV_PIX_FMT_RGB555LE = 46,
        @AV_PIX_FMT_BGR565BE = 47,
        @AV_PIX_FMT_BGR565LE = 48,
        @AV_PIX_FMT_BGR555BE = 49,
        @AV_PIX_FMT_BGR555LE = 50,
        @AV_PIX_FMT_VAAPI_MOCO = 51,
        @AV_PIX_FMT_VAAPI_IDCT = 52,
        @AV_PIX_FMT_VAAPI_VLD = 53,
        @AV_PIX_FMT_VAAPI = 53,
        @AV_PIX_FMT_YUV420P16LE = 54,
        @AV_PIX_FMT_YUV420P16BE = 55,
        @AV_PIX_FMT_YUV422P16LE = 56,
        @AV_PIX_FMT_YUV422P16BE = 57,
        @AV_PIX_FMT_YUV444P16LE = 58,
        @AV_PIX_FMT_YUV444P16BE = 59,
        @AV_PIX_FMT_VDPAU_MPEG4 = 60,
        @AV_PIX_FMT_DXVA2_VLD = 61,
        @AV_PIX_FMT_RGB444LE = 62,
        @AV_PIX_FMT_RGB444BE = 63,
        @AV_PIX_FMT_BGR444LE = 64,
        @AV_PIX_FMT_BGR444BE = 65,
        @AV_PIX_FMT_YA8 = 66,
        @AV_PIX_FMT_Y400A = 66,
        @AV_PIX_FMT_GRAY8A = 66,
        @AV_PIX_FMT_BGR48BE = 67,
        @AV_PIX_FMT_BGR48LE = 68,
        @AV_PIX_FMT_YUV420P9BE = 69,
        @AV_PIX_FMT_YUV420P9LE = 70,
        @AV_PIX_FMT_YUV420P10BE = 71,
        @AV_PIX_FMT_YUV420P10LE = 72,
        @AV_PIX_FMT_YUV422P10BE = 73,
        @AV_PIX_FMT_YUV422P10LE = 74,
        @AV_PIX_FMT_YUV444P9BE = 75,
        @AV_PIX_FMT_YUV444P9LE = 76,
        @AV_PIX_FMT_YUV444P10BE = 77,
        @AV_PIX_FMT_YUV444P10LE = 78,
        @AV_PIX_FMT_YUV422P9BE = 79,
        @AV_PIX_FMT_YUV422P9LE = 80,
        @AV_PIX_FMT_VDA_VLD = 81,
        @AV_PIX_FMT_GBRP = 82,
        @AV_PIX_FMT_GBR24P = 82,
        @AV_PIX_FMT_GBRP9BE = 83,
        @AV_PIX_FMT_GBRP9LE = 84,
        @AV_PIX_FMT_GBRP10BE = 85,
        @AV_PIX_FMT_GBRP10LE = 86,
        @AV_PIX_FMT_GBRP16BE = 87,
        @AV_PIX_FMT_GBRP16LE = 88,
        @AV_PIX_FMT_YUVA422P = 89,
        @AV_PIX_FMT_YUVA444P = 90,
        @AV_PIX_FMT_YUVA420P9BE = 91,
        @AV_PIX_FMT_YUVA420P9LE = 92,
        @AV_PIX_FMT_YUVA422P9BE = 93,
        @AV_PIX_FMT_YUVA422P9LE = 94,
        @AV_PIX_FMT_YUVA444P9BE = 95,
        @AV_PIX_FMT_YUVA444P9LE = 96,
        @AV_PIX_FMT_YUVA420P10BE = 97,
        @AV_PIX_FMT_YUVA420P10LE = 98,
        @AV_PIX_FMT_YUVA422P10BE = 99,
        @AV_PIX_FMT_YUVA422P10LE = 100,
        @AV_PIX_FMT_YUVA444P10BE = 101,
        @AV_PIX_FMT_YUVA444P10LE = 102,
        @AV_PIX_FMT_YUVA420P16BE = 103,
        @AV_PIX_FMT_YUVA420P16LE = 104,
        @AV_PIX_FMT_YUVA422P16BE = 105,
        @AV_PIX_FMT_YUVA422P16LE = 106,
        @AV_PIX_FMT_YUVA444P16BE = 107,
        @AV_PIX_FMT_YUVA444P16LE = 108,
        @AV_PIX_FMT_VDPAU = 109,
        @AV_PIX_FMT_XYZ12LE = 110,
        @AV_PIX_FMT_XYZ12BE = 111,
        @AV_PIX_FMT_NV16 = 112,
        @AV_PIX_FMT_NV20LE = 113,
        @AV_PIX_FMT_NV20BE = 114,
        @AV_PIX_FMT_RGBA64BE = 115,
        @AV_PIX_FMT_RGBA64LE = 116,
        @AV_PIX_FMT_BGRA64BE = 117,
        @AV_PIX_FMT_BGRA64LE = 118,
        @AV_PIX_FMT_YVYU422 = 119,
        @AV_PIX_FMT_VDA = 120,
        @AV_PIX_FMT_YA16BE = 121,
        @AV_PIX_FMT_YA16LE = 122,
        @AV_PIX_FMT_GBRAP = 123,
        @AV_PIX_FMT_GBRAP16BE = 124,
        @AV_PIX_FMT_GBRAP16LE = 125,
        @AV_PIX_FMT_QSV = 126,
        @AV_PIX_FMT_MMAL = 127,
        @AV_PIX_FMT_D3D11VA_VLD = 128,
        @AV_PIX_FMT_CUDA = 129,
        @AV_PIX_FMT_0RGB = 295,
        @AV_PIX_FMT_RGB0 = 296,
        @AV_PIX_FMT_0BGR = 297,
        @AV_PIX_FMT_BGR0 = 298,
        @AV_PIX_FMT_YUV420P12BE = 299,
        @AV_PIX_FMT_YUV420P12LE = 300,
        @AV_PIX_FMT_YUV420P14BE = 301,
        @AV_PIX_FMT_YUV420P14LE = 302,
        @AV_PIX_FMT_YUV422P12BE = 303,
        @AV_PIX_FMT_YUV422P12LE = 304,
        @AV_PIX_FMT_YUV422P14BE = 305,
        @AV_PIX_FMT_YUV422P14LE = 306,
        @AV_PIX_FMT_YUV444P12BE = 307,
        @AV_PIX_FMT_YUV444P12LE = 308,
        @AV_PIX_FMT_YUV444P14BE = 309,
        @AV_PIX_FMT_YUV444P14LE = 310,
        @AV_PIX_FMT_GBRP12BE = 311,
        @AV_PIX_FMT_GBRP12LE = 312,
        @AV_PIX_FMT_GBRP14BE = 313,
        @AV_PIX_FMT_GBRP14LE = 314,
        @AV_PIX_FMT_YUVJ411P = 315,
        @AV_PIX_FMT_BAYER_BGGR8 = 316,
        @AV_PIX_FMT_BAYER_RGGB8 = 317,
        @AV_PIX_FMT_BAYER_GBRG8 = 318,
        @AV_PIX_FMT_BAYER_GRBG8 = 319,
        @AV_PIX_FMT_BAYER_BGGR16LE = 320,
        @AV_PIX_FMT_BAYER_BGGR16BE = 321,
        @AV_PIX_FMT_BAYER_RGGB16LE = 322,
        @AV_PIX_FMT_BAYER_RGGB16BE = 323,
        @AV_PIX_FMT_BAYER_GBRG16LE = 324,
        @AV_PIX_FMT_BAYER_GBRG16BE = 325,
        @AV_PIX_FMT_BAYER_GRBG16LE = 326,
        @AV_PIX_FMT_BAYER_GRBG16BE = 327,
        @AV_PIX_FMT_YUV440P10LE = 328,
        @AV_PIX_FMT_YUV440P10BE = 329,
        @AV_PIX_FMT_YUV440P12LE = 330,
        @AV_PIX_FMT_YUV440P12BE = 331,
        @AV_PIX_FMT_AYUV64LE = 332,
        @AV_PIX_FMT_AYUV64BE = 333,
        @AV_PIX_FMT_VIDEOTOOLBOX = 334,
        @AV_PIX_FMT_P010LE = 335,
        @AV_PIX_FMT_P010BE = 336,
        @AV_PIX_FMT_GBRAP12BE = 337,
        @AV_PIX_FMT_GBRAP12LE = 338,
        @AV_PIX_FMT_GBRAP10BE = 339,
        @AV_PIX_FMT_GBRAP10LE = 340,
        @AV_PIX_FMT_MEDIACODEC = 341,
        @AV_PIX_FMT_NB = 342,
    }
    
    internal enum AVColorPrimaries : int
    {
        @AVCOL_PRI_RESERVED0 = 0,
        @AVCOL_PRI_BT709 = 1,
        @AVCOL_PRI_UNSPECIFIED = 2,
        @AVCOL_PRI_RESERVED = 3,
        @AVCOL_PRI_BT470M = 4,
        @AVCOL_PRI_BT470BG = 5,
        @AVCOL_PRI_SMPTE170M = 6,
        @AVCOL_PRI_SMPTE240M = 7,
        @AVCOL_PRI_FILM = 8,
        @AVCOL_PRI_BT2020 = 9,
        @AVCOL_PRI_SMPTEST428_1 = 10,
        @AVCOL_PRI_SMPTE431 = 11,
        @AVCOL_PRI_SMPTE432 = 12,
        @AVCOL_PRI_NB = 13,
    }
    
    internal enum AVColorTransferCharacteristic : int
    {
        @AVCOL_TRC_RESERVED0 = 0,
        @AVCOL_TRC_BT709 = 1,
        @AVCOL_TRC_UNSPECIFIED = 2,
        @AVCOL_TRC_RESERVED = 3,
        @AVCOL_TRC_GAMMA22 = 4,
        @AVCOL_TRC_GAMMA28 = 5,
        @AVCOL_TRC_SMPTE170M = 6,
        @AVCOL_TRC_SMPTE240M = 7,
        @AVCOL_TRC_LINEAR = 8,
        @AVCOL_TRC_LOG = 9,
        @AVCOL_TRC_LOG_SQRT = 10,
        @AVCOL_TRC_IEC61966_2_4 = 11,
        @AVCOL_TRC_BT1361_ECG = 12,
        @AVCOL_TRC_IEC61966_2_1 = 13,
        @AVCOL_TRC_BT2020_10 = 14,
        @AVCOL_TRC_BT2020_12 = 15,
        @AVCOL_TRC_SMPTEST2084 = 16,
        @AVCOL_TRC_SMPTEST428_1 = 17,
        @AVCOL_TRC_ARIB_STD_B67 = 18,
        @AVCOL_TRC_NB = 19,
    }
    
    internal enum AVColorSpace : int
    {
        @AVCOL_SPC_RGB = 0,
        @AVCOL_SPC_BT709 = 1,
        @AVCOL_SPC_UNSPECIFIED = 2,
        @AVCOL_SPC_RESERVED = 3,
        @AVCOL_SPC_FCC = 4,
        @AVCOL_SPC_BT470BG = 5,
        @AVCOL_SPC_SMPTE170M = 6,
        @AVCOL_SPC_SMPTE240M = 7,
        @AVCOL_SPC_YCOCG = 8,
        @AVCOL_SPC_BT2020_NCL = 9,
        @AVCOL_SPC_BT2020_CL = 10,
        @AVCOL_SPC_SMPTE2085 = 11,
        @AVCOL_SPC_NB = 12,
    }
    
    internal enum AVColorRange : int
    {
        @AVCOL_RANGE_UNSPECIFIED = 0,
        @AVCOL_RANGE_MPEG = 1,
        @AVCOL_RANGE_JPEG = 2,
        @AVCOL_RANGE_NB = 3,
    }
    
    internal enum AVChromaLocation : int
    {
        @AVCHROMA_LOC_UNSPECIFIED = 0,
        @AVCHROMA_LOC_LEFT = 1,
        @AVCHROMA_LOC_CENTER = 2,
        @AVCHROMA_LOC_TOPLEFT = 3,
        @AVCHROMA_LOC_TOP = 4,
        @AVCHROMA_LOC_BOTTOMLEFT = 5,
        @AVCHROMA_LOC_BOTTOM = 6,
        @AVCHROMA_LOC_NB = 7,
    }
    
    internal enum AVSampleFormat : int
    {
        @AV_SAMPLE_FMT_NONE = -1,
        @AV_SAMPLE_FMT_U8 = 0,
        @AV_SAMPLE_FMT_S16 = 1,
        @AV_SAMPLE_FMT_S32 = 2,
        @AV_SAMPLE_FMT_FLT = 3,
        @AV_SAMPLE_FMT_DBL = 4,
        @AV_SAMPLE_FMT_U8P = 5,
        @AV_SAMPLE_FMT_S16P = 6,
        @AV_SAMPLE_FMT_S32P = 7,
        @AV_SAMPLE_FMT_FLTP = 8,
        @AV_SAMPLE_FMT_DBLP = 9,
        @AV_SAMPLE_FMT_S64 = 10,
        @AV_SAMPLE_FMT_S64P = 11,
        @AV_SAMPLE_FMT_NB = 12,
    }
    
    internal enum AVMatrixEncoding : int
    {
        @AV_MATRIX_ENCODING_NONE = 0,
        @AV_MATRIX_ENCODING_DOLBY = 1,
        @AV_MATRIX_ENCODING_DPLII = 2,
        @AV_MATRIX_ENCODING_DPLIIX = 3,
        @AV_MATRIX_ENCODING_DPLIIZ = 4,
        @AV_MATRIX_ENCODING_DOLBYEX = 5,
        @AV_MATRIX_ENCODING_DOLBYHEADPHONE = 6,
        @AV_MATRIX_ENCODING_NB = 7,
    }
    
    internal enum AVFrameSideDataType : int
    {
        @AV_FRAME_DATA_PANSCAN = 0,
        @AV_FRAME_DATA_A53_CC = 1,
        @AV_FRAME_DATA_STEREO3D = 2,
        @AV_FRAME_DATA_MATRIXENCODING = 3,
        @AV_FRAME_DATA_DOWNMIX_INFO = 4,
        @AV_FRAME_DATA_REPLAYGAIN = 5,
        @AV_FRAME_DATA_DISPLAYMATRIX = 6,
        @AV_FRAME_DATA_AFD = 7,
        @AV_FRAME_DATA_MOTION_VECTORS = 8,
        @AV_FRAME_DATA_SKIP_SAMPLES = 9,
        @AV_FRAME_DATA_AUDIO_SERVICE_TYPE = 10,
        @AV_FRAME_DATA_MASTERING_DISPLAY_METADATA = 11,
        @AV_FRAME_DATA_GOP_TIMECODE = 12,
    }
    
    internal enum AVActiveFormatDescription : int
    {
        @AV_AFD_SAME = 8,
        @AV_AFD_4_3 = 9,
        @AV_AFD_16_9 = 10,
        @AV_AFD_14_9 = 11,
        @AV_AFD_4_3_SP_14_9 = 13,
        @AV_AFD_16_9_SP_14_9 = 14,
        @AV_AFD_SP_4_3 = 15,
    }
    
    internal enum AVOptionType : int
    {
        @AV_OPT_TYPE_FLAGS = 0,
        @AV_OPT_TYPE_INT = 1,
        @AV_OPT_TYPE_INT64 = 2,
        @AV_OPT_TYPE_DOUBLE = 3,
        @AV_OPT_TYPE_FLOAT = 4,
        @AV_OPT_TYPE_STRING = 5,
        @AV_OPT_TYPE_RATIONAL = 6,
        @AV_OPT_TYPE_BINARY = 7,
        @AV_OPT_TYPE_DICT = 8,
        @AV_OPT_TYPE_CONST = 128,
        @AV_OPT_TYPE_IMAGE_SIZE = 1397316165,
        @AV_OPT_TYPE_PIXEL_FMT = 1346784596,
        @AV_OPT_TYPE_SAMPLE_FMT = 1397116244,
        @AV_OPT_TYPE_VIDEO_RATE = 1448231252,
        @AV_OPT_TYPE_DURATION = 1146442272,
        @AV_OPT_TYPE_COLOR = 1129270354,
        @AV_OPT_TYPE_CHANNEL_LAYOUT = 1128811585,
        @AV_OPT_TYPE_BOOL = 1112493900,
    }
    
    internal enum av_opt_eval_flags : int
    {
        @AV_OPT_FLAG_IMPLICIT_KEY = 1,
    }
    
    internal unsafe static partial class ffmpeg
    {
        internal const int __STDC_CONSTANT_MACROS = 1;
        internal const int AVCODEC_D3D11VA_H = 1;
        internal const int AVCODEC_DXVA2_H = 1;
        internal const int AVCODEC_QSV_H = 1;
        internal const int AVCODEC_VDA_H = 1;
        internal const int AVCODEC_VDPAU_H = 1;
        internal const int AVCODEC_VIDEOTOOLBOX_H = 1;
        internal const int AVCODEC_XVMC_H = 1;
        internal const int FF_LAMBDA_SHIFT = 7;
        internal const int FF_LAMBDA_SCALE = (1<<FF_LAMBDA_SHIFT);
        internal const int FF_QP2LAMBDA = 118;
        internal const int FF_LAMBDA_MAX = (256*128-1);
        internal const int FF_QUALITY_SCALE = FF_LAMBDA_SCALE;
        internal const ulong AV_NOPTS_VALUE = 0x8000000000000000;
        internal const int AV_TIME_BASE = 1000000;
        internal const int LIBAVUTIL_VERSION_MAJOR = 55;
        internal const int LIBAVUTIL_VERSION_MINOR = 34;
        internal const int LIBAVUTIL_VERSION_MICRO = 100;
        internal const bool FF_API_VDPAU = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_XVMC = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_OPT_TYPE_METADATA = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_DLOG = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_VAAPI = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_FRAME_QP = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_PLUS1_MINUS1 = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_ERROR_FRAME = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_CRC_BIG_TABLE = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const bool FF_API_PKT_PTS = (LIBAVUTIL_VERSION_MAJOR<56);
        internal const int AV_HAVE_BIGENDIAN = 0;
        internal const int AV_HAVE_FAST_UNALIGNED = 1;
        internal const int AV_ERROR_MAX_STRING_SIZE = 64;
        internal const double M_E = 2.7182818284590452354;
        internal const double M_LN2 = 0.69314718055994530942;
        internal const double M_LN10 = 2.30258509299404568402;
        internal const double M_LOG2_10 = 3.32192809488736234787;
        internal const double M_PHI = 1.61803398874989484820;
        internal const double M_PI = 3.14159265358979323846;
        internal const double M_PI_2 = 1.57079632679489661923;
        internal const double M_SQRT1_2 = 0.70710678118654752440;
        internal const double M_SQRT2 = 1.41421356237309504880;
        internal const int AV_LOG_QUIET = -8;
        internal const int AV_LOG_PANIC = 0;
        internal const int AV_LOG_FATAL = 8;
        internal const int AV_LOG_ERROR = 16;
        internal const int AV_LOG_WARNING = 24;
        internal const int AV_LOG_INFO = 32;
        internal const int AV_LOG_VERBOSE = 40;
        internal const int AV_LOG_DEBUG = 48;
        internal const int AV_LOG_TRACE = 56;
        internal const int AV_LOG_MAX_OFFSET = (AV_LOG_TRACE-AV_LOG_QUIET);
        internal const int AV_LOG_SKIP_REPEATED = 1;
        internal const int AV_LOG_PRINT_LEVEL = 2;
        internal const int AVPALETTE_SIZE = 1024;
        internal const int AVPALETTE_COUNT = 256;
        internal const int AV_CH_FRONT_LEFT = 0x00000001;
        internal const int AV_CH_FRONT_RIGHT = 0x00000002;
        internal const int AV_CH_FRONT_CENTER = 0x00000004;
        internal const int AV_CH_LOW_FREQUENCY = 0x00000008;
        internal const int AV_CH_BACK_LEFT = 0x00000010;
        internal const int AV_CH_BACK_RIGHT = 0x00000020;
        internal const int AV_CH_FRONT_LEFT_OF_CENTER = 0x00000040;
        internal const int AV_CH_FRONT_RIGHT_OF_CENTER = 0x00000080;
        internal const int AV_CH_BACK_CENTER = 0x00000100;
        internal const int AV_CH_SIDE_LEFT = 0x00000200;
        internal const int AV_CH_SIDE_RIGHT = 0x00000400;
        internal const int AV_CH_TOP_CENTER = 0x00000800;
        internal const int AV_CH_TOP_FRONT_LEFT = 0x00001000;
        internal const int AV_CH_TOP_FRONT_CENTER = 0x00002000;
        internal const int AV_CH_TOP_FRONT_RIGHT = 0x00004000;
        internal const int AV_CH_TOP_BACK_LEFT = 0x00008000;
        internal const int AV_CH_TOP_BACK_CENTER = 0x00010000;
        internal const int AV_CH_TOP_BACK_RIGHT = 0x00020000;
        internal const int AV_CH_STEREO_LEFT = 0x20000000;
        internal const int AV_CH_STEREO_RIGHT = 0x40000000;
        internal const ulong AV_CH_WIDE_LEFT = 0x0000000080000000UL;
        internal const ulong AV_CH_WIDE_RIGHT = 0x0000000100000000UL;
        internal const ulong AV_CH_SURROUND_DIRECT_LEFT = 0x0000000200000000UL;
        internal const ulong AV_CH_SURROUND_DIRECT_RIGHT = 0x0000000400000000UL;
        internal const ulong AV_CH_LOW_FREQUENCY_2 = 0x0000000800000000UL;
        internal const ulong AV_CH_LAYOUT_NATIVE = 0x8000000000000000UL;
        internal const int AV_CH_LAYOUT_MONO = (AV_CH_FRONT_CENTER);
        internal const int AV_CH_LAYOUT_STEREO = (AV_CH_FRONT_LEFT|AV_CH_FRONT_RIGHT);
        internal const int AV_CH_LAYOUT_2POINT1 = (AV_CH_LAYOUT_STEREO|AV_CH_LOW_FREQUENCY);
        internal const int AV_CH_LAYOUT_2_1 = (AV_CH_LAYOUT_STEREO|AV_CH_BACK_CENTER);
        internal const int AV_CH_LAYOUT_SURROUND = (AV_CH_LAYOUT_STEREO|AV_CH_FRONT_CENTER);
        internal const int AV_CH_LAYOUT_3POINT1 = (AV_CH_LAYOUT_SURROUND|AV_CH_LOW_FREQUENCY);
        internal const int AV_CH_LAYOUT_4POINT0 = (AV_CH_LAYOUT_SURROUND|AV_CH_BACK_CENTER);
        internal const int AV_CH_LAYOUT_4POINT1 = (AV_CH_LAYOUT_4POINT0|AV_CH_LOW_FREQUENCY);
        internal const int AV_CH_LAYOUT_2_2 = (AV_CH_LAYOUT_STEREO|AV_CH_SIDE_LEFT|AV_CH_SIDE_RIGHT);
        internal const int AV_CH_LAYOUT_QUAD = (AV_CH_LAYOUT_STEREO|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT);
        internal const int AV_CH_LAYOUT_5POINT0 = (AV_CH_LAYOUT_SURROUND|AV_CH_SIDE_LEFT|AV_CH_SIDE_RIGHT);
        internal const int AV_CH_LAYOUT_5POINT1 = (AV_CH_LAYOUT_5POINT0|AV_CH_LOW_FREQUENCY);
        internal const int AV_CH_LAYOUT_5POINT0_BACK = (AV_CH_LAYOUT_SURROUND|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT);
        internal const int AV_CH_LAYOUT_5POINT1_BACK = (AV_CH_LAYOUT_5POINT0_BACK|AV_CH_LOW_FREQUENCY);
        internal const int AV_CH_LAYOUT_6POINT0 = (AV_CH_LAYOUT_5POINT0|AV_CH_BACK_CENTER);
        internal const int AV_CH_LAYOUT_6POINT0_FRONT = (AV_CH_LAYOUT_2_2|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER);
        internal const int AV_CH_LAYOUT_HEXAGONAL = (AV_CH_LAYOUT_5POINT0_BACK|AV_CH_BACK_CENTER);
        internal const int AV_CH_LAYOUT_6POINT1 = (AV_CH_LAYOUT_5POINT1|AV_CH_BACK_CENTER);
        internal const int AV_CH_LAYOUT_6POINT1_BACK = (AV_CH_LAYOUT_5POINT1_BACK|AV_CH_BACK_CENTER);
        internal const int AV_CH_LAYOUT_6POINT1_FRONT = (AV_CH_LAYOUT_6POINT0_FRONT|AV_CH_LOW_FREQUENCY);
        internal const int AV_CH_LAYOUT_7POINT0 = (AV_CH_LAYOUT_5POINT0|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT);
        internal const int AV_CH_LAYOUT_7POINT0_FRONT = (AV_CH_LAYOUT_5POINT0|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER);
        internal const int AV_CH_LAYOUT_7POINT1 = (AV_CH_LAYOUT_5POINT1|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT);
        internal const int AV_CH_LAYOUT_7POINT1_WIDE = (AV_CH_LAYOUT_5POINT1|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER);
        internal const int AV_CH_LAYOUT_7POINT1_WIDE_BACK = (AV_CH_LAYOUT_5POINT1_BACK|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER);
        internal const int AV_CH_LAYOUT_OCTAGONAL = (AV_CH_LAYOUT_5POINT0|AV_CH_BACK_LEFT|AV_CH_BACK_CENTER|AV_CH_BACK_RIGHT);
        internal const ulong AV_CH_LAYOUT_HEXADECAGONAL = (AV_CH_LAYOUT_OCTAGONAL|AV_CH_WIDE_LEFT|AV_CH_WIDE_RIGHT|AV_CH_TOP_BACK_LEFT|AV_CH_TOP_BACK_RIGHT|AV_CH_TOP_BACK_CENTER|AV_CH_TOP_FRONT_CENTER|AV_CH_TOP_FRONT_LEFT|AV_CH_TOP_FRONT_RIGHT);
        internal const int AV_CH_LAYOUT_STEREO_DOWNMIX = (AV_CH_STEREO_LEFT|AV_CH_STEREO_RIGHT);
        internal const uint AV_CPU_FLAG_FORCE = 0x80000000;
        internal const int AV_CPU_FLAG_MMX = 0x0001;
        internal const int AV_CPU_FLAG_MMXEXT = 0x0002;
        internal const int AV_CPU_FLAG_MMX2 = 0x0002;
        internal const int AV_CPU_FLAG_3DNOW = 0x0004;
        internal const int AV_CPU_FLAG_SSE = 0x0008;
        internal const int AV_CPU_FLAG_SSE2 = 0x0010;
        internal const int AV_CPU_FLAG_SSE2SLOW = 0x40000000;
        internal const int AV_CPU_FLAG_3DNOWEXT = 0x0020;
        internal const int AV_CPU_FLAG_SSE3 = 0x0040;
        internal const int AV_CPU_FLAG_SSE3SLOW = 0x20000000;
        internal const int AV_CPU_FLAG_SSSE3 = 0x0080;
        internal const int AV_CPU_FLAG_ATOM = 0x10000000;
        internal const int AV_CPU_FLAG_SSE4 = 0x0100;
        internal const int AV_CPU_FLAG_SSE42 = 0x0200;
        internal const int AV_CPU_FLAG_AESNI = 0x80000;
        internal const int AV_CPU_FLAG_AVX = 0x4000;
        internal const int AV_CPU_FLAG_AVXSLOW = 0x8000000;
        internal const int AV_CPU_FLAG_XOP = 0x0400;
        internal const int AV_CPU_FLAG_FMA4 = 0x0800;
        internal const int AV_CPU_FLAG_CMOV = 0x1000;
        internal const int AV_CPU_FLAG_AVX2 = 0x8000;
        internal const int AV_CPU_FLAG_FMA3 = 0x10000;
        internal const int AV_CPU_FLAG_BMI1 = 0x20000;
        internal const int AV_CPU_FLAG_BMI2 = 0x40000;
        internal const int AV_CPU_FLAG_ALTIVEC = 0x0001;
        internal const int AV_CPU_FLAG_VSX = 0x0002;
        internal const int AV_CPU_FLAG_POWER8 = 0x0004;
        internal const int AV_CPU_FLAG_ARMV5TE = (1<<0);
        internal const int AV_CPU_FLAG_ARMV6 = (1<<1);
        internal const int AV_CPU_FLAG_ARMV6T2 = (1<<2);
        internal const int AV_CPU_FLAG_VFP = (1<<3);
        internal const int AV_CPU_FLAG_VFPV3 = (1<<4);
        internal const int AV_CPU_FLAG_NEON = (1<<5);
        internal const int AV_CPU_FLAG_ARMV8 = (1<<6);
        internal const int AV_CPU_FLAG_VFP_VM = (1<<7);
        internal const int AV_CPU_FLAG_SETEND = (1<<16);
        internal const int AV_BUFFER_FLAG_READONLY = (1<<0);
        internal const int AV_DICT_MATCH_CASE = 1;
        internal const int AV_DICT_IGNORE_SUFFIX = 2;
        internal const int AV_DICT_DONT_STRDUP_KEY = 4;
        internal const int AV_DICT_DONT_STRDUP_VAL = 8;
        internal const int AV_DICT_DONT_OVERWRITE = 16;
        internal const int AV_DICT_APPEND = 32;
        internal const int AV_DICT_MULTIKEY = 64;
        internal const int AV_NUM_DATA_POINTERS = 8;
        internal const int AV_FRAME_FLAG_CORRUPT = (1<<0);
        internal const int AV_FRAME_FLAG_DISCARD = (1<<2);
        internal const int FF_DECODE_ERROR_INVALID_BITSTREAM = 1;
        internal const int FF_DECODE_ERROR_MISSING_REFERENCE = 2;
        internal const int AV_OPT_FLAG_ENCODING_PARAM = 1;
        internal const int AV_OPT_FLAG_DECODING_PARAM = 2;
        internal const int AV_OPT_FLAG_METADATA = 4;
        internal const int AV_OPT_FLAG_AUDIO_PARAM = 8;
        internal const int AV_OPT_FLAG_VIDEO_PARAM = 16;
        internal const int AV_OPT_FLAG_SUBTITLE_PARAM = 32;
        internal const int AV_OPT_FLAG_EXPORT = 64;
        internal const int AV_OPT_FLAG_READONLY = 128;
        internal const int AV_OPT_FLAG_FILTERING_PARAM = (1<<16);
        internal const int AV_OPT_SEARCH_CHILDREN = (1<<0);
        internal const int AV_OPT_SEARCH_FAKE_OBJ = (1<<1);
        internal const int AV_OPT_ALLOW_NULL = (1<<2);
        internal const int AV_OPT_MULTI_COMPONENT_RANGE = (1<<12);
        internal const int AV_OPT_SERIALIZE_SKIP_DEFAULTS = 0x00000001;
        internal const int AV_OPT_SERIALIZE_OPT_FLAGS_EXACT = 0x00000002;
        internal const int AV_PIX_FMT_FLAG_BE = (1<<0);
        internal const int AV_PIX_FMT_FLAG_PAL = (1<<1);
        internal const int AV_PIX_FMT_FLAG_BITSTREAM = (1<<2);
        internal const int AV_PIX_FMT_FLAG_HWACCEL = (1<<3);
        internal const int AV_PIX_FMT_FLAG_PLANAR = (1<<4);
        internal const int AV_PIX_FMT_FLAG_RGB = (1<<5);
        internal const int AV_PIX_FMT_FLAG_PSEUDOPAL = (1<<6);
        internal const int AV_PIX_FMT_FLAG_ALPHA = (1<<7);
        internal const int FF_LOSS_RESOLUTION = 0x0001;
        internal const int FF_LOSS_DEPTH = 0x0002;
        internal const int FF_LOSS_COLORSPACE = 0x0004;
        internal const int FF_LOSS_ALPHA = 0x0008;
        internal const int FF_LOSS_COLORQUANT = 0x0010;
        internal const int FF_LOSS_CHROMA = 0x0020;
        private const string libavutil = "avutil-55";
        
        [DllImport(libavutil, EntryPoint = "avutil_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint avutil_version();
        
        [DllImport(libavutil, EntryPoint = "av_version_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_version_info();
        
        [DllImport(libavutil, EntryPoint = "avutil_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avutil_configuration();
        
        [DllImport(libavutil, EntryPoint = "avutil_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avutil_license();
        
        [DllImport(libavutil, EntryPoint = "av_get_media_type_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_media_type_string(AVMediaType @media_type);
        
        [DllImport(libavutil, EntryPoint = "av_get_picture_type_char", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte av_get_picture_type_char(AVPictureType @pict_type);
        
        [DllImport(libavutil, EntryPoint = "av_log2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_log2(uint @v);
        
        [DllImport(libavutil, EntryPoint = "av_log2_16bit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_log2_16bit(uint @v);
        
        [DllImport(libavutil, EntryPoint = "av_strerror", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_strerror(int @errnum, IntPtr @errbuf, ulong @errbuf_size);
        
        [DllImport(libavutil, EntryPoint = "av_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_malloc(ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_mallocz(ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_calloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_calloc(ulong @nmemb, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_realloc(void* @ptr, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_reallocp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_reallocp(void* @ptr, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_realloc_f", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_realloc_f(void* @ptr, ulong @nelem, ulong @elsize);
        
        [DllImport(libavutil, EntryPoint = "av_realloc_array", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_realloc_array(void* @ptr, ulong @nmemb, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_reallocp_array", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_reallocp_array(void* @ptr, ulong @nmemb, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_fast_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_fast_realloc(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavutil, EntryPoint = "av_fast_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fast_malloc(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavutil, EntryPoint = "av_fast_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fast_mallocz(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavutil, EntryPoint = "av_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_free(void* @ptr);
        
        [DllImport(libavutil, EntryPoint = "av_freep", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_freep(void* @ptr);
        
        [DllImport(libavutil, EntryPoint = "av_strdup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_strdup([MarshalAs(UnmanagedType.LPStr)] string @s);
        
        [DllImport(libavutil, EntryPoint = "av_strndup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_strndup([MarshalAs(UnmanagedType.LPStr)] string @s, ulong @len);
        
        [DllImport(libavutil, EntryPoint = "av_memdup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_memdup(void* @p, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_memcpy_backptr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_memcpy_backptr(sbyte* @dst, int @back, int @cnt);
        
        [DllImport(libavutil, EntryPoint = "av_dynarray_add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_dynarray_add(void* @tab_ptr, int* @nb_ptr, void* @elem);
        
        [DllImport(libavutil, EntryPoint = "av_dynarray_add_nofree", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dynarray_add_nofree(void* @tab_ptr, int* @nb_ptr, void* @elem);
        
        [DllImport(libavutil, EntryPoint = "av_dynarray2_add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_dynarray2_add(void** @tab_ptr, int* @nb_ptr, ulong @elem_size, sbyte* @elem_data);
        
        [DllImport(libavutil, EntryPoint = "av_max_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_max_alloc(ulong @max);
        
        [DllImport(libavutil, EntryPoint = "av_reduce", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_reduce(int* @dst_num, int* @dst_den, long @num, long @den, long @max);
        
        [DllImport(libavutil, EntryPoint = "av_mul_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_mul_q(AVRational @b, AVRational @c);
        
        [DllImport(libavutil, EntryPoint = "av_div_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_div_q(AVRational @b, AVRational @c);
        
        [DllImport(libavutil, EntryPoint = "av_add_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_add_q(AVRational @b, AVRational @c);
        
        [DllImport(libavutil, EntryPoint = "av_sub_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_sub_q(AVRational @b, AVRational @c);
        
        [DllImport(libavutil, EntryPoint = "av_d2q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_d2q(double @d, int @max);
        
        [DllImport(libavutil, EntryPoint = "av_nearer_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_nearer_q(AVRational @q, AVRational @q1, AVRational @q2);
        
        [DllImport(libavutil, EntryPoint = "av_find_nearest_q_idx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_find_nearest_q_idx(AVRational @q, AVRational* @q_list);
        
        [DllImport(libavutil, EntryPoint = "av_q2intfloat", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint av_q2intfloat(AVRational @q);
        
        [DllImport(libavutil, EntryPoint = "av_gcd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_gcd(long @a, long @b);
        
        [DllImport(libavutil, EntryPoint = "av_rescale", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_rescale(long @a, long @b, long @c);
        
        [DllImport(libavutil, EntryPoint = "av_rescale_rnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_rescale_rnd(long @a, long @b, long @c, AVRounding @rnd);
        
        [DllImport(libavutil, EntryPoint = "av_rescale_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_rescale_q(long @a, AVRational @bq, AVRational @cq);
        
        [DllImport(libavutil, EntryPoint = "av_rescale_q_rnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_rescale_q_rnd(long @a, AVRational @bq, AVRational @cq, AVRounding @rnd);
        
        [DllImport(libavutil, EntryPoint = "av_compare_ts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_compare_ts(long @ts_a, AVRational @tb_a, long @ts_b, AVRational @tb_b);
        
        [DllImport(libavutil, EntryPoint = "av_compare_mod", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_compare_mod(ulong @a, ulong @b, ulong @mod);
        
        [DllImport(libavutil, EntryPoint = "av_rescale_delta", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_rescale_delta(AVRational @in_tb, long @in_ts, AVRational @fs_tb, int @duration, long* @last, AVRational @out_tb);
        
        [DllImport(libavutil, EntryPoint = "av_add_stable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_add_stable(AVRational @ts_tb, long @ts, AVRational @inc_tb, long @inc);
        
        [DllImport(libavutil, EntryPoint = "av_log", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log(void* @avcl, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt);
        
        [DllImport(libavutil, EntryPoint = "av_vlog", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_vlog(void* @avcl, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, sbyte* @vl);
        
        [DllImport(libavutil, EntryPoint = "av_log_get_level", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_log_get_level();
        
        [DllImport(libavutil, EntryPoint = "av_log_set_level", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_set_level(int @level);
        
        [DllImport(libavutil, EntryPoint = "av_log_set_callback", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_set_callback(IntPtr @callback);
        
        [DllImport(libavutil, EntryPoint = "av_log_default_callback", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_default_callback(void* @avcl, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, sbyte* @vl);
        
        [DllImport(libavutil, EntryPoint = "av_default_item_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_default_item_name(void* @ctx);
        
        [DllImport(libavutil, EntryPoint = "av_default_get_category", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClassCategory av_default_get_category(void* @ptr);
        
        [DllImport(libavutil, EntryPoint = "av_log_format_line", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_format_line(void* @ptr, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, sbyte* @vl, IntPtr @line, int @line_size, int* @print_prefix);
        
        [DllImport(libavutil, EntryPoint = "av_log_format_line2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_log_format_line2(void* @ptr, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, sbyte* @vl, IntPtr @line, int @line_size, int* @print_prefix);
        
        [DllImport(libavutil, EntryPoint = "av_log_set_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_set_flags(int @arg);
        
        [DllImport(libavutil, EntryPoint = "av_log_get_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_log_get_flags();
        
        [DllImport(libavutil, EntryPoint = "av_int_list_length_for_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint av_int_list_length_for_size(uint @elsize, void* @list, ulong @term);
        
        [DllImport(libavutil, EntryPoint = "av_fopen_utf8", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern _iobuf* av_fopen_utf8([MarshalAs(UnmanagedType.LPStr)] string @path, [MarshalAs(UnmanagedType.LPStr)] string @mode);
        
        [DllImport(libavutil, EntryPoint = "av_get_time_base_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_get_time_base_q();
        
        [DllImport(libavutil, EntryPoint = "av_fifo_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFifoBuffer* av_fifo_alloc(uint @size);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_alloc_array", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFifoBuffer* av_fifo_alloc_array(ulong @nmemb, ulong @size);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fifo_free(AVFifoBuffer* @f);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_freep", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fifo_freep(AVFifoBuffer** @f);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_reset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fifo_reset(AVFifoBuffer* @f);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_size(AVFifoBuffer* @f);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_space", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_space(AVFifoBuffer* @f);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_generic_peek_at", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_generic_peek_at(AVFifoBuffer* @f, void* @dest, int @offset, int @buf_size, IntPtr @func);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_generic_peek", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_generic_peek(AVFifoBuffer* @f, void* @dest, int @buf_size, IntPtr @func);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_generic_read", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_generic_read(AVFifoBuffer* @f, void* @dest, int @buf_size, IntPtr @func);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_generic_write", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_generic_write(AVFifoBuffer* @f, void* @src, int @size, IntPtr @func);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_realloc2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_realloc2(AVFifoBuffer* @f, uint @size);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_grow", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_fifo_grow(AVFifoBuffer* @f, uint @additional_space);
        
        [DllImport(libavutil, EntryPoint = "av_fifo_drain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fifo_drain(AVFifoBuffer* @f, int @size);
        
        [DllImport(libavutil, EntryPoint = "av_get_sample_fmt_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_sample_fmt_name(AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_get_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVSampleFormat av_get_sample_fmt([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavutil, EntryPoint = "av_get_alt_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVSampleFormat av_get_alt_sample_fmt(AVSampleFormat @sample_fmt, int @planar);
        
        [DllImport(libavutil, EntryPoint = "av_get_packed_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVSampleFormat av_get_packed_sample_fmt(AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_get_planar_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVSampleFormat av_get_planar_sample_fmt(AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_get_sample_fmt_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_get_sample_fmt_string(IntPtr @buf, int @buf_size, AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_get_bytes_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_bytes_per_sample(AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_sample_fmt_is_planar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_sample_fmt_is_planar(AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_samples_get_buffer_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_samples_get_buffer_size(int* @linesize, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_samples_fill_arrays", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_samples_fill_arrays(sbyte** @audio_data, int* @linesize, sbyte* @buf, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_samples_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_samples_alloc(sbyte** @audio_data, int* @linesize, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_samples_alloc_array_and_samples", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_samples_alloc_array_and_samples(sbyte*** @audio_data, int* @linesize, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_samples_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_samples_copy(sbyte** @dst, sbyte** @src, int @dst_offset, int @src_offset, int @nb_samples, int @nb_channels, AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_samples_set_silence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_samples_set_silence(sbyte** @audio_data, int @offset, int @nb_samples, int @nb_channels, AVSampleFormat @sample_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_audio_fifo_free(AVAudioFifo* @af);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVAudioFifo* av_audio_fifo_alloc(AVSampleFormat @sample_fmt, int @channels, int @nb_samples);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_realloc(AVAudioFifo* @af, int @nb_samples);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_write", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_write(AVAudioFifo* @af, void** @data, int @nb_samples);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_peek", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_peek(AVAudioFifo* @af, void** @data, int @nb_samples);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_peek_at", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_peek_at(AVAudioFifo* @af, void** @data, int @nb_samples, int @offset);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_read", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_read(AVAudioFifo* @af, void** @data, int @nb_samples);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_drain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_drain(AVAudioFifo* @af, int @nb_samples);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_reset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_audio_fifo_reset(AVAudioFifo* @af);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_size(AVAudioFifo* @af);
        
        [DllImport(libavutil, EntryPoint = "av_audio_fifo_space", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_audio_fifo_space(AVAudioFifo* @af);
        
        [DllImport(libavutil, EntryPoint = "av_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ulong av_get_channel_layout([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavutil, EntryPoint = "av_get_channel_layout_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_get_channel_layout_string(IntPtr @buf, int @buf_size, int @nb_channels, ulong @channel_layout);
        
        [DllImport(libavutil, EntryPoint = "av_bprint_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_bprint_channel_layout(AVBPrint* @bp, int @nb_channels, ulong @channel_layout);
        
        [DllImport(libavutil, EntryPoint = "av_get_channel_layout_nb_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_channel_layout_nb_channels(ulong @channel_layout);
        
        [DllImport(libavutil, EntryPoint = "av_get_default_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_get_default_channel_layout(int @nb_channels);
        
        [DllImport(libavutil, EntryPoint = "av_get_channel_layout_channel_index", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_channel_layout_channel_index(ulong @channel_layout, ulong @channel);
        
        [DllImport(libavutil, EntryPoint = "av_channel_layout_extract_channel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ulong av_channel_layout_extract_channel(ulong @channel_layout, int @index);
        
        [DllImport(libavutil, EntryPoint = "av_get_channel_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_channel_name(ulong @channel);
        
        [DllImport(libavutil, EntryPoint = "av_get_channel_description", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_channel_description(ulong @channel);
        
        [DllImport(libavutil, EntryPoint = "av_get_standard_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_standard_channel_layout(uint @index, ulong* @layout, sbyte** @name);
        
        [DllImport(libavutil, EntryPoint = "av_get_cpu_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_cpu_flags();
        
        [DllImport(libavutil, EntryPoint = "av_force_cpu_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_force_cpu_flags(int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_set_cpu_flags_mask", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_set_cpu_flags_mask(int @mask);
        
        [DllImport(libavutil, EntryPoint = "av_parse_cpu_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_parse_cpu_flags([MarshalAs(UnmanagedType.LPStr)] string @s);
        
        [DllImport(libavutil, EntryPoint = "av_parse_cpu_caps", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_parse_cpu_caps(uint* @flags, [MarshalAs(UnmanagedType.LPStr)] string @s);
        
        [DllImport(libavutil, EntryPoint = "av_cpu_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_cpu_count();
        
        [DllImport(libavutil, EntryPoint = "av_buffer_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferRef* av_buffer_alloc(int @size);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_allocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferRef* av_buffer_allocz(int @size);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferRef* av_buffer_create(sbyte* @data, int @size, IntPtr @free, void* @opaque, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_default_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_buffer_default_free(void* @opaque, sbyte* @data);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferRef* av_buffer_ref(AVBufferRef* @buf);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_buffer_unref(AVBufferRef** @buf);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_is_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffer_is_writable(AVBufferRef* @buf);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_get_opaque", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_buffer_get_opaque(AVBufferRef* @buf);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_get_ref_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffer_get_ref_count(AVBufferRef* @buf);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_make_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffer_make_writable(AVBufferRef** @buf);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffer_realloc(AVBufferRef** @buf, int @size);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_pool_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferPool* av_buffer_pool_init(int @size, IntPtr @alloc);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_pool_init2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferPool* av_buffer_pool_init2(int @size, void* @opaque, IntPtr @alloc, IntPtr @pool_free);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_pool_uninit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_buffer_pool_uninit(AVBufferPool** @pool);
        
        [DllImport(libavutil, EntryPoint = "av_buffer_pool_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferRef* av_buffer_pool_get(AVBufferPool* @pool);
        
        [DllImport(libavutil, EntryPoint = "av_dict_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVDictionaryEntry* av_dict_get(AVDictionary* @m, [MarshalAs(UnmanagedType.LPStr)] string @key, AVDictionaryEntry* @prev, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_dict_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dict_count(AVDictionary* @m);
        
        [DllImport(libavutil, EntryPoint = "av_dict_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dict_set(AVDictionary** @pm, [MarshalAs(UnmanagedType.LPStr)] string @key, [MarshalAs(UnmanagedType.LPStr)] string @value, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_dict_set_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dict_set_int(AVDictionary** @pm, [MarshalAs(UnmanagedType.LPStr)] string @key, long @value, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_dict_parse_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dict_parse_string(AVDictionary** @pm, [MarshalAs(UnmanagedType.LPStr)] string @str, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_dict_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dict_copy(AVDictionary** @dst, AVDictionary* @src, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_dict_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_dict_free(AVDictionary** @m);
        
        [DllImport(libavutil, EntryPoint = "av_dict_get_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dict_get_string(AVDictionary* @m, sbyte** @buffer, sbyte @key_val_sep, sbyte @pairs_sep);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_best_effort_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_frame_get_best_effort_timestamp(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_best_effort_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_best_effort_timestamp(AVFrame* @frame, long @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_pkt_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_frame_get_pkt_duration(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_pkt_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_pkt_duration(AVFrame* @frame, long @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_pkt_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_frame_get_pkt_pos(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_pkt_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_pkt_pos(AVFrame* @frame, long @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long av_frame_get_channel_layout(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_channel_layout(AVFrame* @frame, long @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_get_channels(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_channels(AVFrame* @frame, int @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_sample_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_get_sample_rate(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_sample_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_sample_rate(AVFrame* @frame, int @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_metadata", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVDictionary* av_frame_get_metadata(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_metadata", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_metadata(AVFrame* @frame, AVDictionary* @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_decode_error_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_get_decode_error_flags(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_decode_error_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_decode_error_flags(AVFrame* @frame, int @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_pkt_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_get_pkt_size(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_pkt_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_pkt_size(AVFrame* @frame, int @val);
        
        [DllImport(libavutil, EntryPoint = "avpriv_frame_get_metadatap", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVDictionary** avpriv_frame_get_metadatap(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_qp_table", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_frame_get_qp_table(AVFrame* @f, int* @stride, int* @type);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_qp_table", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_set_qp_table(AVFrame* @f, AVBufferRef* @buf, int @stride, int @type);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_colorspace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVColorSpace av_frame_get_colorspace(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_colorspace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_colorspace(AVFrame* @frame, AVColorSpace @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_color_range", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVColorRange av_frame_get_color_range(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_set_color_range", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_set_color_range(AVFrame* @frame, AVColorRange @val);
        
        [DllImport(libavutil, EntryPoint = "av_get_colorspace_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_colorspace_name(AVColorSpace @val);
        
        [DllImport(libavutil, EntryPoint = "av_frame_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFrame* av_frame_alloc();
        
        [DllImport(libavutil, EntryPoint = "av_frame_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_free(AVFrame** @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_ref(AVFrame* @dst, AVFrame* @src);
        
        [DllImport(libavutil, EntryPoint = "av_frame_clone", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFrame* av_frame_clone(AVFrame* @src);
        
        [DllImport(libavutil, EntryPoint = "av_frame_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_unref(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_move_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_move_ref(AVFrame* @dst, AVFrame* @src);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_get_buffer(AVFrame* @frame, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_frame_is_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_is_writable(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_make_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_make_writable(AVFrame* @frame);
        
        [DllImport(libavutil, EntryPoint = "av_frame_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_copy(AVFrame* @dst, AVFrame* @src);
        
        [DllImport(libavutil, EntryPoint = "av_frame_copy_props", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_frame_copy_props(AVFrame* @dst, AVFrame* @src);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_plane_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferRef* av_frame_get_plane_buffer(AVFrame* @frame, int @plane);
        
        [DllImport(libavutil, EntryPoint = "av_frame_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFrameSideData* av_frame_new_side_data(AVFrame* @frame, AVFrameSideDataType @type, int @size);
        
        [DllImport(libavutil, EntryPoint = "av_frame_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFrameSideData* av_frame_get_side_data(AVFrame* @frame, AVFrameSideDataType @type);
        
        [DllImport(libavutil, EntryPoint = "av_frame_remove_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_frame_remove_side_data(AVFrame* @frame, AVFrameSideDataType @type);
        
        [DllImport(libavutil, EntryPoint = "av_frame_side_data_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_frame_side_data_name(AVFrameSideDataType @type);
        
        [DllImport(libavutil, EntryPoint = "av_opt_show2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_show2(void* @obj, void* @av_log_obj, int @req_flags, int @rej_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_defaults", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_opt_set_defaults(void* @s);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_defaults2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_opt_set_defaults2(void* @s, int @mask, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_set_options_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_set_options_string(void* @ctx, [MarshalAs(UnmanagedType.LPStr)] string @opts, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_from_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_from_string(void* @ctx, [MarshalAs(UnmanagedType.LPStr)] string @opts, string[] @shorthand, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep);
        
        [DllImport(libavutil, EntryPoint = "av_opt_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_opt_free(void* @obj);
        
        [DllImport(libavutil, EntryPoint = "av_opt_flag_is_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_flag_is_set(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @field_name, [MarshalAs(UnmanagedType.LPStr)] string @flag_name);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_dict", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_dict(void* @obj, AVDictionary** @options);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_dict2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_dict2(void* @obj, AVDictionary** @options, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_key_value", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_key_value(sbyte** @ropts, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep, uint @flags, sbyte** @rkey, sbyte** @rval);
        
        [DllImport(libavutil, EntryPoint = "av_opt_eval_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_eval_flags(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, int* @flags_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_eval_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_eval_int(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, int* @int_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_eval_int64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_eval_int64(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, long* @int64_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_eval_float", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_eval_float(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, float* @float_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_eval_double", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_eval_double(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, double* @double_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_eval_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_eval_q(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, AVRational* @q_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_find", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVOption* av_opt_find(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @unit, int @opt_flags, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_find2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVOption* av_opt_find2(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @unit, int @opt_flags, int @search_flags, void** @target_obj);
        
        [DllImport(libavutil, EntryPoint = "av_opt_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVOption* av_opt_next(void* @obj, AVOption* @prev);
        
        [DllImport(libavutil, EntryPoint = "av_opt_child_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_opt_child_next(void* @obj, void* @prev);
        
        [DllImport(libavutil, EntryPoint = "av_opt_child_class_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClass* av_opt_child_class_next(AVClass* @parent, AVClass* @prev);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @val, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_int(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, long @val, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_double", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_double(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, double @val, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_q(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVRational @val, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_bin", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_bin(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, sbyte* @val, int @size, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_image_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_image_size(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @w, int @h, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_pixel_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_pixel_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVPixelFormat @fmt, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_sample_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVSampleFormat @fmt, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_video_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_video_rate(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVRational @val, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_channel_layout(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, long @ch_layout, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_set_dict_val", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_set_dict_val(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVDictionary* @val, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, sbyte** @out_val);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_int(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, long* @out_val);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_double", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_double(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, double* @out_val);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_q(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVRational* @out_val);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_image_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_image_size(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, int* @w_out, int* @h_out);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_pixel_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_pixel_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVPixelFormat* @out_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_sample_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVSampleFormat* @out_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_video_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_video_rate(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVRational* @out_val);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_channel_layout(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, long* @ch_layout);
        
        [DllImport(libavutil, EntryPoint = "av_opt_get_dict_val", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_get_dict_val(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVDictionary** @out_val);
        
        [DllImport(libavutil, EntryPoint = "av_opt_ptr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void* av_opt_ptr(AVClass* @avclass, void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavutil, EntryPoint = "av_opt_freep_ranges", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_opt_freep_ranges(AVOptionRanges** @ranges);
        
        [DllImport(libavutil, EntryPoint = "av_opt_query_ranges", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_query_ranges(AVOptionRanges** @param0, void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @key, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_copy(void* @dest, void* @src);
        
        [DllImport(libavutil, EntryPoint = "av_opt_query_ranges_default", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_query_ranges_default(AVOptionRanges** @param0, void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @key, int @flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_is_set_to_default", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_is_set_to_default(void* @obj, AVOption* @o);
        
        [DllImport(libavutil, EntryPoint = "av_opt_is_set_to_default_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_is_set_to_default_by_name(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags);
        
        [DllImport(libavutil, EntryPoint = "av_opt_serialize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_opt_serialize(void* @obj, int @opt_flags, int @flags, sbyte** @buffer, sbyte @key_val_sep, sbyte @pairs_sep);
        
        [DllImport(libavutil, EntryPoint = "av_get_bits_per_pixel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_bits_per_pixel(AVPixFmtDescriptor* @pixdesc);
        
        [DllImport(libavutil, EntryPoint = "av_get_padded_bits_per_pixel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_padded_bits_per_pixel(AVPixFmtDescriptor* @pixdesc);
        
        [DllImport(libavutil, EntryPoint = "av_pix_fmt_desc_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixFmtDescriptor* av_pix_fmt_desc_get(AVPixelFormat @pix_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_pix_fmt_desc_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixFmtDescriptor* av_pix_fmt_desc_next(AVPixFmtDescriptor* @prev);
        
        [DllImport(libavutil, EntryPoint = "av_pix_fmt_desc_get_id", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat av_pix_fmt_desc_get_id(AVPixFmtDescriptor* @desc);
        
        [DllImport(libavutil, EntryPoint = "av_pix_fmt_get_chroma_sub_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_pix_fmt_get_chroma_sub_sample(AVPixelFormat @pix_fmt, int* @h_shift, int* @v_shift);
        
        [DllImport(libavutil, EntryPoint = "av_pix_fmt_count_planes", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_pix_fmt_count_planes(AVPixelFormat @pix_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_color_range_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_color_range_name(AVColorRange @range);
        
        [DllImport(libavutil, EntryPoint = "av_color_primaries_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_color_primaries_name(AVColorPrimaries @primaries);
        
        [DllImport(libavutil, EntryPoint = "av_color_transfer_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_color_transfer_name(AVColorTransferCharacteristic @transfer);
        
        [DllImport(libavutil, EntryPoint = "av_color_space_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_color_space_name(AVColorSpace @space);
        
        [DllImport(libavutil, EntryPoint = "av_chroma_location_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_chroma_location_name(AVChromaLocation @location);
        
        [DllImport(libavutil, EntryPoint = "av_get_pix_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat av_get_pix_fmt([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavutil, EntryPoint = "av_get_pix_fmt_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_pix_fmt_name(AVPixelFormat @pix_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_get_pix_fmt_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_get_pix_fmt_string(IntPtr @buf, int @buf_size, AVPixelFormat @pix_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_read_image_line", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_read_image_line(ushort* @dst, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @data, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @linesize, AVPixFmtDescriptor* @desc, int @x, int @y, int @c, int @w, int @read_pal_component);
        
        [DllImport(libavutil, EntryPoint = "av_write_image_line", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_write_image_line(ushort* @src, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @data, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @linesize, AVPixFmtDescriptor* @desc, int @x, int @y, int @c, int @w);
        
        [DllImport(libavutil, EntryPoint = "av_pix_fmt_swap_endianness", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat av_pix_fmt_swap_endianness(AVPixelFormat @pix_fmt);
        
        [DllImport(libavutil, EntryPoint = "av_get_pix_fmt_loss", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_pix_fmt_loss(AVPixelFormat @dst_pix_fmt, AVPixelFormat @src_pix_fmt, int @has_alpha);
        
        [DllImport(libavutil, EntryPoint = "av_find_best_pix_fmt_of_2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat av_find_best_pix_fmt_of_2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavutil, EntryPoint = "av_image_fill_max_pixsteps", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_image_fill_max_pixsteps([MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @max_pixsteps, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @max_pixstep_comps, AVPixFmtDescriptor* @pixdesc);
        
        [DllImport(libavutil, EntryPoint = "av_image_get_linesize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_get_linesize(AVPixelFormat @pix_fmt, int @width, int @plane);
        
        [DllImport(libavutil, EntryPoint = "av_image_fill_linesizes", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_fill_linesizes([MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @linesizes, AVPixelFormat @pix_fmt, int @width);
        
        [DllImport(libavutil, EntryPoint = "av_image_fill_pointers", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_fill_pointers([MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @data, AVPixelFormat @pix_fmt, int @height, sbyte* @ptr, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @linesizes);
        
        [DllImport(libavutil, EntryPoint = "av_image_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_alloc([MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @pointers, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @linesizes, int @w, int @h, AVPixelFormat @pix_fmt, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_image_copy_plane", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_image_copy_plane(sbyte* @dst, int @dst_linesize, sbyte* @src, int @src_linesize, int @bytewidth, int @height);
        
        [DllImport(libavutil, EntryPoint = "av_image_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_image_copy([MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @dst_data, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @dst_linesizes, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @src_data, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @src_linesizes, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavutil, EntryPoint = "av_image_fill_arrays", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_fill_arrays([MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @dst_data, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @dst_linesize, sbyte* @src, AVPixelFormat @pix_fmt, int @width, int @height, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_image_get_buffer_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_get_buffer_size(AVPixelFormat @pix_fmt, int @width, int @height, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_image_copy_to_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_copy_to_buffer(sbyte* @dst, int @dst_size, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] sbyte*[] @src_data, [MarshalAs(UnmanagedType.LPArray, SizeConst=4)] int[] @src_linesize, AVPixelFormat @pix_fmt, int @width, int @height, int @align);
        
        [DllImport(libavutil, EntryPoint = "av_image_check_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_check_size(uint @w, uint @h, int @log_offset, void* @log_ctx);
        
        [DllImport(libavutil, EntryPoint = "av_image_check_sar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_image_check_sar(uint @w, uint @h, AVRational @sar);
        
    }
}
