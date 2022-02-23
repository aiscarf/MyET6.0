namespace ET
{
    public class Event_EnterZoneSceneAfter_View : AEvent<EventType.EnterZoneSceneAfter>
    {
        protected override async ETTask Run(EventType.EnterZoneSceneAfter args)
        {
            if (args.ZoneScene.SceneType == SceneType.Login)
            {
                await UIManager.Instance.CreateUI(UIType.UILoginScene);
                await UIManager.Instance.CreateUI(UIType.UILogin);
                await UIManager.Instance.CreateUI(UIType.UIRegister);
                await UIManager.Instance.CreateUI(UIType.UIServerList);

                await UIHelper.OpenUI(UIType.UIAGm);
                await UIHelper.OpenUI(UIType.UILoginScene);
            }
            else if (args.ZoneScene.SceneType == SceneType.Main)
            {
                await UIManager.Instance.CreateUI(UIType.UIMain);
                await UIManager.Instance.CreateUI(UIType.UISelectMap);
            }
        }
    }
}