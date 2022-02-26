using System;

namespace ET
{
    public static class MainHelper
    {
        public static async ETTask StartReady()
        {
            try
            {
                var mainScene = ZoneSceneManagerComponent.Instance.CurScene;
                var mainDataComponent = mainScene.GetComponent<MainDataComponent>();
                Session gateSession = mainScene.GetComponent<NetKcpComponent>().Get(mainDataComponent.SessionId);
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
                var mainDataComponent = mainScene.GetComponent<MainDataComponent>();
                Session gateSession = mainScene.GetComponent<NetKcpComponent>().Get(mainDataComponent.SessionId);
                var g2CCancelReady = await gateSession.Call(new C2G_CancelReady()) as G2C_CancelReady;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}