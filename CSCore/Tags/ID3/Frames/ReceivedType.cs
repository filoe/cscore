namespace CSCore.Tags.ID3.Frames
{
    public enum ReceivedType : byte
    {
        Other = 0x0,
        StandardCDAlbum = 0x1,
        CompressedAudioOnCD = 0x2,
        FileOverInternet = 0x3,
        StreamOverInternet = 0x4,
        NoteSheet = 0x5,
        NoteSheetBook = 0x6,
        MusicOnOtherMedia = 0x7,
        NonmusicalMerchandise = 0x8
    }
}