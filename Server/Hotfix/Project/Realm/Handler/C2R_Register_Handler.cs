using System;
using System.Collections.Generic;

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

                var resultsInfos = await dbComponent.Query<DBAccountInfo>(c => c.Account == request.Account);
                if (resultsInfos.Count > 0)
                {
                    response.Error = ErrorCode.ERR_LOGIN_REGISTER_ALREADY_ACCOUNT;
                    response.Message = "注册账号已经存在!";
                    reply();
                    return;
                }

                // DONE: 新建账号
                var uid = IdGenerater.Instance.GenerateId();
                DBAccountInfo newAccount = new DBAccountInfo()
                {
                    Id = uid,
                    Account = request.Account,
                    Password = request.Password
                };
                
                await dbComponent.Save<DBAccountInfo>(newAccount);

                // DONE: 新建玩家数据
                DBPlayerInfo newPlayerInfo = new DBPlayerInfo()
                {
                    Id = uid,
                    Nickname = "",
                    Gold = 0,
                    Diamond = 0,
                    Charge = 0,
                    HeroId = 0,
                    HeroInfos = new List<HeroInfo>(),
                    PetId = 0,
                    PetInfos = new List<PetInfo>(),
                    BagInfos = new List<BagInfo>(),
                    HeadInfo = new HeadInfo(),
                    HeadIds = new List<int>(),
                    FrameIds = new List<int>(),
                    ShowIds = new List<int>(),
                    Score = 0,
                    RoleWinInfo = new RoleWinInfo(),
                    Guide = 0,
                    TaskInfo = new TaskInfo(),
                    Tasks = new List<TaskSingleInfo>(),
                    MaxScore = 0,
                    SeasonAwards = new List<int>(),
                    MapId = 0,
                    UnlockedSkills = new List<int>(),
                    TimeLimitItems = new List<int>(),
                    FriendAsOk = 0,
                    IfTeamInvite = 0,
                };

                await dbComponent.Save<DBPlayerInfo>(newPlayerInfo);
                LogHelper.Console(SceneType.Realm, $"[{newAccount.Account}]注册成功");
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}