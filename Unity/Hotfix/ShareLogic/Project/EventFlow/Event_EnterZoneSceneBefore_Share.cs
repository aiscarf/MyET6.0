namespace ET
{
    public class Event_EnterZoneSceneBefore_Share : AEvent<EventType.EnterZoneSceneBefore>
    {
        protected override async ETTask Run(EventType.EnterZoneSceneBefore args)
        {
            var sceneType = args.ZoneScene.SceneType;
            switch (sceneType)
            {
                case SceneType.Login:
                    args.ZoneScene.AddComponent<LoginDataComponent>();
                    break;
                case SceneType.Main:
                    break;
                case SceneType.Battle:
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}