using System;

namespace CSCore.SoundOut
{
    /// <summary>
    /// Defines functionalities supported by a device.
    /// </summary>
    [Flags]
    public enum MmDeviceSupported
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Supports pitch control.
        /// </summary>
        Pitch = 1,
        /// <summary>
        /// Supports playback rate control.
        /// </summary>
        PlaybackRate = 2,
        /// <summary>
        /// Supports volume control.
        /// </summary>
        Volume = 4,
        /// <summary>
        /// Supports separate left and right volume control.
        /// </summary>
        LeftRightVolume = 8,
        /// <summary>
        /// The driver is synchronous and will block while playing a buffer. 
        /// </summary>
        Synchronous = 16,
        /// <summary>
        /// Returns sample-accurate position information.
        /// </summary>
        SampleAccurate = 32,
        /// <summary>
        /// DirectSound
        /// </summary>
        /// <remarks>Not documented on msdn.</remarks>
        DirectSound = 64
    }
}
