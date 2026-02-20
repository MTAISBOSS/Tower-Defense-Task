using System;
using System.Collections.Generic;
using Service_Locator;
using UnityEngine;

namespace Update_Service
{
    public class UpdateManager : MonoBehaviour, IService
    {
        private readonly Dictionary<Type, IUpdate> _updateServices = new Dictionary<Type, IUpdate>();

        private void OnEnable()
        {
            ServiceLocator.Instance.Register(this);
        }
        private void OnDisable()
        {
            ServiceLocator.Instance.Unregister(this);
        }
        private void Update()
        {
            foreach (var service in _updateServices)
            {
                service.Value?.Update();
            }
        }
        public void Register<T>(T service) where T : IUpdate
        {
            if (_updateServices.ContainsKey(service.GetType()))
            {
                return;
            }
            _updateServices.Add(service.GetType(),service);
        }
        public void UnRegister<T>(T service) where T : IUpdate
        {
            if (_updateServices.ContainsKey(service.GetType()))
            {
                _updateServices.Remove(service.GetType());
            }
        }
    }
}