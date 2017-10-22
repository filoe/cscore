using System;

namespace CSCore.SoundOut.MMInterop
{
    /// <summary>
    /// http: //msdn.microsoft.com/en-us/library/dd743869%28VS.85%29.aspx
    /// </summary>
    internal delegate void WaveCallback(IntPtr handle, WaveMsg msg, IntPtr user, WaveHeader header, IntPtr reserved);
}