using System;

namespace CSCore.DMO
{
    /// <summary>
    /// Encapsulates the properties of an enumerated dmo.
    /// </summary>
    public struct DmoEnumItem
    {
        /// <summary>
        /// Gets or sets the CLSID of the dmo.
        /// </summary>
        public Guid CLSID { get; set; }
        /// <summary>
        /// Gets or sets the friendly name of the dmo.
        /// </summary>
        public string Name { get; set; }
    }
}