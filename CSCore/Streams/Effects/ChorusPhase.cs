namespace CSCore.Streams.Effects
{
    /// <summary>
    /// Defines possible values for the <see cref="DmoChorusEffect.Phase"/> property.
    /// Default value is <see cref="Phase90"/>.
    /// </summary>
    public enum ChorusPhase
    {
        /// <summary>
        /// 180° Phase.
        /// </summary>
        Phase180 = 4,
        /// <summary>
        /// 90° Phase. 
        /// Default value for <see cref="DmoChorusEffect.Phase"/>. 
        /// </summary>
        Phase90 = 3,
        /// <summary>
        /// 0° Phase.
        /// </summary>
        PhaseZero = 2,
        /// <summary>
        /// -90° Phase.
        /// </summary>
        PhaseNegative90 = 1,
        /// <summary>
        /// -180° Phase.
        /// </summary>
        PhaseNegative180 = 0,
    }
}