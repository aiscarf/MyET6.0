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
		public long Uid { get; private set; }
		public string RealmToken { get; private set; }

		public void Awake(long uid, string token)
		{
			this.Uid = uid;
			this.RealmToken = token;
		}
	}
}