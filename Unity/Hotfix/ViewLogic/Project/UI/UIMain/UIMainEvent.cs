namespace ET
{
    [UIEventTag(UIType.UIMain)]
    public class UIMainEvent : UIEvent<UIMainComponent>
    {
        public override async ETTask PreOpen(object args)
        {
            var mainViewDataComponent = MainMgr.GetMainViewDataComponent();
            mainViewDataComponent.CurSelectDungeonProxy.BindProxy(self.CurDungeonProxy);
            self.CurSelectHeroId = mainViewDataComponent.CurSelectHeroId;
            
            await UIManager.Instance.OpenAndCoverAll(UIType.UIMain);
        }

        public override async ETTask PreClose()
        {
            MainMgr.GetMainViewDataComponent().CurSelectDungeonProxy.UnBindProxy(self.CurDungeonProxy);
            await UIManager.Instance.CloseUI(UIType.UIMain);
        }
    }
}