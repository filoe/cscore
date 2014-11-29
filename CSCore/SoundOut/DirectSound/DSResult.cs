namespace CSCore.SoundOut.DirectSound
{
    /// <summary>
    /// http: //msdn.microsoft.com/en-us/library/windows/desktop/ee416775%28v=vs.85%29.aspx
    /// TODO: finish
    /// </summary>
    public enum DSResult
    {
        DS_OK = 0x00000000,
        DSERR_OUTOFMEMORY = 0x00000007,
        DSERR_NOINTERFACE = 0x000001AE,
        DS_NO_VIRTUALIZATION = 0x0878000A,
        DS_INCOMPLETE = 0x08780014,
        DSERR_UNSUPPORTED = unchecked((int)0x80004001),
        DSERR_GENERIC = unchecked((int)0x80004005),
        DSERR_ACCESSDENIED = unchecked((int)0x80070005),
        DSERR_INVALIDPARAM = unchecked((int)0x80070057),
        DSERR_ALLOCATED = unchecked((int)0x8878000A),
        DSERR_CONTROLUNAVAIL = unchecked((int)0x8878001E),
        DSERR_INVALIDCALL = unchecked((int)0x88780032),
        DSERR_PRIOLEVELNEEDED = unchecked((int)0x88780046),
        DSERR_BADFORMAT = unchecked((int)0x88780064),
        DSERR_NODRIVER = unchecked((int)0x88780078),
        DSERR_ALREADYINITIALIZED = unchecked((int)0x88780082),
        DSERR_BUFFERLOST = unchecked((int)0x88780096),
        DSERR_OTHERAPPHASPRIO = unchecked((int)0x887800A0),
        DSERR_UNINITIALIZED = unchecked((int)0x887800AA),
        DSERR_BUFFERTOOSMALL = unchecked((int)0x887800b4),
        DSERR_DS8_REQUIRED = unchecked((int)0x887810BE),
        DSERR_SENDLOOP = unchecked((int)0x887810C8),
        DSERR_BADSENDBUFFERGUID = unchecked((int)0x887810D2),
        DSERR_FXUNAVAILABLE = unchecked((int)0x887810DC),
        DSERR_OBJECTNOTFOUND = unchecked((int)0x88781161),
    }
}