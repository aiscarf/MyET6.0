using System;

namespace ET
{
    public static class MainHelper
    {
        public static async ETTask LoginBattle(string address, int roomId, string token)
        {
            try
            {
                var battleScene = ZoneSceneManagerComponent.Instance.Get((int)SceneType.Battle);
                if (battleScene == null)
                {
                    battleScene = await SceneFactory.CreateZoneScene((int)SceneType.Battle, SceneType.Battle, SceneType.Battle.ToString(), Game.Scene);
                }

                var mainDataComponent = DataHelper.GetDataComponentFromCurScene<MainDataComponent>();
                long uid = mainDataComponent.Uid;
                
                // DONE: 尝试连接战斗服务器.
                Session battleSession = battleScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                B2C_GameMainEnter b2CGameMainEnter = (B2C_GameMainEnter)await battleSession.Call(new C2B_GameMainEnter()
                    { RoomId = roomId, Token = token, Uid = uid });
                if (b2CGameMainEnter.Error > ErrorCode.ERR_Success)
                {
                    battleScene.Dispose();
                    Log.Error(b2CGameMainEnter.Error + " " + b2CGameMainEnter.Message);
                    return;
                }

                // DONE: 切换至战斗场景.
                await ZoneSceneManagerComponent.Instance.ChangeScene(SceneType.Battle);

                battleScene.AddComponent<SessionComponent>().Session = battleSession;
                var battleDataComponent = DataHelper.GetDataComponentFromCurScene<BattleDataComponent>();
                battleDataComponent.Uid = uid;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask ReconnectGate()
        {
            try
            {
                var mainScene = ZoneSceneManagerComponent.Instance.CurScene;
                var mainDataComponent = mainScene.GetComponent<MainDataComponent>();
                var gateSession = mainScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(mainDataComponent.GateAddress));
                var g2CReconnectGate = (G2C_ReconnectGate)await gateSession.Call(new C2G_ReconnectGate() { GateToken = mainDataComponent.GateToken, Uid = mainDataComponent.Uid });
                if (g2CReconnectGate.Error > ErrorCode.ERR_Success)
                {
                    gateSession.Dispose();
                    Log.Error(g2CReconnectGate.Error + " " + g2CReconnectGate.Message);
                    return;
                }
                
                // DONE: 断线重连成功.
                gateSession.AddComponent<PingComponent>();
                gateSession.AddComponent<SessionStateComponent, SceneType>(mainScene.SceneType);
                mainScene.GetComponent<SessionComponent>().Session = gateSession;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        
        public static async ETTask StartReady()
        {
            try
            {
                var mainScene = ZoneSceneManagerComponent.Instance.CurScene;
                Session gateSession = mainScene.GetComponent<SessionComponent>().Session;
                var g2CStartReady = await gateSession.Call(new C2G_StartReady()) as G2C_StartReady;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask CancelReady()
        {
            try
            {
                var mainScene = ZoneSceneManagerComponent.Instance.CurScene;
                Session gateSession = mainScene.GetComponent<SessionComponent>().Session;
                var g2CCancelReady = await gateSession.Call(new C2G_CancelReady()) as G2C_CancelReady;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}