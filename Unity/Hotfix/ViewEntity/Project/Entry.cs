using System;

namespace ET
{
    public static class Entry
    {
        public static void Start()
        {
            Log.Debug("顺利进入Entry");

            if (CodeLoader.Instance == null)
            {
                Log.Debug("热更工程访问不到主工程");
                return;
            }

            var types = CodeLoader.Instance.GetTypes();
            Log.Debug("加载到的类数量: " + types.Length);
            
            try
            {
                CodeLoader.Instance.Update += Game.Update;
                CodeLoader.Instance.LateUpdate += Game.LateUpdate;
                CodeLoader.Instance.OnApplicationQuit += Game.Close;

                Game.EventSystem.Add(CodeLoader.Instance.GetTypes());
                Game.EventSystem.Publish(new EventType.AppStart());
            }
            catch (Exception e)
            {
                Log.Error("不幸发生错误");
                Log.Error(e);
            }
        }
    }
}