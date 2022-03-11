using System;

namespace ET
{
    public class C2B_FrameMsg_Handler: AMRpcHandler<C2B_FrameMsg, B2C_FrameMsg>
    {
        protected override async ETTask Run(Session session, C2B_FrameMsg request, B2C_FrameMsg response, Action reply)
        {
            try
            {
                var sessionFighterComponent = session.GetComponent<SessionFighterComponent>();
                if (sessionFighterComponent == null)
                {
                    response.Error = ErrorCode.ERR_BATTLE_DISCONNECTED;
                    response.Message = "与战斗服断开连接, 请重新进入房间";
                    reply();
                    return;
                }

                // DONE: 找到房间.
                int roomId = sessionFighterComponent.m_fighter.RoomeId;
                var battleComponent = session.DomainScene().GetComponent<BattleComponent>();
                var battleRoom = battleComponent.GetRoom(roomId);
                if (battleRoom == null)
                {
                    response.Error = ErrorCode.ERR_BATTLE_ROOM_NOT_EXIST;
                    reply();
                    return;
                }

                // DONE: 将数据存储至房间内.
                sessionFighterComponent.m_fighter.ClientSyncId = request.FrameId - 1;
                battleRoom.ReceiveClientOper(sessionFighterComponent.m_fighter.Uid, request);
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