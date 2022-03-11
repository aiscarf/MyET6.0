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

        public static async ETTask KickOutPlayer(Scene scene, long uid)
        {
            // DONE: 验证账号是否在线，在线则踢下线
            var onlineComponent = scene.GetComponent<OnlineComponent>();
            if (!onlineComponent.HasOnline(uid))
            {
                return;
            }

            // DONE: 通知Gate网关将其踢下线.
            long gateId = onlineComponent.GetGateId(uid);
            var startSceneConfig = StartSceneConfigCategory.Instance.Get((int)gateId);
            long actorId = startSceneConfig.InstanceId;
            await MessageHelper.CallActor(actorId, new R2G_PlayerKickOutRequest() { Uid = uid });
            onlineComponent.RemoveByUid(uid);

            Log.Info($"RealmServer 玩家[{uid}]已被踢下线");
        }
    }
}