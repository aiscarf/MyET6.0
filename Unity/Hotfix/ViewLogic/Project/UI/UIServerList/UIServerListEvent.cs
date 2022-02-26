namespace ET
{
    [UIEventTag(UIType.UIServerList)]
    public class UIServerListEvent : UIEvent<UIServerListComponent>
    {
        public override async ETTask PreOpen()
        {
            self.ServiceVos = LoginMgr.GetAllServiceVos();
            self.RegionVos = LoginMgr.GetAllRegionVos();

            // DONE: 设置默认选取的服务器大区.
            self.CurSelectRegion = LoginMgr.GetLoginViewDataComponent().CurSelectRegionProxy.GetValue();
            await UIManager.Instance.OpenUI(UIType.UIServerList);
        }

        public override async ETTask PreClose()
        {
            await UIManager.Instance.CloseUI(this.ViewUI.Name);
            LoginMgr.GetLoginViewDataComponent().CurSelectRegionProxy.SetValue(self.CurSelectRegion);
        }
    }
}