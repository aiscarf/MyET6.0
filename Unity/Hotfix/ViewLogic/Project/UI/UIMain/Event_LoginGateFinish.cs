namespace ET
{
    public class Event_LoginGateFinish : AEvent<EventType.LoginGateFinish>
    {
        protected override async ETTask Run(EventType.LoginGateFinish args)
        {
            var mainViewDataComponent = MainMgr.GetMainViewDataComponent();
            var mainDataComponent = DataHelper.GetDataComponentFromCurScene<MainDataComponent>();
            
            // DONE: 设置默认选择的英雄.
            mainViewDataComponent.CurSelectHeroId = mainDataComponent.PlayerInfo.HeroId;
            
            // DONE: 设置默认选择的地图模式.
            var list = DungeonConfigCategory.Instance.GetAllDungeonConfigs();
            int lastSelectDungeonId = PersistentHelper.GetInt(PersistentHelper.LAST_SELECT_DUNGEON_ID);
            DungeonVO lastSelectDungeonVo = null;
            if (list.Count > 0)
            {
                var dungeonConfig = list.Find(config => config.Id == lastSelectDungeonId) ?? list[0];
                lastSelectDungeonVo = new DungeonVO
                {
                    Id = dungeonConfig.Id,
                    Name = dungeonConfig.Name,
                    Background = dungeonConfig.Background
                };
            }
            else
            {
                lastSelectDungeonVo = new DungeonVO()
                {
                    Id = lastSelectDungeonId,
                    Name = "请选择模式",
                };
            }

            mainViewDataComponent.CurSelectDungeonProxy.SetValue(lastSelectDungeonVo);
            
            await UIHelper.OpenUI(UIType.UIMain);
        }
    }
}