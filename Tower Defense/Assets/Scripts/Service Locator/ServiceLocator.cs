using System;
using System.Collections.Generic;
using Utilities;

namespace Service_Locator
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance { get; private set; }

        private Dictionary<string, IService> _services = new Dictionary<string, IService>();

        private ServiceLocator()
        {
        }

        ~ServiceLocator()
        {
            _services.Clear();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        public static void Initialize() => Instance = new ServiceLocator();

        public T Get<T>() where T : IService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                return (T)_services[key];
            }

            throw new Exception($"There is no service with type {key}");
        }

        public void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                Logger.LogWarning($"Service {key} is already installed");
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                _services.Remove(key);
            }
        }
    }
}