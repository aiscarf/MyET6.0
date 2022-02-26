using System.Collections.Generic;

namespace ET
{
    public static class LoginMgr
    {
        public static async ETTask LoginRealm(string account, string password)
        {
            // DONE: 先去登录服务器登录.
            await LoginHelper.LoginRealm(account, password);

            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            var loginViewComponent = GetLoginViewDataComponent();
            loginViewComponent.IsLoginRealmProxy.SetValue(loginDataComponent.IsLoginRealm);
            loginViewComponent.Uid = loginDataComponent.Uid;
            loginViewComponent.RealmToken = loginDataComponent.RealmToken;
        }

        public static async ETTask RegisterRealm(string account, string password)
        {
            // DONE: 登录服务器进行注册.
            await LoginHelper.RegisterRealm(account, password);
        }
        
        public static async ETTask RequestServerList()
        {
            // DONE: 先去服务器请求服务器列表数据.
            await LoginHelper.RequestServerList();

            // DONE: 将服务器列表数据转至业务层.
            var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
            var loginViewComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();

            loginViewComponent.GameServiceVos = new List<GameServiceVO>();
            for (int i = 0; i < loginDataComponent.GameServices.Count; i++)
            {
                var gsDo = loginDataComponent.GameServices[i];
                var gsVo = new GameServiceVO();
                gsVo.ServiceId = gsDo.ServiceId;
                gsVo.ServiceName = gsDo.ServiceName;
                gsVo.ServiceStartTime = gsDo.ServiceStartTime;
                loginViewComponent.GameServiceVos.Add(gsVo);
            }

            loginViewComponent.GameRegionVos = new List<GameRegionVO>();
            for (int i = 0; i < loginDataComponent.GameRegions.Count; i++)
            {
                var grDo = loginDataComponent.GameRegions[i];
                var grVo = new GameRegionVO();
                grVo.RegionId = grDo.RegionId;
                grVo.RegionName = grDo.RegionName;
                grVo.State = grDo.State;
                grVo.Address = grDo.Address;
                loginViewComponent.GameRegionVos.Add(grVo);
            }
        }

        public static async ETTask LoginGate(string gateAddress)
        {
            // TODO 对地址格式进行核验, 错误的话, 进行Tips弹窗.
            if (string.IsNullOrEmpty(gateAddress) || string.IsNullOrWhiteSpace(gateAddress))
            {
                Log.Debug($"Gate地址错误: {gateAddress}");
                return;
            }
            
            // DONE: 存储登录的大区Id.
            var loginViewDataComponent = GetLoginViewDataComponent();
            PersistentHelper.SetInt(PersistentHelper.LAST_SELECT_REGION_ID, loginViewDataComponent.CurSelectRegionProxy.GetValue().RegionId);
            
            await LoginHelper.LoginGate(gateAddress);
        }

        public static LoginViewDataComponent GetLoginViewDataComponent()
        {
            return DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
        }
        public static List<GameServiceVO> GetAllServiceVos()
        {
            var loginViewComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
            return loginViewComponent.GameServiceVos;
        }

        public static List<GameRegionVO> GetAllRegionVos()
        {
            var loginViewComponent = DataHelper.GetDataComponentFromCurScene<LoginViewDataComponent>();
            return loginViewComponent.GameRegionVos;
        }
    }
}