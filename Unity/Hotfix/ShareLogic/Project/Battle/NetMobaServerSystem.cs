using System;

namespace ET
{
    public static class NetMobaServerSystem
    {
        public static async void SendGameEnter(this NetMobaServerComponent self)
        {
            try
            {
                var battleScene = self.DomainScene();
                var battleDataComponent = battleScene.GetComponent<BattleDataComponent>();
                var session = battleScene.GetComponent<SessionComponent>().Session;
                var b2CGameMainEnter = (B2C_GameMainEnter)await session.Call(new C2B_GameMainEnter()
                {
                    RoomId = battleDataComponent.RoomId,
                    Uid = battleDataComponent.Uid,
                    Token = battleDataComponent.Token,
                });

                if (b2CGameMainEnter.Error > 0)
                {
                    switch (b2CGameMainEnter.Error)
                    {
                        case ErrorCode.ERR_BATTLE_ROOM_NOT_EXIST:
                        case ErrorCode.ERR_BATTLE_PLAYER_NOT_EXIST:
                        case ErrorCode.ERR_BATTLE_TOKEN_FAILED:
                            // TODO 异常退出游戏, 返回大厅.
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async void SendFrameMsg(this NetMobaServerComponent self, FrameMsg frameMsg)
        {
            try
            {
                var battleScene = self.DomainScene();
                var battleDataComponent = battleScene.GetComponent<BattleDataComponent>();
                var mobaBattleEntity = battleScene.GetChild<MobaBattleEntity>(battleDataComponent.BattleId);
                var session = battleScene.GetComponent<SessionComponent>().Session;
                var frameSyncComponent = mobaBattleEntity.GetComponent<FrameSyncComponent>();
                var b2CFrameMsg = (B2C_FrameMsg)await session.Call(new C2B_FrameMsg()
                    { FrameId = frameSyncComponent.m_nCurFrame, Msg = frameMsg });
                if (b2CFrameMsg.Error > 0)
                {
                    switch (b2CFrameMsg.Error)
                    {
                        case ErrorCode.ERR_BATTLE_DISCONNECTED:
                            self.SendGameEnter();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}