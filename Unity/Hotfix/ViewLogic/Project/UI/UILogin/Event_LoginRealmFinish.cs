namespace ET
{
    public class Event_LoginRealmFinish : AEvent<EventType.LoginRealmFinish>
    {
        protected override async ETTask Run(EventType.LoginRealmFinish args)
        {
            // DONE: 请求服务器列表.
            await LoginMgr.RequestServerList();
            
            // DONE: 获取最后一次登录的大区Id.
            var loginViewDataComponent = LoginMgr.GetLoginViewDataComponent();
            int lastSelectRegionId = PersistentHelper.GetInt("LastSelectRegionId");
            GameRegionVO lastSelectRegionVo = null;
            if (loginViewDataComponent.GameRegionVos.Count > 0)
            {
                lastSelectRegionVo =
                    loginViewDataComponent.GameRegionVos.Find((rVo) => rVo.RegionId == lastSelectRegionId) ??
                    loginViewDataComponent.GameRegionVos[0];
            }
            else
            {
                lastSelectRegionVo = new GameRegionVO()
                {
                    RegionId = lastSelectRegionId,
                    RegionName = "请配置正确的大区数据",
                };
            }
            loginViewDataComponent.CurSelectRegionProxy.SetValue(lastSelectRegionVo);

            // DONE: 关闭登录界面.
            await UIManager.Instance.DestroyUI(UIType.UILogin);
            
            // DONE: 弹出UIServerList界面.
            await UIHelper.OpenUI(UIType.UIServerList);
        }
    }
}