namespace ET
{
    public partial class UILoginMediator : UIMediator<UILoginComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Register.onClick.AddListener(OnBtnRegisterClick);
            self.EUI_Button_Login.onClick.AddListener(OnBtnLoginClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Login.onClick.RemoveListener(OnBtnLoginClick);
        }

        public override void OnOpen(object data)
        {
        }

        public override void OnClose()
        {
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }

        private async void OnBtnRegisterClick()
        {
            await UIManager.Instance.OpenUI(UIType.UIRegister, null);
        }
        
        private async void OnBtnLoginClick()
        {
            // TODO 1.应对账号密码进行初步的格式校验.
            await LoginHelper.LoginRealm(self.EUI_InputField_Account.text, self.EUI_InputField_Password.text);
        }
    }
}