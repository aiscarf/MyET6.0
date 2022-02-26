namespace ET
{
	[ObjectSystem]
	public class PlayerSystem : AwakeSystem<Player, long, string>
	{
		public override void Awake(Player self, long uid, string token)
		{
			self.Awake(uid, token);
		}
	}

	public sealed class Player : Entity
	{
		public string RealmToken { get; private set; }

		public Session ClientSession { get; set; }
		public EPlayerState PlayerState { get; private set; } = EPlayerState.None;

		public void ChangeState(EPlayerState ePlayerState)
		{
			this.PlayerState = ePlayerState;
		}
		
		public void Awake(long uid, string token)
		{
			this.Id = uid;
			this.RealmToken = token;
		}
	}
}