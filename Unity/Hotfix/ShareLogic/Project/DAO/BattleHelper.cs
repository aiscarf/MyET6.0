using System;

namespace ET
{
    public static class BattleHelper
    {
        public static async ETTask BattleStart()
        {
            try
            {
                var battleScene = ZoneSceneManagerComponent.Instance.CurScene;
                var sessionComponent = battleScene.GetComponent<SessionComponent>();
                Session battleSession = sessionComponent.Session;
                var b2CBattleReady = (B2C_BattleReady)await battleSession.Call(new C2B_BattleReady());
                if (b2CBattleReady.Error > 0)
                {
                    Log.Error(b2CBattleReady.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}