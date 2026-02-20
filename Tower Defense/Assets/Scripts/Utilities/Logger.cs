using System.Diagnostics;

namespace Utilities
{
    public static class Logger
    {
        [Conditional("ENABLE_LOG")]
        public static void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
        [Conditional("ENABLE_LOG")]
        public static void LogError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
        [Conditional("ENABLE_LOG")]
        public static void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
    }
}