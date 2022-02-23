namespace ET
{
    public class Event_LoginGateFinish : AEvent<EventType.LoginGateFinish>
    {
        protected override async ETTask Run(EventType.LoginGateFinish args)
        {
            Log.Debug("进入大厅界面");
            await UIHelper.OpenUI(UIType.UIMain);
        }
    }
}