using System;

namespace ET
{
    public class G2R_PlayerOffline_Handler: AMActorRpcHandler<Scene, G2R_PlayerOfflineRequest, R2G_PlayerOfflineResponse>
    {
        protected override async ETTask Run(Scene scene, G2R_PlayerOfflineRequest request, R2G_PlayerOfflineResponse response, Action reply)
        {
            try
            {
                scene.GetComponent<OnlineComponent>().RemoveByUid(request.Uid);
                scene.GetComponent<RealmTokenComponent>().RemoveToken(request.Uid);

                LogHelper.Console(SceneType.Realm, $"玩家[{request.Uid}]已下线");
                reply();
                await ETTask.CompletedTask;
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