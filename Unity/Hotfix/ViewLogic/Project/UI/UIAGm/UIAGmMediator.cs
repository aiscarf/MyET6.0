namespace ET
{
    public partial class UIAGmMediator : UIMediator<UIAGmComponent>
    {
        public override void OnInit()
        {
            self.EUI_Button_Reload.onClick.AddListener(OnBtnReloadClick);
            self.EUI_Button_Gm.onClick.AddListener(OnBtnGmClick);
        }

        public override void OnDestroy()
        {
            self.EUI_Button_Reload.onClick.RemoveAllListeners();
            self.EUI_Button_Gm.onClick.RemoveAllListeners();
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
        
        async void OnBtnReloadClick()
        {
            Log.Info("执行热重载");
            
            CodeLoader.Instance.LoadLogic();
            Game.EventSystem.Add(CodeLoader.Instance.GetTypes());
            
            await Game.EventSystem.PublishAsync(new EventType.Reload());
        }

        void OnBtnGmClick()
        {
            Log.Info("打开Gm面板 2222222222222222");
        }
    }
}