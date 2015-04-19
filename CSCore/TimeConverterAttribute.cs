using System;

namespace CSCore
{
    /// <summary>
    /// Specifies which <see cref="TimeConverter"/> to use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class TimeConverterAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the <see cref="TimeConverter"/> to use.
        /// </summary>
        public Type TimeConverterType { get; private set; }

        /// <summary>
        /// Gets or sets the arguments to pass to the constructor of the <see cref="TimeConverter"/>. For more information, see <see cref="Activator.CreateInstance(System.Type,object[])"/>.
        /// </summary>
        public object[] Args { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a new instance of the specified <see cref="TimeConverter"/> should be created each time the <see cref="TimeConverterFactory"/> queries the <see cref="TimeConverter"/>.
        /// The default value is false.
        /// </summary>
        public bool ForceNewInstance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeConverterAttribute"/> class based on the type of the <see cref="TimeConverter"/> to use.
        /// </summary>
        /// <param name="timeConverterType">Type of the <see cref="TimeConverter"/> to use.</param>
        /// <exception cref="System.ArgumentNullException">timeConverterType</exception>
        /// <exception cref="System.ArgumentException">Specified type is no time converter.;timeConverterType</exception>
        public TimeConverterAttribute(Type timeConverterType)
        {
            if (timeConverterType == null)
                throw new ArgumentNullException("timeConverterType");
            if(!typeof(TimeConverter).IsAssignableFrom(timeConverterType))
                throw new ArgumentException("Specified type is no time converter.", "timeConverterType");

            TimeConverterType = timeConverterType;
        }
    }
}