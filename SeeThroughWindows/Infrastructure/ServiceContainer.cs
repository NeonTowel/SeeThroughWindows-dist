namespace SeeThroughWindows.Infrastructure
{
    /// <summary>
    /// Simple dependency injection container for the application
    /// </summary>
    public class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly Dictionary<Type, Func<object>> _factories = new();

        /// <summary>
        /// Register a singleton service instance
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : class, TInterface
        {
            _services[typeof(TInterface)] = instance;
        }        /// <summary>
        /// Register a service factory
        /// </summary>
        public void RegisterFactory<TInterface>(Func<TInterface> factory)
        {
#pragma warning disable CS8603 // Possible null reference return
            _factories[typeof(TInterface)] = () => factory();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Register a transient service
        /// </summary>
        public void RegisterTransient<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            _factories[typeof(TInterface)] = () => new TImplementation();
        }

        /// <summary>
        /// Resolve a service instance
        /// </summary>
        public T Resolve<T>()
        {
            var type = typeof(T);            if (_services.TryGetValue(type, out var instance))
            {
                return (T)instance;
            }

            if (_factories.TryGetValue(type, out var factory))
            {
                var result = factory();
                return result != null ? (T)result : throw new InvalidOperationException($"Factory for service of type {type.Name} returned null.");
            }

            throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
        }

        /// <summary>
        /// Check if a service is registered
        /// </summary>
        public bool IsRegistered<T>()
        {
            var type = typeof(T);
            return _services.ContainsKey(type) || _factories.ContainsKey(type);
        }
    }

    /// <summary>
    /// Static service locator for global access
    /// </summary>
    public static class ServiceLocator
    {
        private static ServiceContainer? _container;

        /// <summary>
        /// Initialize the service locator with a container
        /// </summary>
        public static void Initialize(ServiceContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Resolve a service instance
        /// </summary>
        public static T Resolve<T>()
        {
            if (_container == null)
                throw new InvalidOperationException("ServiceLocator not initialized. Call Initialize() first.");

            return _container.Resolve<T>();
        }

        /// <summary>
        /// Check if a service is registered
        /// </summary>
        public static bool IsRegistered<T>()
        {
            return _container?.IsRegistered<T>() ?? false;
        }
    }
}
