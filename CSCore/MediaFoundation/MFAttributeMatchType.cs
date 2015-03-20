namespace CSCore.MediaFoundation
{
    /// <summary>
    ///     Specifies how to compare the attributes on two objects.
    /// </summary>
    public enum MFAttributeMatchType
    {
        /// <summary>
        ///     Check whether all the attributes in pThis exist in pTheirs and have the same data, where pThis is the object whose
        ///     <see cref="MFAttributes.Compare" /> method is being called and pTheirs is the object given in the pTheirs
        ///     parameter.
        /// </summary>
        OurItems = 0,

        /// <summary>
        ///     Check whether all the attributes in pTheirs exist in pThis and have the same data, where pThis is the object whose
        ///     <see cref="MFAttributes.Compare" /> method is being called and pTheirs is the object given in the pTheirs
        ///     parameter.
        /// </summary>
        TheirItems = 1,

        /// <summary>
        ///     Check whether both objects have identical attributes with the same data.
        /// </summary>
        AllItems = 2,

        /// <summary>
        ///     Check whether the attributes that exist in both objects have the same data.
        /// </summary>
        Intersection = 3,

        /// <summary>
        ///     Find the object with the fewest number of attributes, and check if those attributes exist in the other object and
        ///     have the same data.
        /// </summary>
        Smaller = 4
    }
}