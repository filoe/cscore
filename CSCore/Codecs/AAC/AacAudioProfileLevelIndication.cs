namespace CSCore.Codecs.AAC
{
    //see: http://www.nhzjj.com/asp/admin/editor/newsfile/2010318163752818.pdf
    /// <summary>
    /// Specifies the audio profile and level of an Advanced Audio Coding (AAC) stream.
    /// <see cref="AACProfile_L2_0x29"/> is the default setting.
    /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd319560(v=vs.85).aspx"/>.
    /// </summary>
    public enum AacAudioProfileLevelIndication
    {
        /// <summary>
        /// None/Invalid
        /// </summary>
        None = 0,
        /// <summary>
        /// AACProfile_L2_0x29 - Default value
        /// </summary>
        AACProfile_L2_0x29 = 0x29,
        /// <summary>
        /// AACProfile_L4_0x2A
        /// </summary>
        AACProfile_L4_0x2A = 0x2A,
        /// <summary>
        /// AACProfile_L5_0x2B 
        /// </summary>
        AACProfile_L5_0x2B = 0x2B,
        /// <summary>
        /// HighEfficiencyAACProfile_L2_0x2C 
        /// </summary>
        HighEfficiencyAACProfile_L2_0x2C = 0x2C,
        /// <summary>
        /// HighEfficiencyAACProfile_L3_0x2D 
        /// </summary>
        HighEfficiencyAACProfile_L3_0x2D = 0x2D,
        /// <summary>
        /// HighEfficiencyAACProfile_L4_0x2E 
        /// </summary>
        HighEfficiencyAACProfile_L4_0x2E = 0x2E,
        /// <summary>
        /// HighEfficiencyAACProfile_L5_0x2F
        /// </summary>
        HighEfficiencyAACProfile_L5_0x2F = 0x2F,
        /// <summary>
        /// ReservedForIsoUse_0x30 
        /// </summary>
        ReservedForIsoUse_0x30 = 0x30,
        /// <summary>
        /// ReservedForIsoUse_0x31
        /// </summary>
        ReservedForIsoUse_0x31 = 0x31,
        /// <summary>
        /// ReservedForIsoUse_0x32
        /// </summary>
        ReservedForIsoUse_0x32 = 0x32,
        /// <summary>
        /// ReservedForIsoUse_0x33
        /// </summary>
        ReservedForIsoUse_0x33 = 0x33
    }
}