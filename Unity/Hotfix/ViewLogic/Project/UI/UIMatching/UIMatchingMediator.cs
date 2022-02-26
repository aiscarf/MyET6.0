namespace ET
{
    public partial class UIMatchingMediator : UIMediator<UIMatchingComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Cancel.onClick.AddListener(OnBtnCancelClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Cancel.onClick.RemoveAllListeners();
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
        }

        async void OnBtnCancelClick()
        {
            await MainMgr.CancelReady();
            await UIHelper.CloseUI(this);
        }
    }
}