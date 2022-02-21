namespace ET
{
    public static class LogHelper
    {
        public static void Console(SceneType sceneType, string str)
        {
            Log.Console($"[{sceneType.ToString()}]: {str}");
        }

        public static void Console(SceneType sceneType, string str, params object[] args)
        {
            Log.Console($"[{sceneType.ToString()}]: {str}", args);
        }
    }
}