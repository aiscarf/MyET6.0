namespace ET
{
    public sealed class BattleViewDataComponent : Entity
    {
        public long Uid { get; set; }
        public DataProxy<float> LoadingProgressProxy = new DataProxy<float>(0f);
    }
}