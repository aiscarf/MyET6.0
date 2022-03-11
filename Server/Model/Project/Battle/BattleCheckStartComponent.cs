namespace ET
{
    public sealed class BattleCheckStartComponent : Entity
    {
        public BattleRoom BattleRoom { get; set; }
        public long TimerId;
        public long MaxTime;
    }
}