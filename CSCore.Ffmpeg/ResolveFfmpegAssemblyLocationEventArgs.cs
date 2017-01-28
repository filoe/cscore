using System;
using System.IO;

namespace CSCore.Ffmpeg
{
    /// <summary>
    /// Provides data for the <see cref="FfmpegUtils.ResolveFfmpegAssemblyLocation"/> event.
    /// </summary>
    public class ResolveFfmpegAssemblyLocationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public PlatformID Platform { get; private set; }

        /// <summary>
        /// Gets or sets the directory which contains the native Ffmpeg assemblies for the current <see cref="Platform"/> and architecture.
        /// </summary>
        /// <value>
        /// The ffmpeg directory.
        /// </value>
        public DirectoryInfo FfmpegDirectory { get; set; }

        internal ResolveFfmpegAssemblyLocationEventArgs(PlatformID platformId)
        {
            Platform = platformId;
        }
    }
}