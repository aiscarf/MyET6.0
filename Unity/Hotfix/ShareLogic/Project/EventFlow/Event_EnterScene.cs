namespace ET
{
    public class Event_EnterScene : AEvent<EventType.EnterZoneScene>
    {
        protected override async ETTask Run(EventType.EnterZoneScene args)
        {
            int zone = args.ZoneScene.Zone;
            if (zone == 1)
            {
                // TODO Login场景使用HttpComponent.
            }
            else if (zone == 2)
            {
                // TODO Main场景使用TcpComponent.
            }
            else if (zone == 3)
            {
                // TODO Battle场景使用KcpComponent.
            }

            await ETTask.CompletedTask;
        }
    }
}