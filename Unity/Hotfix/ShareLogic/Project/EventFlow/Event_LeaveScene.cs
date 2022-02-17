namespace ET
{
    public class Event_LeaveScene : AEvent<EventType.LeaveZoneScene>
    {
        protected override async ETTask Run(EventType.LeaveZoneScene args)
        {
            if (args.LeaveZone == null)
                return;
            args.LeaveZone.Dispose();
            await ETTask.CompletedTask;
        }
    }
}