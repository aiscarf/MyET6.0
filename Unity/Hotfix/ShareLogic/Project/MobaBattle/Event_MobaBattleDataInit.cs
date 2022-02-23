namespace ET
{
    public class Event_MobaBattleDataInit : AEvent<EventType.MobaBattleDataInit>
    {
        protected override async ETTask Run(EventType.MobaBattleDataInit args)
        {
            await ETTask.CompletedTask;
            
            var mobaBattleEntity =
                ZoneSceneManagerComponent.Instance.CurScene.AddComponent<MobaBattleComponent, MobaBattleData>(
                    new MobaBattleData()
                    {
                        BattleMode = args.eBattleMode,
                        IsNet = args.bIsNet,
                    });
            
            var battleSceneComponent = mobaBattleEntity.AddComponent<BattleSceneComponent, MapData>(args.mapData);
            
            // TODO 初始化角色数据.
            for (int i = 0; i < args.lstPlayerInfos.Count; i++)
            {
                var playerInfo = args.lstPlayerInfos[i];

                var unit = battleSceneComponent.CreateUnit(new AttrData() 
                {
                    TemplateId = playerInfo.heroId,
                    ServerId = playerInfo.uid,
                    SkinId = playerInfo.heroSkinId,
                    NickName = playerInfo.nickname,
                    BornPos = new SVector3(-52000, 0, 1200),
                    BornForward = new SVector3(0, 0, 1000),
                });
            }
            
            mobaBattleEntity.AddComponent<FrameSyncComponent>();
            var inputComponent = mobaBattleEntity.AddComponent<InputComponent>();
            
            if (args.bIsNet)
            {
            }
            else
            {
                mobaBattleEntity.AddComponent<LocalMobaServerComponent>();
            }
            
            mobaBattleEntity.m_battleProcess.Init();
            
            await mobaBattleEntity.m_battleProcess.Start();
        }
    }
}