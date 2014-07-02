namespace CSCore.SoundOut.MMInterop
{
    public enum MMTimeType : uint
    {
        TIME_MS = 0x0001,
        TIME_SAMPLES = 0x0002,
        TIME_BYTES = 0x0004,
        TIME_SMPTE = 0x0008,
        TIME_MIDI = 0x0016,
        TIME_TICKS = 0x0032
    }
}