using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines flags that specify how to convert an audio media type.
    /// </summary>
    [Flags]
// ReSharper disable once InconsistentNaming
    public enum MFWaveFormatExConvertFlags
    {
        /// <summary>
        /// Convert the media type to a <see cref="WaveFormat"/> class if possible, or a <see cref="WaveFormatExtensible"/> class otherwise.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Convert the media type to a <see cref="WaveFormatExtensible"/> class..
        /// </summary>
        ForceExtensible = 1
    }
}