namespace ET
{
    public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
    {
        public override void Destroy(SessionPlayerComponent self)
        {
            // DONE: 开始下线倒计时
            var playerComponent = self.Player.GetParent<PlayerComponent>();
            self.Player.IsConnected = false;
            self.Player.ClientSession = null;

            // DONE: 当玩家处于战斗中的时候断线, 则等到它出来了在释放Player对象.
            if (self.Player.PlayerState == EPlayerState.Game)
            {
                return;
            }

            playerComponent.StartCountdownRemovePlayer(self.Player.Id);
        }
    }
}