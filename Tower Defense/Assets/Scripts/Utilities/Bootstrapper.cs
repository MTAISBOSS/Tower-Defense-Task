using Service_Locator;
using UnityEngine;

namespace Utilities
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            var serviceInitializer = Resources.Load("ServiceInitializer");
            Object.DontDestroyOnLoad(
                Object.Instantiate(
                    serviceInitializer
                )
            );
            ServiceInitializer initializer = new ServiceInitializer();
        }
    }
}