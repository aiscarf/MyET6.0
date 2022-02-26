namespace ET
{
    public partial class UILoginSceneMediator : UIMediator<UILoginSceneComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Enter.onClick.AddListener(OnBtnEnterClick);
            self.EUI_Button_SelectServer.onClick.AddListener(OnBtnSelectServerClick);
        }

        public override void OnDestroy()
        {
        }

        public override void OnOpen()
        {
            self.IsLoginRealmProxy.AddListener(SetButtonActive);
            SetButtonActive(self.IsLoginRealmProxy.GetValue());
            
            self.CurSelectRegionVoProxy.AddListener(SetServerName);
            SetServerName(self.CurSelectRegionVoProxy.GetValue());
        }

        public override void OnClose()
        {
            self.IsLoginRealmProxy.RemoveListener(SetButtonActive);
            self.CurSelectRegionVoProxy.RemoveListener(SetServerName);
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }

        void SetServerName(GameRegionVO data)
        {
            var bLoginRealm = self.IsLoginRealmProxy.GetValue();
            if (!bLoginRealm)
                return;
            self.EUI_Text_ServerName.text = data.RegionName;
        }

        void SetButtonActive(bool b)
        {
            self.EUI_Button_SelectServer.gameObject.SetActive(b);
        }

        void OnBtnEnterClick()
        {
            bool bLoginRealm = self.IsLoginRealmProxy.GetValue();
            if (!bLoginRealm)
            {
                UIHelper.OpenUI(UIType.UILogin).Coroutine();
                return;
            }

            // DONE: 真正进入游戏
            LoginMgr.LoginGate(self.CurSelectRegionVoProxy.GetValue().Address).Coroutine();
        }

        async void OnBtnSelectServerClick()
        {
            await UIHelper.OpenUI(UIType.UIServerList);
        }
    }
}