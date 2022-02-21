using System;

namespace ET
{
    public class G2R_PlayerOnline_Handler: AMActorRpcHandler<Scene, G2R_PlayerOnlineRequest, R2G_PlayerOnlineResponse>
    {
        protected override async ETTask Run(Scene scene, G2R_PlayerOnlineRequest request, R2G_PlayerOnlineResponse response, Action reply)
        {
            try
            {
                await ETTask.CompletedTask;

                // DONE: 验证Token.
                var realmToken = scene.GetComponent<RealmTokenComponent>().GetToken(request.Uid);
                if (string.IsNullOrEmpty(realmToken) || string.IsNullOrWhiteSpace(realmToken) || realmToken != request.RealmToken)
                {
                    response.Error = ErrorCode.ERR_LOGIN_VALID_REALMTOKEN;
                    response.Message = "验证失败";
                    reply();
                    return;
                }

                scene.GetComponent<OnlineComponent>().AddUid(request.Uid, request.GateId);
                LogHelper.Console(SceneType.Realm, $"玩家[{request.Uid}]已上线");
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