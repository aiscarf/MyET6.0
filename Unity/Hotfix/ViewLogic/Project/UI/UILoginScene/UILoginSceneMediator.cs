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
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            bool bLoginRealm = loginDataComponent.IsLoginRealm;
            self.EUI_Button_SelectServer.gameObject.SetActive(bLoginRealm);
            if (!bLoginRealm) 
                return;
            self.EUI_Text_ServerName.text = UIHelper.ViewDataDomain.CurSelectRegion.RegionName;
        }

        public override void OnClose()
        {
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            bool bLoginRealm = loginDataComponent.IsLoginRealm;
            self.EUI_Button_SelectServer.gameObject.SetActive(bLoginRealm);
            if (!bLoginRealm)
                return;
            self.EUI_Text_ServerName.text = UIHelper.ViewDataDomain.CurSelectRegion.RegionName;
        }

        void OnBtnEnterClick()
        {
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            bool bLoginRealm = loginDataComponent.IsLoginRealm;
            if (!bLoginRealm)
            {
                UIHelper.OpenUI(UIType.UILogin).Coroutine();
            }
            else
            {
                // DONE: 真正进入游戏
                LoginHelper.LoginGate(UIHelper.ViewDataDomain.CurSelectRegion.Address).Coroutine();
            }
        }

        async void OnBtnSelectServerClick()
        {
            // TODO 当关闭UIServerList时, 刷新界面.
            await UIHelper.OpenUI(UIType.UIServerList);
        }
    }
}