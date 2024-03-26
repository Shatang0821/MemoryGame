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
}
