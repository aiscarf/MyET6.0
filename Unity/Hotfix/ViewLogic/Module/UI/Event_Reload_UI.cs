namespace ET
{
    public class Event_Reload_UI : AEvent<EventType.Reload>
    {
        protected override async ETTask Run(EventType.Reload args)
        {
            await ETTask.CompletedTask;
            UIMediatorManager.Instance.Reload();
        }
    }
}