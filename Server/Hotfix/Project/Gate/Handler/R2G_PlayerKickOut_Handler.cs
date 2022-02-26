using System;

namespace ET
{
    public class R2G_PlayerKickOut_Handler: AMActorRpcHandler<Scene, R2G_PlayerKickOutRequest, G2R_PlayerKickOutResponse>
    {
        protected override async ETTask Run(Scene scene, R2G_PlayerKickOutRequest request, G2R_PlayerKickOutResponse response, Action reply)
        {
            try
            {
                await ETTask.CompletedTask;

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
                playerComponent.Remove(request.Uid);

                LogHelper.Console(SceneType.Gate, $"Gate服务器已将玩家{request.Uid}踢下线");
                // TODO 应通知客户端.
                
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}