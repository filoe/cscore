namespace CSCore.Ffmpeg
{
    /// <summary>
    /// Defines Ffmpeg Loglevels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Print no output
        /// </summary>
        Quit = -8,
        /// <summary>
        /// Something went really wrong and we will crash now. 
        /// </summary>
        LogPanic = 0,
        /// <summary>
        /// Something went wrong and recovery is not possible. 
        /// </summary>
        Fatal = 8,
        /// <summary>
        /// Something went wrong and cannot losslessly be recovered. 
        /// </summary>
        Error = 16,
        /// <summary>
        /// Something somehow does not look correct. 
        /// </summary>
        Warning = 24,
        /// <summary>
        /// Standard information. 
        /// </summary>
        Info = 32,
        /// <summary>
        /// Detailed information. 
        /// </summary>
        Verbose = 40,
        /// <summary>
        /// Stuff which is only useful for libav* developers. 
        /// </summary>
        Debug = 48
    }
}