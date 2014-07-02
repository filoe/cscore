namespace CSCore.Tags.ID3.Frames
{
    public enum PictureFormat : byte
    {
        None = 0x0,
        Icon32x32,
        OtherIcon = 0x02,
        CoverFront = 0x03,
        CoverBack = 0x04,
        LeafletPage = 0x05,
        Media = 0x06,
        LeadArtist = 0x07,
        Artist = 0x08,
        Conductor = 0x09,
        Band = 0x0A,
        Composer = 0x0B,
        Lyricist = 0x0C,
        RecordingLocation = 0x0D,
        DuringRecording = 0x0E,
        DuringPerformance = 0x0F,
        MovieCapture = 0x10,
        BrightColoredFish = 0x11,
        Illustration = 0x12,
        LogoArtist = 0x13,
        LogoPublisher = 0x14
    }
}