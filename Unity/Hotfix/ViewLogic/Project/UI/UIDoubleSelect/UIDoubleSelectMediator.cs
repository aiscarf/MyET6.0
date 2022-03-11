namespace ET
{
    public partial class UIDoubleSelectMediator : UIMediator<UIDoubleSelectComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Ok.onClick.AddListener(OnBtnOkClick);
            self.EUI_Button_Cancel.onClick.AddListener(OnBtnCancelClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Ok.onClick.RemoveAllListeners();
            self.EUI_Button_Cancel.onClick.RemoveAllListeners();
        }

        public override void OnOpen()
        {
            self.EUI_Text_Title.text = self.Title;
            self.EUI_Text_Content.text = self.Content;
            self.EUI_Text_Ok.text = self.LeftButtonName;
            self.EUI_Text_Cancel.text = self.RightButtonName;
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
            self.IsConfirm = true;
            Close();
        }

        void OnBtnCancelClick()
        {
            self.IsConfirm = false;
            Close();
        }

        async void Close()
        {
            await UIHelper.CloseUI(ViewUI.Name);
        }
    }
}