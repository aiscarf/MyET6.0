namespace ET
{
    public class Event_LoginFinish : AEvent<EventType.LoginFinish>
    {
        protected override async ETTask Run(EventType.LoginFinish args)
        {
            await ETTask.CompletedTask;
        }
    }
}