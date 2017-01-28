using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using CSCore.Ffmpeg.Interops;

namespace CSCore.Ffmpeg
{
    /// <summary>
    /// Contains some utilities for working with ffmpeg.
    /// </summary>
    public static class FfmpegUtils
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private static readonly FfmpegCalls.LogCallback LogCallback;
        private static readonly FfmpegCalls.LogCallback DefaultLogCallback;
        private static readonly object LockObj = new object();

        /// <summary>
        /// Occurs when a ffmpeg log entry was received.
        /// </summary>
        public static event EventHandler<FfmpegLogReceivedEventArgs> FfmpegLogReceived;

        /// <summary>
        /// Occurs when the location of the native FFmpeg binaries has get resolved.
        /// Note: This is currently only available for Windows Platforms.
        /// </summary>
        public static event EventHandler<ResolveFfmpegAssemblyLocationEventArgs> ResolveFfmpegAssemblyLocation; 

        static unsafe FfmpegUtils()
        {
            LogCallback = OnLogMessage;
            DefaultLogCallback = FfmpegCalls.GetDefaultLogCallback();
            FfmpegCalls.SetLogCallback(LogCallback);
        }

        /// <summary>
        /// Gets the output formats.
        /// </summary>
        /// <returns>All supported output formats.</returns>
        public static IEnumerable<Format> GetOutputFormats()
        {
            var outputFormats = FfmpegCalls.GetOutputFormats();
            return outputFormats.Select(format => new Format(format));
        }

        /// <summary>
        /// Gets the input formats.
        /// </summary>
        /// <returns>All supported input formats.</returns>
        public static IEnumerable<Format> GetInputFormats()
        {
            var inputFormats = FfmpegCalls.GetInputFormats();
            return inputFormats.Select(format => new Format(format));
        }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">value</exception>
        public static LogLevel LogLevel
        {
            get { return FfmpegCalls.GetLogLevel(); }
            set
            {
                if ((int)value < (int)LogLevel.Quit || (int)value > (int)LogLevel.Debug || (int)value % 8 != 0)
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(LogLevel));

                FfmpegCalls.SetLogLevel(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether log entries should be passed to the default ffmpeg logger.
        /// </summary>
        /// <value>
        ///   <c>true</c> if log messages should be passed to the default ffmpeg logger; otherwise, <c>false</c>.
        /// </value>
        public static bool LogToDefaultLogger { get; set; }

        private static unsafe void OnLogMessage(void* ptr, int level, byte* fmt, IntPtr vl)
        {
            lock (LockObj)
            {
                if (level >= 0)
                {
                    level &= 0xFF;
                }

                if (level > (int)LogLevel)
                    return;

                if (LogToDefaultLogger)
                {
                    DefaultLogCallback(ptr, level, fmt, vl);
                }

                var eventHandler = FfmpegLogReceived;
                if (eventHandler != null)
                {
                    AVClass? avClass = null;
                    AVClass? parentLogContext = null;
                    AVClass** parentpp = default(AVClass**);
                    if (ptr != null)
                    {
                        avClass = **(AVClass**)ptr;
                        if (avClass.Value.parent_log_context_offset != 0)
                        {
                            parentpp = *(AVClass***)((byte*)ptr + avClass.Value.parent_log_context_offset);
                            if (parentpp != null && *parentpp != null)
                            {
                                parentLogContext = **parentpp;
                            }
                        }
                    }


                    int printPrefix = 1;
                    string line = FfmpegCalls.FormatLine(ptr, level, Marshal.PtrToStringAnsi((IntPtr)fmt), vl, ref printPrefix);

                    eventHandler(null,
                        new FfmpegLogReceivedEventArgs(avClass, parentLogContext, (LogLevel) level, line, ptr, parentpp));
                }
            }
        }

        internal static DirectoryInfo FindFfmpegDirectory(PlatformID platform)
        {
            var resolveEvent = ResolveFfmpegAssemblyLocation;
            ResolveFfmpegAssemblyLocationEventArgs eventArgs = new ResolveFfmpegAssemblyLocationEventArgs(platform);
            if (resolveEvent != null)
                resolveEvent(null, eventArgs);

            return eventArgs.FfmpegDirectory;
        }
    }
}
