namespace ET
{
    public partial class UIMainMediator : UIMediator<UIMainComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Frame.onClick.AddListener(OnBtnFrameClick);
            self.EUI_Button_Mode.onClick.AddListener(OnBtnModeClick);
            self.EUI_Button_Match.onClick.AddListener(OnBtnMatchClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Frame.onClick.RemoveAllListeners();
        }

        public override void OnOpen()
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
            if (UIHelper.ViewDataDomain.CurSelectDungeon == null)
                return;
            self.EUI_Text_Mode.text = UIHelper.ViewDataDomain.CurSelectDungeon.Name;

        }

        void OnBtnFrameClick()
        {
            // TODO 打开头像UI.
        }

        async void OnBtnModeClick()
        {
            await UIHelper.OpenUI(UIType.UISelectMap);
        }

        void OnBtnMatchClick()
        {
            Log.Info("OnBtnMatchClick");
        }
    }
}