namespace ET
{
    public static class BattleMgr
    {
        public static BattleViewDataComponent GetBattleViewDataComponent()
        {
            return DataHelper.GetDataComponentFromCurScene<BattleViewDataComponent>();
        }
    }
}