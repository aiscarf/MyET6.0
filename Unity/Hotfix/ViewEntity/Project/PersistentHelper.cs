using UnityEngine;

namespace ET
{
    public static class PersistentHelper
    {
        public const string LAST_SELECT_REGION_ID = "LastSelectRegionId";
        public const string LAST_SELECT_DUNGEON_ID = "LastSelectDungeonId";

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}