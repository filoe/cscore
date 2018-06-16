using System;
using System.IO;

namespace CSCore.Utils
{
    /// <summary>
    /// Operating system platform
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Windows
        /// </summary>
        Windows,

        /// <summary>
        /// Linux
        /// </summary>
        Linux,

        /// <summary>
        /// MacOSX
        /// </summary>
        MacOSX
    }

    /// <summary>
    ///     Platform detection code
    /// </summary>
    public static class PlatformDetection
    {
        /// <summary>
        ///     Checks which platform we are running on
        ///     Code adapted from here: https://stackoverflow.com/questions/10138040/how-to-detect-properly-windows-linux-mac-operating-systems
        /// </summary>
        /// <returns>The platform we are running on</returns>
        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return Platform.MacOSX;
                    else
                        return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.MacOSX;

                default:
                    return Platform.Windows;
            }
        }
    }
}

