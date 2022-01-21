using System;

namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene zoneScene, string address, string account, string password)
        {
            try
            {
                // 创建一个ETModel层的Session
                R2C_Login r2CLogin = null;
                Session session = null;
                try
                {
                    session = ZoneSceneManagerComponent.Instance.CurScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                    {
                        r2CLogin = (R2C_Login)await session.Call(new C2R_Login()
                            { Account = account, Password = password });
                    }
                }
                finally
                {
                    session?.Dispose();
                }

                // TODO 进入到主场景.
                ZoneSceneManagerComponent.Instance.ChangeScene(2);

                // 创建一个gate Session,并且保存到SessionComponent中
                var mainScene = ZoneSceneManagerComponent.Instance.CurScene;
                Session gateSession = mainScene.GetComponent<NetKcpComponent>()
                    .Create(NetworkHelper.ToIPEndPoint(r2CLogin.Address));
                gateSession.AddComponent<PingComponent>();
                mainScene.AddComponent<SessionComponent>().Session = gateSession;

                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(new C2G_LoginGate()
                    { Key = r2CLogin.Key, GateId = r2CLogin.GateId });
                
                // TODO 初始化用户信息.
                Log.Debug("登陆gate成功!");

                // TODO 打开到主场景界面.
                await Game.EventSystem.PublishAsync(new EventType.LoginFinish() { ZoneScene = zoneScene });
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}