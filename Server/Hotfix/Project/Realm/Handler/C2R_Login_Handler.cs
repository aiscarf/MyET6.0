using System;

namespace ET
{
    public class C2R_Login_Handler: AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
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

                var resultsInfos = await dbComponent.Query<AccountInfo>(c => c.Account == request.Account && c.Password == request.Account);
                if (resultsInfos.Count <= 0)
                {
                    response.Error = ErrorCode.ERR_LOGIN_ACCOUNT_OR_PASSWORD;
                    response.Message = "不存在该账户或密码";
                    reply();
                    return;
                }

                var accountInfo = resultsInfos[0];

                // DONE: 判断是否已经在线, 在线则踢掉
                await RealmHelper.KickOutPlayer(session, accountInfo.Account);

                // TODO 代码版本限制: 判断客户端版本是否足够.

                // TODO 服务器容量限制: 服务器集群连接队列已满, 则不让进入.

                // TODO 白名单
                
                // TODO 权限限制: 开发者, 游戏的主人, Admin可以进入.

                // TODO 进入验证服务器, 等待选择服务器.

                // TODO 生成一个唯一的Token.{时间 + 账户 计算出的 Token}
                response.RealmToken = accountInfo.Account;
                // DONE: 将其记录起来, 记录在登录列表中.
                session.DomainScene().GetComponent<RealmTokenComponent>().AddToken(accountInfo.Account, response.RealmToken);
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}