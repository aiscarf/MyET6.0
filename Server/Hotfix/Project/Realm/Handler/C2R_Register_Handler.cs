using System;

namespace ET
{
    public class C2R_Register_Handler: AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override async ETTask Run(Session session, C2R_Register request, R2C_Register response, Action reply)
        {
            try
            {
                var dbComponent = Game.Scene.GetComponent<DBManagerComponent>().GetZoneDB(session.DomainZone());
                if (dbComponent == null)
                {
                    response.Error = ErrorCore.ERR_ConnectGateKeyError;
                    response.Message = "服务器没有正常连接数据库";
                    reply();
                    return;
                }

                var resultsInfos = await dbComponent.Query<AccountInfo>(c => c.Account == request.Account);
                if (resultsInfos.Count > 0)
                {
                    response.Error = ErrorCode.ERR_LOGIN_REGISTER_ALREADY_ACCOUNT;
                    response.Message = "注册账号已经存在!";
                    reply();
                    return;
                }

                // DONE: 新建账号
                AccountInfo newAccount = new AccountInfo() { Account = request.Account, Password = request.Password };
                await dbComponent.Save<AccountInfo>(newAccount);

                // DONE: 用户数据
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}