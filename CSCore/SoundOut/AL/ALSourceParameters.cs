
namespace CSCore.SoundOut.AL
{
    /// <summary>
    /// Defines ALSource parameters.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public enum ALSourceParameters
    {
        /// <summary>
        /// SourceState
        /// </summary>
        SourceState = 0x1010,

        /// <summary>
        /// BuffersQueued
        /// </summary>
        BuffersQueued = 0x1015,

        /// <summary>
        /// BuffersProcessed
        /// </summary>
        BuffersProcessed = 0x1016,

        /// <summary>
        /// Pitch
        /// </summary>
        Pitch = 0x1003,

        /// <summary>
        /// Position
        /// </summary>
        Position = 0x1004,

        /// <summary>
        /// Direction
        /// </summary>
        Direction = 0x1005,

        /// <summary>
        /// Velocity
        /// </summary>
        Velocity = 0x1006,

        /// <summary>
        /// Gain
        /// </summary>
        Gain = 0x100A,

        /// <summary>
        /// Min gain
        /// </summary>
        MinGain = 0x100D,

        /// <summary>
        /// Max gain
        /// </summary>
        MaxGain = 0x100E,

        /// <summary>
        /// Orientation
        /// </summary>
        Orientation = 0x100F,

        /// <summary>
        /// Max distance
        /// </summary>
        MaxDistance = 0x1023,

        /// <summary>
        /// Roll off factor
        /// </summary>
        RollOffFactor = 0x1021,

        /// <summary>
        /// Cone outer gain
        /// </summary>
        ConeOuterGain = 0x1022,

        /// <summary>
        /// Cone inner angle
        /// </summary>
        ConeInnerAngle = 0x1001,

        /// <summary>
        /// Cone outer angle
        /// </summary>
        ConeOuterAngle = 0x1002,

        /// <summary>
        /// Reference distance
        /// </summary>
        ReferenceDistance = 0x1020,

        /// <summary>
        /// Source relative
        /// </summary>
        SourceRelative = 514
    }
}
