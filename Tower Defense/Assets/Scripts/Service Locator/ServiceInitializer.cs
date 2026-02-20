using Enemy.Base;
using UnityEditor;
using UnityEngine;
using Update_Service;

namespace Service_Locator
{
    public class ServiceInitializer
    {
        public ServiceInitializer()
        {
            ServiceLocator.Initialize();
            GameObject updateManager = new GameObject("Update Manager");
            updateManager.AddComponent<UpdateManager>();

           ServiceLocator.Instance.Register(new EnemyTransformContainer());
        }
    }
}