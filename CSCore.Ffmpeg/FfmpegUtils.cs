using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Represents a ffmpeg format.
        /// </summary>
        public class Format
        {
            /// <summary>
            /// Gets the name of the format.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the long name of the format.
            /// </summary>
            public string LongName { get; private set; }

            /// <summary>
            /// Gets a list of the common codecs.
            /// </summary>
            public ReadOnlyCollection<AvCodecId> Codecs { get; private set; }

            /// <summary>
            /// Gets a list with the common file extensions of the format.
            /// </summary>
            public ReadOnlyCollection<string> FileExtensions { get; private set; }

            /*
             * In order to copy duplicate code, we could use dynamic parameters ...
             * Unfortunately they are not supported by mono on .net 3.5
             */
            internal unsafe Format(AVOutputFormat format)
            {
                LongName = Marshal.PtrToStringAnsi((IntPtr) format.long_name);
                Name = Marshal.PtrToStringAnsi((IntPtr) format.name);

                Codecs = FfmpegCalls.GetCodecOfCodecTag(format.codec_tag).AsReadOnly();
                
                var extensions = Marshal.PtrToStringAnsi((IntPtr)format.extensions);
                FileExtensions = !string.IsNullOrEmpty(extensions) 
                    ? extensions.Split(',').ToList().AsReadOnly() 
                    : Enumerable.Empty<string>().ToList().AsReadOnly();
            }

            internal unsafe Format(AVInputFormat format)
            {
                LongName = Marshal.PtrToStringAnsi((IntPtr)format.long_name);
                Name = Marshal.PtrToStringAnsi((IntPtr)format.name);

                Codecs = FfmpegCalls.GetCodecOfCodecTag(format.codec_tag).AsReadOnly();

                var extensions = Marshal.PtrToStringAnsi((IntPtr)format.extensions);
                FileExtensions = !string.IsNullOrEmpty(extensions)
                    ? extensions.Split(',').ToList().AsReadOnly()
                    : Enumerable.Empty<string>().ToList().AsReadOnly();
            }
        }
    }
}
