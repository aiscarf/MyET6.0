using System;

namespace ET
{
    public class G2R_PlayerOffline_Handler: AMActorRpcHandler<Scene, G2R_PlayerOfflineRequest, R2G_PlayerOfflineResponse>
    {
        protected override async ETTask Run(Scene scene, G2R_PlayerOfflineRequest request, R2G_PlayerOfflineResponse response, Action reply)
        {
            try
            {
                await ETTask.CompletedTask;
                scene.GetComponent<OnlineComponent>().RemoveByToken(request.RealmToken);
                scene.GetComponent<RealmTokenComponent>().RemoveToken(request.RealmToken);
                reply();
            }
            catch (Exception e)
            {
                response.Error = ErrorCore.ERR_RpcFail;
                response.Message = e.ToString();
                reply();
            }
        }
    }
}