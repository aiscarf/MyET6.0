using System;

namespace ET
{
    public class C2B_BattleReady_Handler: AMRpcHandler<C2B_BattleReady, B2C_BattleReady>
    {
        protected override async ETTask Run(Session session, C2B_BattleReady request, B2C_BattleReady response, Action reply)
        {
            try
            {
                var sessionFighterComponent = session.GetComponent<SessionFighterComponent>();
                if (sessionFighterComponent == null)
                {
                    response.Error = ErrorCode.ERR_DISCONNECTED;
                    response.Message = "请重新发送GameMainEnter";
                    reply();
                    return;
                }

                var fighter = sessionFighterComponent.m_fighter;
                fighter.IsReady = true;
                var battleRoom = fighter.GetParent<BattleRoom>();
                battleRoom.CheckRoomReady();
                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}