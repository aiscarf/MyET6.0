namespace ET
{
    public sealed class SessionFightComponentAwakeSystem: AwakeSystem<SessionFighterComponent>
    {
        public override void Awake(SessionFighterComponent self)
        {
        }
    }

    public sealed class SessionFightComponentDestroySystem: DestroySystem<SessionFighterComponent>
    {
        public override void Destroy(SessionFighterComponent self)
        {
            self.m_fighter.IsConnected = false;
            self.m_fighter.ClientSession = null;

            // DONE: 判断是不是游戏内的玩家都离开了房间, 是的话, 提前结束该房间的游戏.
            var battleComponent = self.DomainScene().GetComponent<BattleComponent>();
            var battleRoom = battleComponent.GetRoom(self.m_fighter.RoomeId);
            if (battleRoom == null || battleRoom.IsEnd)
            {
                return;
            }

            var lstFighters = battleRoom.GetAllFighters();
            foreach (var kv in lstFighters)
            {
                if (kv.Value.IsConnected)
                {
                    return;
                }
            }

            // DONE: 开始倒计时, 结束该房间的游戏, 除非有人重连回来.
            battleRoom.StartCountdownDestroy();
        }
    }
}