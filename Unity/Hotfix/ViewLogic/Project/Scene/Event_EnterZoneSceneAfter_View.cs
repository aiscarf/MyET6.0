namespace ET
{
    public class Event_EnterZoneSceneAfter_View : AEvent<EventType.EnterZoneSceneAfter>
    {
        protected override async ETTask Run(EventType.EnterZoneSceneAfter args)
        {
            if (args.ZoneScene.SceneType == SceneType.Login)
            {
                args.ZoneScene.AddComponent<LoginViewDataComponent>();

                await UIManager.Instance.CreateUI(UIType.UILoginScene);
                await UIManager.Instance.CreateUI(UIType.UILogin);
                await UIManager.Instance.CreateUI(UIType.UIRegister);
                await UIManager.Instance.CreateUI(UIType.UIServerList);

                await UIHelper.OpenUI(UIType.UIAGm);
                await UIHelper.OpenUI(UIType.UILoginScene);
            }
            else if (args.ZoneScene.SceneType == SceneType.Main)
            {
                args.ZoneScene.AddComponent<MainViewDataComponent>();

                await UIManager.Instance.CreateUI(UIType.UIMain);
                await UIManager.Instance.CreateUI(UIType.UISelectMap);
                
                await UIHelper.OpenUI(UIType.UIAGm);
            }
            else if (args.ZoneScene.SceneType == SceneType.Battle)
            {
                args.ZoneScene.AddComponent<BattleViewDataComponent>();
            }
        }
    }
}