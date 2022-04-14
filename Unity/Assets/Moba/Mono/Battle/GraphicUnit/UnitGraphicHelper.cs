namespace Scarf.Moba
{
    public static class UnitGraphicHelper
    {
        public static UnitGraphicComponent UnitGraphic(this Unit self)
        {
            return self.GetComponent<UnitGraphicComponent>();
        }
    }
}