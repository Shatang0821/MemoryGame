using System.IO;
using UnityEngine;

namespace FrameWork.Utils
{
    public static class DebugLogger
    {
        public static void Log(object o)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(o);
#endif
        }

        public static void LogWarning(object o)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(o);
#endif  
        }
        
        
    } 
    
    public static class Logger
    {
        private static string logFilePath = Path.Combine(Application.persistentDataPath, "gameLog.txt");

        public static void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(message);
            }
        }
        
    }
}
