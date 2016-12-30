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
    
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVFilterContext
    {
    }
    
    internal unsafe partial struct AVFilterLink
    {
    }
    
    internal unsafe partial struct AVFilterPad
    {
    }
    
    internal unsafe partial struct AVFilterFormats
    {
    }
    
    internal unsafe partial struct AVFilter
    {
        internal sbyte* @name;
        internal sbyte* @description;
        internal AVFilterPad* @inputs;
        internal AVFilterPad* @outputs;
        internal AVClass* @priv_class;
        internal int @flags;
        internal IntPtr @init;
        internal IntPtr @init_dict;
        internal IntPtr @uninit;
        internal IntPtr @query_formats;
        internal int @priv_size;
        internal AVFilter* @next;
        internal IntPtr @process_command;
        internal IntPtr @init_opaque;
    }
    
    internal unsafe partial struct AVFilterInternal
    {
    }
    
    internal unsafe partial struct AVFilterContext
    {
        internal AVClass* @av_class;
        internal AVFilter* @filter;
        internal sbyte* @name;
        internal AVFilterPad* @input_pads;
        internal AVFilterLink** @inputs;
        internal uint @nb_inputs;
        internal AVFilterPad* @output_pads;
        internal AVFilterLink** @outputs;
        internal uint @nb_outputs;
        internal void* @priv;
        internal AVFilterGraph* @graph;
        internal int @thread_type;
        internal AVFilterInternal* @internal;
        internal AVFilterCommand* @command_queue;
        internal sbyte* @enable_str;
        internal void* @enable;
        internal double* @var_values;
        internal int @is_disabled;
        internal AVBufferRef* @hw_device_ctx;
        internal int @nb_threads;
    }
    
    internal unsafe partial struct AVFilterCommand
    {
    }
    
    internal unsafe partial struct AVFilterGraph
    {
    }
    
    internal unsafe partial struct AVFilterLink
    {
        internal AVFilterContext* @src;
        internal AVFilterPad* @srcpad;
        internal AVFilterContext* @dst;
        internal AVFilterPad* @dstpad;
        internal AVMediaType @type;
        internal int @w;
        internal int @h;
        internal AVRational @sample_aspect_ratio;
        internal ulong @channel_layout;
        internal int @sample_rate;
        internal int @format;
        internal AVRational @time_base;
        internal AVFilterFormats* @in_formats;
        internal AVFilterFormats* @out_formats;
        internal AVFilterFormats* @in_samplerates;
        internal AVFilterFormats* @out_samplerates;
        internal AVFilterChannelLayouts* @in_channel_layouts;
        internal AVFilterChannelLayouts* @out_channel_layouts;
        internal int @request_samples;
        internal init_state @init_state;
        internal AVFilterGraph* @graph;
        internal long @current_pts;
        internal long @current_pts_us;
        internal int @age_index;
        internal AVRational @frame_rate;
        internal AVFrame* @partial_buf;
        internal int @partial_buf_size;
        internal int @min_samples;
        internal int @max_samples;
        internal int @status;
        internal int @channels;
        internal uint @flags;
        internal long @frame_count;
        internal void* @video_frame_pool;
        internal int @frame_wanted_in;
        internal int @frame_wanted_out;
        internal AVBufferRef* @hw_frames_ctx;
    }
    
    internal unsafe partial struct AVFilterChannelLayouts
    {
    }
    
    internal unsafe partial struct AVFilterGraphInternal
    {
    }
    
    internal unsafe partial struct AVFilterGraph
    {
        internal AVClass* @av_class;
        internal AVFilterContext** @filters;
        internal uint @nb_filters;
        internal sbyte* @scale_sws_opts;
        internal sbyte* @resample_lavr_opts;
        internal int @thread_type;
        internal int @nb_threads;
        internal AVFilterGraphInternal* @internal;
        internal void* @opaque;
        internal IntPtr @execute;
        internal sbyte* @aresample_swr_opts;
        internal AVFilterLink** @sink_links;
        internal int @sink_links_count;
        internal uint @disable_auto_convert;
    }
    
    internal unsafe partial struct AVFilterInOut
    {
        internal sbyte* @name;
        internal AVFilterContext* @filter_ctx;
        internal int @pad_idx;
        internal AVFilterInOut* @next;
    }
    
    internal unsafe partial struct AVBuffer
    {
    }
    
    internal unsafe partial struct AVBufferPool
    {
    }
    
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVFilterPad
    {
    }
    
    internal unsafe partial struct AVFilterFormats
    {
    }
    
    internal unsafe partial struct AVFilterInternal
    {
    }
    
    internal unsafe partial struct AVFilterGraphInternal
    {
    }
    
    internal unsafe partial struct AVBufferSrcParameters
    {
        internal int @format;
        internal AVRational @time_base;
        internal int @width;
        internal int @height;
        internal AVRational @sample_aspect_ratio;
        internal AVRational @frame_rate;
        internal AVBufferRef* @hw_frames_ctx;
        internal int @sample_rate;
        internal ulong @channel_layout;
    }
    
    internal unsafe partial struct AVBuffer
    {
    }
    
    internal unsafe partial struct AVBufferPool
    {
    }
    
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVFilterPad
    {
    }
    
    internal unsafe partial struct AVFilterFormats
    {
    }
    
    internal unsafe partial struct AVFilterInternal
    {
    }
    
    internal unsafe partial struct AVFilterGraphInternal
    {
    }
    
    internal unsafe partial struct AVBufferSinkParams
    {
        internal AVPixelFormat* @pixel_fmts;
    }
    
    internal unsafe partial struct AVABufferSinkParams
    {
        internal AVSampleFormat* @sample_fmts;
        internal long* @channel_layouts;
        internal int* @channel_counts;
        internal int @all_channel_counts;
        internal int* @sample_rates;
    }
    
    internal enum init_state : int
    {
        @AVLINK_UNINIT = 0,
        @AVLINK_STARTINIT = 1,
        @AVLINK_INIT = 2,
    }
    
    internal enum avfilter_graph_config : int
    {
        @AVFILTER_AUTO_CONVERT_ALL = 0,
        @AVFILTER_AUTO_CONVERT_NONE = -1,
    }
    
    internal enum av_buffersrc_get_nb_failed_requests : int
    {
        @AV_BUFFERSRC_FLAG_NO_CHECK_FORMAT = 1,
        @AV_BUFFERSRC_FLAG_PUSH = 4,
        @AV_BUFFERSRC_FLAG_KEEP_REF = 8,
    }
    
    internal unsafe static partial class ffmpeg
    {
        internal const int LIBAVFILTER_VERSION_MAJOR = 6;
        internal const int LIBAVFILTER_VERSION_MINOR = 65;
        internal const int LIBAVFILTER_VERSION_MICRO = 100;
        internal const bool FF_API_OLD_FILTER_OPTS = (LIBAVFILTER_VERSION_MAJOR<7);
        internal const bool FF_API_OLD_FILTER_OPTS_ERROR = (LIBAVFILTER_VERSION_MAJOR<7);
        internal const bool FF_API_AVFILTER_OPEN = (LIBAVFILTER_VERSION_MAJOR<7);
        internal const bool FF_API_AVFILTER_INIT_FILTER = (LIBAVFILTER_VERSION_MAJOR<7);
        internal const bool FF_API_OLD_FILTER_REGISTER = (LIBAVFILTER_VERSION_MAJOR<7);
        internal const bool FF_API_NOCONST_GET_NAME = (LIBAVFILTER_VERSION_MAJOR<7);
        internal const int AVFILTER_FLAG_DYNAMIC_INPUTS = (1<<0);
        internal const int AVFILTER_FLAG_DYNAMIC_OUTPUTS = (1<<1);
        internal const int AVFILTER_FLAG_SLICE_THREADS = (1<<2);
        internal const int AVFILTER_FLAG_SUPPORT_TIMELINE_GENERIC = (1<<16);
        internal const int AVFILTER_FLAG_SUPPORT_TIMELINE_INTERNAL = (1<<17);
        internal const int AVFILTER_FLAG_SUPPORT_TIMELINE = (AVFILTER_FLAG_SUPPORT_TIMELINE_GENERIC|AVFILTER_FLAG_SUPPORT_TIMELINE_INTERNAL);
        internal const int AVFILTER_THREAD_SLICE = (1<<0);
        internal const int AVFILTER_CMD_FLAG_ONE = 1;
        internal const int AVFILTER_CMD_FLAG_FAST = 2;
        internal const int AV_BUFFERSINK_FLAG_PEEK = 1;
        internal const int AV_BUFFERSINK_FLAG_NO_REQUEST = 2;
        private const string libavfilter = "avfilter-6";
        
        [DllImport(libavfilter, EntryPoint = "avfilter_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint avfilter_version();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avfilter_configuration();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avfilter_license();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_pad_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_pad_count(AVFilterPad* @pads);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_pad_get_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avfilter_pad_get_name(AVFilterPad* @pads, int @pad_idx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_pad_get_type", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVMediaType avfilter_pad_get_type(AVFilterPad* @pads, int @pad_idx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_link", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_link(AVFilterContext* @src, uint @srcpad, AVFilterContext* @dst, uint @dstpad);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_link_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_link_free(AVFilterLink** @link);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_link_get_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_link_get_channels(AVFilterLink* @link);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_link_set_closed", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_link_set_closed(AVFilterLink* @link, int @closed);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_config_links", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_config_links(AVFilterContext* @filter);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_process_command", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_process_command(AVFilterContext* @filter, [MarshalAs(UnmanagedType.LPStr)] string @cmd, [MarshalAs(UnmanagedType.LPStr)] string @arg, IntPtr @res, int @res_len, int @flags);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_register_all();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_uninit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_uninit();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_register(AVFilter* @filter);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilter* avfilter_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilter* avfilter_next(AVFilter* @prev);
        
        [DllImport(libavfilter, EntryPoint = "av_filter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilter** av_filter_next(AVFilter** @filter);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_open(AVFilterContext** @filter_ctx, AVFilter* @filter, [MarshalAs(UnmanagedType.LPStr)] string @inst_name);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_init_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_init_filter(AVFilterContext* @filter, [MarshalAs(UnmanagedType.LPStr)] string @args, void* @opaque);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_init_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_init_str(AVFilterContext* @ctx, [MarshalAs(UnmanagedType.LPStr)] string @args);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_init_dict", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_init_dict(AVFilterContext* @ctx, AVDictionary** @options);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_free(AVFilterContext* @filter);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_insert_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_insert_filter(AVFilterLink* @link, AVFilterContext* @filt, uint @filt_srcpad_idx, uint @filt_dstpad_idx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVClass* avfilter_get_class();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilterGraph* avfilter_graph_alloc();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_alloc_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilterContext* avfilter_graph_alloc_filter(AVFilterGraph* @graph, AVFilter* @filter, [MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_get_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilterContext* avfilter_graph_get_filter(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_add_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_add_filter(AVFilterGraph* @graphctx, AVFilterContext* @filter);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_create_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_create_filter(AVFilterContext** @filt_ctx, AVFilter* @filt, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @args, void* @opaque, AVFilterGraph* @graph_ctx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_set_auto_convert", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_graph_set_auto_convert(AVFilterGraph* @graph, uint @flags);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_config", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_config(AVFilterGraph* @graphctx, void* @log_ctx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_graph_free(AVFilterGraph** @graph);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_inout_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVFilterInOut* avfilter_inout_alloc();
        
        [DllImport(libavfilter, EntryPoint = "avfilter_inout_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avfilter_inout_free(AVFilterInOut** @inout);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_parse", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_parse(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @filters, AVFilterInOut* @inputs, AVFilterInOut* @outputs, void* @log_ctx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_parse_ptr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_parse_ptr(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @filters, AVFilterInOut** @inputs, AVFilterInOut** @outputs, void* @log_ctx);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_parse2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_parse2(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @filters, AVFilterInOut** @inputs, AVFilterInOut** @outputs);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_send_command", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_send_command(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @target, [MarshalAs(UnmanagedType.LPStr)] string @cmd, [MarshalAs(UnmanagedType.LPStr)] string @arg, IntPtr @res, int @res_len, int @flags);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_queue_command", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_queue_command(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @target, [MarshalAs(UnmanagedType.LPStr)] string @cmd, [MarshalAs(UnmanagedType.LPStr)] string @arg, int @flags, double @ts);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_dump", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern sbyte* avfilter_graph_dump(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @options);
        
        [DllImport(libavfilter, EntryPoint = "avfilter_graph_request_oldest", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avfilter_graph_request_oldest(AVFilterGraph* @graph);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersrc_get_nb_failed_requests", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint av_buffersrc_get_nb_failed_requests(AVFilterContext* @buffer_src);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersrc_parameters_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferSrcParameters* av_buffersrc_parameters_alloc();
        
        [DllImport(libavfilter, EntryPoint = "av_buffersrc_parameters_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersrc_parameters_set(AVFilterContext* @ctx, AVBufferSrcParameters* @param);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersrc_write_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersrc_write_frame(AVFilterContext* @ctx, AVFrame* @frame);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersrc_add_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersrc_add_frame(AVFilterContext* @ctx, AVFrame* @frame);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersrc_add_frame_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersrc_add_frame_flags(AVFilterContext* @buffer_src, AVFrame* @frame, int @flags);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersink_get_frame_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersink_get_frame_flags(AVFilterContext* @ctx, AVFrame* @frame, int @flags);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersink_params_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVBufferSinkParams* av_buffersink_params_alloc();
        
        [DllImport(libavfilter, EntryPoint = "av_abuffersink_params_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVABufferSinkParams* av_abuffersink_params_alloc();
        
        [DllImport(libavfilter, EntryPoint = "av_buffersink_set_frame_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void av_buffersink_set_frame_size(AVFilterContext* @ctx, uint @frame_size);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersink_get_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVRational av_buffersink_get_frame_rate(AVFilterContext* @ctx);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersink_get_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersink_get_frame(AVFilterContext* @ctx, AVFrame* @frame);
        
        [DllImport(libavfilter, EntryPoint = "av_buffersink_get_samples", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int av_buffersink_get_samples(AVFilterContext* @ctx, AVFrame* @frame, int @nb_samples);
        
    }
}
