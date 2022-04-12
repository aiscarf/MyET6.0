namespace Scarf.Moba
{
    public static class BuffFactory
    {
        public static Buff CreateBuff(BuffData buffData, Unit creater, Unit target)
        {
            var battle = target.Battle;
            var buff = CObjectPool<Buff>.instance.GetObject();
            buff.Init(battle.GenerateBuffId(), buffData, creater, target);
            return buff;
        }
    }
}