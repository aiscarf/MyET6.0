namespace ET
{
    public class Event_MobaBattleProcessStart : AEvent<EventType.MobaBattleProcessStart>
    {
        protected override async ETTask Run(EventType.MobaBattleProcessStart args)
        {
            await ETTask.CompletedTask;
            await UIManager.Instance.OpenUI(UIType.UIBattle, null);
        }
    }
}