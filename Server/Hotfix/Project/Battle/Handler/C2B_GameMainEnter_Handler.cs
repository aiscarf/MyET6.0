using System;

namespace ET
{
    public class C2B_GameMainEnter_Handler: AMRpcHandler<C2B_GameMainEnter, B2C_GameMainEnter>
    {
        protected override async ETTask Run(Session session, C2B_GameMainEnter request, B2C_GameMainEnter response, Action reply)
        {
            try
            {
                var battleComponent = session.DomainScene().GetComponent<BattleComponent>();
                var battleRoom = battleComponent.GetRoom(request.RoomId);
                if (battleRoom == null)
                {
                    response.Error = ErrorCode.ERR_BATTLE_ROOM_NOT_EXIST;
                    response.Message = "房间不存在";
                    reply();
                    return;
                }

                if (battleRoom.Token != request.Token)
                {
                    response.Error = ErrorCode.ERR_BATTLE_TOKEN_FAILED;
                    response.Message = "Token错误";
                    reply();
                    return;
                }

                var fighter = battleRoom.Get(request.Uid);
                if (fighter == null)
                {
                    response.Error = ErrorCode.ERR_BATTLE_PLAYER_NOT_EXIST;
                    response.Message = "玩家不存在";
                    reply();
                    return;
                }

                if (battleRoom.IsEnd)
                {
                    response.Error = ErrorCode.ERR_BATTLE_ROOM_NOT_EXIST;
                    response.Message = "游戏已经结束";
                    return;
                }

                // DONE: 游戏没有结束, 并且有玩家已经重连回来. 
                if (battleRoom.IsCountdownDestroy)
                {
                    battleRoom.CancelCountdownDestroy();
                }

                // DONE: 绑定Session与Fighter.
                fighter.IsConnected = true;
                fighter.ClientSession = session;
                session.AddComponent<SessionFighterComponent>().m_fighter = fighter;

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