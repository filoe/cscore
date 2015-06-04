using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Represents a MediaFoundation-attribute. 
    /// </summary>
    /// <typeparam name="TValue">The type of the value of the <see cref="MFAttribute{TValue}"/></typeparam>
    public class MFAttribute<TValue>
    {
        /// <summary>
        /// Gets the key of the attribute.
        /// </summary>
        public Guid Key { get; private set; }

        /// <summary>
        /// Gets the value of the attribute.
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MFAttribute{TValue}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public MFAttribute(Guid key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}