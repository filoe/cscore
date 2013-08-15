namespace CSCore.CoreAudioAPI
{
    public enum DataFlow
    {
        Render = 0,
        Capture = Render + 1,
        All = Capture + 1,
        EnumCount = All + 1
    }
}