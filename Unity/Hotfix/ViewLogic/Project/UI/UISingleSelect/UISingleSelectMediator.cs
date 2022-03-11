namespace ET
{
    public partial class UISingleSelectMediator : UIMediator<UISingleSelectComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Ok.onClick.AddListener(OnBtnOkClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Ok.onClick.RemoveAllListeners();
        }

        public override void OnOpen()
        {
            self.EUI_Text_Button.text = self.ButtonName;
            self.EUI_Text_Content.text = self.Content;
            self.EUI_Text_Title.text = self.Title;
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

        void OnBtnOkClick()
        {
            UIHelper.CloseUI(ViewUI.Name).Coroutine();
        }
    }
}