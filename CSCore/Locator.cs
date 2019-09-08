using System;
using System.Collections.Generic;
using System.Text;

namespace CSCore
{
    public sealed class Locator
    {
        public static Locator Instance { get; } = new Locator();

        private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();

        private Locator()
        {
        }

        public void Register<TRegisterAs, TImplementation>(Func<TImplementation> factory) where TImplementation : TRegisterAs
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (_factories.TryGetValue(typeof(TRegisterAs), out var tmp))
                throw new Exception($"Service {nameof(TRegisterAs)} already registered.");

            _factories.Add(typeof(TRegisterAs), () => factory());
        }

        public T Get<T>()
        {
            if(_factories.TryGetValue(typeof(T), out var factory))
                return (T)factory();
            throw new Exception("Service not found.");
        }
    }
}
