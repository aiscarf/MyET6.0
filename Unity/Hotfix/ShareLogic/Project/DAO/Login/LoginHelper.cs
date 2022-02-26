using System;

namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask LoginRealm(string account, string password)
        {
            try
            {
                // DONE: 创建一个LoginSession.
                Session session = ZoneSceneManagerComponent.Instance.CurScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(ConstValue.LoginAddress));
                R2C_Login r2CLogin = (R2C_Login)await session.Call(new C2R_Login() { Account = account, Password = password });
                session.Dispose();

                if (r2CLogin.Error > ErrorCode.ERR_Success)
                {
                    Log.Info(r2CLogin.Error + " " + r2CLogin.Message);
                    return;
                }

                // DONE: RealmToken记录起来.
                var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
                loginDataComponent.Uid = r2CLogin.Uid;
                loginDataComponent.RealmToken = r2CLogin.RealmToken;
                loginDataComponent.IsLoginRealm = true;
                await Game.EventSystem.PublishAsync(new EventType.LoginRealmFinish());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask RegisterRealm(string account, string password)
        {
            try
            {
                // DONE: 创建一个LoginSession, 使用完立即释放调.
                Session session = ZoneSceneManagerComponent.Instance.CurScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(ConstValue.LoginAddress));
                R2C_Register r2CRegister = (R2C_Register)await session.Call(new C2R_Register(){Account = account, Password = password});
                session.Dispose();

                if (r2CRegister.Error > ErrorCode.ERR_Success)
                {
                    Log.Info(r2CRegister.Error + " " + r2CRegister.Message);
                    return;
                }

                Log.Info("注册成功!");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask RequestServerList()
        {
            try
            {
                Session session = ZoneSceneManagerComponent.Instance.CurScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(ConstValue.LoginAddress));
                R2C_ServiceList r2CServiceList = (R2C_ServiceList)await session.Call(new C2R_ServiceList());
                session.Dispose();
                if (r2CServiceList.Error > ErrorCode.ERR_Success)
                {
                    Log.Info(r2CServiceList.Error + " " + r2CServiceList.Message);
                    return;
                }

                // DONE: 将服务器列表数据缓存起来.
                var list1 = r2CServiceList.ServiceList;
                var list2 = r2CServiceList.RegionList;

                var loginData = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
                loginData.GameServices = list1;
                loginData.GameRegions = list2;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask LoginGate(string gateAddress)
        {
            try
            {
                var mainScene = ZoneSceneManagerComponent.Instance.Get((int)SceneType.Main);
                if (mainScene == null)
                {
                    mainScene = await SceneFactory.CreateZoneScene((int)SceneType.Main, SceneType.Main, SceneType.Main.ToString(), Game.Scene);
                }

                // DONE: 尝试连接Gate服务器.
                var loginDataComponent = DataHelper.GetDataComponentFromCurScene<LoginDataComponent>();
                Session gateSession = mainScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(gateAddress));
                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(new C2G_LoginGate()
                    { Uid = loginDataComponent.Uid, RealmToken = loginDataComponent.RealmToken });
                if (g2CLoginGate.Error > ErrorCode.ERR_Success)
                {
                    mainScene.Dispose();
                    Log.Error(g2CLoginGate.Error + " " + g2CLoginGate.Message);
                    return;
                }

                // DONE: 切换至主场景.
                await ZoneSceneManagerComponent.Instance.ChangeScene(SceneType.Main);

                // DONE: 存储服务器的数据.
                var mainDataComponent = DataHelper.GetDataComponentFromCurScene<MainDataComponent>();
                mainDataComponent.SessionId = gateSession.Id;
                mainDataComponent.GateToken = g2CLoginGate.GateToken;
                mainDataComponent.PlayerInfo = g2CLoginGate.PlayerInfo;
                mainDataComponent.FriendInfos = g2CLoginGate.Friends;
                mainDataComponent.ServerTime = g2CLoginGate.Time;

                // DONE: 创建一个gate Session, 并且保存到SessionComponent中.
                gateSession.AddComponent<PingComponent>();
                mainScene.AddComponent<SessionComponent>().Session = gateSession;

                // DONE: 通知已经登录了Gate服务器.
                await Game.EventSystem.PublishAsync(new EventType.LoginGateFinish());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}