namespace ET
{
    public class Event_EnterZoneSceneBefore : AEvent<EventType.EnterZoneSceneBefore>
    {
        protected override async ETTask Run(EventType.EnterZoneSceneBefore args)
        {
            int zone = args.ZoneScene.Zone;
            if (zone == 1)
            {
                args.ZoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType
                    .SessionStreamDispatcherClientOuter);
            }
            else if (zone == 2)
            {
                args.ZoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType
                    .SessionStreamDispatcherClientOuter);
            }
            else if (zone == 3)
            {
                args.ZoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType
                    .SessionStreamDispatcherClientOuter);
            }

            Game.EventSystem.Publish(new EventType.EnterZoneSceneAfter() { ZoneScene = args.ZoneScene });
            await ETTask.CompletedTask;
        }
    }
}