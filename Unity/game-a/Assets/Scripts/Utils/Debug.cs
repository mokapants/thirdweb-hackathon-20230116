using System;
using UnityEngine;

namespace Utils
{
    public static class Debug
    {
        public static void Log(string log)
        {
            UnityEngine.Debug.Log(log);
        }
        
        public static void LogError(Exception exception)
        {
            UnityEngine.Debug.LogError(exception);
        }
    }
}