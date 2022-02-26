namespace ET
{
    public class MatcherAwakeSystem: AwakeSystem<Matcher, long>
    {
        public override void Awake(Matcher self, long uid)
        {
            self.Awake(uid);
        }
    }

    public sealed class Matcher: Entity
    {
        public long Uid { get; private set; }
        public int MapId { get; set; }

        public void Awake(long uid)
        {
            this.Uid = uid;
        }
    }
}