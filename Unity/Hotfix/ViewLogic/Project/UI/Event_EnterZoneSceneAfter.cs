namespace ET
{
    public class Event_EnterZoneSceneAfter : AEvent<EventType.EnterZoneSceneAfter>
    {
        protected override async ETTask Run(EventType.EnterZoneSceneAfter args)
        {
            if (args.ZoneScene.Zone == 1)
            {
                await UIManager.Instance.CreateUI(UIType.UILogin);
                await UIManager.Instance.OpenUI(UIType.UILogin, null);
            }
            else if (args.ZoneScene.Zone == 2)
            {
                UIManager.Instance.CloseUI(UIType.UILogin);
                UIManager.Instance.DestroyUI(UIType.UILogin);

                await UIManager.Instance.CreateUI(UIType.UIMain);
                await UIManager.Instance.OpenUI(UIType.UIMain, null);
            }

            await ETTask.CompletedTask;
        }
    }
}