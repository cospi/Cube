using System;
using System.Collections.Generic;

using UnityEngine;

namespace Cube.ServiceManagement
{
    public sealed class ServiceManager
    {
        private readonly Dictionary<Type, object> _servicesByType = new Dictionary<Type, object>();

        public T GetService<T>() where T : class
        {
            object service;
            _servicesByType.TryGetValue(typeof(T), out service);
            return service as T;
        }

        public void SetService<T>(T service) where T : class
        {
            _servicesByType[typeof(T)] = service;
            Debug.Log($"Set service {typeof(T).Name}: {service}");
        }
    }
}
