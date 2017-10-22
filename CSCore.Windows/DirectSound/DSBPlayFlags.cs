using System;

namespace CSCore.DirectSound
{
    /*
    #define DSBPLAY_LOOPING             0x00000001
    #define DSBPLAY_LOCHARDWARE         0x00000002
    #define DSBPLAY_LOCSOFTWARE         0x00000004
    #define DSBPLAY_TERMINATEBY_TIME    0x00000008
    #define DSBPLAY_TERMINATEBY_DISTANCE    0x000000010
    #define DSBPLAY_TERMINATEBY_PRIORITY    0x000000020
     */

    /// <summary>
    /// Flags specifying how to play a <see cref="DirectSoundBuffer"/>.
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/microsoft.directx_sdk.idirectsoundbuffer8.idirectsoundbuffer8.play.aspx"/>.</remarks>
    [Flags]
    public enum DSBPlayFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x0,
        /// <summary>
        /// After the end of the audio buffer is reached, play restarts at the beginning of the buffer. Play continues until explicitly stopped. This flag must be set when playing a primary buffer.
        /// </summary>
        Looping = 0x00000001,
        /// <summary>
        /// Play this voice in a hardware buffer only. If the hardware has no available voices and no voice management flags are set, the call to <see cref="DirectSoundBuffer.Play(DSBPlayFlags)"/> fails. This flag cannot be combined with <see cref="LocSoftware"/>.
        /// </summary>
        LocHardware = 0x00000002,
        /// <summary>
        /// Play this voice in a software buffer only. This flag cannot be combined with <see cref="LocHardware"/> or any voice management flag.
        /// </summary>
        LocSoftware = 0x00000004,
        /// <summary>
        /// If the hardware has no available voices, a currently playing nonlooping buffer will be stopped to make room for the new buffer. The buffer prematurely terminated is the one with the least time left to play.
        /// </summary>
        TerminateByTime = 0x00000008,
        /// <summary>
        /// If the hardware has no available voices, a currently playing buffer will be stopped to make room for the new buffer. The buffer prematurely terminated will be selected from buffers that have the buffer's <see cref="DSBufferCapsFlags.Mute3DAtMaxDistance"/> flag set and are beyond their maximum distance. If there are no such buffers, the method fails.
        /// </summary>
        TerminateByDistance = 0x000000010,
        /// <summary>
        /// If the hardware has no available voices, a currently playing buffer will be stopped to make room for the new buffer. The buffer prematurely terminated will be the one with the lowest priority as set by the priority parameter passed to <see cref="DirectSoundBuffer.Play(DSBPlayFlags, int)"/> for the buffer.
        /// </summary>
        TerminateByPriority = 0x000000020
    }
}