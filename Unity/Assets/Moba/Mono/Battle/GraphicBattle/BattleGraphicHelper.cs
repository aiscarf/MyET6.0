namespace Scarf.Moba
{
    public static class BattleGraphicHelper
    {
        public static BattleGraphicComponent BattleGraphic(this Battle self)
        {
            return self.GetComponent<BattleGraphicComponent>();
        }
    }
}