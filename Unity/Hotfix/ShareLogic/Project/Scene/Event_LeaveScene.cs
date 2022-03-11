namespace ET
{
    public class Event_LeaveScene : AEvent<EventType.LeaveZoneScene>
    {
        protected override async ETTask Run(EventType.LeaveZoneScene args)
        {
            if (args.LeaveZone == null)
                return;

            // DONE: 离开哪个场景, 就将这个场景的UI全部卸载.
            await UIManager.Instance.DestroyScene(args.LeaveZone);

            var sceneType = args.LeaveZone.SceneType;
            switch (sceneType)
            {
                case SceneType.Login:
                    args.LeaveZone.Dispose();
                    break;
                case SceneType.Main:
                    if (args.NextSceneType == SceneType.Login)
                    {
                        args.LeaveZone.Dispose();
                    }

                    break;
                case SceneType.Battle:
                    // DONE: 离开战斗场景时, 将战斗场景销毁.
                    args.LeaveZone.Dispose();
                    break;
            }
        }
    }
}