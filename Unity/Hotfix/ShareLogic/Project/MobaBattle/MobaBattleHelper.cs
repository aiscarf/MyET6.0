namespace ET
{
    public static class MobaBattleHelper
    {
        /// <summary>
        ///                                                       { Unit          
        ///                                { BattleSceneComponent { Area
        ///          { MobaBattleComponent { FrameSyncComponent   { Obstacle
        /// Scene -> {                     { InputComponent       
        ///          {
        ///          { MobaBattleViewComponent  { ViewComponent
        ///                                     { ...
        ///                                     { ...
        /// </summary>
        public static MobaBattleComponent GetMobaBattleComponent(this Entity self)
        {
            return self.DomainScene().GetComponent<MobaBattleComponent>();
        }

        public static FrameSyncComponent GetFrameSyncComponent(this Entity self)
        {
            return self.GetMobaBattleComponent().GetComponent<FrameSyncComponent>();
        }
    }
}