namespace ET
{
    [ObjectSystem]
    public class PlayerSystem: AwakeSystem<Player, long, string>
    {
        public override void Awake(Player self, long uid, string token)
        {
            self.Awake(uid, token);
        }
    }

    public sealed class Player: Entity
    {
        public string RealmToken { get; private set; }
        public string GateToken { get; set; }
        public string BattleToken { get; set; }
        public int RoomId { get; set; }
        public long BattleActorId { get; set; }
        public ETCancellationToken CancellationToken { get; set; } = new ETCancellationToken();
        public bool IsConnected { get; set; }
        public Session ClientSession { get; set; }
        public EPlayerState PlayerState { get; private set; } = EPlayerState.None;

        public void ChangeState(EPlayerState ePlayerState)
        {
            this.PlayerState = ePlayerState;
        }

        public void SendClient(IMessage message)
        {
            if (this.ClientSession == null)
                return;
            this.ClientSession.Send(message);
        }

        public void Awake(long uid, string token)
        {
            this.Id = uid;
            this.RealmToken = token;
        }
    }
}