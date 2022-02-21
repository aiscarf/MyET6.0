namespace ET
{
    public class Event_EnterZoneSceneAfter : AEvent<EventType.EnterZoneSceneAfter>
    {
        protected override async ETTask Run(EventType.EnterZoneSceneAfter args)
        {
            if (args.ZoneScene.SceneType == SceneType.Login)
            {
                await UIManager.Instance.OpenForeverUI(UIType.UIAGm, null);
                await UIManager.Instance.OpenUI(UIType.UILoginScene, null);
            }
            else if (args.ZoneScene.SceneType == SceneType.Main)
            {
                // await UIManager.Instance.OpenUI(UIType.UIMain, null);
            }

            Log.Info($"ChangeScene_After: {args.ZoneScene.SceneType}");
            await ETTask.CompletedTask;
        }
    }
}