namespace Scarf.Moba
{
    public abstract class GraphicComponent : CComponent
    {
        protected override void OnInit()
        {
            this.Battle.BattleGraphic().AddGraphic(this);
        }

        protected override void OnDestroy()
        {
            this.Battle.BattleGraphic().RemoveGraphic(this);
        }

        public void UnityUpdate()
        {
            OnUnityUpdate();
        }

        protected virtual void OnUnityUpdate()
        {
            
        }
    }
}