using System;

namespace ET
{
    public class G2R_VerifyToken_Handler: AMActorRpcHandler<Scene, G2R_VerifyTokenRequest, R2G_VerifyTokenResponse>
    {
        protected override async ETTask Run(Scene scene, G2R_VerifyTokenRequest request, R2G_VerifyTokenResponse response, Action reply)
        {
            try
            {
                await ETTask.CompletedTask;
                var account = scene.GetComponent<RealmTokenComponent>().GetAccount(request.RealmToken);
                if (string.IsNullOrEmpty(account) || string.IsNullOrWhiteSpace(account))
                {
                    response.Error = ErrorCode.ERR_LOGIN_VALID_REALMTOKEN;
                    response.Message = "RealmToken验证失败!";

                    reply();
                    return;
                }

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