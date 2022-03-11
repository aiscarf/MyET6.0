using System;
using System.Collections.Generic;

namespace ET
{
    public class C2G_LoginGate_Handler: AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
        {
            try
            {
                // DONE: 去Realm服务器进行验证.
                var startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneType(session.DomainZone(), SceneType.Realm);
                long actorId1 = startSceneConfig.InstanceId;

                session.RemoveComponent<SessionAcceptTimeoutComponent>();

                // DONE: 向登录服务器发送玩家上线消息.
                R2G_PlayerOnlineResponse playerOnlineResponse = await MessageHelper.CallActor(actorId1,
                            new G2R_PlayerOnlineRequest()
                            {
                                Uid = request.Uid, RealmToken = request.RealmToken, GateId = session.DomainScene().Id
                            }) as
                        R2G_PlayerOnlineResponse;
                if (playerOnlineResponse.Error > ErrorCode.ERR_Success)
                {
                    response.Error = playerOnlineResponse.Error;
                    response.Message = playerOnlineResponse.Message;
                    return;
                }

                var dbComponent = Game.Scene.GetComponent<DBManagerComponent>().GetZoneDB(session.DomainZone());
                if (dbComponent == null)
                {
                    response.Error = ErrorCore.ERR_ConnectGateKeyError;
                    response.Message = "服务器没有正常连接数据库";
                    reply();
                    return;
                }

                // DONE: 查找玩家数据.
                var dbPlayerInfos = await dbComponent.Query<DBPlayerInfo>(info => info.Id == request.Uid);
                if (dbPlayerInfos.Count <= 0)
                {
                    response.Error = ErrorCode.ERR_LOGIN_ACCOUNT_OR_PASSWORD;
                    response.Message = "不存在该玩家账户, 不予登录";
                    reply();
                    return;
                }

                var playerInfo = dbPlayerInfos[0];

                // DONE: 生成GateToken, 断线重连可用.
                response.GateToken = IdGenerater.Instance.GenerateInstanceId().ToString();

                // DONE: 将服务器当前时间戳发给客户端.
                response.Time = TimeHelper.ServerNow();

                // DONE: 从数据库查询玩家数据.
                response.PlayerInfo = new PlayerInfo();
                response.PlayerInfo.Uid = playerInfo.Id;
                response.PlayerInfo.Nickname = playerInfo.Nickname;
                response.PlayerInfo.FriendCode = "";
                response.PlayerInfo.Gold = playerInfo.Gold;
                response.PlayerInfo.Diamond = playerInfo.Diamond;
                response.PlayerInfo.HeroId = playerInfo.HeroId;
                response.PlayerInfo.Heros = playerInfo.HeroInfos;
                response.PlayerInfo.PetId = playerInfo.PetId;
                response.PlayerInfo.Pets = playerInfo.PetInfos;

                // TODO 查找限时活动的皮肤Id.
                response.PlayerInfo.PublicTowerSkins = new List<int>();
                response.PlayerInfo.PublicFaces = new List<int>();
                response.PlayerInfo.PublicVoices = new List<int>();

                response.PlayerInfo.Bags = playerInfo.BagInfos;
                response.PlayerInfo.HeadInfo = playerInfo.HeadInfo;
                response.PlayerInfo.HeadIdArr = playerInfo.HeadIds;
                response.PlayerInfo.FrameIdArr = playerInfo.FrameIds;
                response.PlayerInfo.ShowIdArr = playerInfo.ShowIds;

                // TODO 查找玩家是否处于组队中.
                response.PlayerInfo.TeamId = "";
                response.PlayerInfo.TeamUsers = new List<TeamPlayerInfo>();

                response.PlayerInfo.Score = playerInfo.Score;
                response.PlayerInfo.WinInfo = playerInfo.RoleWinInfo;
                response.PlayerInfo.Guide = playerInfo.Guide;
                response.PlayerInfo.TaskInfo = playerInfo.TaskInfo;
                response.PlayerInfo.TaskOverall = playerInfo.Tasks;

                // TODO 查找玩家可见任务列表.
                response.PlayerInfo.TaskIdArr = new List<int>();
                response.PlayerInfo.Charge = playerInfo.Charge;

                // TODO 查找活动列表.
                response.PlayerInfo.MinerInfo = new MinerInfo();
                response.PlayerInfo.MaxScore = playerInfo.MaxScore;
                response.PlayerInfo.SeasonAward = playerInfo.SeasonAwards;
                response.PlayerInfo.FriendAskOk = playerInfo.FriendAsOk;

                // TODO 查找所有地图列表.
                response.PlayerInfo.MapList = new List<MapInfo>();

                response.PlayerInfo.MapId = playerInfo.MapId;
                response.PlayerInfo.UnlockedSkills = playerInfo.UnlockedSkills;
                response.PlayerInfo.TimeLimitItems = playerInfo.TimeLimitItems;
                response.PlayerInfo.TeamInviteBlack = playerInfo.TeamInviteBlack;
                response.PlayerInfo.SeasonEndTime = 10000;
                response.PlayerInfo.IfTeamInvite = 0;

                // TODO 查表获取玩家好友数据.
                response.Friends = new List<FriendInfo>();

                PlayerComponent playerComponent = session.DomainScene().GetComponent<PlayerComponent>();
                Player player = null;
                if (!playerComponent.Contains(playerInfo.Id))
                {
                    // DONE: 创建Player对象.
                    player = playerComponent.AddChild<Player, long, string>(playerInfo.Id, request.RealmToken);
                    player.GateToken = response.GateToken;
                    player.ClientSession = session;
                    player.ChangeState(EPlayerState.Hall);
                    playerComponent.Add(player);
                    session.AddComponent<SessionPlayerComponent>().Player = player;
                    session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                }
                else
                {
                    player = playerComponent.Get(playerInfo.Id);
                    player.GateToken = response.GateToken;
                    player.ClientSession = session;
                    if (!player.CancellationToken.IsCancel())
                    {
                        playerComponent.StopCountdownRemovePlayer(playerInfo.Id);
                    }
                }

                response.PlayerState = (int)player.PlayerState;
                reply();

                Log.Info($"玩家{request.Uid}已正常上线");
                LogHelper.Console(SceneType.Gate, $"玩家[{request.Uid}]已正常上线");
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}