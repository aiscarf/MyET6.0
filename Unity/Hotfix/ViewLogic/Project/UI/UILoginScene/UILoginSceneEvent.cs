using UnityEngine;

namespace ET
{
    [UIEventTag(UIType.UILoginScene)]
    public class UILoginSceneEvent : UIEvent<UILoginSceneComponent>
    {
        public override async ETTask PreOpen(object args)
        {
            var loginViewDataComponent = LoginMgr.GetLoginViewDataComponent();
            loginViewDataComponent.IsLoginRealmProxy.BindProxy(self.IsLoginRealmProxy);
            loginViewDataComponent.CurSelectRegionProxy.BindProxy(self.CurSelectRegionVoProxy);
            await UIManager.Instance.OpenAndCoverAll(UIType.UILoginScene);
        }

        public override async ETTask PreClose()
        {
            var loginViewDataComponent = LoginMgr.GetLoginViewDataComponent();
            loginViewDataComponent.IsLoginRealmProxy.UnBindProxy(self.IsLoginRealmProxy);
            loginViewDataComponent.CurSelectRegionProxy.UnBindProxy(self.CurSelectRegionVoProxy);
            await UIManager.Instance.CloseUI(this.ViewUI.Name);
        }
    }
}