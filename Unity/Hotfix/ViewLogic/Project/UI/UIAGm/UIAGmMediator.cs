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

        async void OnBtnGmClick()
        {
            Log.Info("打开Gm面板 2222222222222222");

            UIHelper.ShowSingleSelect("提示", "测试测试测试", "确定", () => { Log.Debug("单击确认按钮"); });

            bool b = await UIHelper.ShowDoubleSelectAsync("测试", "测试测试测试测试测试", "确定", "取消");
            if (b)
            {
                Log.Debug("确定按钮");
            }
            else
            {
                Log.Debug("失效按钮");
            }
        }
    }
}