using System;
using UnityEngine;

namespace Utils
{
    public static class Debug
    {
#if UNITY_EDITOR
        private static bool isOutput = true;
#else
        private static bool isOutput = false;
#endif

        public static void Log(string log)
        {
            if (!isOutput) return;
            UnityEngine.Debug.Log(log);
        }

        public static void LogError(Exception exception)
        {
            if (!isOutput) return;
            UnityEngine.Debug.LogError(exception);
        }
    }
}