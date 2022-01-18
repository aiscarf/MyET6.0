namespace ET
{
    public class MobaBattleComponentAwakeSystem : AwakeSystem<MobaBattleComponent, MobaBattleData>
    {
        public override void Awake(MobaBattleComponent self, MobaBattleData data)
        {
            self.m_battleMode = data.BattleMode;
            self.m_bIsNet = data.IsNet;

            switch (self.m_battleMode)
            {
                case EBattleMode.EDefault:
                case EBattleMode.E3v3:
                    self.m_battleProcess = self.m_bIsNet ? new Server3v3Process() : new Local3v3Process();
                    break;
                case EBattleMode.ENoviceGuide:
                    self.m_battleProcess = new NoviceGuideProcess();
                    break;
                case EBattleMode.E1v1:
                    break;
                case EBattleMode.EBoss:
                    break;
                case EBattleMode.EScuffle:
                    break;
                case EBattleMode.EDefense:
                    break;
            }
            
            Game.EventSystem.Publish(new EventType.MobaGameEntryAwake());
        }
    }
    
    public class MobaBattleComponentDestroySystem: DestroySystem<MobaBattleComponent>
    {
        public override void Destroy(MobaBattleComponent self)
        {
            
        }
    }

    public static class MobaBattleSystem
    {
        
    }
}