using System;

namespace ET
{
    public class C2R_Login_Handler: AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            try
            {
                var scene = session.DomainScene();
                var dbComponent = Game.Scene.GetComponent<DBManagerComponent>().GetZoneDB(scene.Zone);
                if (dbComponent == null)
                {
                    response.Error = ErrorCore.ERR_ConnectGateKeyError;
                    response.Message = "服务器没有正常连接数据库";
                    reply();
                    return;
                }

                var resultsInfos = await dbComponent.Query<DBAccountInfo>(c => c.Account == request.Account && c.Password == request.Password);
                if (resultsInfos.Count <= 0)
                {
                    response.Error = ErrorCode.ERR_LOGIN_ACCOUNT_OR_PASSWORD;
                    response.Message = "不存在该账户或密码";
                    reply();
                    return;
                }
                
                var accountInfo = resultsInfos[0];

                // // DONE: 判断是否已经在线, 在线则踢掉
                // await RealmHelper.KickOutPlayer(scene, accountInfo.Id);

                // TODO 代码版本限制: 判断客户端版本是否足够.

                // TODO 服务器容量限制: 服务器集群连接队列已满, 则不让进入.

                // TODO 白名单
                
                // TODO 权限限制: 开发者, 游戏的主人, Admin可以进入.

                // TODO 进入验证服务器, 等待选择服务器.

                // DONE: 生成一个唯一的Token.
                response.Uid = accountInfo.Id;
                
                // DONE: 重新生成一个新的Token.
                var realmTokenComponent = scene.GetComponent<RealmTokenComponent>();
                if (realmTokenComponent.HasUid(accountInfo.Id))
                {
                    realmTokenComponent.RemoveToken(accountInfo.Id);
                }
                response.RealmToken = IdGenerater.Instance.GenerateInstanceId().ToString();
                realmTokenComponent.AddToken(accountInfo.Id, response.RealmToken);

                LogHelper.Console(SceneType.Realm, $"用户[{accountInfo.Account}]登录成功");
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}