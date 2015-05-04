using System;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Flags that specify buffer-object capabilities.
    /// </summary>
    [Flags]
    public enum DSBufferCapsFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x0,
        /// <summary>
        /// The buffer is a primary buffer.
        /// </summary>
        PrimaryBuffer = 0x00000001,
        /// <summary>
        /// The buffer is in on-board hardware memory.
        /// </summary>
        Static = 0x00000002,
        /// <summary>
        /// The buffer uses hardware mixing.
        /// </summary>
        LocHardware = 0x00000004,
        /// <summary>
        /// The buffer is in software memory and uses software mixing.
        /// </summary>
        LocSoftware = 0x00000008,
        /// <summary>
        /// The buffer has 3D control capability.
        /// </summary>
        Control3D = 0x00000010,
        /// <summary>
        /// The buffer has frequency control capability.
        /// </summary>
        ControlFrequency = 0x00000020,
        /// <summary>
        /// The buffer has pan control capability.
        /// </summary>
        ControlPan = 0x00000040,
        /// <summary>
        /// The buffer has volume control capability.
        /// </summary>
        ControlVolume = 0x00000080,
        /// <summary>
        /// The buffer has position notification capability.
        /// </summary>
        ControlPositionNotify = 0x00000100,
        /// <summary>
        /// The buffer supports effects processing.
        /// </summary>
        ControlEffect = 0x00000200,
        /// <summary>
        /// The buffer has sticky focus. If the user switches to another application not using DirectSound, the buffer is still audible. However, if the user switches to another DirectSound application, the buffer is muted.
        /// </summary>
        StickyFocus = 0x00004000,
        /// <summary>
        /// The buffer is a global sound buffer. With this flag set, an application using DirectSound can continue to play its buffers if the user switches focus to another application, even if the new application uses DirectSound.
        /// For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.dsbcaps%28v=vs.85%29.aspx"/>.
        /// </summary>
        GlobalFocus = 0x00008000,
        /// <summary>
        /// The buffer uses the new behavior of the play cursor when <see cref="DirectSoundBuffer.GetCurrentPosition"/> is called. For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.reference.dsbcaps%28v=vs.85%29.aspx"/>.
        /// </summary>
        GetCurrentPosition2 = 0x00010000,
        /// <summary>
        /// The sound is reduced to silence at the maximum distance. The buffer will stop playing when the maximum distance is exceeded, so that processor time is not wasted. Applies only to software buffers.
        /// </summary>
        Mute3DAtMaxDistance = 0x00020000,
#pragma warning disable 618
        /// <summary>
        /// The buffer can be assigned to a hardware or software resource at play time, or when <see cref="DirectSoundBuffer.AcquireResourcesNative"/> is called.
        /// </summary>
        LocDefer = 0x00040000,
#pragma warning restore 618
        /// <summary>
        /// Force <see cref="DirectSoundBuffer.GetCurrentPosition"/> to return the buffer's true play position. This flag is only valid in Windows Vista.
        /// </summary>
        TruePlayPosition = 0x00080000
    }
}