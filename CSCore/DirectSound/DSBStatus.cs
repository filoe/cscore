using System;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Defines flags that describe the status of a <see cref="DirectSoundBuffer"/>.
    /// </summary>
    [Flags]
    public enum DSBStatusFlags
    {
        /// <summary>
        /// The buffer is playing. If this value is not set, the buffer is stopped.
        /// </summary>
        Playing = 0x00000001,
        /// <summary>
        /// The buffer is lost and must be restored before it can be played or locked.
        /// </summary>
        BufferLost = 0x00000002,
        /// <summary>
        /// The buffer is being looped. If this value is not set, the buffer will stop when it reaches the end of the sound data. This value is returned only in combination with <see cref="Playing"/>.
        /// </summary>
        Looping = 0x00000004,
        /// <summary>
        /// The buffer is playing in hardware. Set only for buffers created with the <see cref="DSBufferCapsFlags.LocDefer"/> flag.
        /// </summary>
        LocHardware = 0x00000008,
        /// <summary>
        /// The buffer is playing in software. Set only for buffers created with the <see cref="DSBufferCapsFlags.LocDefer"/> flag.
        /// </summary>
        LocSoftware = 0x00000010,
        /// <summary>
        /// The buffer was prematurely terminated by the voice manager and is not playing. Set only for buffers created with the <see cref="DSBufferCapsFlags.LocDefer"/> flag.
        /// </summary>
        Terminated = 0x00000020
    }
}