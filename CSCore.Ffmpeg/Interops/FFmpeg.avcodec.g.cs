#pragma warning disable CS0649
#pragma warning disable IDE1006

using System;
using System.Runtime.InteropServices;

namespace CSCore.Ffmpeg.Interops
{
    internal unsafe partial struct AVBuffer
    {
    }
    
    internal unsafe partial struct AVBufferPool
    {
    }
    
    internal unsafe partial struct AVBPrint
    {
    }
    
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVCodecDescriptor
    {
        internal AvCodecId @id;
        internal AVMediaType @type;
        internal sbyte* @name;
        internal sbyte* @long_name;
        internal int @props;
        internal sbyte** @mime_types;
        internal AVProfile* @profiles;
    }
    
    internal unsafe partial struct AVProfile
    {
    }
    
    internal unsafe partial struct RcOverride
    {
        internal int @start_frame;
        internal int @end_frame;
        internal int @qscale;
        internal float @quality_factor;
    }
    
    internal unsafe partial struct AVPanScan
    {
        internal int @id;
        internal int @width;
        internal int @height;
        internal fixed short @position0[2]; internal fixed short @position1[2]; internal fixed short @position2[2]; 
    }
    
    internal unsafe partial struct AVCPBProperties
    {
        internal int @max_bitrate;
        internal int @min_bitrate;
        internal int @avg_bitrate;
        internal int @buffer_size;
        internal ulong @vbv_delay;
    }
    
    internal unsafe partial struct AVPacketSideData
    {
        internal sbyte* @data;
        internal int @size;
        internal AVPacketSideDataType @type;
    }
    
    internal unsafe partial struct AVPacket
    {
        internal AVBufferRef* @buf;
        internal long @pts;
        internal long @dts;
        internal sbyte* @data;
        internal int @size;
        internal int @stream_index;
        internal int @flags;
        internal AVPacketSideData* @side_data;
        internal int @side_data_elems;
        internal long @duration;
        internal long @pos;
        internal long @convergence_duration;
    }
    
    internal unsafe partial struct AVCodecInternal
    {
    }
    
    internal unsafe partial struct AVCodecContext
    {
        internal AVClass* @av_class;
        internal int @log_level_offset;
        internal AVMediaType @codec_type;
        internal AVCodec* @codec;
        internal fixed sbyte @codec_name[32]; 
        internal AvCodecId @codec_id;
        internal uint @codec_tag;
        internal uint @stream_codec_tag;
        internal void* @priv_data;
        internal AVCodecInternal* @internal;
        internal void* @opaque;
        internal long @bit_rate;
        internal int @bit_rate_tolerance;
        internal int @global_quality;
        internal int @compression_level;
        internal int @flags;
        internal int @flags2;
        internal sbyte* @extradata;
        internal int @extradata_size;
        internal AVRational @time_base;
        internal int @ticks_per_frame;
        internal int @delay;
        internal int @width;
        internal int @height;
        internal int @coded_width;
        internal int @coded_height;
        internal int @gop_size;
        internal AVPixelFormat @pix_fmt;
        internal int @me_method;
        internal IntPtr @draw_horiz_band;
        internal IntPtr @get_format;
        internal int @max_b_frames;
        internal float @b_quant_factor;
        internal int @rc_strategy;
        internal int @b_frame_strategy;
        internal float @b_quant_offset;
        internal int @has_b_frames;
        internal int @mpeg_quant;
        internal float @i_quant_factor;
        internal float @i_quant_offset;
        internal float @lumi_masking;
        internal float @temporal_cplx_masking;
        internal float @spatial_cplx_masking;
        internal float @p_masking;
        internal float @dark_masking;
        internal int @slice_count;
        internal int @prediction_method;
        internal int* @slice_offset;
        internal AVRational @sample_aspect_ratio;
        internal int @me_cmp;
        internal int @me_sub_cmp;
        internal int @mb_cmp;
        internal int @ildct_cmp;
        internal int @dia_size;
        internal int @last_predictor_count;
        internal int @pre_me;
        internal int @me_pre_cmp;
        internal int @pre_dia_size;
        internal int @me_subpel_quality;
        internal int @dtg_active_format;
        internal int @me_range;
        internal int @intra_quant_bias;
        internal int @inter_quant_bias;
        internal int @slice_flags;
        internal int @xvmc_acceleration;
        internal int @mb_decision;
        internal ushort* @intra_matrix;
        internal ushort* @inter_matrix;
        internal int @scenechange_threshold;
        internal int @noise_reduction;
        internal int @me_threshold;
        internal int @mb_threshold;
        internal int @intra_dc_precision;
        internal int @skip_top;
        internal int @skip_bottom;
        internal float @border_masking;
        internal int @mb_lmin;
        internal int @mb_lmax;
        internal int @me_penalty_compensation;
        internal int @bidir_refine;
        internal int @brd_scale;
        internal int @keyint_min;
        internal int @refs;
        internal int @chromaoffset;
        internal int @scenechange_factor;
        internal int @mv0_threshold;
        internal int @b_sensitivity;
        internal AVColorPrimaries @color_primaries;
        internal AVColorTransferCharacteristic @color_trc;
        internal AVColorSpace @colorspace;
        internal AVColorRange @color_range;
        internal AVChromaLocation @chroma_sample_location;
        internal int @slices;
        internal AVFieldOrder @field_order;
        internal int @sample_rate;
        internal int @channels;
        internal AVSampleFormat @sample_fmt;
        internal int @frame_size;
        internal int @frame_number;
        internal int @block_align;
        internal int @cutoff;
        internal ulong @channel_layout;
        internal ulong @request_channel_layout;
        internal AVAudioServiceType @audio_service_type;
        internal AVSampleFormat @request_sample_fmt;
        internal IntPtr @get_buffer2;
        internal int @refcounted_frames;
        internal float @qcompress;
        internal float @qblur;
        internal int @qmin;
        internal int @qmax;
        internal int @max_qdiff;
        internal float @rc_qsquish;
        internal float @rc_qmod_amp;
        internal int @rc_qmod_freq;
        internal int @rc_buffer_size;
        internal int @rc_override_count;
        internal RcOverride* @rc_override;
        internal sbyte* @rc_eq;
        internal long @rc_max_rate;
        internal long @rc_min_rate;
        internal float @rc_buffer_aggressivity;
        internal float @rc_initial_cplx;
        internal float @rc_max_available_vbv_use;
        internal float @rc_min_vbv_overflow_use;
        internal int @rc_initial_buffer_occupancy;
        internal int @coder_type;
        internal int @context_model;
        internal int @lmin;
        internal int @lmax;
        internal int @frame_skip_threshold;
        internal int @frame_skip_factor;
        internal int @frame_skip_exp;
        internal int @frame_skip_cmp;
        internal int @trellis;
        internal int @min_prediction_order;
        internal int @max_prediction_order;
        internal long @timecode_frame_start;
        internal IntPtr @rtp_callback;
        internal int @rtp_payload_size;
        internal int @mv_bits;
        internal int @header_bits;
        internal int @i_tex_bits;
        internal int @p_tex_bits;
        internal int @i_count;
        internal int @p_count;
        internal int @skip_count;
        internal int @misc_bits;
        internal int @frame_bits;
        internal sbyte* @stats_out;
        internal sbyte* @stats_in;
        internal int @workaround_bugs;
        internal int @strict_std_compliance;
        internal int @error_concealment;
        internal int @debug;
        internal int @debug_mv;
        internal int @err_recognition;
        internal long @reordered_opaque;
        internal AVHWAccel* @hwaccel;
        internal void* @hwaccel_context;
        internal fixed ulong @error[8]; 
        internal int @dct_algo;
        internal int @idct_algo;
        internal int @bits_per_coded_sample;
        internal int @bits_per_raw_sample;
        internal int @lowres;
        internal AVFrame* @coded_frame;
        internal int @thread_count;
        internal int @thread_type;
        internal int @active_thread_type;
        internal int @thread_safe_callbacks;
        internal IntPtr @execute;
        internal IntPtr @execute2;
        internal int @nsse_weight;
        internal int @profile;
        internal int @level;
        internal AVDiscard @skip_loop_filter;
        internal AVDiscard @skip_idct;
        internal AVDiscard @skip_frame;
        internal sbyte* @subtitle_header;
        internal int @subtitle_header_size;
        internal int @error_rate;
        internal ulong @vbv_delay;
        internal int @side_data_only_packets;
        internal int @initial_padding;
        internal AVRational @framerate;
        internal AVPixelFormat @sw_pix_fmt;
        internal AVRational @pkt_timebase;
        internal AVCodecDescriptor* @codec_descriptor;
        internal long @pts_correction_num_faulty_pts;
        internal long @pts_correction_num_faulty_dts;
        internal long @pts_correction_last_pts;
        internal long @pts_correction_last_dts;
        internal sbyte* @sub_charenc;
        internal int @sub_charenc_mode;
        internal int @skip_alpha;
        internal int @seek_preroll;
        internal ushort* @chroma_intra_matrix;
        internal sbyte* @dump_separator;
        internal sbyte* @codec_whitelist;
        internal uint @properties;
        internal AVPacketSideData* @coded_side_data;
        internal int @nb_coded_side_data;
        internal AVBufferRef* @hw_frames_ctx;
        internal int @sub_text_format;
        internal int @trailing_padding;
    }
    
    internal unsafe partial struct AVHWAccel
    {
    }
    
    internal unsafe partial struct AVCodec
    {
    }
    
    internal unsafe partial struct AVProfile
    {
        internal int @profile;
        internal sbyte* @name;
    }
    
    internal unsafe partial struct AVCodecDefault
    {
    }
    
    internal unsafe partial struct AVSubtitle
    {
    }
    
    internal unsafe partial struct AVCodec
    {
        internal sbyte* @name;
        internal sbyte* @long_name;
        internal AVMediaType @type;
        internal AvCodecId @id;
        internal int @capabilities;
        internal AVRational* @supported_framerates;
        internal AVPixelFormat* @pix_fmts;
        internal int* @supported_samplerates;
        internal AVSampleFormat* @sample_fmts;
        internal ulong* @channel_layouts;
        internal sbyte @max_lowres;
        internal AVClass* @priv_class;
        internal AVProfile* @profiles;
        internal int @priv_data_size;
        internal AVCodec* @next;
        internal IntPtr @init_thread_copy;
        internal IntPtr @update_thread_context;
        internal AVCodecDefault* @defaults;
        internal IntPtr @init_static_data;
        internal IntPtr @init;
        internal IntPtr @encode_sub;
        internal IntPtr @encode2;
        internal IntPtr @decode;
        internal IntPtr @close;
        internal IntPtr @send_frame;
        internal IntPtr @send_packet;
        internal IntPtr @receive_frame;
        internal IntPtr @receive_packet;
        internal IntPtr @flush;
        internal int @caps_internal;
    }
    
    internal unsafe partial struct MpegEncContext
    {
    }
    
    internal unsafe partial struct AVHWAccel
    {
        internal sbyte* @name;
        internal AVMediaType @type;
        internal AvCodecId @id;
        internal AVPixelFormat @pix_fmt;
        internal int @capabilities;
        internal AVHWAccel* @next;
        internal IntPtr @alloc_frame;
        internal IntPtr @start_frame;
        internal IntPtr @decode_slice;
        internal IntPtr @end_frame;
        internal int @frame_priv_data_size;
        internal IntPtr @decode_mb;
        internal IntPtr @init;
        internal IntPtr @uninit;
        internal int @priv_data_size;
    }
    
    internal unsafe partial struct AVPicture
    {
        internal sbyte* @data0; internal sbyte* @data1; internal sbyte* @data2; internal sbyte* @data3; internal sbyte* @data4; internal sbyte* @data5; internal sbyte* @data6; internal sbyte* @data7; 
        internal fixed int @linesize[8]; 
    }
    
    internal unsafe partial struct AVSubtitleRect
    {
        internal int @x;
        internal int @y;
        internal int @w;
        internal int @h;
        internal int @nb_colors;
        internal AVPicture @pict;
        internal sbyte* @data0; internal sbyte* @data1; internal sbyte* @data2; internal sbyte* @data3; 
        internal fixed int @linesize[4]; 
        internal AVSubtitleType @type;
        internal sbyte* @text;
        internal sbyte* @ass;
        internal int @flags;
    }
    
    internal unsafe partial struct AVSubtitle
    {
        internal ushort @format;
        internal uint @start_display_time;
        internal uint @end_display_time;
        internal uint @num_rects;
        internal AVSubtitleRect** @rects;
        internal long @pts;
    }
    
    internal unsafe partial struct AVCodecParameters
    {
        internal AVMediaType @codec_type;
        internal AvCodecId @codec_id;
        internal uint @codec_tag;
        internal sbyte* @extradata;
        internal int @extradata_size;
        internal int @format;
        internal long @bit_rate;
        internal int @bits_per_coded_sample;
        internal int @bits_per_raw_sample;
        internal int @profile;
        internal int @level;
        internal int @width;
        internal int @height;
        internal AVRational @sample_aspect_ratio;
        internal AVFieldOrder @field_order;
        internal AVColorRange @color_range;
        internal AVColorPrimaries @color_primaries;
        internal AVColorTransferCharacteristic @color_trc;
        internal AVColorSpace @color_space;
        internal AVChromaLocation @chroma_location;
        internal int @video_delay;
        internal ulong @channel_layout;
        internal int @channels;
        internal int @sample_rate;
        internal int @block_align;
        internal int @frame_size;
        internal int @initial_padding;
        internal int @trailing_padding;
        internal int @seek_preroll;
    }
    
    internal unsafe partial struct AVCodecParserContext
    {
        internal void* @priv_data;
        internal AVCodecParser* @parser;
        internal long @frame_offset;
        internal long @cur_offset;
        internal long @next_frame_offset;
        internal int @pict_type;
        internal int @repeat_pict;
        internal long @pts;
        internal long @dts;
        internal long @last_pts;
        internal long @last_dts;
        internal int @fetch_timestamp;
        internal int @cur_frame_start_index;
        internal fixed long @cur_frame_offset[4]; 
        internal fixed long @cur_frame_pts[4]; 
        internal fixed long @cur_frame_dts[4]; 
        internal int @flags;
        internal long @offset;
        internal fixed long @cur_frame_end[4]; 
        internal int @key_frame;
        internal long @convergence_duration;
        internal int @dts_sync_point;
        internal int @dts_ref_dts_delta;
        internal int @pts_dts_delta;
        internal fixed long @cur_frame_pos[4]; 
        internal long @pos;
        internal long @last_pos;
        internal int @duration;
        internal AVFieldOrder @field_order;
        internal AVPictureStructure @picture_structure;
        internal int @output_picture_number;
        internal int @width;
        internal int @height;
        internal int @coded_width;
        internal int @coded_height;
        internal int @format;
    }
    
    internal unsafe partial struct AVCodecParser
    {
    }
    
    internal unsafe partial struct AVCodecParser
    {
        internal fixed int @codec_ids[5]; 
        internal int @priv_data_size;
        internal IntPtr @parser_init;
        internal IntPtr @parser_parse;
        internal IntPtr @parser_close;
        internal IntPtr @split;
        internal AVCodecParser* @next;
    }
    
    internal unsafe partial struct ReSampleContext
    {
    }
    
    internal unsafe partial struct AVResampleContext
    {
    }
    
    internal unsafe partial struct AVBitStreamFilterContext
    {
        internal void* @priv_data;
        internal AVBitStreamFilter* @filter;
        internal AVCodecParserContext* @parser;
        internal AVBitStreamFilterContext* @next;
        internal sbyte* @args;
    }
    
    internal unsafe partial struct AVBitStreamFilter
    {
    }
    
    internal unsafe partial struct AVBSFInternal
    {
    }
    
    internal unsafe partial struct AVBSFContext
    {
        internal AVClass* @av_class;
        internal AVBitStreamFilter* @filter;
        internal AVBSFInternal* @internal;
        internal void* @priv_data;
        internal AVCodecParameters* @par_in;
        internal AVCodecParameters* @par_out;
        internal AVRational @time_base_in;
        internal AVRational @time_base_out;
    }
    
    internal unsafe partial struct AVBitStreamFilter
    {
        internal sbyte* @name;
        internal AvCodecId* @codec_ids;
        internal AVClass* @priv_class;
        internal int @priv_data_size;
        internal IntPtr @init;
        internal IntPtr @filter;
        internal IntPtr @close;
    }
    
    internal unsafe partial struct AVBSFList
    {
    }

    internal enum Motion_Est_ID : int
    {
        @ME_ZERO = 1,
        @ME_FULL = 2,
        @ME_LOG = 3,
        @ME_PHODS = 4,
        @ME_EPZS = 5,
        @ME_X1 = 6,
        @ME_HEX = 7,
        @ME_UMH = 8,
        @ME_TESA = 9,
        @ME_ITER = 50,
    }
    
    internal enum AVDiscard : int
    {
        @AVDISCARD_NONE = -16,
        @AVDISCARD_DEFAULT = 0,
        @AVDISCARD_NONREF = 8,
        @AVDISCARD_BIDIR = 16,
        @AVDISCARD_NONINTRA = 24,
        @AVDISCARD_NONKEY = 32,
        @AVDISCARD_ALL = 48,
    }
    
    internal enum AVAudioServiceType : int
    {
        @AV_AUDIO_SERVICE_TYPE_MAIN = 0,
        @AV_AUDIO_SERVICE_TYPE_EFFECTS = 1,
        @AV_AUDIO_SERVICE_TYPE_VISUALLY_IMPAIRED = 2,
        @AV_AUDIO_SERVICE_TYPE_HEARING_IMPAIRED = 3,
        @AV_AUDIO_SERVICE_TYPE_DIALOGUE = 4,
        @AV_AUDIO_SERVICE_TYPE_COMMENTARY = 5,
        @AV_AUDIO_SERVICE_TYPE_EMERGENCY = 6,
        @AV_AUDIO_SERVICE_TYPE_VOICE_OVER = 7,
        @AV_AUDIO_SERVICE_TYPE_KARAOKE = 8,
        @AV_AUDIO_SERVICE_TYPE_NB = 9,
    }
    
    internal enum AVPacketSideDataType : int
    {
        @AV_PKT_DATA_PALETTE = 0,
        @AV_PKT_DATA_NEW_EXTRADATA = 1,
        @AV_PKT_DATA_PARAM_CHANGE = 2,
        @AV_PKT_DATA_H263_MB_INFO = 3,
        @AV_PKT_DATA_REPLAYGAIN = 4,
        @AV_PKT_DATA_DISPLAYMATRIX = 5,
        @AV_PKT_DATA_STEREO3D = 6,
        @AV_PKT_DATA_AUDIO_SERVICE_TYPE = 7,
        @AV_PKT_DATA_QUALITY_STATS = 8,
        @AV_PKT_DATA_FALLBACK_TRACK = 9,
        @AV_PKT_DATA_CPB_PROPERTIES = 10,
        @AV_PKT_DATA_SKIP_SAMPLES = 70,
        @AV_PKT_DATA_JP_DUALMONO = 71,
        @AV_PKT_DATA_STRINGS_METADATA = 72,
        @AV_PKT_DATA_SUBTITLE_POSITION = 73,
        @AV_PKT_DATA_MATROSKA_BLOCKADDITIONAL = 74,
        @AV_PKT_DATA_WEBVTT_IDENTIFIER = 75,
        @AV_PKT_DATA_WEBVTT_SETTINGS = 76,
        @AV_PKT_DATA_METADATA_UPDATE = 77,
        @AV_PKT_DATA_MPEGTS_STREAM_ID = 78,
        @AV_PKT_DATA_MASTERING_DISPLAY_METADATA = 79,
    }
    
    internal enum AVSideDataParamChangeFlags : int
    {
        @AV_SIDE_DATA_PARAM_CHANGE_CHANNEL_COUNT = 1,
        @AV_SIDE_DATA_PARAM_CHANGE_CHANNEL_LAYOUT = 2,
        @AV_SIDE_DATA_PARAM_CHANGE_SAMPLE_RATE = 4,
        @AV_SIDE_DATA_PARAM_CHANGE_DIMENSIONS = 8,
    }
    
    internal enum AVFieldOrder : int
    {
        @AV_FIELD_UNKNOWN = 0,
        @AV_FIELD_PROGRESSIVE = 1,
        @AV_FIELD_TT = 2,
        @AV_FIELD_BB = 3,
        @AV_FIELD_TB = 4,
        @AV_FIELD_BT = 5,
    }
    
    internal enum AVSubtitleType : int
    {
        @SUBTITLE_NONE = 0,
        @SUBTITLE_BITMAP = 1,
        @SUBTITLE_TEXT = 2,
        @SUBTITLE_ASS = 3,
    }
    
    internal enum AVPictureStructure : int
    {
        @AV_PICTURE_STRUCTURE_UNKNOWN = 0,
        @AV_PICTURE_STRUCTURE_TOP_FIELD = 1,
        @AV_PICTURE_STRUCTURE_BOTTOM_FIELD = 2,
        @AV_PICTURE_STRUCTURE_FRAME = 3,
    }
    
    internal enum AVLockOp : int
    {
        @AV_LOCK_CREATE = 0,
        @AV_LOCK_OBTAIN = 1,
        @AV_LOCK_RELEASE = 2,
        @AV_LOCK_DESTROY = 3,
    }
    
    internal unsafe static partial class ffmpeg
    {
        internal const int LIBAVCODEC_VERSION_MAJOR = 57;
        internal const int LIBAVCODEC_VERSION_MINOR = 64;
        internal const int LIBAVCODEC_VERSION_MICRO = 100;
        internal const bool FF_API_VIMA_DECODER = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_AUDIO_CONVERT = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_AVCODEC_RESAMPLE = FF_API_AUDIO_CONVERT;
        internal const bool FF_API_GETCHROMA = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_MISSING_SAMPLE = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_LOWRES = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_CAP_VDPAU = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_BUFS_VDPAU = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_VOXWARE = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_SET_DIMENSIONS = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_DEBUG_MV = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_AC_VLC = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_OLD_MSMPEG4 = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_ASPECT_EXTENDED = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_ARCH_ALPHA = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_ERROR_RATE = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_QSCALE_TYPE = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_MB_TYPE = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_MAX_BFRAMES = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_NEG_LINESIZES = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_EMU_EDGE = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_ARCH_SH4 = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_ARCH_SPARC = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_UNUSED_MEMBERS = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_IDCT_XVIDMMX = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_INPUT_PRESERVED = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_NORMALIZE_AQP = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_GMC = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_MV0 = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_CODEC_NAME = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_AFD = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_VISMV = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_AUDIOENC_DELAY = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_VAAPI_CONTEXT = (LIBAVCODEC_VERSION_MAJOR<58);
        internal const bool FF_API_AVCTX_TIMEBASE = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_MPV_OPT = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_STREAM_CODEC_TAG = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_QUANT_BIAS = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_RC_STRATEGY = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_CODED_FRAME = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_MOTION_EST = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_WITHOUT_PREFIX = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_SIDEDATA_ONLY_PKT = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_VDPAU_PROFILE = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_CONVERGENCE_DURATION = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_AVPICTURE = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_AVPACKET_OLD_API = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_RTP_CALLBACK = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_VBV_DELAY = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_CODER_TYPE = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_STAT_BITS = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_PRIVATE_OPT = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_ASS_TIMING = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_OLD_BSF = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_COPY_CONTEXT = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_GET_CONTEXT_DEFAULTS = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const bool FF_API_NVENC_OLD_NAME = (LIBAVCODEC_VERSION_MAJOR<59);
        internal const int AV_CODEC_PROP_INTRA_ONLY = (1<<0);
        internal const int AV_CODEC_PROP_LOSSY = (1<<1);
        internal const int AV_CODEC_PROP_LOSSLESS = (1<<2);
        internal const int AV_CODEC_PROP_REORDER = (1<<3);
        internal const int AV_CODEC_PROP_BITMAP_SUB = (1<<16);
        internal const int AV_CODEC_PROP_TEXT_SUB = (1<<17);
        internal const int AV_INPUT_BUFFER_PADDING_SIZE = 32;
        internal const int AV_INPUT_BUFFER_MIN_SIZE = 16384;
        internal const int FF_INPUT_BUFFER_PADDING_SIZE = 32;
        internal const int FF_MIN_BUFFER_SIZE = 16384;
        internal const int FF_MAX_B_FRAMES = 16;
        internal const int AV_CODEC_FLAG_UNALIGNED = (1<<0);
        internal const int AV_CODEC_FLAG_QSCALE = (1<<1);
        internal const int AV_CODEC_FLAG_4MV = (1<<2);
        internal const int AV_CODEC_FLAG_OUTPUT_CORRUPT = (1<<3);
        internal const int AV_CODEC_FLAG_QPEL = (1<<4);
        internal const int AV_CODEC_FLAG_PASS1 = (1<<9);
        internal const int AV_CODEC_FLAG_PASS2 = (1<<10);
        internal const int AV_CODEC_FLAG_LOOP_FILTER = (1<<11);
        internal const int AV_CODEC_FLAG_GRAY = (1<<13);
        internal const int AV_CODEC_FLAG_PSNR = (1<<15);
        internal const int AV_CODEC_FLAG_TRUNCATED = (1<<16);
        internal const int AV_CODEC_FLAG_INTERLACED_DCT = (1<<18);
        internal const int AV_CODEC_FLAG_LOW_DELAY = (1<<19);
        internal const int AV_CODEC_FLAG_GLOBAL_HEADER = (1<<22);
        internal const int AV_CODEC_FLAG_BITEXACT = (1<<23);
        internal const int AV_CODEC_FLAG_AC_PRED = (1<<24);
        internal const int AV_CODEC_FLAG_INTERLACED_ME = (1<<29);
        internal const uint AV_CODEC_FLAG_CLOSED_GOP = (1U<<31);
        internal const int AV_CODEC_FLAG2_FAST = (1<<0);
        internal const int AV_CODEC_FLAG2_NO_OUTPUT = (1<<2);
        internal const int AV_CODEC_FLAG2_LOCAL_HEADER = (1<<3);
        internal const int AV_CODEC_FLAG2_DROP_FRAME_TIMECODE = (1<<13);
        internal const int AV_CODEC_FLAG2_CHUNKS = (1<<15);
        internal const int AV_CODEC_FLAG2_IGNORE_CROP = (1<<16);
        internal const int AV_CODEC_FLAG2_SHOW_ALL = (1<<22);
        internal const int AV_CODEC_FLAG2_EXPORT_MVS = (1<<28);
        internal const int AV_CODEC_FLAG2_SKIP_MANUAL = (1<<29);
        internal const int AV_CODEC_FLAG2_RO_FLUSH_NOOP = (1<<30);
        internal const int AV_CODEC_CAP_DRAW_HORIZ_BAND = (1<<0);
        internal const int AV_CODEC_CAP_DR1 = (1<<1);
        internal const int AV_CODEC_CAP_TRUNCATED = (1<<3);
        internal const int AV_CODEC_CAP_DELAY = (1<<5);
        internal const int AV_CODEC_CAP_SMALL_LAST_FRAME = (1<<6);
        internal const int AV_CODEC_CAP_HWACCEL_VDPAU = (1<<7);
        internal const int AV_CODEC_CAP_SUBFRAMES = (1<<8);
        internal const int AV_CODEC_CAP_EXPERIMENTAL = (1<<9);
        internal const int AV_CODEC_CAP_CHANNEL_CONF = (1<<10);
        internal const int AV_CODEC_CAP_FRAME_THREADS = (1<<12);
        internal const int AV_CODEC_CAP_SLICE_THREADS = (1<<13);
        internal const int AV_CODEC_CAP_PARAM_CHANGE = (1<<14);
        internal const int AV_CODEC_CAP_AUTO_THREADS = (1<<15);
        internal const int AV_CODEC_CAP_VARIABLE_FRAME_SIZE = (1<<16);
        internal const int AV_CODEC_CAP_AVOID_PROBING = (1<<17);
        internal const int AV_CODEC_CAP_INTRA_ONLY = 0x40000000;
        internal const uint AV_CODEC_CAP_LOSSLESS = 0x80000000;
        internal const int CODEC_FLAG_UNALIGNED = AV_CODEC_FLAG_UNALIGNED;
        internal const int CODEC_FLAG_QSCALE = AV_CODEC_FLAG_QSCALE;
        internal const int CODEC_FLAG_4MV = AV_CODEC_FLAG_4MV;
        internal const int CODEC_FLAG_OUTPUT_CORRUPT = AV_CODEC_FLAG_OUTPUT_CORRUPT;
        internal const int CODEC_FLAG_QPEL = AV_CODEC_FLAG_QPEL;
        internal const int CODEC_FLAG_GMC = 0x0020;
        internal const int CODEC_FLAG_MV0 = 0x0040;
        internal const int CODEC_FLAG_INPUT_PRESERVED = 0x0100;
        internal const int CODEC_FLAG_PASS1 = AV_CODEC_FLAG_PASS1;
        internal const int CODEC_FLAG_PASS2 = AV_CODEC_FLAG_PASS2;
        internal const int CODEC_FLAG_GRAY = AV_CODEC_FLAG_GRAY;
        internal const int CODEC_FLAG_EMU_EDGE = 0x4000;
        internal const int CODEC_FLAG_PSNR = AV_CODEC_FLAG_PSNR;
        internal const int CODEC_FLAG_TRUNCATED = AV_CODEC_FLAG_TRUNCATED;
        internal const int CODEC_FLAG_NORMALIZE_AQP = 0x00020000;
        internal const int CODEC_FLAG_INTERLACED_DCT = AV_CODEC_FLAG_INTERLACED_DCT;
        internal const int CODEC_FLAG_LOW_DELAY = AV_CODEC_FLAG_LOW_DELAY;
        internal const int CODEC_FLAG_GLOBAL_HEADER = AV_CODEC_FLAG_GLOBAL_HEADER;
        internal const int CODEC_FLAG_BITEXACT = AV_CODEC_FLAG_BITEXACT;
        internal const int CODEC_FLAG_AC_PRED = AV_CODEC_FLAG_AC_PRED;
        internal const int CODEC_FLAG_LOOP_FILTER = AV_CODEC_FLAG_LOOP_FILTER;
        internal const int CODEC_FLAG_INTERLACED_ME = AV_CODEC_FLAG_INTERLACED_ME;
        internal const uint CODEC_FLAG_CLOSED_GOP = AV_CODEC_FLAG_CLOSED_GOP;
        internal const int CODEC_FLAG2_FAST = AV_CODEC_FLAG2_FAST;
        internal const int CODEC_FLAG2_NO_OUTPUT = AV_CODEC_FLAG2_NO_OUTPUT;
        internal const int CODEC_FLAG2_LOCAL_HEADER = AV_CODEC_FLAG2_LOCAL_HEADER;
        internal const int CODEC_FLAG2_DROP_FRAME_TIMECODE = AV_CODEC_FLAG2_DROP_FRAME_TIMECODE;
        internal const int CODEC_FLAG2_IGNORE_CROP = AV_CODEC_FLAG2_IGNORE_CROP;
        internal const int CODEC_FLAG2_CHUNKS = AV_CODEC_FLAG2_CHUNKS;
        internal const int CODEC_FLAG2_SHOW_ALL = AV_CODEC_FLAG2_SHOW_ALL;
        internal const int CODEC_FLAG2_EXPORT_MVS = AV_CODEC_FLAG2_EXPORT_MVS;
        internal const int CODEC_FLAG2_SKIP_MANUAL = AV_CODEC_FLAG2_SKIP_MANUAL;
        internal const int CODEC_CAP_DRAW_HORIZ_BAND = AV_CODEC_CAP_DRAW_HORIZ_BAND;
        internal const int CODEC_CAP_DR1 = AV_CODEC_CAP_DR1;
        internal const int CODEC_CAP_TRUNCATED = AV_CODEC_CAP_TRUNCATED;
        internal const int CODEC_CAP_HWACCEL = 0x0010;
        internal const int CODEC_CAP_DELAY = AV_CODEC_CAP_DELAY;
        internal const int CODEC_CAP_SMALL_LAST_FRAME = AV_CODEC_CAP_SMALL_LAST_FRAME;
        internal const int CODEC_CAP_HWACCEL_VDPAU = AV_CODEC_CAP_HWACCEL_VDPAU;
        internal const int CODEC_CAP_SUBFRAMES = AV_CODEC_CAP_SUBFRAMES;
        internal const int CODEC_CAP_EXPERIMENTAL = AV_CODEC_CAP_EXPERIMENTAL;
        internal const int CODEC_CAP_CHANNEL_CONF = AV_CODEC_CAP_CHANNEL_CONF;
        internal const int CODEC_CAP_NEG_LINESIZES = 0x0800;
        internal const int CODEC_CAP_FRAME_THREADS = AV_CODEC_CAP_FRAME_THREADS;
        internal const int CODEC_CAP_SLICE_THREADS = AV_CODEC_CAP_SLICE_THREADS;
        internal const int CODEC_CAP_PARAM_CHANGE = AV_CODEC_CAP_PARAM_CHANGE;
        internal const int CODEC_CAP_AUTO_THREADS = AV_CODEC_CAP_AUTO_THREADS;
        internal const int CODEC_CAP_VARIABLE_FRAME_SIZE = AV_CODEC_CAP_VARIABLE_FRAME_SIZE;
        internal const int CODEC_CAP_INTRA_ONLY = AV_CODEC_CAP_INTRA_ONLY;
        internal const uint CODEC_CAP_LOSSLESS = AV_CODEC_CAP_LOSSLESS;
        internal const int HWACCEL_CODEC_CAP_EXPERIMENTAL = 0x0200;
        internal const int MB_TYPE_INTRA4x4 = 0x0001;
        internal const int MB_TYPE_INTRA16x16 = 0x0002;
        internal const int MB_TYPE_INTRA_PCM = 0x0004;
        internal const int MB_TYPE_16x16 = 0x0008;
        internal const int MB_TYPE_16x8 = 0x0010;
        internal const int MB_TYPE_8x16 = 0x0020;
        internal const int MB_TYPE_8x8 = 0x0040;
        internal const int MB_TYPE_INTERLACED = 0x0080;
        internal const int MB_TYPE_DIRECT2 = 0x0100;
        internal const int MB_TYPE_ACPRED = 0x0200;
        internal const int MB_TYPE_GMC = 0x0400;
        internal const int MB_TYPE_SKIP = 0x0800;
        internal const int MB_TYPE_P0L0 = 0x1000;
        internal const int MB_TYPE_P1L0 = 0x2000;
        internal const int MB_TYPE_P0L1 = 0x4000;
        internal const int MB_TYPE_P1L1 = 0x8000;
        internal const int MB_TYPE_L0 = (MB_TYPE_P0L0|MB_TYPE_P1L0);
        internal const int MB_TYPE_L1 = (MB_TYPE_P0L1|MB_TYPE_P1L1);
        internal const int MB_TYPE_L0L1 = (MB_TYPE_L0|MB_TYPE_L1);
        internal const int MB_TYPE_QUANT = 0x00010000;
        internal const int MB_TYPE_CBP = 0x00020000;
        internal const int FF_QSCALE_TYPE_MPEG1 = 0;
        internal const int FF_QSCALE_TYPE_MPEG2 = 1;
        internal const int FF_QSCALE_TYPE_H264 = 2;
        internal const int FF_QSCALE_TYPE_VP56 = 3;
        internal const int AV_GET_BUFFER_FLAG_REF = (1<<0);
        internal const int AV_PKT_FLAG_KEY = 0x0001;
        internal const int AV_PKT_FLAG_CORRUPT = 0x0002;
        internal const int AV_PKT_FLAG_DISCARD = 0x0004;
        internal const int FF_COMPRESSION_DEFAULT = -1;
        internal const int FF_ASPECT_EXTENDED = 15;
        internal const int FF_RC_STRATEGY_XVID = 1;
        internal const int FF_PRED_LEFT = 0;
        internal const int FF_PRED_PLANE = 1;
        internal const int FF_PRED_MEDIAN = 2;
        internal const int FF_CMP_SAD = 0;
        internal const int FF_CMP_SSE = 1;
        internal const int FF_CMP_SATD = 2;
        internal const int FF_CMP_DCT = 3;
        internal const int FF_CMP_PSNR = 4;
        internal const int FF_CMP_BIT = 5;
        internal const int FF_CMP_RD = 6;
        internal const int FF_CMP_ZERO = 7;
        internal const int FF_CMP_VSAD = 8;
        internal const int FF_CMP_VSSE = 9;
        internal const int FF_CMP_NSSE = 10;
        internal const int FF_CMP_W53 = 11;
        internal const int FF_CMP_W97 = 12;
        internal const int FF_CMP_DCTMAX = 13;
        internal const int FF_CMP_DCT264 = 14;
        internal const int FF_CMP_MEDIAN_SAD = 15;
        internal const int FF_CMP_CHROMA = 256;
        internal const int FF_DTG_AFD_SAME = 8;
        internal const int FF_DTG_AFD_4_3 = 9;
        internal const int FF_DTG_AFD_16_9 = 10;
        internal const int FF_DTG_AFD_14_9 = 11;
        internal const int FF_DTG_AFD_4_3_SP_14_9 = 13;
        internal const int FF_DTG_AFD_16_9_SP_14_9 = 14;
        internal const int FF_DTG_AFD_SP_4_3 = 15;
        internal const int FF_DEFAULT_QUANT_BIAS = 999999;
        internal const int SLICE_FLAG_CODED_ORDER = 0x0001;
        internal const int SLICE_FLAG_ALLOW_FIELD = 0x0002;
        internal const int SLICE_FLAG_ALLOW_PLANE = 0x0004;
        internal const int FF_MB_DECISION_SIMPLE = 0;
        internal const int FF_MB_DECISION_BITS = 1;
        internal const int FF_MB_DECISION_RD = 2;
        internal const int FF_CODER_TYPE_VLC = 0;
        internal const int FF_CODER_TYPE_AC = 1;
        internal const int FF_CODER_TYPE_RAW = 2;
        internal const int FF_CODER_TYPE_RLE = 3;
        internal const int FF_CODER_TYPE_DEFLATE = 4;
        internal const int FF_BUG_AUTODETECT = 1;
        internal const int FF_BUG_OLD_MSMPEG4 = 2;
        internal const int FF_BUG_XVID_ILACE = 4;
        internal const int FF_BUG_UMP4 = 8;
        internal const int FF_BUG_NO_PADDING = 16;
        internal const int FF_BUG_AMV = 32;
        internal const int FF_BUG_AC_VLC = 0;
        internal const int FF_BUG_QPEL_CHROMA = 64;
        internal const int FF_BUG_STD_QPEL = 128;
        internal const int FF_BUG_QPEL_CHROMA2 = 256;
        internal const int FF_BUG_DIRECT_BLOCKSIZE = 512;
        internal const int FF_BUG_EDGE = 1024;
        internal const int FF_BUG_HPEL_CHROMA = 2048;
        internal const int FF_BUG_DC_CLIP = 4096;
        internal const int FF_BUG_MS = 8192;
        internal const int FF_BUG_TRUNCATED = 16384;
        internal const int FF_COMPLIANCE_VERY_STRICT = 2;
        internal const int FF_COMPLIANCE_STRICT = 1;
        internal const int FF_COMPLIANCE_NORMAL = 0;
        internal const int FF_COMPLIANCE_UNOFFICIAL = -1;
        internal const int FF_COMPLIANCE_EXPERIMENTAL = -2;
        internal const int FF_EC_GUESS_MVS = 1;
        internal const int FF_EC_DEBLOCK = 2;
        internal const int FF_EC_FAVOR_INTER = 256;
        internal const int FF_DEBUG_PICT_INFO = 1;
        internal const int FF_DEBUG_RC = 2;
        internal const int FF_DEBUG_BITSTREAM = 4;
        internal const int FF_DEBUG_MB_TYPE = 8;
        internal const int FF_DEBUG_QP = 16;
        internal const int FF_DEBUG_MV = 32;
        internal const int FF_DEBUG_DCT_COEFF = 0x00000040;
        internal const int FF_DEBUG_SKIP = 0x00000080;
        internal const int FF_DEBUG_STARTCODE = 0x00000100;
        internal const int FF_DEBUG_PTS = 0x00000200;
        internal const int FF_DEBUG_ER = 0x00000400;
        internal const int FF_DEBUG_MMCO = 0x00000800;
        internal const int FF_DEBUG_BUGS = 0x00001000;
        internal const int FF_DEBUG_VIS_QP = 0x00002000;
        internal const int FF_DEBUG_VIS_MB_TYPE = 0x00004000;
        internal const int FF_DEBUG_BUFFERS = 0x00008000;
        internal const int FF_DEBUG_THREADS = 0x00010000;
        internal const int FF_DEBUG_GREEN_MD = 0x00800000;
        internal const int FF_DEBUG_NOMC = 0x01000000;
        internal const int FF_DEBUG_VIS_MV_P_FOR = 0x00000001;
        internal const int FF_DEBUG_VIS_MV_B_FOR = 0x00000002;
        internal const int FF_DEBUG_VIS_MV_B_BACK = 0x00000004;
        internal const int AV_EF_CRCCHECK = (1<<0);
        internal const int AV_EF_BITSTREAM = (1<<1);
        internal const int AV_EF_BUFFER = (1<<2);
        internal const int AV_EF_EXPLODE = (1<<3);
        internal const int AV_EF_IGNORE_ERR = (1<<15);
        internal const int AV_EF_CAREFUL = (1<<16);
        internal const int AV_EF_COMPLIANT = (1<<17);
        internal const int AV_EF_AGGRESSIVE = (1<<18);
        internal const int FF_DCT_AUTO = 0;
        internal const int FF_DCT_FASTINT = 1;
        internal const int FF_DCT_INT = 2;
        internal const int FF_DCT_MMX = 3;
        internal const int FF_DCT_ALTIVEC = 5;
        internal const int FF_DCT_FAAN = 6;
        internal const int FF_IDCT_AUTO = 0;
        internal const int FF_IDCT_INT = 1;
        internal const int FF_IDCT_SIMPLE = 2;
        internal const int FF_IDCT_SIMPLEMMX = 3;
        internal const int FF_IDCT_ARM = 7;
        internal const int FF_IDCT_ALTIVEC = 8;
        internal const int FF_IDCT_SH4 = 9;
        internal const int FF_IDCT_SIMPLEARM = 10;
        internal const int FF_IDCT_IPP = 13;
        internal const int FF_IDCT_XVID = 14;
        internal const int FF_IDCT_XVIDMMX = 14;
        internal const int FF_IDCT_SIMPLEARMV5TE = 16;
        internal const int FF_IDCT_SIMPLEARMV6 = 17;
        internal const int FF_IDCT_SIMPLEVIS = 18;
        internal const int FF_IDCT_FAAN = 20;
        internal const int FF_IDCT_SIMPLENEON = 22;
        internal const int FF_IDCT_SIMPLEALPHA = 23;
        internal const int FF_IDCT_SIMPLEAUTO = 128;
        internal const int FF_THREAD_FRAME = 1;
        internal const int FF_THREAD_SLICE = 2;
        internal const int FF_PROFILE_UNKNOWN = -99;
        internal const int FF_PROFILE_RESERVED = -100;
        internal const int FF_PROFILE_AAC_MAIN = 0;
        internal const int FF_PROFILE_AAC_LOW = 1;
        internal const int FF_PROFILE_AAC_SSR = 2;
        internal const int FF_PROFILE_AAC_LTP = 3;
        internal const int FF_PROFILE_AAC_HE = 4;
        internal const int FF_PROFILE_AAC_HE_V2 = 28;
        internal const int FF_PROFILE_AAC_LD = 22;
        internal const int FF_PROFILE_AAC_ELD = 38;
        internal const int FF_PROFILE_MPEG2_AAC_LOW = 128;
        internal const int FF_PROFILE_MPEG2_AAC_HE = 131;
        internal const int FF_PROFILE_DNXHD = 0;
        internal const int FF_PROFILE_DNXHR_LB = 1;
        internal const int FF_PROFILE_DNXHR_SQ = 2;
        internal const int FF_PROFILE_DNXHR_HQ = 3;
        internal const int FF_PROFILE_DNXHR_HQX = 4;
        internal const int FF_PROFILE_DNXHR_444 = 5;
        internal const int FF_PROFILE_DTS = 20;
        internal const int FF_PROFILE_DTS_ES = 30;
        internal const int FF_PROFILE_DTS_96_24 = 40;
        internal const int FF_PROFILE_DTS_HD_HRA = 50;
        internal const int FF_PROFILE_DTS_HD_MA = 60;
        internal const int FF_PROFILE_DTS_EXPRESS = 70;
        internal const int FF_PROFILE_MPEG2_422 = 0;
        internal const int FF_PROFILE_MPEG2_HIGH = 1;
        internal const int FF_PROFILE_MPEG2_SS = 2;
        internal const int FF_PROFILE_MPEG2_SNR_SCALABLE = 3;
        internal const int FF_PROFILE_MPEG2_MAIN = 4;
        internal const int FF_PROFILE_MPEG2_SIMPLE = 5;
        internal const int FF_PROFILE_H264_CONSTRAINED = (1<<9);
        internal const int FF_PROFILE_H264_INTRA = (1<<11);
        internal const int FF_PROFILE_H264_BASELINE = 66;
        internal const int FF_PROFILE_H264_CONSTRAINED_BASELINE = (66|FF_PROFILE_H264_CONSTRAINED);
        internal const int FF_PROFILE_H264_MAIN = 77;
        internal const int FF_PROFILE_H264_EXTENDED = 88;
        internal const int FF_PROFILE_H264_HIGH = 100;
        internal const int FF_PROFILE_H264_HIGH_10 = 110;
        internal const int FF_PROFILE_H264_HIGH_10_INTRA = (110|FF_PROFILE_H264_INTRA);
        internal const int FF_PROFILE_H264_MULTIVIEW_HIGH = 118;
        internal const int FF_PROFILE_H264_HIGH_422 = 122;
        internal const int FF_PROFILE_H264_HIGH_422_INTRA = (122|FF_PROFILE_H264_INTRA);
        internal const int FF_PROFILE_H264_STEREO_HIGH = 128;
        internal const int FF_PROFILE_H264_HIGH_444 = 144;
        internal const int FF_PROFILE_H264_HIGH_444_PREDICTIVE = 244;
        internal const int FF_PROFILE_H264_HIGH_444_INTRA = (244|FF_PROFILE_H264_INTRA);
        internal const int FF_PROFILE_H264_CAVLC_444 = 44;
        internal const int FF_PROFILE_VC1_SIMPLE = 0;
        internal const int FF_PROFILE_VC1_MAIN = 1;
        internal const int FF_PROFILE_VC1_COMPLEX = 2;
        internal const int FF_PROFILE_VC1_ADVANCED = 3;
        internal const int FF_PROFILE_MPEG4_SIMPLE = 0;
        internal const int FF_PROFILE_MPEG4_SIMPLE_SCALABLE = 1;
        internal const int FF_PROFILE_MPEG4_CORE = 2;
        internal const int FF_PROFILE_MPEG4_MAIN = 3;
        internal const int FF_PROFILE_MPEG4_N_BIT = 4;
        internal const int FF_PROFILE_MPEG4_SCALABLE_TEXTURE = 5;
        internal const int FF_PROFILE_MPEG4_SIMPLE_FACE_ANIMATION = 6;
        internal const int FF_PROFILE_MPEG4_BASIC_ANIMATED_TEXTURE = 7;
        internal const int FF_PROFILE_MPEG4_HYBRID = 8;
        internal const int FF_PROFILE_MPEG4_ADVANCED_REAL_TIME = 9;
        internal const int FF_PROFILE_MPEG4_CORE_SCALABLE = 10;
        internal const int FF_PROFILE_MPEG4_ADVANCED_CODING = 11;
        internal const int FF_PROFILE_MPEG4_ADVANCED_CORE = 12;
        internal const int FF_PROFILE_MPEG4_ADVANCED_SCALABLE_TEXTURE = 13;
        internal const int FF_PROFILE_MPEG4_SIMPLE_STUDIO = 14;
        internal const int FF_PROFILE_MPEG4_ADVANCED_SIMPLE = 15;
        internal const int FF_PROFILE_JPEG2000_CSTREAM_RESTRICTION_0 = 1;
        internal const int FF_PROFILE_JPEG2000_CSTREAM_RESTRICTION_1 = 2;
        internal const int FF_PROFILE_JPEG2000_CSTREAM_NO_RESTRICTION = 32768;
        internal const int FF_PROFILE_JPEG2000_DCINEMA_2K = 3;
        internal const int FF_PROFILE_JPEG2000_DCINEMA_4K = 4;
        internal const int FF_PROFILE_VP9_0 = 0;
        internal const int FF_PROFILE_VP9_1 = 1;
        internal const int FF_PROFILE_VP9_2 = 2;
        internal const int FF_PROFILE_VP9_3 = 3;
        internal const int FF_PROFILE_HEVC_MAIN = 1;
        internal const int FF_PROFILE_HEVC_MAIN_10 = 2;
        internal const int FF_PROFILE_HEVC_MAIN_STILL_PICTURE = 3;
        internal const int FF_PROFILE_HEVC_REXT = 4;
        internal const int FF_LEVEL_UNKNOWN = -99;
        internal const int FF_SUB_CHARENC_MODE_DO_NOTHING = -1;
        internal const int FF_SUB_CHARENC_MODE_AUTOMATIC = 0;
        internal const int FF_SUB_CHARENC_MODE_PRE_DECODER = 1;
        internal const int FF_CODEC_PROPERTY_LOSSLESS = 0x00000001;
        internal const int FF_CODEC_PROPERTY_CLOSED_CAPTIONS = 0x00000002;
        internal const int FF_SUB_TEXT_FMT_ASS = 0;
        internal const int FF_SUB_TEXT_FMT_ASS_WITH_TIMINGS = 1;
        internal const int AV_HWACCEL_FLAG_IGNORE_LEVEL = (1<<0);
        internal const int AV_HWACCEL_FLAG_ALLOW_HIGH_DEPTH = (1<<1);
        internal const int AV_SUBTITLE_FLAG_FORCED = 0x00000001;
        internal const int AV_PARSER_PTS_NB = 4;
        internal const int PARSER_FLAG_COMPLETE_FRAMES = 0x0001;
        internal const int PARSER_FLAG_ONCE = 0x0002;
        internal const int PARSER_FLAG_FETCHED_OFFSET = 0x0004;
        internal const int PARSER_FLAG_USE_CODEC_TS = 0x1000;
        private const string libavcodec = "avcodec-57";
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_pkt_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_codec_get_pkt_timebase(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_pkt_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_codec_set_pkt_timebase(AVCodecContext* @avctx, AVRational @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_codec_descriptor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecDescriptor* av_codec_get_codec_descriptor(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_codec_descriptor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_codec_set_codec_descriptor(AVCodecContext* @avctx, AVCodecDescriptor* @desc);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_codec_properties", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint av_codec_get_codec_properties(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_codec_get_lowres(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_codec_set_lowres(AVCodecContext* @avctx, int @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_seek_preroll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_codec_get_seek_preroll(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_seek_preroll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_codec_set_seek_preroll(AVCodecContext* @avctx, int @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_chroma_intra_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ushort* av_codec_get_chroma_intra_matrix(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_chroma_intra_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_codec_set_chroma_intra_matrix(AVCodecContext* @avctx, ushort* @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_max_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_codec_get_max_lowres(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodec* av_codec_next(AVCodec* @c);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint avcodec_version();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avcodec_configuration();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avcodec_license();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_register(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_register_all();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_alloc_context3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecContext* avcodec_alloc_context3(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_free_context(AVCodecContext** @avctx);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_context_defaults3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_get_context_defaults3(AVCodecContext* @s, AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClass* avcodec_get_class();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_frame_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClass* avcodec_get_frame_class();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_subtitle_rect_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClass* avcodec_get_subtitle_rect_class();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_copy_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_copy_context(AVCodecContext* @dest, AVCodecContext* @src);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecParameters* avcodec_parameters_alloc();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_parameters_free(AVCodecParameters** @par);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_parameters_copy(AVCodecParameters* @dst, AVCodecParameters* @src);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_from_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_parameters_from_context(AVCodecParameters* @par, AVCodecContext* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_to_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_parameters_to_context(AVCodecContext* @codec, AVCodecParameters* @par);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_open2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_open2(AVCodecContext* @avctx, AVCodec* @codec, AVDictionary** @options);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_close(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "avsubtitle_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avsubtitle_free(AVSubtitle* @sub);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPacket* av_packet_alloc();
        
        [DllImport(libavcodec, EntryPoint = "av_packet_clone", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPacket* av_packet_clone(AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_packet_free(AVPacket** @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_init_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_init_packet(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_new_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_new_packet(AVPacket* @pkt, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_shrink_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_shrink_packet(AVPacket* @pkt, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_grow_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_grow_packet(AVPacket* @pkt, int @grow_by);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_from_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_from_data(AVPacket* @pkt, sbyte* @data, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_dup_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_dup_packet(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_copy_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_copy_packet(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_copy_packet_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_copy_packet_side_data(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_free_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_free_packet(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_packet_new_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_add_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_add_side_data(AVPacket* @pkt, AVPacketSideDataType @type, sbyte* @data, ulong @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_shrink_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_shrink_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_packet_get_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int* @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_merge_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_merge_side_data(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_split_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_split_side_data(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_side_data_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_packet_side_data_name(AVPacketSideDataType @type);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_pack_dictionary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* av_packet_pack_dictionary(AVDictionary* @dict, int* @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_unpack_dictionary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_unpack_dictionary(sbyte* @data, int @size, AVDictionary** @dict);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_free_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_packet_free_side_data(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_ref(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_packet_unref(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_move_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_packet_move_ref(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_copy_props", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_packet_copy_props(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_rescale_ts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_packet_rescale_ts(AVPacket* @pkt, AVRational @tb_src, AVRational @tb_dst);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_decoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodec* avcodec_find_decoder(AvCodecId @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_decoder_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodec* avcodec_find_decoder_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_get_buffer2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_default_get_buffer2(AVCodecContext* @s, AVFrame* @frame, int @flags);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_edge_width", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint avcodec_get_edge_width();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_align_dimensions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_align_dimensions(AVCodecContext* @s, int* @width, int* @height);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_align_dimensions2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_align_dimensions2(AVCodecContext* @s, int* @width, int* @height, [MarshalAs(UnmanagedType.LPArray, SizeConst=8)] int[] @linesize_align);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_enum_to_chroma_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_enum_to_chroma_pos(int* @xpos, int* @ypos, AVChromaLocation @pos);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_chroma_pos_to_enum", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVChromaLocation avcodec_chroma_pos_to_enum(int @xpos, int @ypos);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_decode_audio4", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_decode_audio4(AVCodecContext* @avctx, AVFrame* @frame, int* @got_frame_ptr, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_decode_video2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_decode_video2(AVCodecContext* @avctx, AVFrame* @picture, int* @got_picture_ptr, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_decode_subtitle2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_decode_subtitle2(AVCodecContext* @avctx, AVSubtitle* @sub, int* @got_sub_ptr, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_send_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_send_packet(AVCodecContext* @avctx, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_receive_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_receive_frame(AVCodecContext* @avctx, AVFrame* @frame);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_send_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_send_frame(AVCodecContext* @avctx, AVFrame* @frame);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_receive_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_receive_packet(AVCodecContext* @avctx, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecParser* av_parser_next(AVCodecParser* @c);
        
        [DllImport(libavcodec, EntryPoint = "av_register_codec_parser", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_register_codec_parser(AVCodecParser* @parser);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecParserContext* av_parser_init(int @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_parse2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_parser_parse2(AVCodecParserContext* @s, AVCodecContext* @avctx, sbyte** @poutbuf, int* @poutbuf_size, sbyte* @buf, int @buf_size, long @pts, long @dts, long @pos);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_change", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_parser_change(AVCodecParserContext* @s, AVCodecContext* @avctx, sbyte** @poutbuf, int* @poutbuf_size, sbyte* @buf, int @buf_size, int @keyframe);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_parser_close(AVCodecParserContext* @s);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_encoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodec* avcodec_find_encoder(AvCodecId @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_encoder_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodec* avcodec_find_encoder_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_encode_audio2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_encode_audio2(AVCodecContext* @avctx, AVPacket* @avpkt, AVFrame* @frame, int* @got_packet_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_encode_video2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_encode_video2(AVCodecContext* @avctx, AVPacket* @avpkt, AVFrame* @frame, int* @got_packet_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_encode_subtitle", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_encode_subtitle(AVCodecContext* @avctx, sbyte* @buf, int @buf_size, AVSubtitle* @sub);
        
        [DllImport(libavcodec, EntryPoint = "av_audio_resample_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ReSampleContext* av_audio_resample_init(int @output_channels, int @input_channels, int @output_rate, int @input_rate, AVSampleFormat @sample_fmt_out, AVSampleFormat @sample_fmt_in, int @filter_length, int @log2_phase_count, int @linear, double @cutoff);
        
        [DllImport(libavcodec, EntryPoint = "audio_resample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int audio_resample(ReSampleContext* @s, short* @output, short* @input, int @nb_samples);
        
        [DllImport(libavcodec, EntryPoint = "audio_resample_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void audio_resample_close(ReSampleContext* @s);
        
        [DllImport(libavcodec, EntryPoint = "av_resample_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVResampleContext* av_resample_init(int @out_rate, int @in_rate, int @filter_length, int @log2_phase_count, int @linear, double @cutoff);
        
        [DllImport(libavcodec, EntryPoint = "av_resample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_resample(AVResampleContext* @c, short* @dst, short* @src, int* @consumed, int @src_size, int @dst_size, int @update_ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_resample_compensate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_resample_compensate(AVResampleContext* @c, int @sample_delta, int @compensation_distance);
        
        [DllImport(libavcodec, EntryPoint = "av_resample_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_resample_close(AVResampleContext* @c);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avpicture_alloc(AVPicture* @picture, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avpicture_free(AVPicture* @picture);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_fill", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avpicture_fill(AVPicture* @picture, sbyte* @ptr, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avpicture_layout(AVPicture* @src, AVPixelFormat @pix_fmt, int @width, int @height, sbyte* @dest, int @dest_size);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_get_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avpicture_get_size(AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "av_picture_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_picture_copy(AVPicture* @dst, AVPicture* @src, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "av_picture_crop", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_picture_crop(AVPicture* @dst, AVPicture* @src, AVPixelFormat @pix_fmt, int @top_band, int @left_band);
        
        [DllImport(libavcodec, EntryPoint = "av_picture_pad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_picture_pad(AVPicture* @dst, AVPicture* @src, int @height, int @width, AVPixelFormat @pix_fmt, int @padtop, int @padbottom, int @padleft, int @padright, int* @color);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_chroma_sub_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_get_chroma_sub_sample(AVPixelFormat @pix_fmt, int* @h_shift, int* @v_shift);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_pix_fmt_to_codec_tag", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint avcodec_pix_fmt_to_codec_tag(AVPixelFormat @pix_fmt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_pix_fmt_loss", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_get_pix_fmt_loss(AVPixelFormat @dst_pix_fmt, AVPixelFormat @src_pix_fmt, int @has_alpha);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_best_pix_fmt_of_list", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat avcodec_find_best_pix_fmt_of_list(AVPixelFormat* @pix_fmt_list, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_best_pix_fmt_of_2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat avcodec_find_best_pix_fmt_of_2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_best_pix_fmt2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat avcodec_find_best_pix_fmt2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_get_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVPixelFormat avcodec_default_get_format(AVCodecContext* @s, AVPixelFormat* @fmt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_set_dimensions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_set_dimensions(AVCodecContext* @s, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "av_get_codec_tag_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern ulong av_get_codec_tag_string(IntPtr @buf, ulong @buf_size, uint @codec_tag);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_string(IntPtr @buf, int @buf_size, AVCodecContext* @enc, int @encode);
        
        [DllImport(libavcodec, EntryPoint = "av_get_profile_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string av_get_profile_name(AVCodec* @codec, int @profile);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_profile_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avcodec_profile_name(AvCodecId @codec_id, int @profile);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_execute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_default_execute(AVCodecContext* @c, IntPtr @func, void* @arg, int* @ret, int @count, int @size);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_execute2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_default_execute2(AVCodecContext* @c, IntPtr @func, void* @arg, int* @ret, int @count);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_fill_audio_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_fill_audio_frame(AVFrame* @frame, int @nb_channels, AVSampleFormat @sample_fmt, sbyte* @buf, int @buf_size, int @align);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_flush_buffers", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avcodec_flush_buffers(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_get_bits_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_bits_per_sample(AvCodecId @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "av_get_pcm_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AvCodecId av_get_pcm_codec(AVSampleFormat @fmt, int @be);
        
        [DllImport(libavcodec, EntryPoint = "av_get_exact_bits_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_exact_bits_per_sample(AvCodecId @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "av_get_audio_frame_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_audio_frame_duration(AVCodecContext* @avctx, int @frame_bytes);
        
        [DllImport(libavcodec, EntryPoint = "av_get_audio_frame_duration2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_get_audio_frame_duration2(AVCodecParameters* @par, int @frame_bytes);
        
        [DllImport(libavcodec, EntryPoint = "av_register_bitstream_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_register_bitstream_filter(AVBitStreamFilter* @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBitStreamFilterContext* av_bitstream_filter_init([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bitstream_filter_filter(AVBitStreamFilterContext* @bsfc, AVCodecContext* @avctx, [MarshalAs(UnmanagedType.LPStr)] string @args, sbyte** @poutbuf, int* @poutbuf_size, sbyte* @buf, int @buf_size, int @keyframe);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_bitstream_filter_close(AVBitStreamFilterContext* @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBitStreamFilter* av_bitstream_filter_next(AVBitStreamFilter* @f);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBitStreamFilter* av_bsf_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBitStreamFilter* av_bsf_next(void** @opaque);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_alloc(AVBitStreamFilter* @filter, AVBSFContext** @ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_init(AVBSFContext* @ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_send_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_send_packet(AVBSFContext* @ctx, AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_receive_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_receive_packet(AVBSFContext* @ctx, AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_bsf_free(AVBSFContext** @ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClass* av_bsf_get_class();
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBSFList* av_bsf_list_alloc();
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_bsf_list_free(AVBSFList** @lst);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_append", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_list_append(AVBSFList* @lst, AVBSFContext* @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_append2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_list_append2(AVBSFList* @lst, [MarshalAs(UnmanagedType.LPStr)] string @bsf_name, AVDictionary** @options);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_finalize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_list_finalize(AVBSFList** @lst, AVBSFContext** @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_parse_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_list_parse_str([MarshalAs(UnmanagedType.LPStr)] string @str, AVBSFContext** @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_get_null_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_bsf_get_null_filter(AVBSFContext** @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_fast_padded_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fast_padded_malloc(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavcodec, EntryPoint = "av_fast_padded_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_fast_padded_mallocz(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavcodec, EntryPoint = "av_xiphlacing", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint av_xiphlacing(sbyte* @s, uint @v);
        
        [DllImport(libavcodec, EntryPoint = "av_log_missing_feature", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_missing_feature(void* @avc, [MarshalAs(UnmanagedType.LPStr)] string @feature, int @want_sample);
        
        [DllImport(libavcodec, EntryPoint = "av_log_ask_for_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_log_ask_for_sample(void* @avc, [MarshalAs(UnmanagedType.LPStr)] string @msg);
        
        [DllImport(libavcodec, EntryPoint = "av_register_hwaccel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_register_hwaccel(AVHWAccel* @hwaccel);
        
        [DllImport(libavcodec, EntryPoint = "av_hwaccel_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVHWAccel* av_hwaccel_next(AVHWAccel* @hwaccel);
        
        [DllImport(libavcodec, EntryPoint = "av_lockmgr_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_lockmgr_register(IntPtr @cb);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_type", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVMediaType avcodec_get_type(AvCodecId @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avcodec_get_name(AvCodecId @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_is_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avcodec_is_open(AVCodecContext* @s);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_is_encoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_codec_is_encoder(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_is_decoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_codec_is_decoder(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_descriptor_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecDescriptor* avcodec_descriptor_get(AvCodecId @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_descriptor_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecDescriptor* avcodec_descriptor_next(AVCodecDescriptor* @prev);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_descriptor_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCodecDescriptor* avcodec_descriptor_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "av_cpb_properties_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVCPBProperties* av_cpb_properties_alloc(ulong* @size);
        
    }
}
