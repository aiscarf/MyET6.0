namespace ET
{
    public class Event_LeaveScene : AEvent<EventType.LeaveZoneScene>
    {
        protected override async ETTask Run(EventType.LeaveZoneScene args)
        {
            if (args.ZoneScene == null)
                return;
            int zone = args.ZoneScene.Zone;
            if (zone == 1)
            {
                args.ZoneScene.Dispose();
            }
            else if (zone == 2)
            {
                // TODO 主场景不进行卸载.
            }
            else if (zone == 3)
            {
                args.ZoneScene.Dispose();
            }

            await ETTask.CompletedTask;
        }
    }
}