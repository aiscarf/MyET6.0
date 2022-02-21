﻿namespace ET
{
    public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
    {
        public override async void Destroy(SessionPlayerComponent self)
        {
            // DONE: 获取Realm服务器ActorId.
            var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(self.Player.DomainZone(), SceneType.Realm);
            long actorId1 = startSceneConfig.InstanceId;

            // DONE: 发送断线消息.
            await MessageHelper.CallActor(actorId1, new G2R_PlayerOfflineRequest() { Uid = self.Player.Uid, RealmToken = self.Player.RealmToken });
            var playerComponent = self.Player.GetParent<PlayerComponent>();
            playerComponent.Remove(self.Player.Id);

            LogHelper.Console(SceneType.Gate, $"玩家[{self.Player.Uid}]已下线");
        }
    }
}