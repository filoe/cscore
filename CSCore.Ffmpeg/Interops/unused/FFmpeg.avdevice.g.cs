using System;
using System.Runtime.InteropServices;

namespace CSCore.Ffmpeg.Interops
{
    internal unsafe partial struct AVDictionary
    {
    }
    
    internal unsafe partial struct AVBuffer
    {
    }
    
    internal unsafe partial struct AVBufferPool
    {
    }
    
    internal unsafe partial struct AVBPrint
    {
    }
    
    internal unsafe partial struct AVCodecInternal
    {
    }
    
    internal unsafe partial struct AVCodecDefault
    {
    }
    
    internal unsafe partial struct MpegEncContext
    {
    }
    
    internal unsafe partial struct ReSampleContext
    {
    }
    
    internal unsafe partial struct AVResampleContext
    {
    }
    
    internal unsafe partial struct AVBSFInternal
    {
    }
    
    internal unsafe partial struct AVBSFList
    {
    }
    
    internal unsafe partial struct AVBPrint
    {
    }
    
    internal unsafe partial struct AVDeviceInfoList
    {
    }
    
    internal unsafe partial struct AVDeviceCapabilitiesQuery
    {
    }
    
    internal unsafe partial struct AVCodecTag
    {
    }
    
    internal unsafe partial struct AVStreamInternal
    {
    }
    
    internal unsafe partial struct AVFormatInternal
    {
    }
    
    internal unsafe partial struct AVDeviceRect
    {
        internal int @x;
        internal int @y;
        internal int @width;
        internal int @height;
    }
    
    internal unsafe partial struct AVDeviceCapabilitiesQuery
    {
        internal AVClass* @av_class;
        internal AVFormatContext* @device_context;
        internal int @codec;
        internal AVSampleFormat @sample_format;
        internal AVPixelFormat @pixel_format;
        internal int @sample_rate;
        internal int @channels;
        internal long @channel_layout;
        internal int @window_width;
        internal int @window_height;
        internal int @frame_width;
        internal int @frame_height;
        internal AVRational @fps;
    }
    
    internal unsafe partial struct AVDeviceInfo
    {
        internal sbyte* @device_name;
        internal sbyte* @device_description;
    }
    
    internal unsafe partial struct AVDeviceInfoList
    {
        internal AVDeviceInfo** @devices;
        internal int @nb_devices;
        internal int @default_device;
    }
    
    internal enum AVAppToDevMessageType : int
    {
        @AV_APP_TO_DEV_NONE = 1313820229,
        @AV_APP_TO_DEV_WINDOW_SIZE = 1195724621,
        @AV_APP_TO_DEV_WINDOW_REPAINT = 1380274241,
        @AV_APP_TO_DEV_PAUSE = 1346458912,
        @AV_APP_TO_DEV_PLAY = 1347174745,
        @AV_APP_TO_DEV_TOGGLE_PAUSE = 1346458964,
        @AV_APP_TO_DEV_SET_VOLUME = 1398165324,
        @AV_APP_TO_DEV_MUTE = 541939028,
        @AV_APP_TO_DEV_UNMUTE = 1431131476,
        @AV_APP_TO_DEV_TOGGLE_MUTE = 1414354260,
        @AV_APP_TO_DEV_GET_VOLUME = 1196838732,
        @AV_APP_TO_DEV_GET_MUTE = 1196250452,
    }
    
    internal enum AVDevToAppMessageType : int
    {
        @AV_DEV_TO_APP_NONE = 1313820229,
        @AV_DEV_TO_APP_CREATE_WINDOW_BUFFER = 1111708229,
        @AV_DEV_TO_APP_PREPARE_WINDOW_BUFFER = 1112560197,
        @AV_DEV_TO_APP_DISPLAY_WINDOW_BUFFER = 1111771475,
        @AV_DEV_TO_APP_DESTROY_WINDOW_BUFFER = 1111770451,
        @AV_DEV_TO_APP_BUFFER_OVERFLOW = 1112491596,
        @AV_DEV_TO_APP_BUFFER_UNDERFLOW = 1112884812,
        @AV_DEV_TO_APP_BUFFER_READABLE = 1112687648,
        @AV_DEV_TO_APP_BUFFER_WRITABLE = 1113018912,
        @AV_DEV_TO_APP_MUTE_STATE_CHANGED = 1129141588,
        @AV_DEV_TO_APP_VOLUME_LEVEL_CHANGED = 1129729868,
    }
    
    internal unsafe static partial class ffmpeg
    {
        internal const int LIBAVDEVICE_VERSION_MAJOR = 57;
        internal const int LIBAVDEVICE_VERSION_MINOR = 1;
        internal const int LIBAVDEVICE_VERSION_MICRO = 100;
        private const string libavdevice = "avdevice-57";
        
        [DllImport(libavdevice, EntryPoint = "avdevice_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern uint avdevice_version();
        
        [DllImport(libavdevice, EntryPoint = "avdevice_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avdevice_configuration();
        
        [DllImport(libavdevice, EntryPoint = "avdevice_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        internal static extern string avdevice_license();
        
        [DllImport(libavdevice, EntryPoint = "avdevice_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avdevice_register_all();
        
        [DllImport(libavdevice, EntryPoint = "av_input_audio_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVInputFormat* av_input_audio_device_next(AVInputFormat* @d);
        
        [DllImport(libavdevice, EntryPoint = "av_input_video_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVInputFormat* av_input_video_device_next(AVInputFormat* @d);
        
        [DllImport(libavdevice, EntryPoint = "av_output_audio_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVOutputFormat* av_output_audio_device_next(AVOutputFormat* @d);
        
        [DllImport(libavdevice, EntryPoint = "av_output_video_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern AVOutputFormat* av_output_video_device_next(AVOutputFormat* @d);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_app_to_dev_control_message", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avdevice_app_to_dev_control_message(AVFormatContext* @s, AVAppToDevMessageType @type, void* @data, ulong @data_size);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_dev_to_app_control_message", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avdevice_dev_to_app_control_message(AVFormatContext* @s, AVDevToAppMessageType @type, void* @data, ulong @data_size);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_capabilities_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avdevice_capabilities_create(AVDeviceCapabilitiesQuery** @caps, AVFormatContext* @s, AVDictionary** @device_options);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_capabilities_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avdevice_capabilities_free(AVDeviceCapabilitiesQuery** @caps, AVFormatContext* @s);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_list_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avdevice_list_devices(AVFormatContext* @s, AVDeviceInfoList** @device_list);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_free_list_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void avdevice_free_list_devices(AVDeviceInfoList** @device_list);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_list_input_sources", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avdevice_list_input_sources(AVInputFormat* @device, [MarshalAs(UnmanagedType.LPStr)] string @device_name, AVDictionary* @device_options, AVDeviceInfoList** @device_list);
        
        [DllImport(libavdevice, EntryPoint = "avdevice_list_output_sinks", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int avdevice_list_output_sinks(AVOutputFormat* @device, [MarshalAs(UnmanagedType.LPStr)] string @device_name, AVDictionary* @device_options, AVDeviceInfoList** @device_list);
        
    }
}
