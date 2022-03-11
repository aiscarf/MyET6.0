namespace ET
{
    public sealed class MobaBattleEntityAwakeSystem : AwakeSystem<MobaBattleEntity>
    {
        public override void Awake(MobaBattleEntity self)
        {
        }
    }

    public sealed class MobaBattleEntityDestroySystem : DestroySystem<MobaBattleEntity>
    {
        public override void Destroy(MobaBattleEntity self)
        {
        }
    }

    public static class MobaBattleSystem
    {
        public static void Init(this MobaBattleEntity self, MobaBattleData data)
        {
            // DONE: 初始化地图数据.
            var battleSceneComponent = self.AddComponent<BattleSceneComponent, MapData>(data.MapData);
            self.m_bIsNet = data.IsNet;
            
            // DONE: 初始化角色数据.
            for (int i = 0; i < data.Players.Count; i++)
            {
                var playerInfo = data.Players[i];

                // TODO 根据地图数据, 根据阵营 从不同的位置出生.
                var unit = battleSceneComponent.CreateUnit(new AttrData()
                {
                    TemplateId = playerInfo.HeroId,
                    ServerId = playerInfo.Uid,
                    SkinId = playerInfo.HeroSkinId,
                    NickName = playerInfo.Nickname,
                    BornPos = new SVector3(0, 0, -90000),
                    BornForward = new SVector3(0, 0, 1000),
                });
            }

            // DONE: 创建游戏玩法.
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
                    self.m_battleProcess = self.m_bIsNet ? new Server1v1Process() : new Local1v1Process();
                    break;
                case EBattleMode.EBoss:
                    break;
                case EBattleMode.EScuffle:
                    break;
                case EBattleMode.EDefense:
                    break;
            }

            self.m_battleProcess.Init();
            Game.EventSystem.Publish(new EventType.MobaGameEntryAwake());
        }

        public static void GameOver(this MobaBattleEntity self)
        {
            Log.Console("游戏结束");
            self.m_battleProcess.Destroy();
        }
    }
}