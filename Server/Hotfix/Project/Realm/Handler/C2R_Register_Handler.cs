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
                
                // DONE: 初始化账号数据.
                var config = InitAccountConfigCategory.Instance.Get(1);
                newPlayerInfo.Gold = config.Gold;
                newPlayerInfo.Diamond = config.Diamond;

                // DONE: 1.初始化账号英雄数据.
                if (config.Hero != null && config.Hero.Length > 0)
                {
                    newPlayerInfo.HeroId = config.Hero[0];
                    for (int i = 0; i < config.Hero.Length; i++)
                    {
                        var heroConfig = HeroConfigCategory.Instance.Get(config.Hero[i]);
                        if (heroConfig == null)
                        {
                            Log.Error($"初始化游戏账号数据错误Id={config.Id}, HeroId={config.Hero[i]}");
                            continue;
                        }

                        var heroInfo = new HeroInfo();
                        heroInfo.HeroId = heroConfig.Id;
                        heroInfo.Level = 1;
                        heroInfo.SkinId = heroConfig.SkillId;
                        newPlayerInfo.HeroInfos.Add(heroInfo);
                    }
                }

                // TODO: 2.初始化账号背包数据.
                if (config.Item != null && config.Item.Length > 0)
                {
                    for (int i = 0; i < config.Item.Length; i++)
                    {
                        
                    }
                }
                

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