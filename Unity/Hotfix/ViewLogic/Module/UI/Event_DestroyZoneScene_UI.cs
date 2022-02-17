namespace ET
{
    public class Event_DestroyZoneScene_UI : AEvent<EventType.DestroyZoneScene>
    {
        protected override async ETTask Run(EventType.DestroyZoneScene args)
        {
            await UIManager.Instance.DestroyScene(args.ZoneScene);
        }
    }
}