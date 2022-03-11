namespace ET
{
    public class Event_MobaBattleProcessStart : AEvent<EventType.MobaBattleProcessStart>
    {
        protected override async ETTask Run(EventType.MobaBattleProcessStart args)
        {
            await UIHelper.OpenUI(UIType.UIBattle);
            await UIHelper.CloseUI(UIType.UIBattleLoading);
        }
    }
}