using System;

namespace Scarf.Moba
{
    public static class BattleLog
    {
        public static void Log(string str)
        {
            UnityEngine.Debug.Log(str);
        }

        public static void Error(string str)
        {
            UnityEngine.Debug.LogError(str);
        }

        public static void Error(Exception ex)
        {
            UnityEngine.Debug.LogError(ex.ToString());
        }
    }
}