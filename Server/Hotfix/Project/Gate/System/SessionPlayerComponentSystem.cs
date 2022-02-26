namespace ET
{
    public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
    {
        public override async void Destroy(SessionPlayerComponent self)
        {
            var playerComponent = self.Player.GetParent<PlayerComponent>();
            playerComponent.Remove(self.Player.Id);
            LogHelper.Console(SceneType.Gate, $"玩家[{self.Player.Id}]已下线");
            
            // DONE: 发送断线消息.
            var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(self.Player.DomainZone(), SceneType.Realm);
            long actorId1 = startSceneConfig.InstanceId;
            await MessageHelper.CallActor(actorId1, new G2R_PlayerOfflineRequest() { Uid = self.Player.Id, RealmToken = self.Player.RealmToken });

        }
    }
}