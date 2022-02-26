namespace ET
{
    [UIEventTag(UIType.UISelectMap)]
    public class UISelectMapEvent : UIEvent<UISelectMapComponent>
    {
        public override async ETTask PreOpen()
        {
            // DONE: 准备数据.
            var list = DungeonConfigCategory.Instance.GetAllDungeonConfigs();
            for (int i = 0; i < list.Count; i++)
            {
                var configData = list[i];
                var dungeonVo = new DungeonVO
                {
                    Id = configData.Id,
                    Name = configData.Name,
                    Background = configData.Background
                };
                self.DungeonVos.Add(dungeonVo);
            }

            // DONE: 设置默认选择的副本数据.
            self.CurSelectDungeonVo = MainMgr.GetMainViewDataComponent().CurSelectDungeonProxy.GetValue();

            // DONE: 加载图集.
            await AtlasHelper.LoadAtlasAsync(AtlasHelper.ATLAS_DUNGEON);
            await UIManager.Instance.OpenUI(ViewUI.Name);
        }

        public override async ETTask PreClose()
        {
            MainMgr.GetMainViewDataComponent().CurSelectDungeonProxy.SetValue(self.CurSelectDungeonVo);

            // TODO 倒计时卸载界面.
            await UIManager.Instance.DestroyUI(ViewUI.Name);
            AtlasHelper.UnLoadAtlas(AtlasHelper.ATLAS_DUNGEON);
        }
    }
}