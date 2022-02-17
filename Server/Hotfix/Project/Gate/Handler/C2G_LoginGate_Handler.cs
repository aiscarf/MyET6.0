using System;

namespace ET
{
    public class C2G_LoginGate_Handler: AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
        {
            try
            {
                // DONE: 去Realm服务器进行验证.
                var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(session.DomainZone(), SceneType.Realm);
                long actorId1 = startSceneConfig.InstanceId;
                R2G_VerifyTokenResponse verifyTokenResponse =
                        await MessageHelper.CallActor(actorId1, new G2R_VerifyTokenRequest() { RealmToken = request.RealmToken }) as
                                R2G_VerifyTokenResponse;
                if (verifyTokenResponse == null)
                {
                    response.Error = ErrorCore.ERR_ConnectGateKeyError;
                    response.Message = "RealmToken验证失败!";
                    return;
                }

                if (verifyTokenResponse.Error > ErrorCode.ERR_Success)
                {
                    response.Error = verifyTokenResponse.Error;
                    response.Message = verifyTokenResponse.Message;
                    return;
                }

                session.RemoveComponent<SessionAcceptTimeoutComponent>();

                // DONE: 创建Player对象.
                Scene scene = session.DomainScene();
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.AddChild<Player, string>(request.RealmToken.ToString());
                playerComponent.Add(player);
                session.AddComponent<SessionPlayerComponent>().Player = player;
                session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

                // DONE: 向登录服务器发送玩家上线消息.
                R2G_PlayerOnlineResponse playerOnlineResponse =
                        await MessageHelper.CallActor(actorId1, new G2R_PlayerOnlineRequest() { RealmToken = request.RealmToken }) as
                                R2G_PlayerOnlineResponse;
                if (playerOnlineResponse.Error > ErrorCode.ERR_Success)
                {
                    response.Error = playerOnlineResponse.Error;
                    response.Message = playerOnlineResponse.Message;
                    return;
                }

                Log.Info($"玩家{request.RealmToken}已正常上线");
                // TODO 生成GateToken, 用于断线重连用.
                response.GateToken = request.RealmToken + "|GateToken";
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }

            await ETTask.CompletedTask;
        }
    }
}