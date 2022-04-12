namespace XNode
{
    public static class NodeDebug
    {
        public static void LogWarning(string content)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(content);
#endif
        }

        public static void LogError(string content)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(content);
#endif
        }
    }
}