using System;

namespace ET
{
    public class R2G_PlayerKickOut_Handler: AMActorRpcHandler<Scene, R2G_PlayerKickOutRequest, G2R_PlayerKickOutResponse>
    {
        protected override async ETTask Run(Scene scene, R2G_PlayerKickOutRequest request, G2R_PlayerKickOutResponse response, Action reply)
        {
            try
            {
                // DONE: 验证服务器说要断开客户端连接.
                var playerComponent = scene.GetComponent<PlayerComponent>();
                var player = playerComponent.Get(request.Uid);
                if (player == null)
                {
                    response.Error = ErrorCore.ERR_RpcFail;
                    response.Message = $"该玩家[{request.Uid}]已经下线";
                    reply();
                    return;
                }

                if (player.IsConnected && player.ClientSession != null && !player.ClientSession.IsDisposed)
                {
                    // DONE: 通知客户端, 被挤下线.
                    player.ClientSession.Send(new G2C_OnPlayerKickOut());
                    player.ClientSession.Dispose();
                    player.ClientSession = null;
                }

                LogHelper.Console(SceneType.Gate, $"Gate服务器已将玩家{request.Uid}踢下线");

                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}