namespace ET
{
    public class Event_LoginRealmFinish : AEvent<EventType.LoginRealmFinish>
    {
        protected override async ETTask Run(EventType.LoginRealmFinish args)
        {
            // DONE: 请求服务器列表.
            await LoginHelper.RequestServerList();
            
            // DONE: 设置默认选取的服务器大区.
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            var loginViewDataComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
            if (loginViewDataComponent.CurSelectRegion == null)
            {
                // TODO 应该读取本地的配置文件, 将之前选择的大区进行一次赋值.
                loginViewDataComponent.CurSelectRegion = loginDataComponent.GetRegionByIndex(0);
            }

            // DONE: 关闭登录界面.
            await UIManager.Instance.DestroyUI(UIType.UILogin);
            
            // DONE: 弹出UIServerList界面.
            await UIManager.Instance.OpenUI(UIType.UIServerList, null);
        }
    }
}