using System;

namespace ET
{
    public class Event_PlayerKickOut_View : AEvent<EventType.PlayerKickOut>
    {
        protected override async ETTask Run(EventType.PlayerKickOut args)
        {
            try
            {
                await UIHelper.ShowSingleSelectAsync("提示", "您的账号在其他设备登录!", "确认");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}