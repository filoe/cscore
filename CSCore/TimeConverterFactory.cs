using System;
using System.Collections.Generic;
using System.Linq;

namespace CSCore
{
    /// <summary>
    /// Provides <see cref="TimeConverter"/>s for converting raw time values (e.g. bytes, samples,...) to a <see cref="TimeSpan"/> and back.
    /// </summary>
    public sealed class TimeConverterFactory
    {
// ReSharper disable once InconsistentNaming
        private readonly static TimeConverterFactory _instance = new TimeConverterFactory();

        /// <summary>
        /// Gets the default instance of the factory.
        /// </summary>
        public static TimeConverterFactory Instance
        {
            get { return _instance; }
        }

        private readonly Dictionary<Type, TimeConverter> _timeConverters;
        private readonly Dictionary<Type, CacheItem> _cache;

        private TimeConverterFactory()
        {
            _timeConverters = new Dictionary<Type, TimeConverter>();
            _cache = new Dictionary<Type, CacheItem>();

            RegisterTimeConverterForSourceType<IWaveSource>(TimeConverter.WaveSourceTimeConverter);
            RegisterTimeConverterForSourceType<ISampleSource>(TimeConverter.SampleSourceTimeConverter);
        }

        /// <summary>
        /// Registers a new <see cref="TimeConverter"/> for a specific source type.
        /// </summary>
        /// <param name="timeConverter">The <see cref="TimeConverter"/> to register.</param>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <exception cref="ArgumentNullException">timeConverter is null.</exception>
        /// <exception cref="ArgumentException">There is already a <see cref="TimeConverter"/> registered for the specified <typeparamref name="TSource"/>.</exception>
        /// <remarks>The <see cref="TimeConverterFactory"/> class uses the source type to find choose the best <see cref="TimeConverter"/> for an <see cref="IAudioSource"/>. For more information, see <see cref="GetTimeConverterForSourceType"/>.</remarks>
        public void RegisterTimeConverterForSourceType<TSource>(TimeConverter timeConverter)
            where TSource : IAudioSource
        {
            if (timeConverter == null)
                throw new ArgumentNullException("timeConverter");

            var type = typeof (TSource);
            if (_timeConverters.ContainsKey(type))
                throw new ArgumentException("A timeconverter for the same source type got already registered.");

            _timeConverters.Add(type, timeConverter);
        }

        /// <summary>
        /// Unregisters a previously registered <see cref="TimeConverter"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type, that got passed to the <see cref="RegisterTimeConverterForSourceType{TSource}"/> method previously.</typeparam>
        /// <exception cref="ArgumentException">The specified source type could not be found.</exception>
        public void UnregisterTimeConverter<TSource>()
            where TSource : IAudioSource
        {
            var type = typeof (TSource);
            if(!_timeConverters.ContainsKey(type))
                throw new ArgumentException("There is no timeconverter registered for the specified source type.");

            _timeConverters.Remove(type);
        }

        /// <summary>
        /// Gets the <see cref="TimeConverter"/> for the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="IAudioSource"/> object to get the <see cref="TimeConverter"/> for.</param>
        /// <typeparam name="TSource">The type of the <paramref name="source"/>.</typeparam>
        /// <returns>The best <see cref="TimeConverter"/> for the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">The specified <paramref name="source"/> is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// Specified type is no AudioSource.;type
        /// or
        /// No registered time converter for the specified source type was found.
        /// or
        /// Multiple possible time converters, for the specified source type, were found. Specify which time converter to use, through the <see cref="TimeConverterAttribute"/>.
        /// </exception>
        /// <remarks>
        /// The <see cref="GetTimeConverterForSource{TSource}(TSource)"/> chooses the best <see cref="TimeConverter"/> for the specified <paramref name="source"/>.
        /// If there is no <see cref="TimeConverterAttribute"/> applied to the <see cref="IAudioSource"/> object (the <paramref name="source"/>), it looks up the inheritance hierarchy (interfaces included) of the <see cref="IAudioSource"/> object
        /// and searches for all registered source types. If there is a match it returns the associated <see cref="TimeConverter"/>. If there are more or less than one match BUT no <see cref="TimeConverterAttribute"/>
        /// it throws an exception.</remarks>
        public TimeConverter GetTimeConverterForSource<TSource>(TSource source) where TSource : class, IAudioSource
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return GetTimeConverterForSourceType(source.GetType());
        }

        /// <summary>
        /// Gets the <see cref="TimeConverter"/> for the specified source type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <returns>The best <see cref="TimeConverter"/> for the specified source type.</returns>
        /// <exception cref="System.ArgumentException">
        /// Specified type is no AudioSource.;type
        /// or
        /// No registered time converter for the specified source type was found.
        /// or
        /// Multiple possible time converters, for the specified source type, were found. Specify which time converter to use, through the <see cref="TimeConverterAttribute"/>.
        /// </exception>
        /// <remarks>
        /// The <see cref="GetTimeConverterForSource{TSource}()"/> chooses the best <see cref="TimeConverter"/> for the specified source type.
        /// If there is no <see cref="TimeConverterAttribute"/> applied to the <see cref="IAudioSource"/> object, it looks up the inheritance hierarchy (interfaces included) of the <see cref="IAudioSource"/> object
        /// and searches for all registered source types. If there is a match it returns the associated <see cref="TimeConverter"/>. If there are more or less than one match BUT no <see cref="TimeConverterAttribute"/>
        /// it throws an exception.</remarks>
        public TimeConverter GetTimeConverterForSource<TSource>()
            where TSource : IAudioSource
        {
            return GetTimeConverterForSourceType(typeof (TSource));
        }

        /// <summary>
        /// Gets the <see cref="TimeConverter"/> for the specified <paramref name="sourceType"/>.
        /// </summary>
        /// <param name="sourceType">The <see cref="Type"/> to get the associated <see cref="TimeConverter"/> for.</param>
        /// <returns>The best <see cref="TimeConverter"/> for the specified <paramref name="sourceType"/>.</returns>
        /// <exception cref="System.ArgumentException">
        /// Specified type is no AudioSource.;type
        /// or
        /// No registered time converter for the specified source type was found.
        /// or
        /// Multiple possible time converters, for the specified source type, were found. Specify which time converter to use, through the <see cref="TimeConverterAttribute"/>.
        /// </exception>
        /// <remarks>
        /// The <see cref="GetTimeConverterForSourceType(Type)"/> chooses the best <see cref="TimeConverter"/> for the specified <paramref name="sourceType"/>.
        /// If there is no <see cref="TimeConverterAttribute"/> applied to the <see cref="IAudioSource"/> object (the <paramref name="sourceType"/>), it looks up the inheritance hierarchy (interfaces included) of the <see cref="IAudioSource"/> object
        /// and searches for all registered source types. If there is a match it returns the associated <see cref="TimeConverter"/>. If there are more or less than one match BUT no <see cref="TimeConverterAttribute"/>
        /// it throws an exception.</remarks>
        public TimeConverter GetTimeConverterForSourceType(Type sourceType)
        {
            if (sourceType == null)
                throw new ArgumentNullException("sourceType");

            if(!typeof(IAudioSource).IsAssignableFrom(sourceType))
                throw new ArgumentException("Specified type is no AudioSource.", "sourceType");

            //we may got it already in the cache
            if (_cache.ContainsKey(sourceType))
                return _cache[sourceType].GetTimeConverter();

            //nope, there is nothing in the cache

            //search for a TimeConverterAttribute
            var attribute =
                sourceType.GetCustomAttributes(typeof (TimeConverterAttribute), false).FirstOrDefault() as
                    TimeConverterAttribute;

            TimeConverter timeConverter = null;
            try
            {
                if (attribute == null)
                {
                    //there is no attribute
                    //search for base types
                    var baseTypes = GetTypes(sourceType).Where(x => _timeConverters.ContainsKey(x)).ToArray();
                    //we've got a match
                    if (baseTypes.Length == 1)
                    {
                        timeConverter = _timeConverters[baseTypes.First()];
                        return timeConverter;
                    }
                    //we've got no match
                    if (baseTypes.Length == 0)
                        throw new ArgumentException(
                            "No registered time converter for the specified source type was found.");
                    //else baseTypes.Length > 1    
                    throw new ArgumentException(
                        "Multiple possible time converters, for the specified source type, were found. Specify which time converter to use, through the TimeConverterAttribute.");
                }

                var timeConverterType = attribute.TimeConverterType;
                timeConverter = (TimeConverter) Activator.CreateInstance(timeConverterType, attribute.Args);

                return timeConverter;
            }
            finally
            {
                //add the result to the cache
                if (timeConverter != null)
                {
                    CacheItem cacheItem;
                    if (attribute == null)
                        cacheItem = new CacheItem {CreateNewInstance = false, TimeConverter = timeConverter};
                    else
                        cacheItem = new CacheItem
                        {
                            CreateNewInstance = attribute.ForceNewInstance,
                            TimeConverterAttribute = attribute,
                            TimeConverter = attribute.ForceNewInstance ? null : timeConverter
                        };

                    _cache[sourceType] = cacheItem;
                }
            }
        }

        /// <summary>
        /// Clears the internal cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        private IEnumerable<Type> GetTypes(Type type)
        {
            //copied from dadhi see http://stackoverflow.com/questions/1823655/given-a-c-sharp-type-get-its-base-classes-and-implemented-interfaces
            return type.BaseType == typeof(object)
                ? type.GetInterfaces()
                : Enumerable
                    .Repeat(type.BaseType, 1)
                    .Concat(type.GetInterfaces())
                    .Concat(GetTypes(type.BaseType))
                    .Distinct();
        }

        private class CacheItem
        {
            public TimeConverter TimeConverter { get; set; }

            public TimeConverterAttribute TimeConverterAttribute { get; set; }

            public bool CreateNewInstance { get; set; }

            public TimeConverter GetTimeConverter()
            {
                if(CreateNewInstance)
                    return (TimeConverter)Activator.CreateInstance(TimeConverterAttribute.TimeConverterType, TimeConverterAttribute.Args);
                return TimeConverter;
            }
        }
    }
}