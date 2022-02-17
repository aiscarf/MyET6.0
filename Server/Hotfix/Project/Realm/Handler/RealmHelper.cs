using System.Collections.Generic;

namespace ET
{
    public static class RealmHelper
    {
        public static StartSceneConfig GetGate(int zone)
        {
            List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
            int n = RandomHelper.RandomNumber(0, zoneGates.Count);
            return zoneGates[n];
        }

        public static async ETTask KickOutPlayer(Session session, string account)
        {
            //验证账号是否在线，在线则踢下线
            string realmToken = session.DomainScene().GetComponent<RealmTokenComponent>().GetToken(account);
            if (string.IsNullOrEmpty(realmToken) || string.IsNullOrWhiteSpace(realmToken))
            {
                return;
            }
            
            // TODO 通知Gate网关将其踢下线.
            Log.Info($"账户[{account}]已被踢下线");
            await ETTask.CompletedTask;
        }
    }
}