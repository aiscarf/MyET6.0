using System;

namespace ET
{
    public class R2G_PlayerKickOut_Handler: AMActorRpcHandler<Scene, R2G_PlayerKickOutRequest, G2R_PlayerKickOutResponse>
    {
        protected override async ETTask Run(Scene scene, R2G_PlayerKickOutRequest request, G2R_PlayerKickOutResponse response, Action reply)
        {
            try
            {
                // TODO 验证服务器说要断开客户端连接.
            }
            catch (Exception e)
            {
                response.Error = ErrorCore.ERR_RpcFail;
                response.Message = e.ToString();
                reply();
            }

            await ETTask.CompletedTask;
        }
    }
}