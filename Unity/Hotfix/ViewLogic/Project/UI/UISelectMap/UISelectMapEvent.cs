namespace ET
{
    [UIEventTag(UIType.UISelectMap)]
    public class UISelectMapEvent : UIEvent
    {
        public override async ETTask PreOpen()
        {
            var ui = await UIHelper.GetOrCreateUI(Name);
            
            // DONE: 准备数据.
            var selectMapViewData = ui.GetComponent<UISelectMapComponent>();
            // TODO 真数据应查寻表格.
            // TODO 假数据.
            for (int i = 1; i <= 10; i++)
            {
                selectMapViewData.DungeonVos.Add(new DungeonVO() { Id = i, Name = $"测试副本{i}", ImageName = "ui_mmap_chibi" });
            }
            
            // DONE: 加载图集.
            await AtlasHelper.LoadAtlasAsync(AtlasHelper.ATLAS_DUNGEON);
            await UIManager.Instance.OpenUI(Name);
        }

        public override async ETTask PreClose()
        {
            AtlasHelper.UnLoadAtlas(AtlasHelper.ATLAS_DUNGEON);
            await UIManager.Instance.DestroyUI(Name);
        }
    }
}