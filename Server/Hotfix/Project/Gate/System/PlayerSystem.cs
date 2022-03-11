namespace ET
{
    public static class PlayerSystem
    {
        public static async void StartCountdownRemovePlayer(this PlayerComponent self, long playerId)
        {
            var player = self.Get(playerId);
            if (player == null)
            {
                return;
            }

            // DONE: 等待60s后移除改玩家.
            bool b = await TimerComponent.Instance.WaitAsync(60 * 1000, player.CancellationToken);
            if (!b)
            {
                return;
            }

            // DONE: 玩家彻底下线时, 需要同步玩家的状态. 
            switch (player.PlayerState)
            {
                case EPlayerState.None:
                    break;
                case EPlayerState.Hall:
                    break;
                case EPlayerState.Match:
                    // DONE: 通知匹配服取消匹配.
                    var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(self.DomainZone(), SceneType.Match);
                    long actorId1 = startSceneConfig.InstanceId;
                    ActorMessageSenderComponent.Instance.Send(actorId1, new G2M_CancelMatch() { Uid = player.Id });
                    break;
                case EPlayerState.Game:
                    break;
            }

            // DONE: 释放玩家对象.
            self.Remove(playerId);
            player.Dispose();
            LogHelper.Console(SceneType.Gate, $"玩家[{playerId}]已下线");

            // DONE: 通知登录服务器玩家已下线.
            var realmSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(self.DomainZone(), SceneType.Realm);
            await MessageHelper.CallActor(realmSceneConfig.InstanceId,
                new G2R_PlayerOfflineRequest() { Uid = player.Id, RealmToken = player.RealmToken });
        }

        public static void StopCountdownRemovePlayer(this PlayerComponent self, long playerId)
        {
            var player = self.Get(playerId);
            if (player == null)
            {
                return;
            }

            player.CancellationToken.Cancel();
        }
    }
}